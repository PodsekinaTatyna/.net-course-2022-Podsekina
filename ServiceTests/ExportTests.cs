using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Models;
using Services;
using ExportTool;
using ModelsDb;

namespace ServiceTests
{
    public class ExportTests
    {
        [Fact]
        public void WriteClientListToCsv_FromDatabaseTest()
        {
            //Arrenge
            BankContext bankContext = new BankContext(); 

            string directoryPath = Path.Combine("C:", "Users", "Professional", "Desktop", ".net-course-2022-Podsekina");
            string fileName = "clientListFromDb.csv";

            ExportService exportService = new ExportService(directoryPath, fileName);

            List<Client> clients = new List<Client>();
            foreach(var client in bankContext.Clients)
            {
                clients.Add(new Client
                {
                    Id = client.Id,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    PassportID = client.PassportID,
                    DateOfBirth = client.DateOfBirth,
                    Bonus = client.Bonus,
                    PhoneNumber = client.PhoneNumber
                });
            }
            

            //Act
            exportService.WriteClientListToCsv(clients);

            //Asert
            Assert.True(true);
        }

        [Fact]
        public void ReadClientListFromCsv_ToDatabase_Test()
        {
            //Arrenge
            TestDataGenerator testDataGenerator = new TestDataGenerator();
            ClientService clientService = new ClientService();
            string directoryPath = Path.Combine("C:", "Users", "Professional", "Desktop", ".net-course-2022-Podsekina");
            string fileName = "clientListToDb.csv";

            ExportService exportService = new ExportService(directoryPath, fileName);

            List<Client> clients = new List<Client>();
            List<Client> clientsFromCsv = new List<Client>();

            for (int i = 0; i < 10; i++)
            {
                clients.Add(testDataGenerator.GetFakeDataClient().Generate());
            }

            //Act
            exportService.WriteClientListToCsv(clients);

            clientsFromCsv = exportService.ReadClientListFromCsv();

            foreach(var client in clientsFromCsv)
            {
                clientService.AddNewClient(client);
            }

            //Asert
            Assert.True(true);
        }

    }
}
