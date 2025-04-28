using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDataAccess.Model
{
    internal class Employee
    {
        public Employee()
        {
        }

        public Employee(string name)
        {
            Name = name;
        }

        public Employee(int employeeId, string name, int YearsEmployed)
        {
            EmployeeId = employeeId;
            Name = name;
            this.YearsEmployeed = YearsEmployed;
        }

        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public int YearsEmployeed { get; set; }
        public int CompanyId { get; set; }

    }
}
