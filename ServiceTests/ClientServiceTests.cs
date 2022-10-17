using Models;
using ModelsDb;
using Services;
using Services.Filters;
using Services.Exceptions;
using System.Linq;
using Xunit;
using Services.Storages;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;

namespace ServiceTests
{
    public class ClientServiceTests
    {

        [Fact]
        public async Task AddNewClientLimit18YearsExceptionTest()
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
                await clientService.AddNewClientAsync(client);
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
        public async Task AddNewClientNoPassportDataExceptionTest()
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
                await clientService.AddNewClientAsync(client);
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
        public async Task AddNewClientTryingToAddAnExistingClientTest()
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
                await clientService.AddNewClientAsync(oldClient);
                await clientService.AddNewClientAsync(newClient);

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
        public async Task GetClients_ByTheSpecifiedFilter_Test()
        {
            // Arrange
            ClientService clientService = new ClientService();
            TestDataGenerator testDataGenerator = new TestDataGenerator();
            ClientFilter clientFilter = new ClientFilter();
            Client client = new Client();

            for (int i = 0; i < 10; i++)
            {
                client = testDataGenerator.GetFakeDataClient().Generate();
                await clientService.AddNewClientAsync(client);
            };

            int page = 1;
            int limit = 10;
             
            // Act/Assert
            clientFilter.FirstName = client.FirstName;
            clientFilter.LastName = client.LastName;
            clientFilter.PhoneNumber = client.PhoneNumber;

            Assert.NotNull(clientService.GetClientsAsync(clientFilter, page, limit));

            clientFilter.FirstName = null;
            clientFilter.LastName = null;
            clientFilter.PhoneNumber = null;

            clientFilter.StartDate = await clientService.bankContext.Clients.MinAsync(p => p.DateOfBirth);

            Assert.NotNull(await clientService.GetClientsAsync(clientFilter, page, limit));
            clientFilter.StartDate = default;

            clientFilter.EndDate = await clientService.bankContext.Clients.MaxAsync(p => p.DateOfBirth);

            Assert.NotNull(await clientService.GetClientsAsync(clientFilter, page, limit));
            clientFilter.StartDate = default;

            var averageAge = await clientService.bankContext.Clients.AverageAsync(p => p.DateOfBirth.Year);

        }

        [Fact]
        public async Task DeleteClient_KeyNotFoundException_ClientRemoved_Test()
        {
            // Arrange
            ClientService clientService = new ClientService();
            TestDataGenerator testDataGenerator = new TestDataGenerator();

            Client existsClient = testDataGenerator.GetFakeDataClient().Generate();

            Client noExistsClient = testDataGenerator.GetFakeDataClient().Generate();

            // Act/Assert
            try
            {
                await clientService.AddNewClientAsync(existsClient);
                await Assert.ThrowsAsync<KeyNotFoundException>(() => clientService.DeleteClientAsync(noExistsClient));

                await clientService.DeleteClientAsync(existsClient);
                Assert.Null(await clientService.bankContext.Clients.FirstOrDefaultAsync(p => p.Id == existsClient.Id));
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }

        }

        [Fact]
        public async Task UpdateClient_KeyNotFoundException_ClientUpdated_Test()
        {
            // Arrange
            ClientService clientService = new ClientService();
            TestDataGenerator testDataGenerator = new TestDataGenerator();

            Client existsClient = testDataGenerator.GetFakeDataClient().Generate();

            Client noExistsClient = testDataGenerator.GetFakeDataClient().Generate();

            // Act/Assert
            try
            {
                await clientService.AddNewClientAsync(existsClient);
                await clientService.UpdateClientAsync(existsClient);

                await Assert.ThrowsAsync<KeyNotFoundException>(() => clientService.UpdateClientAsync(noExistsClient));

            }
            catch (Exception ex)
            {
                Assert.True(false);
            }

        }

        [Fact]
        public async Task AddNewAccount_NoExistsClient_And_AccountAlreadyExistsExceptionTest()
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
                await clientService.AddNewClientAsync(client);
                await clientService.AddNewAccountAsync(client.Id, newAccount);

                await Assert.ThrowsAsync<KeyNotFoundException>(() => clientService.AddNewAccountAsync(noExistsClient.Id, newAccount));
                await Assert.ThrowsAsync<AccountAlreadyExistsException>(() => clientService.AddNewAccountAsync(client.Id, newAccount));
                Assert.NotNull(await clientService.bankContext.Accounts.FirstOrDefaultAsync(p => p.ClientId == client.Id && p.CurrencyName == newAccount.Currency.Name));
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }

        }

        [Fact]
        public async Task UpdateAccount_NoExistsClient_And_NoExistsAccountException_AccountUpdated_Test()
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
                await clientService.AddNewClientAsync(existsClient);
                await clientService.UpdateAccountAsync(existsClient.Id, existsAccount);

                await Assert.ThrowsAsync<KeyNotFoundException>(() => clientService.UpdateAccountAsync(noExistsClient.Id, existsAccount));
                await Assert.ThrowsAsync<NullReferenceException>(() => clientService.UpdateAccountAsync(existsClient.Id, noExistsAccount));

            }
            catch (Exception ex)
            {
                Assert.True(false);
            }

        }

        [Fact]
        public async Task DeleteAccount_NoExistsClient_And_NoExistsAccountException_AccountRemoved_Test()
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
                await clientService.AddNewClientAsync(existsClient);

                await Assert.ThrowsAsync<KeyNotFoundException>(() => clientService.DeleteAccountAsync(noExistsClient.Id, existsAccount));
                await Assert.ThrowsAsync<NullReferenceException>(() => clientService.DeleteAccountAsync(existsClient.Id, noExistsAccount));

                await clientService.DeleteAccountAsync(existsClient.Id, existsAccount);
                Assert.Null(await clientService.bankContext.Accounts.FirstOrDefaultAsync(p => p.ClientId == existsClient.Id && p.CurrencyName == existsAccount.Currency.Name));
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }

        }

    }
}
