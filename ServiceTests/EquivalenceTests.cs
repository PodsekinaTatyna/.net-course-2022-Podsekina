using Xunit;
using Services;
using Models;
using System.Security.Principal;

namespace ServiceTests
{
    public class EquivalenceTests
    {
        [Fact]
        public void GetHashCodeNecessityPositivTest()
        {
            //Arrange
            TestDataGenerator testDataGenerator = new TestDataGenerator();
            Dictionary<Client, List<Account>> accounDictionary = testDataGenerator.GetAccounDictionary();
            var firstAccount = accounDictionary.First();

            Client client = new Client
            {
                FirstName = firstAccount.Key.FirstName,
                LastName = firstAccount.Key.LastName,
                PassportID = firstAccount.Key.PassportID,
                DateOfBirth = firstAccount.Key.DateOfBirth,
                PhoneNumber = firstAccount.Key.PhoneNumber,
            };

            //Act
            var expectedAccount = accounDictionary[firstAccount.Key];
            var actualAccount = accounDictionary[client];

            //Assert
            Assert.Equal(expectedAccount, actualAccount);
        }


        [Fact]
        public void GetHashCodeNecessityPositivEmployeeTest()
        {
            // Arrange
            TestDataGenerator testDataGenerator = new TestDataGenerator();
            List<Employee> employees = testDataGenerator.GetEmployeesList();
            var firstEmployee = employees.First();

            Employee newEmployee = new Employee
            {
                FirstName = firstEmployee.FirstName,
                LastName = firstEmployee.LastName,
                PassportID = firstEmployee.PassportID,
                DateOfBirth = firstEmployee.DateOfBirth,
                Salary = firstEmployee.Salary,
                Contract = firstEmployee.Contract,
            };

            //Act
            var actualEmployee = employees.FirstOrDefault(p => p.Equals(newEmployee));
            var isNewEmployeeInList = employees.Contains(newEmployee);

            //Assert
            Assert.Equal(firstEmployee, actualEmployee);
        }
    }
}
