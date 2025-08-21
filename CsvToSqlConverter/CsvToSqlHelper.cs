using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToSqlConverter
{
    public class CsvToSqlHelper
    {
        public static void ImportCsvToSql(string csvFilePath, string connectionString, string tableName)
        {
            if (!File.Exists(csvFilePath))
                throw new FileNotFoundException("CSV file not found.", csvFilePath);

            using var connection = new SqlConnection(connectionString);
            connection.Open();

            using var reader = new StreamReader(csvFilePath);
            string? headerLine = reader.ReadLine();

            if (string.IsNullOrWhiteSpace(headerLine))
                throw new InvalidOperationException("CSV file has no header.");

            string[] headers = headerLine.Split(',');

            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] values = line.Split(',');

                string insertQuery = BuildInsertQuery(tableName, headers, values);
                using var cmd = new SqlCommand(insertQuery, connection);
                cmd.ExecuteNonQuery();
            }
        }

        private static string BuildInsertQuery(string tableName, string[] headers, string[] values)
        {
            for (int i = 0; i < values.Length; i++)
                values[i] = $"'{values[i].Replace("'", "''")}'"; // escape quotes

            string columns = string.Join(",", headers);
            string rowValues = string.Join(",", values);

            return $"INSERT INTO {tableName} ({columns}) VALUES ({rowValues});";
        }
    }
}
