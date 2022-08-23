using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Client : Person
    {
        public Client(string firstName, string lastName, int id, DateTime dateOfBirth) : base(firstName, lastName, id, dateOfBirth)
        {
        }
    }
}
