using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Employee : Person
    {
        public string Contract { get; set; }

        public Employee(string firstName, string lastName, int id, DateTime dateOfBirth) : base(firstName, lastName, id, dateOfBirth)
        {
        }
    }
}
