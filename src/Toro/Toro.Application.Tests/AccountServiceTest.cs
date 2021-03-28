using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using Toro.Application.Services;
using Toro.Domain.Models;
using Toro.Domain.Repositories;
using Toro.Domain.Services;
using Xunit;

namespace Toro.Application.Tests
{
    public class AccountServiceTest
    {
        #region Fields

        private readonly ServiceProvider serviceProvider;

        #endregion

        #region Mock Fields

        private Guid _accountId1 = new Guid("5413730b-d1d7-4d27-877c-2adb3f1254c3");
        private string _accountNumber1 = "123456";
        private Guid _accountId2 = new Guid("cbdbd0c6-2372-4904-ae16-1ebc67074226");
        private string _accountNumber2 = "789012";
        private Guid _accountId3 = new Guid("a511e48c-61bb-4621-879d-dbec0f52e423");
        private string _accountNumber3 = "345678";

        private Account _acc1 = new Account()
        {
            AccountId = new Guid("5413730b-d1d7-4d27-877c-2adb3f1254c3"),
            AccountNumber = "123456",
            Bank = "352",
            Branch = "0001",
            Cpf = "11111111111",
            Name = "Fulano de Teste",
            Balance = 1030.50m
        };
        private Account _acc2 = new Account()
        {
            AccountId = new Guid("cbdbd0c6-2372-4904-ae16-1ebc67074226"),
            AccountNumber = "789012",
            Bank = "352",
            Branch = "0001",
            Cpf = "22222222222",
            Name = "Ciclano de Teste",
            Balance = 349.12m
        };
        private Account _acc3 = new Account()
        {
            AccountId = new Guid("a511e48c-61bb-4621-879d-dbec0f52e423"),
            AccountNumber = "345678",
            Bank = "352",
            Branch = "0001",
            Cpf = "33333333333",
            Name = "Beltrano de Teste",
            Balance = 12583.87m
        };

        #endregion

        #region Constructors

        public AccountServiceTest()
        {
            var accountReadOnlyRepoMock = new Mock<IAccountReadOnlyRepository>();
            var accountWriteOnlyRepositoryMock = new Mock<IAccountWriteOnlyRepository>();
            var accountHistoryWriteOnlyRepositoryMock = new Mock<IAccountHistoryWriteOnlyRepository>();

            MockAccountReadOnlyRepository(accountReadOnlyRepoMock);
            MockAccountWriteOnlyRepository(accountWriteOnlyRepositoryMock);
            MockAccountHistoryWriteOnlyRepository(accountHistoryWriteOnlyRepositoryMock);

            IServiceCollection services = new ServiceCollection();

            services.AddScoped<IAccountService, AccountService>();

            services.AddTransient(f => accountReadOnlyRepoMock.Object);
            services.AddTransient(f => accountWriteOnlyRepositoryMock.Object);
            services.AddTransient(f => accountHistoryWriteOnlyRepositoryMock.Object);

            serviceProvider = services.BuildServiceProvider();
        }

        #endregion

        #region Mock Fake Repositories

        private void MockAccountReadOnlyRepository(Mock<IAccountReadOnlyRepository> mock)
        {
            mock
                .Setup(f => f.GetAccountAsync(_accountId1))
                .ReturnsAsync(_acc1);

            mock
                .Setup(f => f.GetAccountAsync(_accountId2))
                .ReturnsAsync(_acc2);

            mock
                .Setup(f => f.GetAccountAsync(_accountId3))
                .ReturnsAsync(_acc3);

            mock
                .Setup(f => f.GetAccountAsync(_accountNumber1))
                .ReturnsAsync(_acc1);
            mock
                .Setup(f => f.GetAccountAsync(_accountNumber2))
                .ReturnsAsync(_acc2);
            mock
                .Setup(f => f.GetAccountAsync(_accountNumber3))
                .ReturnsAsync(_acc3);
        }

        private void MockAccountWriteOnlyRepository(Mock<IAccountWriteOnlyRepository> mock)
        {
            mock.Setup(f => f.SaveAsync(It.IsAny<Account>()))
                .Returns(Task.FromResult(true));
        }

        private void MockAccountHistoryWriteOnlyRepository(Mock<IAccountHistoryWriteOnlyRepository> mock)
        {
            mock.Setup(f => f.SaveAsync(It.IsAny<AccountHistory>()))
                .Returns(Task.FromResult(true));
        }

        #endregion

        #region Test Methods

        [Fact]
        public async Task Deposit_NullTransaction_Test()
        {
            var accountService = serviceProvider.GetService<IAccountService>();

            var result = await accountService.Deposit(null);

            Assert.False(result.Success);
            Assert.True(result.Message.ToUpper() == "OBJETO DE TRANSAÇÃO NULO");
        }

        [Fact]
        public async Task Deposit_NullOriginAccount_Test()
        {
            var accountService = serviceProvider.GetService<IAccountService>();

            var transactionEvent = new TransactionEvent()
            {
                Amount = 1000m,
                EventType = TransactionEventTypes.Transfer,
                Origin = null,
                Target = new Account()
                {
                    Bank = "352",
                    Branch = "0001",
                    AccountNumber = "123456"
                }
            };

            var result = await accountService.Deposit(transactionEvent);

            Assert.False(result.Success);
            Assert.True(result.Message.ToUpper() == "CONTA DE ORIGEM NÃO PODE SER NULA");
        }

        [Fact]
        public async Task Deposit_NullTargetAccount_Test()
        {
            var accountService = serviceProvider.GetService<IAccountService>();

            var transactionEvent = new TransactionEvent()
            {
                Amount = 1000m,
                EventType = TransactionEventTypes.Transfer,
                Origin = new Account()
                {
                    Bank = "371",
                    Branch = "0123",
                    Cpf = "11111111111"
                },
                Target = null
            };

            var result = await accountService.Deposit(transactionEvent);

            Assert.False(result.Success);
            Assert.True(result.Message.ToUpper() == "CONTA DE DESTINO NÃO PODE SER NULA");
        }

        [Fact]
        public async Task Deposit_AmountEqualsZero_Test()
        {
            var accountService = serviceProvider.GetService<IAccountService>();

            var transactionEvent = new TransactionEvent()
            {
                Amount = 0m,
                EventType = TransactionEventTypes.Transfer,
                Origin = new Account()
                {
                    Bank = "371",
                    Branch = "0123",
                    Cpf = "11111111111"
                },
                Target = new Account()
                {
                    Bank = "352",
                    Branch = "0001",
                    AccountNumber = "123456"
                }
            };

            var result = await accountService.Deposit(transactionEvent);

            Assert.False(result.Success);
            Assert.True(result.Message.ToUpper() == "VALOR DA TRANSAÇÃO PRECISA SER MAIOR QUE ZERO");
        }

        [Fact]
        public async Task Deposit_AmountLessThanZero_Test()
        {
            var accountService = serviceProvider.GetService<IAccountService>();

            var transactionEvent = new TransactionEvent()
            {
                Amount = -0.1m,
                EventType = TransactionEventTypes.Transfer,
                Origin = new Account()
                {
                    Bank = "371",
                    Branch = "0123",
                    Cpf = "11111111111"
                },
                Target = new Account()
                {
                    Bank = "352",
                    Branch = "0001",
                    AccountNumber = "123456"
                }
            };

            var result = await accountService.Deposit(transactionEvent);

            Assert.False(result.Success);
            Assert.True(result.Message.ToUpper() == "VALOR DA TRANSAÇÃO PRECISA SER MAIOR QUE ZERO");
        }

        [Fact]
        public async Task Deposit_InexistentTargetAccount_Test()
        {
            var accountService = serviceProvider.GetService<IAccountService>();

            var transactionEvent = new TransactionEvent()
            {
                Amount = 500m,
                EventType = TransactionEventTypes.Transfer,
                Origin = new Account()
                {
                    Bank = "371",
                    Branch = "0123",
                    Cpf = "11111111111"
                },
                Target = new Account()
                {
                    Bank = "352",
                    Branch = "0001",
                    AccountNumber = "998877"
                }
            };

            var result = await accountService.Deposit(transactionEvent);

            Assert.False(result.Success);
            Assert.True(result.Message.ToUpper() == "CONTA DE DESTINO INEXISTENTE");
        }

        [Fact]
        public async Task Deposit_DifferentCpf_Test()
        {
            var accountService = serviceProvider.GetService<IAccountService>();

            var transactionEvent = new TransactionEvent()
            {
                Amount = 500m,
                EventType = TransactionEventTypes.Transfer,
                Origin = new Account()
                {
                    Bank = "001",
                    Branch = "6453",
                    Cpf = "55555555555"
                },
                Target = new Account()
                {
                    Bank = "352",
                    Branch = "0001",
                    AccountNumber = "123456"
                }
            };

            var result = await accountService.Deposit(transactionEvent);

            Assert.False(result.Success);
            Assert.True(result.Message.ToUpper() == "CPF DA CONTA DE ORIGEM É DIFERENTE DO CPF DA CONTA DE DESTINO");
        }

        [Fact]
        public async Task Deposit_Successful_Test()
        {
            var accountService = serviceProvider.GetService<IAccountService>();

            var transactionEvent = new TransactionEvent()
            {
                Amount = 500m,
                EventType = TransactionEventTypes.Transfer,
                Origin = new Account()
                {
                    Bank = "001",
                    Branch = "0123",
                    Cpf = "11111111111"
                },
                Target = new Account()
                {
                    Bank = "352",
                    Branch = "0001",
                    AccountNumber = "123456"
                }
            };

            var result = await accountService.Deposit(transactionEvent);

            Assert.True(result.Success);
            Assert.True(string.IsNullOrWhiteSpace(result.Message));
        }

        #endregion
    }
}
