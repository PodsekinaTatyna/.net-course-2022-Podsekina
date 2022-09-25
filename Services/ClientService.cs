using ModelsDb;
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

        public ClientDb GetClient(Guid id)
        {
            return _clientStorage.Data.Clients.First(p => p.Id == id);
        }

        public void AddNewClient(ClientDb client)
        {
            if ((DateTime.Now.Year - client.DateOfBirth.Year) < 18)
                throw new Limit18YearsException("Клиент не может быть моложе 18 лет");

            if (client.PassportID == 0)
                throw new NoPassportDataException("Паспортные данные обязательно должны быть введены");

            if (_clientStorage.Data.Clients.Contains(client))
                throw new ArgumentException("Такой клиент уже существует");

            _clientStorage.Add(client);

        }

        public void UpdateClient(ClientDb client)
        {
            if (!_clientStorage.Data.Clients.Contains(client))
                throw new KeyNotFoundException("В базе нет такого клиента");

            _clientStorage.Update(client);

        }

        public void DeleteClient(ClientDb client)
        {
            if (!_clientStorage.Data.Clients.Contains(client))
                throw new KeyNotFoundException("В базе нет такого клиента");

            _clientStorage.Delete(client);

        }

        public List<ClientDb> GetClients(ClientFilter clientFilter)
        {
            var filteredDictionary = _clientStorage.Data.Clients.Select(p => p);

            if (clientFilter.FirstName != null)
               filteredDictionary = filteredDictionary.Where(p => p.FirstName == clientFilter.FirstName);

            if (clientFilter.LastName != null)
                filteredDictionary = filteredDictionary.Where(p => p.LastName == clientFilter.LastName);

            if (clientFilter.PhoneNumber != null)
                filteredDictionary = filteredDictionary.Where(p => p.PhoneNumber == clientFilter.PhoneNumber);
            
            if (clientFilter.StartDate != default)
                filteredDictionary = filteredDictionary.Where(p => p.DateOfBirth == clientFilter.StartDate);
            
            if (clientFilter.EndDate != default)
                filteredDictionary = filteredDictionary.Where(p => p.DateOfBirth == clientFilter.EndDate);

            return filteredDictionary.ToList();

        }

        public void AddNewAccount(Guid id, AccountDb account)
        {
            if (_clientStorage.Data.Clients.FirstOrDefault(p => p.Id == id) == null)
                throw new KeyNotFoundException("В базе нет такого клиента");

            if (_clientStorage.Data.Accounts.FirstOrDefault(p => p.clientId == id && p.Currency == account.Currency) != null) 
                throw new AccountAlreadyExistsException("У клиента уже есть такой счет");

            _clientStorage.AddAccount(account);

        }

        public void DeleteAccount(Guid id, AccountDb account)
        {
            if (_clientStorage.Data.Clients.FirstOrDefault(p => p.Id == id) == null)
                throw new KeyNotFoundException("В базе нет такого клиента");

            if (_clientStorage.Data.Accounts.FirstOrDefault(p => p.clientId == id && p.Currency == account.Currency) == null)
                throw new NullReferenceException("У клиента нет такого счета");

            _clientStorage.DeleteAccount(account);

        }

        public void UpdateAccount(Guid id, AccountDb account)
        {
            if (_clientStorage.Data.Clients.FirstOrDefault(p => p.Id == id) == null)
                throw new KeyNotFoundException("В базе нет такого клиента");

            if (_clientStorage.Data.Accounts.FirstOrDefault(p => p.clientId == id && p.Currency == account.Currency) == null)
                throw new NullReferenceException("У клиента нет такого счета");

            _clientStorage.UpdateAccount(account);

        }
    }
}
