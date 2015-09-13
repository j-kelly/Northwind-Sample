namespace Northwind.Domain.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Builders;
    using System.Data.Entity.Migrations.Model;
    using System.IO;
    using System.Runtime.CompilerServices;

    public abstract class DbMigrationExtender : DbMigration
    {
        private static readonly string DropStatsSqlFormat = DatabaseMigrationHelper.GetSharedEmbeddedResource("DropStatisticInsideMigration.sql");

        public void DropStatistics(string table, string column)
        {
            // seems weird that we can't call / use : Sql("DROP STATISTIC ...) here, but we can't (context issue)!
            //var dropStatsSql = string.Format(DropStatsSqlFormat, table, column);
            //Sql(dropStatsSql);
        }

        public void DropColumn(string table, string column)
        {
            table = table.ToUpper().Replace("DBO.", string.Empty);
            DropStatistics(table, column);
            base.DropColumn("dbo." + table, column);
        }

        public void AlterColumn(string table, string column, int newLength)
        {
            table = table.ToUpper().Replace("DBO.", string.Empty);
            DropStatistics(table, column);
            base.AlterColumn("dbo." + table, column, c => c.String(maxLength: newLength));
        }

        public void AlterColumn(string table, string column, Func<ColumnBuilder, ColumnModel> columnAction)
        {
            table = table.ToUpper().Replace("DBO.", string.Empty);
            DropStatistics(table, column);
            base.AlterColumn("dbo." + table, column, columnAction);
        }

        public void SqlScript(string resourceName, [CallerFilePath] string sourceFile = "")
        {
            var releaseNumber = Path.GetFileName(Path.GetDirectoryName(sourceFile));
            var sql = DatabaseMigrationHelper.GetEmbeddedResource(int.Parse(releaseNumber), resourceName);
            Sql(sql);
        }

        public void DropIndexWithCheck(string tableName, string indexName)
        {
            var sql = string.Format(
                        @"IF EXISTS (SELECT name FROM sys.indexes WHERE name='{0}' AND object_id = OBJECT_ID('{1}')) DROP INDEX {1} ON {0};",
                        tableName,
                        indexName);

            Sql(sql);
        }
    }
}
