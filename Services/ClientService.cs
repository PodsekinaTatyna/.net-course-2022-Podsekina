using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Models;
using Services.Exceptions;
using Services.Filters;

namespace Services
{
    public class ClientService
    {
        private ClientStorage _clientStorage { get; set; }
        public ClientService(ClientStorage clientStorage)
        {
            this._clientStorage = clientStorage;
        }

        public void AddNewClient(Client client)
        {
            if (DateTime.Now.Year - client.DateOfBirth.Year < 18)
                throw new Limit18YearsException("Клиент не может быть моложе 18 лет");

            if (client.PassportID == 0)
                throw new NoPassportDataException("Паспортные данные обязательно должны быть введены");

            _clientStorage.Add(client);
        }

        public Dictionary<Client, List<Account>> GetClients(ClientFilter clientFilter)
        {
            Dictionary<Client, List<Account>> filteredDictionary = new Dictionary<Client, List<Account>>();

            if (clientFilter.FirstName != null)
               filteredDictionary = _clientStorage._dictionaryClient.Where(p => p.Key.FirstName == clientFilter.FirstName).ToDictionary(kvp => kvp.Key, kvp =>kvp.Value);

            if (clientFilter.LastName != null)
                filteredDictionary = _clientStorage._dictionaryClient.Where(p => p.Key.LastName == clientFilter.LastName).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            if (clientFilter.PhoneNumber != null)
                filteredDictionary = _clientStorage._dictionaryClient.Where(p => p.Key.PhoneNumber == clientFilter.PhoneNumber).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            
            if (clientFilter.StartDate != default)
                filteredDictionary = _clientStorage._dictionaryClient.Where(p => p.Key.DateOfBirth == clientFilter.StartDate).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            
            if (clientFilter.EndDate != default)
                filteredDictionary = _clientStorage._dictionaryClient.Where(p => p.Key.DateOfBirth.Date == clientFilter.EndDate.Date).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return filteredDictionary;
        }

        public void AddNewAccount(Client client)
        {
            if (!_clientStorage._dictionaryClient.ContainsKey(client))
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

            if (_clientStorage._dictionaryClient[client].FirstOrDefault(p => p.Currency.Name == newAccount.Currency.Name) != null)
                throw new AccountAlreadyExistsException("У клиента уже есть такой счет");

            _clientStorage._dictionaryClient[client].Add(newAccount);
        }

        public void AccountEditing(Client client, Account account)
        {
            if (!_clientStorage._dictionaryClient.ContainsKey(client))
                throw new KeyNotFoundException("В базе нет такого клиента");

            var oldAccount = _clientStorage._dictionaryClient[client].FirstOrDefault(p => p.Currency.Name == account.Currency.Name);

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
