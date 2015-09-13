namespace NUnit.Framework
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public static class NUnitExtensions
    {
        [DebuggerStepThrough]
        public static void ShouldEqual(this object actualValue, object expectedValue)
        {
            ShouldEqual(actualValue, expectedValue, string.Empty);
        }

        [DebuggerStepThrough]
        public static void ShouldEqual(this object actualValue, object expectedValue, string message)
        {
            if (expectedValue == null)
            {
                Assert.IsNull(actualValue, message);
                return;
            }

            Assert.AreEqual(expectedValue, actualValue, message);
        }

        [DebuggerStepThrough]
        public static void ShouldNotEqual(this object actualValue, object expectedValue)
        {
            ShouldNotEqual(actualValue, expectedValue, string.Empty);
        }

        [DebuggerStepThrough]
        public static void ShouldNotEqual(this object actualValue, object expectedValue, string message)
        {
            if (expectedValue == null)
            {
                Assert.IsNotNull(actualValue, message);
                return;
            }

            Assert.AreNotEqual(expectedValue, actualValue, message);
        }

        [DebuggerStepThrough]
        public static void ShouldNotContain(this IEnumerable<object> items, object expectedItem)
        {
            ShouldEqual(items, expectedItem, string.Empty);
        }

        [DebuggerStepThrough]
        public static void ShouldNotContain(this IEnumerable<object> items, object expectedItem, string message)
        {
            if (expectedItem == null)
            {
                Assert.IsNull(items, message);
                return;
            }

            var contains = items.Contains(expectedItem);
            if (!contains)
                return;

            if (string.IsNullOrWhiteSpace(message))
                Assert.Fail(string.Format("Items should not contain: {0}", expectedItem));
            else
                Assert.Fail(message);
        }

        [DebuggerStepThrough]
        public static void ShouldContain(this IEnumerable<object> items, object expectedItem)
        {
            ShouldEqual(items, expectedItem, string.Empty);
        }

        [DebuggerStepThrough]
        public static void ShouldContain(this IEnumerable<object> items, object expectedItem, string message)
        {
            if (expectedItem == null)
            {
                Assert.IsNull(items, message);
                return;
            }

            Assert.Contains(expectedItem, items.ToList());
        }

        [DebuggerStepThrough]
        public static TSource AtIndex<TSource>(this IEnumerable<TSource> source, int index)
        {
            return source.Skip(index).First();
        }

        [DebuggerStepThrough]
        public static TSource Second<TSource>(this IEnumerable<TSource> source)
        {
            return source.Skip(1).First();
        }

        [DebuggerStepThrough]
        public static TSource Third<TSource>(this IEnumerable<TSource> source)
        {
            return source.Skip(2).First();
        }

        [DebuggerStepThrough]
        public static TSource Fourth<TSource>(this IEnumerable<TSource> source)
        {
            return source.Skip(3).First();
        }

        [DebuggerStepThrough]
        public static TSource Fifth<TSource>(this IEnumerable<TSource> source)
        {
            return source.Skip(4).First();
        }

        [DebuggerStepThrough]
        public static TSource Sixeth<TSource>(this IEnumerable<TSource> source)
        {
            return source.Skip(5).First();
        }

        [DebuggerStepThrough]
        public static TSource Seventh<TSource>(this IEnumerable<TSource> source)
        {
            return source.Skip(6).First();
        }

        [DebuggerStepThrough]
        public static TSource Eighth<TSource>(this IEnumerable<TSource> source)
        {
            return source.Skip(7).First();
        }

        [DebuggerStepThrough]
        public static TSource Nineth<TSource>(this IEnumerable<TSource> source)
        {
            return source.Skip(8).First();
        }
    }
}
