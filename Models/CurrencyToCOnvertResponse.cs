using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CurrencyToCOnvertResponse
    {
        public string Error { get; set; }

        public string ErrorMessage { get; set; }

        public float Amount { get; set; }   
    }
}
