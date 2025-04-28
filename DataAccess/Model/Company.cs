using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDataAccess.Model
{
    internal class Company
    {
        private List<Employee> employees;
        private String companyName;

        public List<Employee> Employees { get { return employees; } }
        public int CompanyId { get; set; }
        public String CompanyName
        {
            get
            {
                return companyName;
            }
            set
            {
                companyName = value;
            }
        }

    }
}
