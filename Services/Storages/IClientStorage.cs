using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Storages
{
    public interface IClientStorage : IStorage<Client>
    {
        public Dictionary<Client, List<Account>> Data { get;}

        public void AddAccount(Client client, Account account);
        public void UpdateAccount(Client client, Account account);
        public void DeleteAccount(Client client, Account account);

    }
}
