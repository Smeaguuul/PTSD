using EmployeeDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDataAccess.Mappers
{
    internal class EmployeeMapper
    {
        public static DTO.Model.Employee Map(Employee employee)
        {
            return new DTO.Model.Employee(employee.EmployeeId, employee.Name, employee.YearsEmployeed);
        }
        public static Employee Map(DTO.Model.Employee employee)
        {
            return new Employee(employee.EmployeeId, employee.Name, employee.YearsEmployed);
        }

        internal static void Update(DTO.Model.Employee employee, Employee dataemp)
        {
            dataemp.Name= employee.Name;
            dataemp.YearsEmployeed = employee.YearsEmployed;
        }

        internal static DTO.Model.CompanyDetail MapDetail(Company company)
        {
            DTO.Model.CompanyDetail retur = new DTO.Model.CompanyDetail();
            retur.CompanyName = company.CompanyName;
            retur.Employees = EmployeeMapper.Map(company.Employees);
            return retur;
        }

        private static List<DTO.Model.Employee> Map(List<Employee> employees)
        {
            List<DTO.Model.Employee> retur = new List<DTO.Model.Employee>();
            foreach (Employee employee in employees)
            {
                retur.Add(EmployeeMapper.Map(employee));
            }
            return retur;
        }

        public static DTO.Model.CompanyOverview Map(Company company)
        {
            return new DTO.Model.CompanyOverview(company.CompanyId, company.CompanyName);
        }

    }
}
