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
        public async Task ReadingAndWritingCsv_InTwoDifferentStreams_Test()
        {                    
            Thread threadWritingCsv = new Thread(async () =>
            {
                ClientService clientService = new ClientService();
                BankContext bankContext = new BankContext();

                string directoryPath = Path.Combine("C:", "Users", "Professional", "Desktop", ".net-course-2022-Podsekina");
                string fileName = "DataFromDbMultithreading.csv";

                ExportService exportServiceWritingCsv = new ExportService(directoryPath, fileName);

                List<Client> clients = new List<Client>();

                for (int i = 0; i < bankContext.Clients.Count(); i++)
                {
                    clients.Add(clientService.GetClientsAsync(new ClientFilter(), i + 1, 1).Result.FirstOrDefault());

                    _output.WriteLine($"{clients[i].Id} {clients[i].FirstName} {clients[i].LastName}");
                    Thread.Sleep(100);
                }

                await exportServiceWritingCsv.WriteClientListToCsvAsync(clients);
                                
            });

            Thread threadReadingCsv = new Thread(async () =>
            {
                ClientService clientService = new ClientService();
                
                string directoryPath = Path.Combine("C:", "Users", "Professional", "Desktop", ".net-course-2022-Podsekina");
                string fileName = "DataToDbMultithreading.csv";

                ExportService exportServiceReadingCsv = new ExportService(directoryPath, fileName);

                List<Client> clientsFromCsv = await exportServiceReadingCsv.ReadClientListFromCsvAsync();

                foreach (var client in clientsFromCsv)
                {
                    clientService.AddNewClientAsync(client);
                    Thread.Sleep(100);
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

        }

        [Fact]
        public void AsyncStartTest_Test()
        {
            ThreadPool.SetMaxThreads(10, 10);
            ThreadPool.GetAvailableThreads(out int worker, out int completion);

            _output.WriteLine("Максимальное число рабочих потоков: " + worker);

            for (int i = 0; i < 14; i++)
            {
                StartTestTask();

                Task.Delay(100).Wait();

                ThreadPool.GetAvailableThreads(out worker, out completion);
                _output.WriteLine("Оставшиеся потоки: " + worker);
            }


            async Task StartTestTask()
            {
                await Task.Run(() =>
                {
                    _output.WriteLine("Выполняется работа");
                    Task.Delay(3000).Wait();
                });
            }
        }
    }
}

