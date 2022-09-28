using Models;
using Services.Exceptions;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Services.Storages;
using ModelsDb;
using Services.Filters;

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

        [Fact]
        public void DeleteEmployee_KeyNotFoundException_EmployeeRemoved_Test()
        {
            // Arrange
            EmployeeService employeeService = new EmployeeService();
            TestDataGenerator testDataGenerator = new TestDataGenerator();

            Employee existsEmployee = testDataGenerator.GetFakeDataEmployee().Generate();

            Employee noExistsEmployee = testDataGenerator.GetFakeDataEmployee().Generate();

            // Act/Assert
            try
            {
                employeeService.AddNewEmployee(existsEmployee);
                Assert.Throws<KeyNotFoundException>(() => employeeService.DeleteEmployee(noExistsEmployee));

                employeeService.DeleteEmployee(existsEmployee);
                Assert.Null(employeeService.bankContext.Employees.FirstOrDefault(p => p.Id == existsEmployee.Id));
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }

        }

        [Fact]
        public void UpdateEmployee_KeyNotFoundException_EmployeeUpdated_Test()
        {
            // Arrange
            EmployeeService employeeService = new EmployeeService();
            TestDataGenerator testDataGenerator = new TestDataGenerator();

            Employee existsEmployee = testDataGenerator.GetFakeDataEmployee().Generate();

            Employee noExistsEmployee = testDataGenerator.GetFakeDataEmployee().Generate();

            // Act/Assert
            try
            {
                employeeService.AddNewEmployee(existsEmployee);
                employeeService.UpdateEmployee(existsEmployee);

                Assert.Throws<KeyNotFoundException>(() => employeeService.UpdateEmployee(noExistsEmployee));
                
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }

        }

        [Fact]
        public void GetClients_ByTheSpecifiedFilter_Test()
        {
            // Arrange
            EmployeeService employeeService = new EmployeeService();
            TestDataGenerator testDataGenerator = new TestDataGenerator();
            EmployeeFilter employeeFilter = new EmployeeFilter();
            Employee employee = new Employee();

            for (int i = 0; i < 10; i++)
            {
                employee = testDataGenerator.GetFakeDataEmployee().Generate();
                employeeService.AddNewEmployee(employee);
            };

            int page = 1;
            int limit = 10;

            // Act/Assert
            employeeFilter.FirstName = employee.FirstName;
            employeeFilter.LastName = employee.LastName;

            Assert.NotNull(employeeService.GetEmployees(employeeFilter, page, limit));

            employeeFilter.FirstName = null;
            employeeFilter.LastName = null;
         
            employeeFilter.StartDate = employeeService.bankContext.Clients.Min(p => p.DateOfBirth);

            Assert.NotNull(employeeService.GetEmployees(employeeFilter, page, limit));
            employeeFilter.StartDate = default;

            employeeFilter.EndDate = employeeService.bankContext.Clients.Max(p => p.DateOfBirth);

            Assert.NotNull(employeeService.GetEmployees(employeeFilter, page, limit));
            employeeFilter.StartDate = default;

        }
    }

}
