using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Models;
using Services.Exceptions;

namespace Services
{
    public class ClientService
    {
        private Dictionary<Client, List<Account>> dictionaryClient = new Dictionary<Client, List<Account>>();

        public void AddNewClient(Client client)
        {
            if (DateTime.Now.Year - client.DateOfBirth.Year < 18)
                throw new Limit18YearsException("Клиент не может быть моложе 18 лет");

            if (client.PassportID == 0)
                throw new NoPassportDataException("Паспортные данные обязательно должны быть введены");

            if (dictionaryClient.ContainsKey(client))
                throw new ArgumentException("Такой клиент уже существует");

            dictionaryClient.Add(
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

        public void AddNewAccount(Client client)
        {
            if (!dictionaryClient.ContainsKey(client))
                throw new KeyNotFoundException("В базе нет такого клиента");

            Account newAccount = new Account
            {
                Currency = new Curreny
                {
                    Code = 978,
                    Name = "EUR",
                },
                Amount = 0
            };

            if (dictionaryClient[client].FirstOrDefault(p => p.Currency.Name == newAccount.Currency.Name) != null)
                throw new AccountAlreadyExistsException("У клиента уже есть такой счет");

            dictionaryClient[client].Add(newAccount);
        }

        public void AccountEditing(Client client, Account account)
        {
            if (!dictionaryClient.ContainsKey(client))
                throw new KeyNotFoundException("В базе нет такого клиента");

            var oldAccount = dictionaryClient[client].FirstOrDefault(p => p.Currency.Name == account.Currency.Name);

            if (oldAccount == null)
                throw new NullReferenceException("У клиента нет такого счета");

            oldAccount = new Account
            {
                Currency = new Curreny
                {
                    Code = oldAccount.Currency.Code,
                    Name = oldAccount.Currency.Name,
                },
                Amount = oldAccount.Amount
            };
        }
    }
}
