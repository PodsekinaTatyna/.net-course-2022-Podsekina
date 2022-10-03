﻿using Models;
using ModelsDb;
using Services;
using Services.Filters;
using Services.Exceptions;
using System.Linq;
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
            ClientService clientService = new ClientService();

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
            ClientService clientService = new ClientService();

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
            ClientService clientService = new ClientService();
            TestDataGenerator testDataGenerator = new TestDataGenerator();

            Client oldClient = testDataGenerator.GetFakeDataClient().Generate();

            Client newClient = new Client()
            {
                Id = oldClient.Id,
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
            ClientService clientService = new ClientService();
            TestDataGenerator testDataGenerator = new TestDataGenerator();
            ClientFilter clientFilter = new ClientFilter();
            Client client = new Client();

            for (int i = 0; i < 10; i++)
            {
                client = testDataGenerator.GetFakeDataClient().Generate();
                clientService.AddNewClient(client);
            };

            int page = 1;
            int limit = 10;
             
            // Act/Assert
            clientFilter.FirstName = client.FirstName;
            clientFilter.LastName = client.LastName;
            clientFilter.PhoneNumber = client.PhoneNumber;

            Assert.NotNull(clientService.GetClients(clientFilter, page, limit));

            clientFilter.FirstName = null;
            clientFilter.LastName = null;
            clientFilter.PhoneNumber = null;

            clientFilter.StartDate = clientService.bankContext.Clients.Min(p => p.DateOfBirth);

            Assert.NotNull(clientService.GetClients(clientFilter, page, limit));
            clientFilter.StartDate = default;

            clientFilter.EndDate = clientService.bankContext.Clients.Max(p => p.DateOfBirth);

            Assert.NotNull(clientService.GetClients(clientFilter, page, limit));
            clientFilter.StartDate = default;

            var averageAge = clientService.bankContext.Clients.Average(p => p.DateOfBirth.Year);

        }

        [Fact]
        public void DeleteClient_KeyNotFoundException_ClientRemoved_Test()
        {
            // Arrange
            ClientService clientService = new ClientService();
            TestDataGenerator testDataGenerator = new TestDataGenerator();

            Client existsClient = testDataGenerator.GetFakeDataClient().Generate();

            Client noExistsClient = testDataGenerator.GetFakeDataClient().Generate();

            // Act/Assert
            try
            {
                clientService.AddNewClient(existsClient);
                Assert.Throws<KeyNotFoundException>(() => clientService.DeleteClient(noExistsClient));

                clientService.DeleteClient(existsClient);
                Assert.Null(clientService.bankContext.Clients.FirstOrDefault(p => p.Id == existsClient.Id));
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
            ClientService clientService = new ClientService();
            TestDataGenerator testDataGenerator = new TestDataGenerator();

            Client existsClient = testDataGenerator.GetFakeDataClient().Generate();

            Client noExistsClient = testDataGenerator.GetFakeDataClient().Generate();

            // Act/Assert
            try
            {
                clientService.AddNewClient(existsClient);
                clientService.UpdateClient(existsClient);

                Assert.Throws<KeyNotFoundException>(() => clientService.UpdateClient(noExistsClient));
             
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
            ClientService clientService = new ClientService();
            TestDataGenerator testDataGenerator = new TestDataGenerator();

            Client client = testDataGenerator.GetFakeDataClient().Generate();

            Client noExistsClient = testDataGenerator.GetFakeDataClient().Generate();

            Account newAccount = new Account
            {
                Currency = new Curreny
                {
                    Name = "EUR",
                    Code = 978
                },
                Amount = 0
            };

            // Act/Assert
            try
            {
                clientService.AddNewClient(client);
                clientService.AddNewAccount(client.Id, newAccount);

                Assert.Throws<KeyNotFoundException>(() => clientService.AddNewAccount(noExistsClient.Id, newAccount));
                Assert.Throws<AccountAlreadyExistsException>(() => clientService.AddNewAccount(client.Id, newAccount));
                Assert.NotNull(clientService.bankContext.Accounts.FirstOrDefault(p => p.ClientId == client.Id && p.CurrencyName == newAccount.Currency.Name));
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
            ClientService clientService = new ClientService();
            TestDataGenerator testDataGenerator = new TestDataGenerator();

            Client existsClient = testDataGenerator.GetFakeDataClient().Generate();

            Client noExistsClient = testDataGenerator.GetFakeDataClient().Generate();

            
            Account noExistsAccount = new Account
            {
                Currency = new Curreny 
                {
                    Name = "EUR",
                    Code = 978
                },
                Amount = 0
            };
            Account existsAccount = new Account
            {
                Currency = new Curreny 
                { 
                    Name = "USD",
                    Code = 840
                },
                Amount = 0
            };

            // Act/Assert
            try
            {
                clientService.AddNewClient(existsClient);
                clientService.UpdateAccount(existsClient.Id, existsAccount);

                Assert.Throws<KeyNotFoundException>(() => clientService.UpdateAccount(noExistsClient.Id,existsAccount));
                Assert.Throws<NullReferenceException>(() => clientService.UpdateAccount(existsClient.Id, noExistsAccount));

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
            ClientService clientService = new ClientService();
            TestDataGenerator testDataGenerator = new TestDataGenerator();

            Client existsClient = testDataGenerator.GetFakeDataClient().Generate();

            Client noExistsClient = testDataGenerator.GetFakeDataClient().Generate();

            Account noExistsAccount = new Account
            {
                Currency = new Curreny
                {
                    Name = "EUR",
                    Code = 978
                },
                Amount = 0
            };
            Account existsAccount = new Account
            {
                Currency = new Curreny
                {
                    Name = "USD",
                    Code = 840
                },
                Amount = 0
            };

            // Act/Assert
            try
            {
                clientService.AddNewClient(existsClient);           

                Assert.Throws<KeyNotFoundException>(() => clientService.DeleteAccount(noExistsClient.Id, existsAccount));
                Assert.Throws<NullReferenceException>(() => clientService.DeleteAccount(existsClient.Id, noExistsAccount));

                clientService.DeleteAccount(existsClient.Id, existsAccount);
                Assert.Null(clientService.bankContext.Accounts.FirstOrDefault(p => p.ClientId == existsClient.Id && p.CurrencyName == existsAccount.Currency.Name));
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }

        }      

    }
}
