using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CurrencyToConvert
    {
        public string Key { get; set; }

        public string FromCurrency { get; set; }

        public string ToCurrency { get; set; }

        public float Amount { get; set; }

    }
}
