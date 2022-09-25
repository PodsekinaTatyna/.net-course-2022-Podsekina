using Models;
using ModelsDb;

namespace Services.Storages
{
    public class ClientStorage : IClientStorage
    {
        private BankContext data = new BankContext();

        public BankContext Data => data;


        public void Add(ClientDb client)
        {
          
            Data.Clients.Add(client);
            Data.Accounts.Add(
              new AccountDb
              {
                  Client = client,
                  Currency = "USD",
                  Amount = 0
              });
            Data.SaveChanges();
        }

        public void Delete(ClientDb client)
        {
            Data.Clients.Remove(client);
            Data.SaveChanges();
        }

        public void Update(ClientDb client)
        {
            Data.Clients.Update(client);
            Data.SaveChanges();
        }

        public void AddAccount(AccountDb account)
        {
            Data.Accounts.Add(account);
            Data.SaveChanges();
        }

        public void DeleteAccount(AccountDb account)
        {
            Data.Accounts.Remove(account);
            Data.SaveChanges();
        }

        public void UpdateAccount(AccountDb account)
        {
            Data.Accounts.Update(account);
            Data.SaveChanges();
        }
    }


}
