namespace Toro.WebApi.Dtos
{
    public class TransactionEventPost
    {
        public string Event { get; set; }
        public TransactionEventTargetAccount Target { get; set; }
        public TransactionEventOriginAccount Origin { get; set; }
        public decimal Amount { get; set; }
    }
}
