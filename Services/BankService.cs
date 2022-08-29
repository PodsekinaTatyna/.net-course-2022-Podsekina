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

    }
}
