using DTO.Model;
using EmployeeDataAccess.Context;
using EmployeeDataAccess.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace EmployeeDataAccess.Repositories
{
    public class EmployeeRepository
    {
        public static Employee getEmployee(int id)
        {
            using (EmployeeContext context = new EmployeeContext())
            {
                //Maybe throw exception if not found
                return EmployeeMapper.Map(context.Employees.Where( e => e.EmployeeId== id).First());
            }
        }
        public static void AddEmployee(DTO.Model.Employee employee)
        {
            using (EmployeeContext context = new EmployeeContext())
            {
                EmployeeDataAccess.Model.Employee emp = EmployeeMapper.Map(employee);
                emp.CompanyId = 1;
                context.Employees.Add(emp);
                context.SaveChanges();
            }
        }

        public static void EditEmployee(Employee employee)
        {
            using (EmployeeContext context = new EmployeeContext())
            {
                //Virker ikke:
                //Model.Employee dataemp = EmployeeMapper.Map(employee);
                //Virker:
                Model.Employee dataemp = context.Employees.Find(employee.EmployeeId);
                EmployeeMapper.Update(employee, dataemp);
                context.SaveChanges();

                //Undgå at læse entity først:
                //Model.Employee dataemp = EmployeeMapper.Map(employee);
                //sæt tilstanden til modified:
                //context.Update(dataemp);
                //context.SaveChanges();
                //eller:
                //context.Attach(dataemp);
                //context.Entry(dataemp).State = EntityState.Modified;
                //context.SaveChanges();
                //eller hvis man kun vil opdatere en enkelt property:
                //context.Attach(dataemp);
                //context.Entry(dataemp).Property(p => p.Name).IsModified = true;
                //context.SaveChanges();
            }
        }

        public static List<CompanyOverview> AllCompanies()
        {
            List<CompanyOverview> retur = new List<CompanyOverview>();
            using (EmployeeContext context = new EmployeeContext())
            {
                foreach (Model.Company c in context.Companys)
                {
                    retur.Add(EmployeeMapper.Map(c));
                }
            }
            return retur;
        }
        public static CompanyDetail CompanyDetail(int CompanyId)
        {
            using (EmployeeContext context = new EmployeeContext())
            {
                IQueryable<Model.Company> company = context.Companys.Where(c => c.CompanyId == CompanyId).Include(c => c.Employees);
                if (company.Count() == 1)
                {
                    return EmployeeMapper.MapDetail(company.First());
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

        }

        public static void AddEmployeeToCompany(int EmployeeId, int CompanyId)
        {
            //using (EmployeeContext context = new EmployeeContext())
            //{
            //    Model.Company c = context.Companys.Where(c => c.CompanyId == CompanyId).Include(c => c.Employees).FirstOrDefault();
            //    if (c != null)
            //    {
            //        Model.Employee emp = context.Employees.Where(c => c.EmployeeId == EmployeeId).FirstOrDefault();
            //        c.Employees.Add(emp);
            //    }
            //    context.SaveChanges();
            //}
            using (EmployeeContext context = new EmployeeContext())
            {
                Model.Employee emp = context.Employees.Where(c => c.EmployeeId == EmployeeId).FirstOrDefault();
                emp.CompanyId = CompanyId;
                context.SaveChanges();
            }
        }
    }
}
