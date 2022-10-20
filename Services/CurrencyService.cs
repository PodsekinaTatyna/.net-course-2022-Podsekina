using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Docker.DotNet.Models;
using Models;

namespace Services
{
    public class CurrencyService
    {

        public async Task<CurrencyToCOnvertResponse> ConvertCurrency(CurrencyToConvert data)
        {
            CurrencyToCOnvertResponse response;

            using (var client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await client.GetAsync($"https://www.amdoren.com/api/currency.php?api_key={data.Key}" +
                                                                            $"&from={data.FromCurrency}&to={data.ToCurrency}&amount={data.Amount}");

                string message = await responseMessage.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<CurrencyToCOnvertResponse>(message);

            }

            return response;
        }
    }
}
