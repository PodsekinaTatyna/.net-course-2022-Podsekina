using Models;
using Services;
using Services.Filters;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Services.Storages;
using System.Security.Principal;

namespace ServiceTests
{
    public class ClientServiceTests
    {
        [Fact]
        public void AddNewClientLimit18YearsExceptionTest()
        {
            // Arrange
            ClientService clientService = new ClientService(new ClientStorage());

            Client client = new Client()
            {
                FirstName = "Игорь",
                LastName = "Петоров",
                PassportID = 123456,
                DateOfBirth = new DateTime(2007, 2, 2),
                PhoneNumber = "123456789"
            };

            // Act/Assert
            try
            {
                clientService.AddNewClient(client);
            }
            catch (Limit18YearsException ex)
            {
                Assert.Equal(typeof(Limit18YearsException), ex.GetType());
                Assert.Equal("Клиент не может быть моложе 18 лет", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }

        }

        [Fact]
        public void AddNewClientNoPassportDataExceptionTest()
        {
            // Arrange
            ClientService clientService = new ClientService(new ClientStorage());

            Client client = new Client()
            {
                FirstName = "Игорь",
                LastName = "Петоров",
                PassportID = 0,
                DateOfBirth = new DateTime(2000, 2, 2),
                PhoneNumber = "123456789"
            };

            // Act/Assert
            try
            {
                clientService.AddNewClient(client);
            }
            catch (NoPassportDataException ex)
            {
                Assert.Equal(typeof(NoPassportDataException), ex.GetType());
                Assert.Equal("Паспортные данные обязательно должны быть введены", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void AddNewClientTryingToAddAnExistingClientTest()
        {
            // Arrange
            ClientService clientService = new ClientService(new ClientStorage());

            Client oldClient = new Client()
            {
                FirstName = "Игорь",
                LastName = "Петоров",
                PassportID = 12345,
                DateOfBirth = new DateTime(2000, 2, 2),
                PhoneNumber = "123456789"
            };
            Client newClient = new Client()
            {
                FirstName = oldClient.FirstName,
                LastName = oldClient.LastName,
                PassportID = oldClient.PassportID,
                DateOfBirth = oldClient.DateOfBirth,
                PhoneNumber = oldClient.PhoneNumber
            };

            // Act/Assert
            try
            {
                clientService.AddNewClient(oldClient);
                clientService.AddNewClient(newClient);

            }
            catch (ArgumentException ex)
            {
                Assert.Equal(typeof(ArgumentException), ex.GetType());
                Assert.Equal("Такой клиент уже существует", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }
        }

       

        [Fact]
        public void GetClients_ByTheSpecifiedFilter_Test()
        {
            // Arrange
            ClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService(clientStorage);
            TestDataGenerator testDataGenerator = new TestDataGenerator();
            ClientFilter clientFilter = new ClientFilter();

            for (int i = 0; i < 100; i++)
            {
                clientStorage.Add(testDataGenerator.GetFakeDataClient().Generate());
            }

            Client client = clientStorage.Data.Keys.First();
             
            // Act/Assert
            clientFilter.FirstName = client.FirstName;
            clientFilter.LastName = client.LastName;
            clientFilter.PhoneNumber = client.PhoneNumber;

            Assert.Equal(clientStorage.Data[client], clientService.GetClients(clientFilter)[client]);

            clientFilter.FirstName = null;
            clientFilter.LastName = null;
            clientFilter.PhoneNumber = null;

            clientFilter.StartDate = clientStorage.Data.Min(p => p.Key.DateOfBirth);

            Assert.NotNull(clientService.GetClients(clientFilter));
            clientFilter.StartDate = default;

            clientFilter.EndDate = clientStorage.Data.Max(p => p.Key.DateOfBirth);

            Assert.NotNull(clientService.GetClients(clientFilter));
            clientFilter.StartDate = default;

            var averageAge = clientStorage.Data.Average(p => p.Key.DateOfBirth.Year);

        }

        [Fact]
        public void DeleteClient_KeyNotFoundException_ClientRemoved_Test()
        {
            // Arrange
            ClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService(clientStorage);

            Client existsClient = new Client()
            {
                FirstName = "Игорь",
                LastName = "Петоров",
                PassportID = 12345,
                DateOfBirth = new DateTime(2000, 2, 2),
                PhoneNumber = "123456789"
            };
            Client noExistsClient = new Client()
            {
                FirstName = "Максим",
                LastName = "Петоров",
                PassportID = 23445,
                DateOfBirth = new DateTime(2000, 2, 2),
                PhoneNumber = "123456789"
            };

            // Act/Assert
            try
            {
                clientService.AddNewClient(existsClient);
                Assert.Throws<KeyNotFoundException>(() => clientService.DeleteClient(noExistsClient));

                clientService.DeleteClient(existsClient);
                Assert.DoesNotContain(existsClient, clientStorage.Data.Keys);
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }

        }

        [Fact]
        public void UpdateClient_KeyNotFoundException_ClientUpdated_Test()
        {
            // Arrange
            ClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService(clientStorage);

            Client existsClient = new Client()
            {
                FirstName = "Игорь",
                LastName = "Петоров",
                PassportID = 12345,
                DateOfBirth = new DateTime(2000, 2, 2),
                PhoneNumber = "123456789"
            };
            Client noExistsClient = new Client()
            {
                FirstName = "Максим",
                LastName = "Петоров",
                PassportID = 23445,
                DateOfBirth = new DateTime(2000, 2, 2),
                PhoneNumber = "123456789"
            };

            // Act/Assert
            try
            {
                clientService.AddNewClient(existsClient);
                clientService.UpdateClient(existsClient);

                Assert.Throws<KeyNotFoundException>(() => clientService.UpdateClient(noExistsClient));
                Assert.Same(existsClient, clientStorage.Data.Keys.First(p => p.PassportID == existsClient.PassportID));
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }

        }

        [Fact]
        public void AddNewAccount_NoExistsClient_And_AccountAlreadyExistsExceptionTest()
        {
            // Arrange
            ClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService(clientStorage);

            Client client = new Client()
            {
                FirstName = "Игорь",
                LastName = "Петоров",
                PassportID = 12345,
                DateOfBirth = new DateTime(2000, 2, 2),
                PhoneNumber = "123456789"
            };
            Client noExistsClient = new Client()
            {
                FirstName = "Максим",
                LastName = "Петоров",
                PassportID = 23445,
                DateOfBirth = new DateTime(2000, 2, 2),
                PhoneNumber = "123456789"
            };
            Account newAccount = new Account
            {
                Currency = new Curreny
                {
                    Code = 978,
                    Name = "EUR",
                },
                Amount = 0
            };

            // Act/Assert
            try
            {
                clientService.AddNewClient(client);
                clientService.AddNewAccount(client, newAccount);

                Assert.Throws<KeyNotFoundException>(() => clientService.AddNewAccount(noExistsClient, newAccount));
                Assert.Throws<AccountAlreadyExistsException>(() => clientService.AddNewAccount(client, newAccount));
                Assert.Contains(newAccount, clientStorage.Data[client]);
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }

        }

        [Fact]
        public void UpdateAccount_NoExistsClient_And_NoExistsAccountException_AccountUpdated_Test()
        {
            // Arrange
            ClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService(clientStorage);

            Client existsClient = new Client()
            {
                FirstName = "Игорь",
                LastName = "Петоров",
                PassportID = 12345,
                DateOfBirth = new DateTime(2000, 2, 2),
                PhoneNumber = "123456789"
            };
            Client noExistsClient = new Client()
            {
                FirstName = "Максим",
                LastName = "Петоров",
                PassportID = 23445,
                DateOfBirth = new DateTime(2000, 2, 2),
                PhoneNumber = "123456789"
            };
            Account existsAccount = new Account
            {
                Currency = new Curreny
                {
                    Code = 840,
                    Name = "USD",
                },
                Amount = 0
            };
            Account noExistsAccount = new Account
            {
                Currency = new Curreny
                {
                    Code = 978,
                    Name = "EUR",
                },
                Amount = 0
            };

            // Act/Assert
            try
            {
                clientService.AddNewClient(existsClient);
                clientService.UpdateAccount(existsClient, existsAccount);

                Assert.Throws<KeyNotFoundException>(() => clientService.UpdateAccount(noExistsClient,existsAccount));
                Assert.Throws<NullReferenceException>(() => clientService.UpdateAccount(existsClient, noExistsAccount));
                Assert.NotSame(existsAccount, clientStorage.Data[existsClient].First((p => p.Currency.Name == existsAccount.Currency.Name)));
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }

        }

        [Fact]
        public void DeleteAccount_NoExistsClient_And_NoExistsAccountException_AccountRemoved_Test()
        {
            // Arrange
            ClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService(clientStorage);

            Client existsClient = new Client()
            {
                FirstName = "Игорь",
                LastName = "Петоров",
                PassportID = 12345,
                DateOfBirth = new DateTime(2000, 2, 2),
                PhoneNumber = "123456789"
            };
            Client noExistsClient = new Client()
            {
                FirstName = "Максим",
                LastName = "Петоров",
                PassportID = 23445,
                DateOfBirth = new DateTime(2000, 2, 2),
                PhoneNumber = "123456789"
            };
            Account existsAccount = new Account
            {
                Currency = new Curreny
                {
                    Code = 840,
                    Name = "USD",
                },
                Amount = 0
            };
            Account noExistsAccount = new Account
            {
                Currency = new Curreny
                {
                    Code = 978,
                    Name = "EUR",
                },
                Amount = 0
            };

            // Act/Assert
            try
            {
                clientService.AddNewClient(existsClient);
                Assert.Throws<KeyNotFoundException>(() => clientService.DeleteAccount(noExistsClient, existsAccount));
                Assert.Throws<NullReferenceException>(() => clientService.DeleteAccount(existsClient, noExistsAccount));

                clientService.DeleteAccount(existsClient, existsAccount);
                Assert.DoesNotContain(existsAccount, clientStorage.Data[existsClient]);
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }

        }      

    }
}
