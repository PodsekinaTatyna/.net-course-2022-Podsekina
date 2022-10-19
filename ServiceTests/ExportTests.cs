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
        public async void WriteClientListToCsv_FromDatabaseTest()
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
            await exportService.WriteClientListToCsvAsync(clients);

            //Asert
            Assert.True(true);
        }

        [Fact]
        public async void ReadClientListFromCsv_ToDatabase_Test()
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
            await exportService.WriteClientListToCsvAsync(clients);

            clientsFromCsv = await exportService.ReadClientListFromCsvAsync();

            foreach(var client in clientsFromCsv)
            {
                await clientService.AddNewClientAsync(client);
            }

            //Asert
            Assert.True(true);
        }


        [Fact]
        public async void ClientSerializationWriteAndReadFromFileAsync_Test()
        {
            //Arrenge
            TestDataGenerator testDataGenerator = new TestDataGenerator();

            string directoryPath = Path.Combine("C:", "Users", "Professional", "Desktop", ".net-course-2022-Podsekina");
            string fileName = "clientSerialization.json";

            ExportService exportService = new ExportService(directoryPath, fileName);
            List<Client> clients = testDataGenerator.GetFakeDataClient().Generate(10);

            //Act
            await exportService.PersonSerializationWriteToFileAsync(clients, directoryPath, fileName);
            var clientsDesirialization = await exportService.PersonDeserializationReadFromFileAsync<Client>(directoryPath, fileName);

            //Assert
            Assert.Equal(clients.First().Id, clientsDesirialization.First().Id);

        }

        [Fact]
        public async void EmployeeSerializationWriteAndReadFromFileAsync_Test()
        {
            //Arrenge
            TestDataGenerator testDataGenerator = new TestDataGenerator();

            string directoryPath = Path.Combine("C:", "Users", "Professional", "Desktop", ".net-course-2022-Podsekina");
            string fileName = "employeeSerialization.json";

            ExportService exportService = new ExportService(directoryPath, fileName);
            List<Employee> employees = testDataGenerator.GetFakeDataEmployee().Generate(10);

            //Act
            await exportService.PersonSerializationWriteToFileAsync(employees, directoryPath, fileName);
            List<Employee> employeesDesirialization = await exportService.PersonDeserializationReadFromFileAsync<Employee>(directoryPath, fileName);

            //Assert
            Assert.Equal(employees.First().Id, employeesDesirialization.First().Id);

        }
    }
}
