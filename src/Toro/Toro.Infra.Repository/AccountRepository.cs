using Dapper;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Toro.Domain.Models;
using Toro.Domain.Repositories;

namespace Toro.Infra.Repository
{
    public class AccountRepository : IAccountReadOnlyRepository, IAccountWriteOnlyRepository
    {
        #region Fields

        private readonly IDbConnection dbConnection;

        #endregion

        #region Constructors

        static AccountRepository() => SqlMapper.AddTypeMap(typeof(string), DbType.AnsiString);

        public AccountRepository(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        #endregion

        #region IAccountReadOnlyRepository Members

        public async Task<Account> GetAccountAsync(Guid accountId)
        {
            if (accountId == Guid.Empty)
                throw new ArgumentNullException(nameof(accountId));

            var queryParams = new DynamicParameters();
            queryParams.Add("AccountId", accountId);

            var query = @"SELECT AccountId, Cpf, Bank, Branch, AccountNumber, Name, Balance
                            FROM Account WITH (NOLOCK)
                            WHERE AccountId = @AccountId";

            var accounts = await dbConnection.QueryAsync<Account>(query, queryParams);
            return accounts.FirstOrDefault();
        }

        public async Task<Account> GetAccountAsync(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentNullException(nameof(accountNumber));

            var queryParams = new DynamicParameters();
            queryParams.Add("AccountNumber", accountNumber);

            var query = @"SELECT AccountId, Cpf, Bank, Branch, AccountNumber, Name, Balance
                            FROM Account WITH (NOLOCK)
                            WHERE AccountNumber = @AccountNumber";

            var accounts = await dbConnection.QueryAsync<Account>(query, queryParams);
            return accounts.FirstOrDefault();
        }

        #endregion

        #region IAccountWriteOnlyRepository Members

        public async Task SaveAsync(Account account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));

            var accountParams = new DynamicParameters();
            accountParams.Add("Cpf", account.Cpf);
            accountParams.Add("Bank", account.Bank);
            accountParams.Add("Branch", account.Branch);
            accountParams.Add("AccountNumber", account.AccountNumber);
            accountParams.Add("Name", account.Name);
            accountParams.Add("Balance", account.Balance);

            string query;
            if (account.AccountId == Guid.Empty)
            {
                query = @"INSERT INTO Account(Cpf, Bank, Branch, AccountNumber, Name, Balance)
                            VALUES (@Cpf, @Bank, @Branch, @AccountNumber, @Name, @Balance)";
            }
            else
            {
                accountParams.Add("AccountId", account.AccountId);

                query = @"UPDATE Account
                            SET Cpf = @Cpf,
                                Bank = @Bank,
                                Branch = @Branch,
                                AccountNumber = @AccountNumber,
                                Name = @Name,
                                Balance = @Balance
                            WHERE AccountId = @AccountId";
            }

            await dbConnection.ExecuteAsync(query, accountParams);
        }

        #endregion
    }
}
