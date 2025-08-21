using CsvToDbDynamicTableConverter;
using CsvToSqlApi.Data;
using CsvToSqlApi.Models;
using CsvToSqlConverter;
using Microsoft.AspNetCore.Mvc;

namespace CsvToSqlApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IConfiguration _config;

        public EmployeesController(DBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // Get all employees
        [HttpGet]
        public IActionResult GetAll()
        {
            var employees = _context.Employees.ToList();
            return Ok(employees);
        }

        // Add a single employee manually
        [HttpPost]
        public IActionResult Add(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return Ok(employee);
        }

        // Import CSV to DB
        [HttpPost("import")]
        public async Task<IActionResult> ImportCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Please upload a valid CSV file.");

            // Save uploaded file to a temporary location
            var tempPath = Path.GetTempFileName();
            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string connString = _config.GetConnectionString("DefaultConnection");

            // Use CsvToSqlHelper to import
            CsvToSqlHelper.ImportCsvToSql(
                tempPath,        
                connString,
                "Employees"
            );

            // Delete temp file after processing
            System.IO.File.Delete(tempPath);

            return Ok("CSV imported successfully!");
        }

        // Import CSV to DB - Dynamic Table Creation
        [HttpPost("CsvImportDynamicTable")]
        [Consumes("multipart/form-data")]
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

    }
}
