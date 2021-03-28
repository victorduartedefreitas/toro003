using Dapper;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Toro.Domain.Models;
using Toro.Domain.Repositories;

namespace Toro.Infra.Repository
{
    public class AccountHistoryRepository : IAccountHistoryReadOnlyRepository, IAccountHistoryWriteOnlyRepository
    {
        #region Fields

        private readonly IDbConnection dbConnection;

        #endregion

        #region Constructors

        static AccountHistoryRepository() => SqlMapper.AddTypeMap(typeof(string), DbType.AnsiString);

        public AccountHistoryRepository(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        #endregion

        #region IAccountHistoryReadOnlyRepository Members

        public async Task<AccountHistory> GetHistoryAsync(Guid accountId)
        {
            if (accountId == Guid.Empty)
                throw new ArgumentNullException(nameof(accountId));

            var queryParams = new DynamicParameters();
            queryParams.Add("AccountId", accountId);

            var query = @"SELECT HistoryId, AccountId, Date, TransactionType, Description, Amount
                            FROM AccountHistory WITH (NOLOCK)
                            WHERE AccountId = @AccountId";

            var accounts = await dbConnection.QueryAsync<AccountHistory>(query, queryParams);
            return accounts.FirstOrDefault();
        }

        #endregion

        #region IAccountHistoryWriteOnlyRepository Members

        public async Task SaveAsync(AccountHistory accountHistory)
        {
            if (accountHistory == null)
                throw new ArgumentNullException(nameof(accountHistory));

            var queryParams = new DynamicParameters();
            queryParams.Add("AccountId", accountHistory.AccountId);
            queryParams.Add("Date", accountHistory.Date);
            queryParams.Add("TransactionType", accountHistory.TransactionType);
            queryParams.Add("Description", accountHistory.Description);
            queryParams.Add("Amount", accountHistory.Amount);
            
            string query = @"INSERT INTO AccountHistory(AccountId, Date, TransactionType, Description, Amount)
                            VALUES (@AccountId, @Date, @TransactionType, @Description, @Amount)";

            await dbConnection.ExecuteAsync(query, queryParams);
        }

        #endregion
    }
}
