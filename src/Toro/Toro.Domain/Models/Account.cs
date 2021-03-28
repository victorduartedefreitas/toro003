using System;

namespace Toro.Domain.Models
{
    public class Account
    {
        #region Properties

        public Guid AccountId { get; set; }
        public string Cpf { get; set; }
        public string Bank { get; set; }
        public string Branch { get; set; }
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }

        #endregion
    }
}
