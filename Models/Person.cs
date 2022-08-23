using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ID { get; set; }
        public DateTime DateOfBirth { get; set; }

        public Person(string firstName, string lastName, int id, DateTime dateOfBirth)
        {
            FirstName = firstName;
            LastName = lastName;
            ID = id;
            DateOfBirth = dateOfBirth;
        }
    }
}
