using ModelsDb;
using Models;
using Services.Exceptions;
using Services.Filters;
using Services.Storages;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using Bogus.DataSets;

namespace Services
{
    public class ClientService
    {
        public BankContext bankContext { get; set; }

        public ClientService()
        {
            bankContext = new BankContext();
        }

        public async Task<ClientDb> GetClientAsync(Guid id)
        {
            return await bankContext.Clients.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddNewClientAsync(Client client)
        {
            var clientDb = new ClientDb
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                PassportID = client.PassportID,
                DateOfBirth = client.DateOfBirth,
                Bonus = client.Bonus,
                PhoneNumber = client.PhoneNumber
            };

            if ((DateTime.Now.Year - clientDb.DateOfBirth.Year) < 18)
                throw new Limit18YearsException("Клиент не может быть моложе 18 лет");

            if (clientDb.PassportID == 0)
                throw new NoPassportDataException("Паспортные данные обязательно должны быть введены");

            if (await bankContext.Clients.FirstOrDefaultAsync(p => p.Id == client.Id) != null)
                throw new ArgumentException("Такой клиент уже существует");

            await bankContext.Clients.AddAsync(clientDb);

            await bankContext.Accounts.AddAsync(new AccountDb
            {
                Amount = 0,
                ClientId = clientDb.Id,
                CurrencyName = "USD",
                Currency = new CurrencyDb
                {
                    Name = "USD",
                    Code = 840,
                }
            });

            await bankContext.SaveChangesAsync();
        }

        public async Task UpdateClientAsync(Client client)
        {
            var clientDb = await bankContext.Clients.FirstOrDefaultAsync(p => p.Id == client.Id);

            if (clientDb == null)
                throw new KeyNotFoundException("В базе нет такого клиента");
            clientDb.Id = client.Id;
            clientDb.FirstName = client.FirstName;
            clientDb.LastName = client.LastName;
            clientDb.PassportID = client.PassportID;
            clientDb.DateOfBirth = client.DateOfBirth;
            clientDb.Bonus = client.Bonus;
            clientDb.PhoneNumber = client.PhoneNumber;

            bankContext.Clients.Update(clientDb);
            await bankContext.SaveChangesAsync();
            
        }

        public async Task DeleteClientAsync(Client client)
        {
            var clientDb = await bankContext.Clients.FirstOrDefaultAsync(p => p.Id == client.Id);

            if (clientDb == null)
                throw new KeyNotFoundException("В базе нет такого клиента");

            bankContext.Clients.Remove(clientDb);
            await bankContext.SaveChangesAsync();
                     
        }

        public async Task<List<Client>> GetClientsAsync(ClientFilter clientFilter, int page, int limit)
        {
            var query = bankContext.Clients.Select(p => p).AsNoTracking();

            if (clientFilter.FirstName != null)
               query = query.Where(p => p.FirstName == clientFilter.FirstName);

            if (clientFilter.LastName != null)
                query = query.Where(p => p.LastName == clientFilter.LastName);

            if (clientFilter.PhoneNumber != null)
                query = query.Where(p => p.PhoneNumber == clientFilter.PhoneNumber);
            
            if (clientFilter.StartDate != default)
                query = query.Where(p => p.DateOfBirth == clientFilter.StartDate);
            
            if (clientFilter.EndDate != default)
                query = query.Where(p => p.DateOfBirth == clientFilter.EndDate);

            var filteredList = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();

            var clientList = new List<Client>();

            foreach(ClientDb clientDb in filteredList)
            {
                clientList.Add(new Client
                {
                    Id = clientDb.Id,
                    FirstName = clientDb.FirstName,
                    LastName = clientDb.LastName,
                    PassportID = clientDb.PassportID,
                    DateOfBirth = clientDb.DateOfBirth,
                    Bonus = clientDb.Bonus,
                    PhoneNumber = clientDb.PhoneNumber
                });
            }

            return clientList;

        }

        public async Task AddNewAccountAsync(Guid id, Account account)
        {
            var accountDb = new AccountDb
            {
                Amount = account.Amount,
                ClientId = id,
                CurrencyName = account.Currency.Name,
                Currency = new CurrencyDb
                {
                    Name = account.Currency.Name,
                    Code = account.Currency.Code,
                }
            };

            if (await bankContext.Clients.FirstOrDefaultAsync(p => p.Id == id) == null)
                throw new KeyNotFoundException("В базе нет такого клиента");        

            if (await bankContext.Accounts.FirstOrDefaultAsync(p => p.ClientId == id && p.CurrencyName == account.Currency.Name) != null) 
                throw new AccountAlreadyExistsException("У клиента уже есть такой счет");

            await bankContext.Accounts.AddAsync(accountDb);
            await bankContext.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(Guid id, Account account)
        {            
            if (await bankContext.Clients.FirstOrDefaultAsync(p => p.Id == id) == null)
                throw new KeyNotFoundException("В базе нет такого клиента");

            var accountDb = await bankContext.Accounts.FirstOrDefaultAsync(p => p.ClientId == id && p.CurrencyName == account.Currency.Name);

            if (accountDb == null)
                throw new NullReferenceException("У клиента нет такого счета");

            bankContext.Accounts.Remove(accountDb);
            await bankContext.SaveChangesAsync();

        }

        public async Task UpdateAccountAsync(Guid id, Account account)
        {
            if (await bankContext.Clients.FirstOrDefaultAsync(p => p.Id == id) == null)
                throw new KeyNotFoundException("В базе нет такого клиента");

            var accountDb = await bankContext.Accounts.FirstOrDefaultAsync(p => p.ClientId == id && p.CurrencyName == account.Currency.Name);

            if (accountDb == null)
                throw new NullReferenceException("У клиента нет такого счета");

            accountDb.Amount = account.Amount;
            accountDb.CurrencyName = account.Currency.Name;

            bankContext.Accounts.Update(accountDb);
            await bankContext.SaveChangesAsync();

        }
    }
}
