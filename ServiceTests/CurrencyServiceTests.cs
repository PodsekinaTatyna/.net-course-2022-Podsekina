using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Services;
using Models;
namespace ServiceTests
{
    public class CurrencyServiceTests
    {
        [Fact]
        public async Task ConvertCurrencySuccessfulTest()
        {
            //Arrange
            CurrencyService currencyService = new CurrencyService();
            var data = new CurrencyToConvert
            {
                Key = "zQDENFgQLFyiVCFh2jRmswc62EaL5r",
                FromCurrency = "USD",
                ToCurrency = "MDL",
                Amount = 50
            };

            //Act
            var response = await currencyService.ConvertCurrency(data);

            //Assert
            Assert.Equal("0", response.Error);
        }
    }
}
