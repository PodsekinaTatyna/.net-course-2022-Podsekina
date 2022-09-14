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
            
            client = new Client
            {
                FirstName = oldClient.FirstName,
                LastName = oldClient.LastName,
                PassportID = oldClient.PassportID,
                PhoneNumber = oldClient.PhoneNumber,
                DateOfBirth = oldClient.DateOfBirth
            };

            Delete(oldClient);
            Add(client);

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
            int accountIndex = Data[client].IndexOf(Data[client].First((p => p.Currency.Name == account.Currency.Name)));

            Data[client][accountIndex] = new Account
            {
                Currency = new Curreny
                {
                    Code = account.Currency.Code,
                    Name = account.Currency.Name,
                },
                Amount = account.Amount
            };
        }
    }


}
