# 📂 CSV to SQL Dynamic Table Importer

[![NuGet](https://img.shields.io/nuget/v/CsvToDbDynamicTableConverter.svg)](https://www.nuget.org/packages/CsvToDbDynamicTableConverter/)  
[![Downloads](https://img.shields.io/nuget/dt/CsvToDbDynamicTableConverter.svg)](https://www.nuget.org/packages/CsvToDbDynamicTableConverter/)

This package provides an ASP.NET Core Web API that allows you to upload a CSV file, dynamically create a SQL Server table based on the CSV header, and insert all CSV rows into the newly created table.

---

## ✨ Features
- ✅ Automatically reads CSV headers
- ✅ Dynamically creates a SQL Server table 
- ✅ Inserts CSV rows into the created table  
- ✅ Supports ASP.NET Core API integration 
- ✅ Powered by CsvHelper, Microsoft.Data.SqlClient

---

## 🚀 Installation
Install the NuGet package:

```bash
dotnet add package CsvToDbDynamicTableConverter --version 1.0.0
```

---

## Usage Example
 
```csharp
using CsvToSqlDynamicTableConverter;

CsvToSqlDynamicTableHelper.ImportCsvToSqlAndCreateTable(
    @"C:\files\employees.csv",
    "Server=localhost;Database=MyDB;Trusted_Connection=True;TrustServerCertificate=True;",
    "Employees"
);
```

---

## Sample CSV

```csharp
Id,FirstName,LastName,Email,Department
1,John,Doe,john.doe@example.com,HR
2,Jane,Smith,jane.smith@example.com,IT
3,Bob,Johnson,bob.johnson@example.com,Finance
```

---

## Usage Example (ASP.NET Core API)
 
```csharp
public IActionResult ImportCsvDynamicTableCreation(IFormFile file, string TableName)
{
    if (file == null || file.Length == 0)
        return BadRequest("Please upload a valid CSV file.");

    string tempPath = Path.GetTempFileName();

    using (var stream = new FileStream(tempPath, FileMode.Create))
    {
        file.CopyTo(stream);
    }

    string connString = _config.GetConnectionString("DefaultConnection");

    // Table will be created dynamically from CSV header
    CsvToSqlDynamicTableHelper.ImportCsvToSqlAndCreateTable(
        tempPath,
        connString,
        TableName
    );

    return Ok("CSV uploaded and table created successfully!");
}
```

---

## Notes
- All columns are created as NVARCHAR(MAX).  
- Works with SQL Server only.  

 
---
 
## License
MIT License


