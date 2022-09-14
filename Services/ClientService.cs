using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Bogus.DataSets;
using Models;
using Services.Exceptions;
using Services.Filters;
using Services.Storages;

namespace Services
{
    public class ClientService
    {
        private IClientStorage _clientStorage { get; set; }
        public ClientService(IClientStorage clientStorage)
        {
            _clientStorage = clientStorage;

        }

        public void AddNewClient(Client client)
        {
            if (DateTime.Now.Year - client.DateOfBirth.Year < 18)
                throw new Limit18YearsException("Клиент не может быть моложе 18 лет");

            if (client.PassportID == 0)
                throw new NoPassportDataException("Паспортные данные обязательно должны быть введены");

            if (_clientStorage.Data.ContainsKey(client))
                throw new ArgumentException("Такой клиент уже существует");

            _clientStorage.Add(client);

        }

        public void UpdateClient(Client client)
        {
            if (!_clientStorage.Data.ContainsKey(client))
                throw new KeyNotFoundException("В базе нет такого клиента");

            _clientStorage.Update(client);

        }

        public void DeleteClient(Client client)
        {
            if (!_clientStorage.Data.ContainsKey(client))
                throw new KeyNotFoundException("В базе нет такого клиента");

            _clientStorage.Delete(client);

        }

        public Dictionary<Client, List<Account>> GetClients(ClientFilter clientFilter)
        {
            var filteredDictionary = _clientStorage.Data.Select(p => p);

            if (clientFilter.FirstName != null)
               filteredDictionary = filteredDictionary.Where(p => p.Key.FirstName == clientFilter.FirstName);

            if (clientFilter.LastName != null)
                filteredDictionary = filteredDictionary.Where(p => p.Key.LastName == clientFilter.LastName);

            if (clientFilter.PhoneNumber != null)
                filteredDictionary = filteredDictionary.Where(p => p.Key.PhoneNumber == clientFilter.PhoneNumber);
            
            if (clientFilter.StartDate != default)
                filteredDictionary = filteredDictionary.Where(p => p.Key.DateOfBirth == clientFilter.StartDate);
            
            if (clientFilter.EndDate != default)
                filteredDictionary = filteredDictionary.Where(p => p.Key.DateOfBirth.Date == clientFilter.EndDate.Date);

            return filteredDictionary.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        }

        public void AddNewAccount(Client client, Account account)
        {
            if (!_clientStorage.Data.ContainsKey(client))
                throw new KeyNotFoundException("В базе нет такого клиента");

            if (_clientStorage.Data[client].FirstOrDefault(p => p.Currency.Name == account.Currency.Name) != null)
                throw new AccountAlreadyExistsException("У клиента уже есть такой счет");

            _clientStorage.AddAccount(client, account);

        } 

        public void DeleteAccount(Client client, Account account)
        {
            if (!_clientStorage.Data.ContainsKey(client))
                throw new KeyNotFoundException("В базе нет такого клиента");

            if (_clientStorage.Data[client].FirstOrDefault(p => p.Currency.Name == account.Currency.Name) == null)
                throw new NullReferenceException("У клиента нет такого счета");

            _clientStorage.DeleteAccount(client, account);

        }

        public void UpdateAccount(Client client, Account account)
        {
            if (!_clientStorage.Data.ContainsKey(client))
                throw new KeyNotFoundException("В базе нет такого клиента");

            if (_clientStorage.Data[client].FirstOrDefault(p => p.Currency.Name == account.Currency.Name) == null)
                throw new NullReferenceException("У клиента нет такого счета");

            _clientStorage.UpdateAccount(client, account);

        }
    }
}
