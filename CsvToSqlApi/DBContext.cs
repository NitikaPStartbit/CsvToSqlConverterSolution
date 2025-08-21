using Microsoft.EntityFrameworkCore;
using CsvToSqlApi.Models;

namespace CsvToSqlApi.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {}

        public DbSet<Employee> Employees { get; set; }
    }
}

