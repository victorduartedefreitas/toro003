using System;

namespace Toro.Domain.Models
{
    public class AccountHistory
    {
        #region Properties

        public Guid HistoryId { get; set; }
        public Guid AccountId { get; set; }
        public DateTimeOffset Date { get; set; }
        public AccountTransactionTypes TransactionType { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

        #endregion
    }
}
