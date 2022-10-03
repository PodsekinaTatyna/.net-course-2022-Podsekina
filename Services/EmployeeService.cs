using Models;
using ModelsDb;
using Services.Exceptions;
using Services.Filters;
using Services.Storages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Services
{
    public class EmployeeService
    {
        public BankContext bankContext { get; set; }

        public EmployeeService()
        {
            bankContext = new BankContext();
        }

        public void AddNewEmployee(Employee employee)
        {
            var employeeDb = new EmployeeDb
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                PassportID = employee.PassportID,
                DateOfBirth = employee.DateOfBirth,
                Bonus = employee.Bonus,
                Salary = employee.Salary,
                Contract = employee.Contract
            }; 

            if ((DateTime.Now - employee.DateOfBirth).Days < 18)
                throw new Limit18YearsException("Сотрудник не может быть моложе 18 лет");

            if (employee.PassportID == 0)
                throw new NoPassportDataException("Паспортные данные обязательно должны быть введены");
            bankContext.Employees.Add(employeeDb);
            bankContext.SaveChanges();

        }

        public void DeleteEmployee(Employee employee)
        {
            var employeeDb = bankContext.Employees.FirstOrDefault(p => p.Id == employee.Id);

            if (employeeDb == null)
                throw new KeyNotFoundException("В базе нет такого сотрудника");

            bankContext.Employees.Remove(employeeDb);
            bankContext.SaveChanges();

        }

        public void UpdateEmployee(Employee employee)
        {
            var employeeDb = bankContext.Employees.FirstOrDefault(p => p.Id == employee.Id);

            if (employeeDb == null)
                throw new KeyNotFoundException("В базе нет такого сотрудника");

            bankContext.Employees.Update(employeeDb);
            bankContext.SaveChanges();

        }

        public List<Employee> GetEmployees(EmployeeFilter employeeFilter, int page, int limit)
        {
            var query = bankContext.Employees.Select(p => p);

            if (employeeFilter.FirstName != null)
                query = query.Where(p => p.FirstName == employeeFilter.FirstName);

            if (employeeFilter.LastName != null)
                query = query.Where(p => p.LastName == employeeFilter.LastName);

            if (employeeFilter.StartDate != default)
                query = query.Where(p => p.DateOfBirth == employeeFilter.StartDate);

            if (employeeFilter.EndDate != default)
                query = query.Where(p => p.DateOfBirth == employeeFilter.EndDate);

            var filteredList = query.Skip((page - 1) * limit).Take(limit).ToList();

            var employeetList = new List<Employee>();

            foreach (EmployeeDb employeeDb in filteredList)
            {
                employeetList.Add(new Employee
                {
                    Id = employeeDb.Id,
                    FirstName = employeeDb.FirstName,
                    LastName = employeeDb.LastName,
                    PassportID = employeeDb.PassportID,
                    DateOfBirth = employeeDb.DateOfBirth,
                    Bonus = employeeDb.Bonus,
                    Salary = employeeDb.Salary,
                    Contract = employeeDb.Contract
                });
            }

            return employeetList;

        }

    }
}
