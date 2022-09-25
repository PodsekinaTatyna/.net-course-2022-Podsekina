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

namespace Services
{
    public class EmployeeService
    {
        private EmployeeStorage _employeeStorage { get; set; }
        public EmployeeService(EmployeeStorage employeeStorage)
        {
            _employeeStorage = employeeStorage;
        }

        public void AddNewEmployee(EmployeeDb employee)
        {
            if ((DateTime.Now - employee.DateOfBirth).Days < 18)
                throw new Limit18YearsException("Сотрудник не может быть моложе 18 лет");

            if (employee.PassportID == 0)
                throw new NoPassportDataException("Паспортные данные обязательно должны быть введены");

            _employeeStorage.Add(employee);
        }

        public void DeleteEmployee(EmployeeDb employee)
        {
            if (!_employeeStorage.Data.Employees.Contains(employee))
                throw new KeyNotFoundException("В базе нет такого сотрудника");

            _employeeStorage.Delete(employee);
        }

        public void UpdateEmployee(EmployeeDb employee)
        {
            if (!_employeeStorage.Data.Employees.Contains(employee))
                throw new KeyNotFoundException("В базе нет такого сотрудника");
    
            _employeeStorage.Update(employee);
        }

        public List<EmployeeDb> GetEmployees(EmployeeFilter employeeFilter)
        {
            var filteredDictionary = _employeeStorage.Data.Employees.Select(p => p);

            if (employeeFilter.FirstName != null)
                filteredDictionary = filteredDictionary.Where(p => p.FirstName == employeeFilter.FirstName);

            if (employeeFilter.LastName != null)
                filteredDictionary = filteredDictionary.Where(p => p.LastName == employeeFilter.LastName);

            if (employeeFilter.StartDate != default)
                filteredDictionary = filteredDictionary.Where(p => p.DateOfBirth == employeeFilter.StartDate);

            if (employeeFilter.EndDate != default)
                filteredDictionary = filteredDictionary.Where(p => p.DateOfBirth == employeeFilter.EndDate);

            return filteredDictionary.ToList();

        }

    }
}
