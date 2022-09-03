using Bogus.DataSets;
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
        public int Salary { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is Employee))
                return false;

            var employee = (Employee)obj;
            return employee.FirstName == FirstName &&
                   employee.LastName == LastName &&
                   employee.PassportID == PassportID &&
                   employee.DateOfBirth == DateOfBirth &&
                   employee.Contract == Contract &&
                   employee.Salary == Salary;

        }
        public override int GetHashCode()
        {
            return FirstName.GetHashCode() + LastName.GetHashCode() + PassportID.GetHashCode() + DateOfBirth.GetHashCode() + Contract.GetHashCode() + Salary.GetHashCode();

        }
    }
}
