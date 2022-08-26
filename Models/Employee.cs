using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Employee : Person
    {
        public string? Contract { get; set; }
        public int Salary { get; set; }

    }
}
