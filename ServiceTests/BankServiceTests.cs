using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ServiceTests
{
    public class BankServiceTests
    {
        [Fact]
        public void AddToBlackLis_IsPersonInBlackList_Test()
        {
            //Arrange
            BankService bankService = new BankService();

            Employee employee = new Employee()
            {
                FirstName = "Игорь",
                LastName = "Петоров",
                PassportID = 12345,
                DateOfBirth = new DateTime(2007, 2, 2),
                Contract = "Контракт",
                Salary = 134,
            };
            Client client = new Client()
            {
                FirstName = "Игорь",
                LastName = "Петоров",
                PassportID = 0,
                DateOfBirth = new DateTime(2000, 2, 2),
                PhoneNumber = "123456789"
            };

            //Act           
            bankService.AddToBlackList(employee);
            bankService.AddToBlackList(client);            
          
            //Assert
            Assert.True(bankService.IsPersonInBlackList(employee));
            Assert.True(bankService.IsPersonInBlackList(client));
        }
    }
}
