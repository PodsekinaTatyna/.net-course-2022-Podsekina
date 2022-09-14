using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Bogus.DataSets;
using System.Reflection.Emit;
using System.Dynamic;

namespace Services
{
    public class TestDataGenerator
    {

        public Faker<Client> GetFakeDataClient()
        {
            var generator = new Faker<Client>("ru")
                .StrictMode(true)
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.PassportID, f => f.Random.Int(10000, 100000))
                .RuleFor(u => u.DateOfBirth, f => f.Date.Past(80))
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.Bonus, f => f.Random.Int(1, 10));
            return generator;

        }

        public Faker<Employee> GetFakeDataEmployee()
        {
            var generator = new Faker<Employee>("ru")
                .StrictMode(true)
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.PassportID, f => f.Random.Int(10000, 100000))
                .RuleFor(u => u.DateOfBirth, f => f.Date.Past(80))
                .RuleFor(u => u.Contract, (f, u) => "ID: " + u.PassportID + 
                                                    "\nFull Name: " + u.FirstName + " " + u.LastName + 
                                                    "\nDate of Birth: " + u.DateOfBirth.ToString("D"))
                .RuleFor(u => u.Salary, f => f.Finance.Random.Int(1000,5000))
                       .RuleFor(u => u.Bonus, f => f.Random.Int(1, 10)); ;

            return generator;

        }

      

        public List<Client> GetClientsList()
        {
            var clientList = GetFakeDataClient().Generate(1000);

            return clientList;
        }

        public Dictionary<string, Client> GetClientsDictionary()
        {
            Dictionary<string, Client> clientDictionary = new Dictionary<string, Client>();


            for (int i = 0; i < 1000; i++)
            {
                Client client = GetFakeDataClient().Generate();
                clientDictionary.Add(client.PhoneNumber, client);
            }

            return clientDictionary;
        }

        public List<Employee> GetEmployeesList()
        {
            var employeeList = GetFakeDataEmployee().Generate(1000);        

            return employeeList;
        }

        public Dictionary<Client,List<Account>> GetAccounDictionary()
        {
            Dictionary<Client, List<Account>> accounDictionary = new Dictionary<Client, List<Account>>();

            accounDictionary.Add(
                new Client
                {
                    FirstName = "Андрей",
                    LastName = "Попов",
                    PassportID = 123456,
                    DateOfBirth = new DateTime(),
                    PhoneNumber = "123456789"
                },
                new List<Account>
                {
                    new Account
                    {
                        Currency = new Curreny
                        {
                            Code = 120,
                            Name = "USD",
                        },
                        Amount = 1233,
                    },
                    new Account
                    {
                        Currency = new Curreny
                        {
                            Code = 200,
                            Name = "RUP",
                        },
                        Amount = 345,
                    }
                });
                   
            accounDictionary.Add(
                new Client
                {
                    FirstName = "Анна",
                    LastName = "Иванова",
                    PassportID = 654321,
                    DateOfBirth = new DateTime(),
                    PhoneNumber = "987654321"
                },
                new List<Account>
                {
                    new Account
                    {
                        Currency = new Curreny
                        {
                           Code = 120,
                           Name = "USD",
                        },
                        Amount = 234
                    },
                    new Account
                    {
                        Currency = new Curreny
                        {
                            Code = 200,
                            Name = "RUP",
                        },
                        Amount = 5365,
                    }
                } );

            return accounDictionary;
        }
    }
}
