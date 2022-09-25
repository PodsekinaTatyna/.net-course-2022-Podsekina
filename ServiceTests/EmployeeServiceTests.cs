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
            EmployeeService employeeService = new EmployeeService(new EmployeeStorage());
            EmployeeDb employee = new EmployeeDb()
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
            EmployeeService employeeService = new EmployeeService(new EmployeeStorage());
            EmployeeDb employee = new EmployeeDb()
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
            EmployeeStorage employeeStorage = new EmployeeStorage();
            EmployeeService employeeService = new EmployeeService(employeeStorage);
            TestDataGenerator testDataGenerator = new TestDataGenerator();

            var existsEmployee = testDataGenerator.GetFakeDataEmployee().Generate();

            var noExistsEmployee = testDataGenerator.GetFakeDataEmployee().Generate();

            // Act/Assert
            try
            {
                employeeService.AddNewEmployee(existsEmployee);
                Assert.Throws<KeyNotFoundException>(() => employeeService.DeleteEmployee(noExistsEmployee));

                employeeService.DeleteEmployee(existsEmployee);
                Assert.DoesNotContain(existsEmployee, employeeStorage.Data.Employees);
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
            EmployeeStorage employeeStorage = new EmployeeStorage();
            EmployeeService employeeService = new EmployeeService(employeeStorage);
            TestDataGenerator testDataGenerator = new TestDataGenerator();

            var existsEmployee = testDataGenerator.GetFakeDataEmployee().Generate();

            var noExistsEmployee = testDataGenerator.GetFakeDataEmployee().Generate();

            // Act/Assert
            try
            {
                employeeService.AddNewEmployee(existsEmployee);
                employeeService.UpdateEmployee(existsEmployee);

                Assert.Throws<KeyNotFoundException>(() => employeeService.UpdateEmployee(noExistsEmployee));
                Assert.Same(existsEmployee, employeeStorage.Data.Employees.First(p => p.PassportID == existsEmployee.PassportID));
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }

        }

        [Fact]
        public void GetEmployees_ByTheSpecifiedFilter_Test()
        {
            // Arrange
            EmployeeStorage employeeStorage = new EmployeeStorage();
            EmployeeService employeeService = new EmployeeService(employeeStorage);
            TestDataGenerator testDataGenerator = new TestDataGenerator();
            EmployeeFilter employeeFilter = new EmployeeFilter();

            for (int i = 0; i < 10; i++)
            {
                employeeService.AddNewEmployee(testDataGenerator.GetFakeDataEmployee().Generate());
            }

            EmployeeDb client = employeeStorage.Data.Employees.First();

            // Act/Assert
            employeeFilter.FirstName = client.FirstName;
            employeeFilter.LastName = client.LastName;

            Assert.NotNull(employeeService.GetEmployees(employeeFilter));

            employeeFilter.FirstName = null;
            employeeFilter.LastName = null;

            employeeFilter.StartDate = employeeStorage.Data.Employees.Min(p => p.DateOfBirth);

            Assert.NotNull(employeeService.GetEmployees(employeeFilter));
            employeeFilter.StartDate = default;

            employeeFilter.EndDate = employeeStorage.Data.Employees.Max(p => p.DateOfBirth);

            Assert.NotNull(employeeService.GetEmployees(employeeFilter));
            employeeFilter.StartDate = default;

        }
    }

}
