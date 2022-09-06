using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Services.Exceptions;

namespace Services
{
    public class ClientService
    {
        private Dictionary<Client,Account> dictionaryClient = new Dictionary<Client,Account>();

        public void AddNewClient(Client client)
        {
            if(DateTime.Now.Year - client.DateOfBirth.Year < 18)
                throw new Limit18YearsException("Клиент не может быть моложе 18 лет"); 

            if (client.PassportID == 0)
                throw new NoPassportDataException("Паспортные данные обязательно должны быть введены");

            if (dictionaryClient.ContainsKey(client))
                throw new ArgumentException("Такой клиент уже существует");

            dictionaryClient.Add(
                client,
                new Account
                {
                    Currency = new Curreny
                    {
                        Name = "USD",
                        Code = 120
                    },
                    Amount = 1234
                });
        }
    }
}
