using System;
using System.Threading.Tasks;
using Toro.Domain.Models;
using Toro.Domain.Repositories;
using Toro.Domain.Services;

namespace Toro.Application.Services
{
    public class AccountService : IAccountService
    {
        #region Fields

        private readonly IAccountHistoryWriteOnlyRepository accountHistoryWriteOnlyRepository;
        private readonly IAccountReadOnlyRepository accountReadOnlyRepository;
        private readonly IAccountWriteOnlyRepository accountWriteOnlyRepository;

        #endregion

        #region Constructors

        public AccountService(IAccountHistoryWriteOnlyRepository accountHistoryWriteOnlyRepository,
            IAccountReadOnlyRepository accountReadOnlyRepository,
            IAccountWriteOnlyRepository accountWriteOnlyRepository)
        {
            this.accountHistoryWriteOnlyRepository = accountHistoryWriteOnlyRepository ?? throw new ArgumentNullException(nameof(accountHistoryWriteOnlyRepository));
            this.accountReadOnlyRepository = accountReadOnlyRepository ?? throw new ArgumentNullException(nameof(accountReadOnlyRepository));
            this.accountWriteOnlyRepository = accountWriteOnlyRepository ?? throw new ArgumentNullException(nameof(accountWriteOnlyRepository));
        }

        #endregion

        #region IAccountService Members

        public async Task<DefaultServiceReturn> Deposit(TransactionEvent transaction)
        {
            if (transaction == null)
                return new DefaultServiceReturn(false, "Objeto de transação nulo");
            if (transaction.Origin == null)
                return new DefaultServiceReturn(false, "Conta de origem não pode ser nula");
            if (transaction.Target == null)
                return new DefaultServiceReturn(false, "Conta de destino não pode ser nula");
            if (transaction.Amount <= 0)
                return new DefaultServiceReturn(false, "Valor da transação precisa ser maior que zero");
            if (transaction.Target.Cpf != transaction.Origin.Cpf)
                return new DefaultServiceReturn(false, "CPF da conta de origem é diferente do CPF da conta de destino");

            var account = await accountReadOnlyRepository.GetAccountAsync(transaction.Target.AccountNumber);
            if (account == null)
                return new DefaultServiceReturn(false, "Conta de destino inexistente");

            var history = new AccountHistory()
            {
                HistoryId = Guid.NewGuid(),
                AccountId = account.AccountId,
                TransactionType = AccountTransactionTypes.Deposit,
                Date = DateTimeOffset.Now,
                Description = "DEPOSITO",
                Amount = transaction.Amount
            };

            await accountHistoryWriteOnlyRepository.SaveAsync(history);

            account.Balance += transaction.Amount;
            await accountWriteOnlyRepository.SaveAsync(account);

            return new DefaultServiceReturn(true, string.Empty);
        }

        #endregion
    }
}
