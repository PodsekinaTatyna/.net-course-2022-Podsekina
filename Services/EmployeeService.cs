using Models;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class EmployeeService
    {
        private List<Employee> listEmployee = new List<Employee>();

        public void AddNewEmployee(Employee employee)
        {
            if (DateTime.Now.Year - employee.DateOfBirth.Year < 18)
                throw new Limit18YearsException("Сотрудник не может быть моложе 18 лет");

            if (employee.PassportID == 0)
                throw new NoPassportDataException("Паспортные данные обязательно должны быть введены");

            listEmployee.Add(employee);
        }
    }
}
