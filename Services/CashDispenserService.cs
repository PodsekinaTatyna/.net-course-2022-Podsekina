using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CashDispenserService
    {
        public Task AccountCashingOut(Guid id, Account account)
        {
            return Task.Run(() =>
            {
                ClientService clientService = new ClientService();

                for (int i = 0; i < 10; i++)
                {
                    if (account.Amount >= 10)
                    {
                        account.Amount -= 10;
                        clientService.UpdateAccountAsync(id, account);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException($"Недостаточно средств. Баланс: {account.Amount}");
                    }
                    Task.Delay(5000).Wait();
                }

            });
        }

    }
}
