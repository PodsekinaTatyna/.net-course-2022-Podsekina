using Models;
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
            ClientService clientService = new ClientService(new ClientStorage());

            ClientDb client = new ClientDb()
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

            ClientDb client = new ClientDb()
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
            TestDataGenerator testDataGenerator = new TestDataGenerator();

            ClientDb oldClient = testDataGenerator.GetFakeDataClient().Generate();

            ClientDb newClient = new ClientDb()
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
            ClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService(clientStorage);
            TestDataGenerator testDataGenerator = new TestDataGenerator();
            ClientFilter clientFilter = new ClientFilter();

            for (int i = 0; i < 10; i++)
            {
                clientStorage.Add(testDataGenerator.GetFakeDataClient().Generate());
            }

            ClientDb client = clientStorage.Data.Clients.FirstOrDefault();
             
            // Act/Assert
            clientFilter.FirstName = client.FirstName;
            clientFilter.LastName = client.LastName;
            clientFilter.PhoneNumber = client.PhoneNumber;

            Assert.NotNull(clientService.GetClients(clientFilter));

            clientFilter.FirstName = null;
            clientFilter.LastName = null;
            clientFilter.PhoneNumber = null;

            clientFilter.StartDate = clientStorage.Data.Clients.Min(p => p.DateOfBirth);

            Assert.NotNull(clientService.GetClients(clientFilter));
            clientFilter.StartDate = default;

            clientFilter.EndDate = clientStorage.Data.Clients.Max(p => p.DateOfBirth);

            Assert.NotNull(clientService.GetClients(clientFilter));
            clientFilter.StartDate = default;

            var averageAge = clientStorage.Data.Clients.Average(p => p.DateOfBirth.Year);

        }

        [Fact]
        public void DeleteClient_KeyNotFoundException_ClientRemoved_Test()
        {
            // Arrange
            ClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService(clientStorage);
            TestDataGenerator testDataGenerator = new TestDataGenerator();

            ClientDb existsClient = testDataGenerator.GetFakeDataClient().Generate();

            ClientDb noExistsClient = testDataGenerator.GetFakeDataClient().Generate();

            // Act/Assert
            try
            {
                clientService.AddNewClient(existsClient);
                Assert.Throws<KeyNotFoundException>(() => clientService.DeleteClient(noExistsClient));

                clientService.DeleteClient(existsClient);
                Assert.DoesNotContain(existsClient, clientStorage.Data.Clients);
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
            TestDataGenerator testDataGenerator = new TestDataGenerator();

            ClientDb existsClient = testDataGenerator.GetFakeDataClient().Generate();

            ClientDb noExistsClient = testDataGenerator.GetFakeDataClient().Generate();

            // Act/Assert
            try
            {
                clientService.AddNewClient(existsClient);
                clientService.UpdateClient(existsClient);

                Assert.Throws<KeyNotFoundException>(() => clientService.UpdateClient(noExistsClient));
                Assert.Same(existsClient, clientStorage.Data.Clients.First(p => p.PassportID == existsClient.PassportID));
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
            TestDataGenerator testDataGenerator = new TestDataGenerator();

            ClientDb client = testDataGenerator.GetFakeDataClient().Generate();

            ClientDb noExistsClient = testDataGenerator.GetFakeDataClient().Generate();

            AccountDb newAccount = new AccountDb
            {
                clientId = client.Id,
                Currency = "EUR",
                Amount = 0
            };

            // Act/Assert
            try
            {
                clientService.AddNewClient(client);
                clientService.AddNewAccount(client.Id, newAccount);

                Assert.Throws<KeyNotFoundException>(() => clientService.AddNewAccount(noExistsClient.Id, newAccount));
                Assert.Throws<AccountAlreadyExistsException>(() => clientService.AddNewAccount(client.Id, newAccount));
                Assert.NotNull(clientStorage.Data.Accounts.FirstOrDefault(p => p.clientId == newAccount.clientId && p.Currency == newAccount.Currency));
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
            TestDataGenerator testDataGenerator = new TestDataGenerator();

            ClientDb existsClient = testDataGenerator.GetFakeDataClient().Generate();

            ClientDb noExistsClient = testDataGenerator.GetFakeDataClient().Generate();

            
            AccountDb noExistsAccount = new AccountDb
            {
                clientId = existsClient.Id,
                Currency = "EUR",
                Amount = 0
            };

            // Act/Assert
            try
            {
                clientService.AddNewClient(existsClient);

                var existsAccount = existsClient.Accounts.First(p => p.Currency == "USD");

                clientService.UpdateAccount(existsClient.Id, existsAccount);

                Assert.Throws<KeyNotFoundException>(() => clientService.UpdateAccount(noExistsClient.Id,existsAccount));
                Assert.Throws<NullReferenceException>(() => clientService.UpdateAccount(existsClient.Id, noExistsAccount));
                Assert.NotSame(existsAccount, clientStorage.Data.Accounts.First((p => p.Currency == existsAccount.Currency)));
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
            TestDataGenerator testDataGenerator = new TestDataGenerator();

            ClientDb existsClient = testDataGenerator.GetFakeDataClient().Generate();

            ClientDb noExistsClient = testDataGenerator.GetFakeDataClient().Generate();

            AccountDb noExistsAccount = new AccountDb
            {
                clientId = existsClient.Id,
                Currency = "EUR",
                Amount = 0
            };

            // Act/Assert
            try
            {
                clientService.AddNewClient(existsClient);
                var existsAccount = existsClient.Accounts.First(p => p.Currency == "USD");

                Assert.Throws<KeyNotFoundException>(() => clientService.DeleteAccount(noExistsClient.Id, existsAccount));
                Assert.Throws<NullReferenceException>(() => clientService.DeleteAccount(existsClient.Id, noExistsAccount));

                clientService.DeleteAccount(existsClient.Id, existsAccount);
                Assert.Null(clientStorage.Data.Accounts.FirstOrDefault(p => p.clientId == existsAccount.clientId && p.Currency == existsAccount.Currency));
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }

        }      

    }
}
