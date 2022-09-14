using Bogus.DataSets;
using Models;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Services.Storages
{
    public class ClientStorage : IClientStorage
    {
        private Dictionary<Client, List<Account>> data = new Dictionary<Client, List<Account>>();

        public Dictionary<Client, List<Account>> Data => data;

        public void Add(Client client)
        {
            Data.Add(
                client,
                new List<Account>
                {
                    new Account
                    {
                        Currency = new Curreny
                        {
                            Code = 840,
                            Name = "USD",
                        },
                        Amount = 0
                    }
                });
        }

        public void Delete(Client client)
        {
            Data.Remove(client);
        }

        public void Update(Client client)
        {
            var oldClient = Data.Keys.First(p => p.PassportID == client.PassportID);

            oldClient.FirstName = client.FirstName;
            oldClient.LastName = client.LastName;
            oldClient.PassportID = client.PassportID;
            oldClient.PhoneNumber = client.PhoneNumber;
            oldClient.DateOfBirth = client.DateOfBirth;

        }

        public void AddAccount(Client client, Account account)
        {
            Data[client].Add(account);
        }

        public void DeleteAccount(Client client, Account account)
        {
            Data[client].Remove(account);
        }

        public void UpdateAccount(Client client, Account account)
        {
            var oldAccount = Data[client].First(p => p.Currency.Name == account.Currency.Name);

            oldAccount.Currency = new Curreny
            {
                Code = account.Currency.Code,
                Name = account.Currency.Name,
            };
            oldAccount.Amount = account.Amount;

        }
    }


}
