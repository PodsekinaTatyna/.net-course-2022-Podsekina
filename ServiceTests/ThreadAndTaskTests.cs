using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportTool;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Models;
using ModelsDb;
using Services;
using Services.Filters;
using Xunit;
using Xunit.Abstractions;

namespace ServiceTests
{
    public class ThreadAndTaskTests
    {
        private ITestOutputHelper _output;

        public ThreadAndTaskTests(ITestOutputHelper output)
        {
            _output = output;
        }

        object locker = new();

        [Fact]
        public void AccountReplenishment_InTwoDifferentStreams_Test()
        {
            Account account = new Account();

            Thread threadA = new Thread(() =>
            {
                lock (locker)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        account.Amount += 100;
                        Thread.Sleep(100);
                        _output.WriteLine($"{Thread.CurrentThread.Name} начислил 100, на счету {account.Amount}");
                    }
                }                                
            });

            threadA.Name = "threadA";

            Thread threadB = new Thread(() =>
            {
                lock (locker)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        account.Amount += 100;
                        Thread.Sleep(100);
                        _output.WriteLine($"{Thread.CurrentThread.Name} начислил 100, на счету {account.Amount}");
                    }
                }
            });

            threadB.Name = "threadB";

            threadA.Start();
            threadB.Start();


            Thread.Sleep(10000);
        }


        [Fact]
        public void ReadingAndWritingCsv_InTwoDifferentStreams_Test()
        {                    
            Thread threadWritingCsv = new Thread(() =>
            {
                ClientService clientService = new ClientService();
                BankContext bankContext = new BankContext();

                string directoryPath = Path.Combine("C:", "Users", "Professional", "Desktop", ".net-course-2022-Podsekina");
                string fileName = "DataFromDbMultithreading.csv";

                ExportService exportServiceWritingCsv = new ExportService(directoryPath, fileName);

                List<Client> clients = new List<Client>();

                for (int i = 0; i < bankContext.Clients.Count(); i++)
                {
                    clients.Add(clientService.GetClients(new ClientFilter(), i + 1, 1).SingleOrDefault());

                    _output.WriteLine($"{clients[i].Id} {clients[i].FirstName} {clients[i].LastName}");
                    Thread.Sleep(100);
                }

                exportServiceWritingCsv.WriteClientListToCsv(clients);
                                
            });

            Thread threadReadingCsv = new Thread(() =>
            {
                ClientService clientService = new ClientService();
                
                string directoryPath = Path.Combine("C:", "Users", "Professional", "Desktop", ".net-course-2022-Podsekina");
                string fileName = "DataToDbMultithreading.csv";

                ExportService exportServiceReadingCsv = new ExportService(directoryPath, fileName);

                List<Client> clientsFromCsv = exportServiceReadingCsv.ReadClientListFromCsv();

                foreach (var client in clientsFromCsv)
                {
                    clientService.AddNewClient(client);
                    Thread.Sleep(100);
                }
                
            });

            threadWritingCsv.Start();
            threadReadingCsv.Start();

            threadWritingCsv.Join();
            threadReadingCsv.Join();
        }
    }

}
