using Models;
using ModelsDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class RateUpdaterService
    {
        private ClientService _clientService;
        public RateUpdaterService(ClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task AccruingInterestAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                BankContext _bankContext = new BankContext();

                var accountsDb = _bankContext.Accounts.Take(10).ToList();

                foreach (var accountDb in accountsDb)
                {
                    var account = new Account
                    {
                        Amount = accountDb.Amount,
                        Currency = new Curreny { Name = accountDb.CurrencyName }
                    };

                    account.Amount += (decimal)5;
                    await _clientService.UpdateAccountAsync(accountDb.ClientId, account);
                }

                Task.Delay(5000).Wait();
            }
        }
    }
}
