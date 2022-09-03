using Bogus.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Client : Person
    {
       public string PhoneNumber { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is Client))
                return false;

            var client = (Client)obj;
            return client.FirstName == FirstName &&
                   client.LastName == LastName &&
                   client.PassportID == PassportID &&
                   client.DateOfBirth == DateOfBirth &&
                   client.PhoneNumber == PhoneNumber;

        }
        public override int GetHashCode()
        {
            return FirstName.GetHashCode() + LastName.GetHashCode() + PassportID.GetHashCode() + DateOfBirth.GetHashCode() + PhoneNumber.GetHashCode();
             
        }
    }
    
}
