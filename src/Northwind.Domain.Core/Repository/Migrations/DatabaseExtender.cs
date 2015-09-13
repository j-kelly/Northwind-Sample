namespace Northwind.Domain.Core.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Northwind.Domain.Core.Entities;

    internal class DatabaseMigrationHelper
    {
        public static string DropConstraintSql(string tableName, string constraintName)
        {
            var format = @"IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='{1}')
                            ALTER TABLE dbo.{0} DROP CONSTRAINT {1}";

            var retVal = string.Format(format, tableName, constraintName);
            return retVal;
        }

        public static bool CheckIndexExists(NRepository_NorthwindContext context, string field, string tableName)
        {
            string checkSqlCommand = string.Format("SELECT name FROM sys.indexes WHERE name='IX_{0}' AND object_id = OBJECT_ID('{1}')", field, tableName);
            var checkResults = context.Database.SqlQuery<string>(checkSqlCommand);
            return checkResults.Any();
        }

        public static void CreateNonClusteredNonUniqueIndex(NRepository_NorthwindContext context, string field, string tableName)
        {
            if (!CheckIndexExists(context, field, tableName))
            {
                context.Database.ExecuteSqlCommand(string.Format("CREATE INDEX IX_{0} ON {1} ({0})", field, tableName));
            }
        }

        public static void CreateNonClusteredNonUniqueIndex(NRepository_NorthwindContext context, IList<string> fields, string tableName, string indexName)
        {
            if (CheckIndexExists(context, indexName, tableName))
            {
                return;
            }

            var sql = string.Format("CREATE NONCLUSTERED INDEX IX_{0} ON {1} (", indexName, tableName);
            for (var i = 0; i < fields.Count; i++)
            {
                sql = sql + fields[i];
                if (i < (fields.Count - 1))
                {
                    sql = sql + ", ";
                }
            }

            sql = sql + ")";

            context.Database.ExecuteSqlCommand(sql);
        }

        public static void CreateNonClusteredNonUniqueIndexWithIncludes(string field, string tableName, IList<string> includes, NRepository_NorthwindContext context)
        {
            if (CheckIndexExists(context, field, tableName))
            {
                return;
            }

            var sql = string.Format("CREATE NONCLUSTERED INDEX IX_{0} ON {1} ({2})", field, tableName, field);

            sql += " INCLUDE (";

            for (var i = 0; i < includes.Count; i++)
            {
                sql += includes[i];
                if (i < (includes.Count - 1))
                {
                    sql = sql + ", ";
                }
            }

            sql = sql + ") WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF)";

            context.Database.ExecuteSqlCommand(sql);
        }

        public static void CreateNonClusteredNonUniqueIndexWithFieldsAndIncludes(string indexName, string tableName, IList<string> fields, IList<string> includes, NRepository_NorthwindContext context)
        {
            if (CheckIndexExists(context, indexName, tableName))
            {
                return;
            }

            var sql = string.Format("CREATE NONCLUSTERED INDEX IX_{0} ON {1} (", indexName, tableName);

            for (var i = 0; i < fields.Count; i++)
            {
                sql = sql + fields[i];
                if (i < (fields.Count - 1))
                {
                    sql = sql + ", ";
                }
            }

            sql = sql + ")";

            sql += " INCLUDE (";

            for (var i = 0; i < includes.Count; i++)
            {
                sql += includes[i];
                if (i < (includes.Count - 1))
                {
                    sql = sql + ", ";
                }
            }

            sql = sql + ") WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF)";

            context.Database.ExecuteSqlCommand(sql);
        }

        public static void CreateUniqueConstraint(NRepository_NorthwindContext context, string tableName, string field)
        {
            var dropSql =
                string.Format(
                    "IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[UC_{0}_{1}]') AND type in (N'UQ')) "
                    + "ALTER TABLE [dbo].[{0}] DROP CONSTRAINT [UC_{0}_{1}]",
                    tableName,
                    field);

            context.Database.ExecuteSqlCommand(dropSql);

            var sql = string.Format("ALTER TABLE [dbo].[{0}] ADD CONSTRAINT [UC_{0}_{1}] UNIQUE ({1})", tableName, field);
            context.Database.ExecuteSqlCommand(sql);
        }

        public static void AddStoredProcedure(NRepository_NorthwindContext context, string resourceName)
        {
            var sqlScript = GetSharedEmbeddedResource(resourceName);
            context.Database.ExecuteSqlCommand(sqlScript);
        }

        public static bool CheckStoredProcedureExists(NRepository_NorthwindContext context, string procedureName)
        {
            var checkSqlCommand = string.Format("SELECT name FROM sys.objects WHERE type = 'P' AND name = '{0}'", procedureName);
            var checkResults = context.Database.SqlQuery<string>(checkSqlCommand);
            return checkResults.Any();
        }

        public static string GetSharedEmbeddedResource(string resourceName)
        {
            const string ResourcePath = "Northwind.Domain.Core.Migrations._SharedResources.";

            string fullPath = ResourcePath + resourceName;
            return GetEmbeddedResourceWithPath(fullPath);
        }

        public static string GetEmbeddedResourceWithPath(string resourceName)
        {
            var asm = typeof(DatabaseMigrationHelper).Assembly;
            var names = asm.GetManifestResourceNames();
            using (var resourceStream = asm.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    throw new ApplicationException("Unknown resource path: " + resourceName);
                }

                var sr = new StreamReader(resourceStream);
                return sr.ReadToEnd();
            }
        }

        public static string GetEmbeddedResource(int releaseNumber, string resourceName)
        {
            const string ResourcePath = "Northwind.Domain.Core.Migrations";

            string fullPath = string.Format("{0}._{1}.Scripts.{2}", ResourcePath, releaseNumber, resourceName);
            var asm = typeof(DatabaseMigrationHelper).Assembly;
            using (var resourceStream = asm.GetManifestResourceStream(fullPath))
            {
                if (resourceStream == null)
                {
                    throw new ApplicationException("Unknown resource path: " + fullPath);
                }

                var sr = new StreamReader(resourceStream);
                return sr.ReadToEnd();
            }
        }
    }
}
