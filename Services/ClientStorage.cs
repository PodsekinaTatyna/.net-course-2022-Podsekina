using Models;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services 
{
    public class ClientStorage : IStorage
    {
        public readonly Dictionary<Client, List<Account>> _dictionaryClient = new Dictionary<Client, List<Account>>();

        public void Add(Client client)
        {
            _dictionaryClient.Add(
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

        public void Editing()
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }
    }

   
}
