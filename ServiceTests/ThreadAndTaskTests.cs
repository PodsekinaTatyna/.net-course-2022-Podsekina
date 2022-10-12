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
                BankContext bankContext = new BankContext();

                string directoryPath = Path.Combine("C:", "Users", "Professional", "Desktop", ".net-course-2022-Podsekina");
                string fileName = "DataFromDbMultithreading.csv";

                ExportService exportService = new ExportService(directoryPath, fileName);

                List<Client> clients = new List<Client>();
                lock (locker)
                {
                    foreach (var client in bankContext.Clients)
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
                        Thread.Sleep(100);
                    }

                    exportService.WriteClientListToCsv(clients);
                }

                foreach (var client in clients)
                {
                    _output.WriteLine($"{client.Id} {client.FirstName} {client.LastName}");
                }
                
            });

            Thread threadReadingCsv = new Thread(() =>
            {
                ClientService clientService = new ClientService();

                string directoryPath = Path.Combine("C:", "Users", "Professional", "Desktop", ".net-course-2022-Podsekina");
                string fileName = "DataToDbMultithreading.csv";

                ExportService exportService = new ExportService(directoryPath, fileName);

                lock (locker)
                {
                    List<Client> clientsFromCsv = exportService.ReadClientListFromCsv();

                    foreach (var client in clientsFromCsv)
                    {
                        clientService.AddNewClient(client);
                    }
                }
            });

            threadWritingCsv.Start();
            threadReadingCsv.Start();

            threadWritingCsv.Join();
            threadReadingCsv.Join();
        }

        [Fact]
        public void RateUpdater_Test()
        {
            RateUpdaterService rateUpdater = new RateUpdaterService(new ClientService());

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            var taskRateUpdater = rateUpdater.AccruingInterest(cancellationToken);
            taskRateUpdater.Wait(20000);

            cancellationTokenSource.Cancel();

        }


        [Fact]
        public void AccountCashingOut_Test()
        {
            CashDispenserService cashDispenser = new CashDispenserService();
            BankContext _bankContext = new BankContext();

            var tasks = new List<Task>();

            for (int i = 0; i < 10; i++)
            {
                var accountDb = _bankContext.Accounts.Skip(i).Take(1).SingleOrDefault();

                var account = new Account
                {
                    Amount = accountDb.Amount,
                    Currency = new Curreny { Name = accountDb.CurrencyName }
                };

                tasks.Add(cashDispenser.AccountCashingOut(accountDb.ClientId, account));
                Task.Delay(1000).Wait();
            }

            foreach (Task task in tasks)
            {
                task.Wait();
            }

        }
    }
}

}
