﻿using Models;
using Services;
using Services.Filters;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ServiceTests
{
    public class ClientServiceTests
    {
        [Fact]
        public void AddNewClientLimit18YearsExceptionTest()
        {
            ClientService clientService = new ClientService(new ClientStorage());

            Client client = new Client()
            {
                FirstName = "Игорь",
                LastName = "Петоров",
                PassportID = 123456,
                DateOfBirth = new DateTime(2007, 2, 2),
                PhoneNumber = "123456789"
            };

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
            ClientService clientService = new ClientService(new ClientStorage());

            Client client = new Client()
            {
                FirstName = "Игорь",
                LastName = "Петоров",
                PassportID = 0,
                DateOfBirth = new DateTime(2000, 2, 2),
                PhoneNumber = "123456789"
            };

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
        public void AddNewAccount_NoExistsClient_And_AccountAlreadyExistsExceptionTest()
        {
            ClientService clientService = new ClientService(new ClientStorage());

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

            try
            {
                clientService.AddNewClient(client);
                clientService.AddNewAccount(client);

                Assert.Throws<KeyNotFoundException>(() => clientService.AddNewAccount(noExistsClient));
                Assert.Throws<AccountAlreadyExistsException>(() => clientService.AddNewAccount(client));
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void AccountEditing_NoExistsClient_And_NoExistsAccountExceptionTest()
        {
            ClientService clientService = new ClientService(new ClientStorage());

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

            try
            {
                clientService.AddNewClient(existsClient);
                clientService.AccountEditing(existsClient, existsAccount);

                Assert.Throws<KeyNotFoundException>(() => clientService.AccountEditing(noExistsClient,existsAccount));
                Assert.Throws<NullReferenceException>(() => clientService.AccountEditing(existsClient, noExistsAccount));
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

            Client client = clientStorage._dictionaryClient.Keys.First();
             
            // Act/Assert
            clientFilter.FirstName = client.FirstName;
            clientFilter.LastName = client.LastName;
            clientFilter.PhoneNumber = client.PhoneNumber;

            Assert.Equal(clientStorage._dictionaryClient[client], clientService.GetClients(clientFilter)[client]);

            clientFilter.FirstName = null;
            clientFilter.LastName = null;
            clientFilter.PhoneNumber = null;

            clientFilter.StartDate = clientStorage._dictionaryClient.Min(p => p.Key.DateOfBirth);

            Assert.NotNull(clientService.GetClients(clientFilter));
            clientFilter.StartDate = default;

            clientFilter.EndDate = clientStorage._dictionaryClient.Max(p => p.Key.DateOfBirth);

            Assert.NotNull(clientService.GetClients(clientFilter));
            clientFilter.StartDate = default;

            var averageAge = clientStorage._dictionaryClient.Average(p => p.Key.DateOfBirth.Year);

        }

    }
}
