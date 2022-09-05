using Models;
using Services.Exceptions;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ServiceTests
{
    public class EmployeeServiceTests
    {
        [Fact]
        public void AddNewEmployeeLimit18YearsExceptionTest()
        {
            EmployeeService employeeService = new EmployeeService();
            Employee employee = new Employee()
            {
                FirstName = "Игорь",
                LastName = "Петоров",
                PassportID = 12345,
                DateOfBirth = new DateTime(2007, 2, 2),
                Contract = "Контракт",
                Salary = 134,
            };

            try
            {
                employeeService.AddNewEmployee(employee);
            }
            catch (Limit18YearsException ex)
            {
                Assert.Equal(typeof(Limit18YearsException), ex.GetType());
                Assert.Equal("Сотрудник не может быть моложе 18 лет", ex.Message);
            }

        }

        [Fact]
        public void AddNewEmployeeNoPassportDataExceptionTest()
        {
            EmployeeService employeeService = new EmployeeService();
            Employee employee = new Employee()
            {
                FirstName = "Игорь",
                LastName = "Петоров",
                PassportID = 0,
                DateOfBirth = new DateTime(2000, 2, 2),
                Contract = "Контракт",
                Salary = 134,
            };

            try
            {
                employeeService.AddNewEmployee(employee);
            }
            catch (NoPassportDataException ex)
            {
                Assert.Equal(typeof(NoPassportDataException), ex.GetType());
                Assert.Equal("Паспортные данные обязательно должны быть введены", ex.Message);
            }
        }
    }
}
