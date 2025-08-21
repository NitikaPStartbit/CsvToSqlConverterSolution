using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace CsvToDbDynamicTableConverter
{
    public class CsvToSqlDynamicTableHelper
    {
        public static void ImportCsvToSqlAndCreateTable(string filePath, string connectionString, string tableName)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            using var dr = new CsvDataReader(csv);

            DataTable dt = new DataTable();
            dt.Load(dr);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // 🔹 Drop table if exists
                string dropQuery = $@"IF OBJECT_ID('{tableName}', 'U') IS NOT NULL DROP TABLE {tableName};";
                using (SqlCommand dropCmd = new SqlCommand(dropQuery, conn))
                {
                    dropCmd.ExecuteNonQuery();
                }

                // Build CREATE TABLE dynamically from DataTable schema
                string createTableSql = $"CREATE TABLE {tableName} (";
                foreach (DataColumn column in dt.Columns)
                {
                    createTableSql += $"[{column.ColumnName}] NVARCHAR(MAX),";
                }
                createTableSql = createTableSql.TrimEnd(',') + ")";

                using (SqlCommand createCmd = new SqlCommand(createTableSql, conn))
                {
                    createCmd.ExecuteNonQuery();
                }

                // Bulk insert data
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.WriteToServer(dt);
                }
            }
        }
    }
}
