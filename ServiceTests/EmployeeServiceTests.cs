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
using Microsoft.EntityFrameworkCore;

namespace ServiceTests
{
    public class EmployeeServiceTests
    {
        [Fact]
        public async Task AddNewEmployeeLimit18YearsExceptionTest()
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
                await employeeService.AddNewEmployeeAsync(employee);
            }
            catch (Limit18YearsException ex)
            {
                Assert.Equal(typeof(Limit18YearsException), ex.GetType());
                Assert.Equal("Сотрудник не может быть моложе 18 лет", ex.Message);
            }

        }

        [Fact]
        public async Task AddNewEmployeeNoPassportDataExceptionTest()
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
                await employeeService.AddNewEmployeeAsync(employee);
            }
            catch (NoPassportDataException ex)
            {
                Assert.Equal(typeof(NoPassportDataException), ex.GetType());
                Assert.Equal("Паспортные данные обязательно должны быть введены", ex.Message);
            }
        }

        [Fact]
        public async Task DeleteEmployee_KeyNotFoundException_EmployeeRemoved_Test()
        {
            // Arrange
            EmployeeService employeeService = new EmployeeService();
            TestDataGenerator testDataGenerator = new TestDataGenerator();

            Employee existsEmployee = testDataGenerator.GetFakeDataEmployee().Generate();

            Employee noExistsEmployee = testDataGenerator.GetFakeDataEmployee().Generate();

            // Act/Assert
            try
            {
                await employeeService.AddNewEmployeeAsync(existsEmployee);
                await Assert.ThrowsAsync<KeyNotFoundException>(() => employeeService.DeleteEmployeeAsync(noExistsEmployee));

                await employeeService.DeleteEmployeeAsync(existsEmployee);
                Assert.Null(await employeeService.bankContext.Employees.FirstOrDefaultAsync(p => p.Id == existsEmployee.Id));
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }

        }

        [Fact]
        public async Task UpdateEmployee_KeyNotFoundException_EmployeeUpdated_Test()
        {
            // Arrange
            EmployeeService employeeService = new EmployeeService();
            TestDataGenerator testDataGenerator = new TestDataGenerator();

            Employee existsEmployee = testDataGenerator.GetFakeDataEmployee().Generate();

            Employee noExistsEmployee = testDataGenerator.GetFakeDataEmployee().Generate();

            // Act/Assert
            try
            {
                await employeeService.AddNewEmployeeAsync(existsEmployee);
                await employeeService.UpdateEmployeeAsync(existsEmployee);

                await Assert.ThrowsAsync<KeyNotFoundException>(() => employeeService.UpdateEmployeeAsync(noExistsEmployee));
                
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }

        }

        [Fact]
        public async Task GetEmployees_ByTheSpecifiedFilter_Test()
        {
            // Arrange
            EmployeeService employeeService = new EmployeeService();
            TestDataGenerator testDataGenerator = new TestDataGenerator();
            EmployeeFilter employeeFilter = new EmployeeFilter();
            Employee employee = new Employee();

            for (int i = 0; i < 10; i++)
            {
                employee = testDataGenerator.GetFakeDataEmployee().Generate();
                await employeeService.AddNewEmployeeAsync(employee);
            };

            int page = 1;
            int limit = 10;

            // Act/Assert
            employeeFilter.FirstName = employee.FirstName;
            employeeFilter.LastName = employee.LastName;

            Assert.NotNull(await employeeService.GetEmployeesAsync(employeeFilter, page, limit));

            employeeFilter.FirstName = null;
            employeeFilter.LastName = null;
         
            employeeFilter.StartDate = await employeeService.bankContext.Clients.MinAsync(p => p.DateOfBirth);

            Assert.NotNull(await employeeService.GetEmployeesAsync(employeeFilter, page, limit));
            employeeFilter.StartDate = default;

            employeeFilter.EndDate = await employeeService.bankContext.Clients.MaxAsync(p => p.DateOfBirth);

            Assert.NotNull(await employeeService.GetEmployeesAsync(employeeFilter, page, limit));
            employeeFilter.StartDate = default;

        }
    }

}
