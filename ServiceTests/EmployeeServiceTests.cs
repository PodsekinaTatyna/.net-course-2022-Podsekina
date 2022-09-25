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

namespace ServiceTests
{
    //public class EmployeeServiceTests
    //{
    //    [Fact]
    //    public void AddNewEmployeeLimit18YearsExceptionTest()
    //    {
    //        EmployeeService employeeService = new EmployeeService(new EmployeeStorage());
    //        Employee employee = new Employee()
    //        {
    //            FirstName = "Игорь",
    //            LastName = "Петоров",
    //            PassportID = 12345,
    //            DateOfBirth = new DateTime(2007, 2, 2),
    //            Contract = "Контракт",
    //            Salary = 134,
    //        };

    //        try
    //        {
    //            employeeService.AddNewEmployee(employee);
    //        }
    //        catch (Limit18YearsException ex)
    //        {
    //            Assert.Equal(typeof(Limit18YearsException), ex.GetType());
    //            Assert.Equal("Сотрудник не может быть моложе 18 лет", ex.Message);
    //        }

    //    }

    //    [Fact]
    //    public void AddNewEmployeeNoPassportDataExceptionTest()
    //    {
    //        EmployeeService employeeService = new EmployeeService(new EmployeeStorage());
    //        Employee employee = new Employee()
    //        {
    //            FirstName = "Игорь",
    //            LastName = "Петоров",
    //            PassportID = 0,
    //            DateOfBirth = new DateTime(2000, 2, 2),
    //            Contract = "Контракт",
    //            Salary = 134,
    //        };

    //        try
    //        {
    //            employeeService.AddNewEmployee(employee);
    //        }
    //        catch (NoPassportDataException ex)
    //        {
    //            Assert.Equal(typeof(NoPassportDataException), ex.GetType());
    //            Assert.Equal("Паспортные данные обязательно должны быть введены", ex.Message);
    //        }
    //    }

    //    [Fact]
    //    public void DeleteEmployee_KeyNotFoundException_EmployeeRemoved_Test()
    //    {
    //        // Arrange
    //        EmployeeStorage employeeStorage = new EmployeeStorage();
    //        EmployeeService employeeService = new EmployeeService(employeeStorage);

    //        Employee existsEmployee = new Employee()
    //        {
    //            FirstName = "Игорь",
    //            LastName = "Петоров",
    //            PassportID = 12345,
    //            DateOfBirth = new DateTime(2000, 2, 2),
    //            Contract = "Контракт",
    //            Salary = 134,
    //        };
    //        Employee noExistsEmployee = new Employee()
    //        {
    //            FirstName = "Валерий",
    //            LastName = "Валько",
    //            PassportID = 123456,
    //            DateOfBirth = new DateTime(2000, 3, 2),
    //            Contract = "Контракт",
    //            Salary = 134,
    //        };

    //        // Act/Assert
    //        try
    //        {
    //            employeeService.AddNewEmployee(existsEmployee);              
    //            Assert.Throws<KeyNotFoundException>(() => employeeService.DeleteEmployee(noExistsEmployee));

    //            employeeService.DeleteEmployee(existsEmployee);
    //            Assert.DoesNotContain(existsEmployee, employeeStorage._employeeList);
    //        }
    //        catch (Exception ex)
    //        {
    //            Assert.True(false);
    //        }

    //    }

    //    [Fact]
    //    public void UpdateEmployee_KeyNotFoundException_EmployeeUpdated_Test()
    //    {
    //        // Arrange
    //        EmployeeStorage employeeStorage = new EmployeeStorage();
    //        EmployeeService employeeService = new EmployeeService(employeeStorage);

    //        Employee existsEmployee = new Employee()
    //        {
    //            FirstName = "Игорь",
    //            LastName = "Петоров",
    //            PassportID = 12345,
    //            DateOfBirth = new DateTime(2000, 2, 2),
    //            Contract = "Контракт",
    //            Salary = 134,
    //        };
    //        Employee noExistsEmployee = new Employee()
    //        {
    //            FirstName = "Валерий",
    //            LastName = "Валько",
    //            PassportID = 123456,
    //            DateOfBirth = new DateTime(2000, 3, 2),
    //            Contract = "Контракт",
    //            Salary = 134,
    //        };

    //        // Act/Assert
    //        try
    //        {
    //            employeeService.AddNewEmployee(existsEmployee);
    //            employeeService.UpdateEmployee(existsEmployee);

    //            Assert.Throws<KeyNotFoundException>(() => employeeService.UpdateEmployee(noExistsEmployee));
    //            Assert.Same(existsEmployee, employeeStorage._employeeList.First(p => p.PassportID == existsEmployee.PassportID));
    //        }
    //        catch (Exception ex)
    //        {
    //            Assert.True(false);
    //        }

    //    }
    //}

}
