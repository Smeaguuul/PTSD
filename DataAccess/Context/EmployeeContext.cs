using EmployeeDataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EmployeeDataAccess.Context
{
    internal class EmployeeContext : DbContext
    {
        public EmployeeContext()
        {
            bool created = Database.EnsureCreated();
            if (created)
            {
                Debug.WriteLine("Database created");
            }

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=EAA-SH-KLBO-KU\\SQLEXPRESS;Initial Catalog=Employees;Integrated Security = SSPI; TrustServerCertificate=true");
            optionsBuilder.LogTo(message => Debug.WriteLine(message));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(new Employee[] {
                new Employee{EmployeeId=-1,Name="Olsen"},
                new Employee{EmployeeId=-2,Name="Hansen"},
                new Employee{EmployeeId=-3,Name="Jensen"}
            });
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Company> Companys { get; set; }
    }
}
