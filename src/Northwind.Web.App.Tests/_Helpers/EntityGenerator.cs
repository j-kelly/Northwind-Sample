namespace Northwind.Web.App.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class EntityGenerator
    {
        private const int maxDepthConst = 5;
        private readonly bool _ThrowOnError;

        private int _DepthCounter = 1;
        private int _CurrentCount = 1;

        public EntityGenerator()
            : this(maxDepthConst, true)
        {
        }

        public EntityGenerator(bool throwOnError)
            : this(maxDepthConst, throwOnError)
        {
        }

        public EntityGenerator(int maxDepth, bool throwOnError)
        {
            _ThrowOnError = throwOnError;
            MaxDepth = maxDepth;
        }

        public int MaxDepth
        {
            get;
            set;
        }

        public static T Create<T>(params Action<T>[] updates)
        {
            return new EntityGenerator().CreateEntity<T>(updates);
        }

        public T CreateEntity<T>(params Action<T>[] updates)
        {
            var retVal = (T)CreateEntity(typeof(T));
            updates.ToList().ForEach(action => action(retVal));
            return retVal;
        }

        public object CreateEntity(Type type)
        {
            if (_CurrentCount++ >= MaxDepth)
            {
                _CurrentCount--;
                return null;
            }
            else
            {
                try
                {
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        var genType = type.GetGenericArguments().Single();
                        var val = GetValue(genType);
                        _CurrentCount--;
                        return val;
                    }

                    var retVal = Activator.CreateInstance(type);
                    AddValues(retVal, retVal.GetType().GetProperties());
                    _CurrentCount--;
                    return retVal;
                }
                catch (Exception)
                {
                    if (!_ThrowOnError)
                        return null;

                    throw;
                }
            }
        }

        public void AddValues(object parentObject, PropertyInfo[] properties)
        {
            foreach (var prop in properties)
            {
                // Can use set_PropertyNameHere so we can set private variables - One for the future
                if (prop.GetSetMethod() == null)
                    return;

                SetSingleValue(parentObject, prop);
            }
        }

        private void SetSingleValue(object parentObject, PropertyInfo prop)
        {
            if (prop.PropertyType == typeof(string))
            {
                prop.SetValue(parentObject, prop.Name, null);
                return;
            }

            if (prop.PropertyType == typeof(DateTime) ||
                prop.PropertyType == typeof(DateTime?))
            {
                prop.SetValue(parentObject, DateTime.Today, null);
                return;
            }

            if (prop.PropertyType == typeof(short) ||
                prop.PropertyType == typeof(short?))
            {
                prop.SetValue(parentObject, (short)_DepthCounter++, null);
                return;
            }

            if (prop.PropertyType == typeof(int) ||
                prop.PropertyType == typeof(int?))
            {
                prop.SetValue(parentObject, _DepthCounter++, null);
                return;
            }

            if (prop.PropertyType == typeof(decimal) ||
                prop.PropertyType == typeof(decimal?))
            {
                prop.SetValue(parentObject, (decimal)_DepthCounter++, null);
                return;
            }

            if (prop.PropertyType.IsEnum)
            {
                prop.SetValue(parentObject, 1, null);
                return;
            }

            if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType))
            {

                var type = prop.PropertyType.IsArray
                    ? prop.PropertyType.GetElementType()
                    : prop.PropertyType.GetGenericArguments().Single();

                var obj = GetValue(type);
                if (obj == null)
                    return;

                if (prop.PropertyType.IsArray)
                {
                    var arrayType = prop.PropertyType.GetElementType();
                    var array = Array.CreateInstance(arrayType, 1);
                    array.SetValue(obj, 0);
                    prop.SetValue(parentObject, array, null);
                    return;
                }

                var listType = typeof(List<>);
                var constructedListType = listType.MakeGenericType(type);
                var retVal = Activator.CreateInstance(constructedListType);

                var mi = retVal.GetType().GetMethod("Add");
                mi.Invoke(retVal, new object[] { obj });

                prop.SetValue(parentObject, retVal, null);
                return;
            }

            var val = CreateEntity(prop.PropertyType);
            if (prop.PropertyType.IsGenericType &&
                prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                prop.PropertyType.GetGenericArguments().Single().IsEnum)
            {
                val = null;
            }

            prop.SetValue(parentObject, val, null);
        }

        private object GetValue(Type type)
        {
            if (type == typeof(string))
            {
                return typeof(string).Name + _DepthCounter++;
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                return DateTime.Today;
            }
            else if (type == typeof(short) || type == typeof(short?))
            {
                return (short)_DepthCounter++;
            }
            else if (type == typeof(int) || type == typeof(int?))
            {
                return _DepthCounter++;
            }
            else if (type == typeof(decimal) || type == typeof(decimal?))
            {
                return (decimal)_DepthCounter++;
            }
            else if (type.IsEnum)
            {
                return Enum.Parse(type, Enum.GetNames(type).First());
            }
            else
            {
                return CreateEntity(type);
            }
        }
    }
}
