using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BankService
    {
        public int BankOwnersSalaries(decimal profit, decimal expenses, int numberOfOwners)
        {
            return (int)((profit - expenses) / numberOfOwners);
        }
    }
}
