using Microsoft.EntityFrameworkCore;
using Models;
using ModelsDb;
using Services.Exceptions;
using Services.Filters;
using Services.Storages;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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

        public async Task<Employee> GetEmployeeAsync(Guid id)
        {

            var employeeDb = await bankContext.Employees.FirstOrDefaultAsync(p => p.Id == id);

            return new Employee
            {
                Id = employeeDb.Id,
                FirstName = employeeDb.FirstName,
                LastName = employeeDb.LastName,
                PassportID = employeeDb.PassportID,
                DateOfBirth = employeeDb.DateOfBirth,
                Bonus = employeeDb.Bonus,
                Salary = employeeDb.Salary,
                Contract = employeeDb.Contract,
            };
        }

        public async Task AddNewEmployeeAsync(Employee employee)
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
            await bankContext.Employees.AddAsync(employeeDb);
            await bankContext.SaveChangesAsync();

        }

        public async Task DeleteEmployeeAsync(Employee employee)
        {
            var employeeDb = bankContext.Employees.FirstOrDefault(p => p.Id == employee.Id);

            if (employeeDb == null)
                throw new KeyNotFoundException("В базе нет такого сотрудника");

            bankContext.Employees.Remove(employeeDb);
            await bankContext.SaveChangesAsync();

        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            var employeeDb = bankContext.Employees.FirstOrDefault(p => p.Id == employee.Id);

            if (employeeDb == null)
                throw new KeyNotFoundException("В базе нет такого сотрудника");

            employeeDb.Id = employee.Id;
            employeeDb.FirstName = employee.FirstName;
            employeeDb.LastName = employee.LastName;
            employeeDb.PassportID = employee.PassportID;
            employeeDb.DateOfBirth = employee.DateOfBirth;
            employeeDb.Bonus = employee.Bonus;
            employeeDb.Salary = employee.Salary;
            employeeDb.Contract = employee.Contract;

            bankContext.Employees.Update(employeeDb);
            await bankContext.SaveChangesAsync();

        }

        public async Task<List<Employee>> GetEmployeesAsync(EmployeeFilter employeeFilter, int page, int limit)
        {
            var query = bankContext.Employees.Select(p => p).AsNoTracking();

            if (employeeFilter.FirstName != null)
                query = query.Where(p => p.FirstName == employeeFilter.FirstName);

            if (employeeFilter.LastName != null)
                query = query.Where(p => p.LastName == employeeFilter.LastName);

            if (employeeFilter.StartDate != default)
                query = query.Where(p => p.DateOfBirth == employeeFilter.StartDate);

            if (employeeFilter.EndDate != default)
                query = query.Where(p => p.DateOfBirth == employeeFilter.EndDate);

            var filteredList = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();

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
