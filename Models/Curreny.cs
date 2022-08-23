using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public struct Curreny
    {
        public int ID { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
        public Curreny(int id, decimal amount, string name)
        {
            ID = id;
            Amount = amount;
            Name = name;
        }

    }
}
