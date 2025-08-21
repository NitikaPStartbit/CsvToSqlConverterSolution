# 📂 CSV to SQL Dynamic Table Importer

This package provides an ASP.NET Core Web API that allows you to upload a CSV file, dynamically create a SQL Server table based on the CSV header, and insert all CSV rows into the newly created table.

---

## 🚀 Features
- Upload a CSV file
- Automatically creates a SQL table based on CSV headers
- Inserts all CSV rows into the created table
- Supports SQL Server via 'Microsoft.Data.SqlClient'
- Uses 'CsvHelper' for parsing CSV files

---

## 🛠️ Technologies
- ASP.NET Core Web API
- CsvHelper
- Microsoft.Data.SqlClient

- ## Installation
```bash
dotnet add package CsvToDbDynamicTableConverter --version 1.0.0

