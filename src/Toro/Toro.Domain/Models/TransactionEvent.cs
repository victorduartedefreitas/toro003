namespace Toro.Domain.Models
{
    public class TransactionEvent
    {
        #region Properties

        public TransactionEventTypes EventType { get; set; }
        public Account Origin { get; set; }
        public Account Target { get; set; }
        public decimal Amount { get; set; }

        #endregion
    }
}
