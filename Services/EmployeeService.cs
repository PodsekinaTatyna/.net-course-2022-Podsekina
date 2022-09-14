using Models;
using Services.Exceptions;
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

        public void AddNewEmployee(Employee employee)
        {
            if (DateTime.Now.Year - employee.DateOfBirth.Year < 18)
                throw new Limit18YearsException("Сотрудник не может быть моложе 18 лет");

            if (employee.PassportID == 0)
                throw new NoPassportDataException("Паспортные данные обязательно должны быть введены");

            _employeeStorage.Add(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            if (!_employeeStorage._employeeList.Contains(employee))
                throw new KeyNotFoundException("В базе нет такого сотрудника");

            _employeeStorage.Delete(employee);
        }

        public void UpdateEmployee(Employee employee)
        {
            if (!_employeeStorage._employeeList.Contains(employee))
                throw new KeyNotFoundException("В базе нет такого сотрудника");
    
            _employeeStorage.Update(employee);
        }

    }
}
