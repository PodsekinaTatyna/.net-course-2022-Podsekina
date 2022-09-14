using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Services
{
    public class BankService
    {
        private List<Person> blackList = new List<Person>();

        public int BankOwnersSalaries(decimal profit, decimal expenses, int numberOfOwners)
        {
            return (int)((profit - expenses) / numberOfOwners);
        }

        public Employee ClientConverToEmployee(Client client)
        {
            return new Employee
            {
                FirstName = client.FirstName,
                LastName = client.LastName,
                PassportID = client.PassportID,
                DateOfBirth = client.DateOfBirth,
            };
        }

        public void AddBonus(Person person)
        {
            person.Bonus++;
        }

        public void AddToBlackList<T>(T person) where T : Person
        {
            blackList.Add(person);
        }

        public bool IsPersonInBlackList<T>(T person) where T : Person
        {
            return blackList.Contains(person);
        }
    }
}
