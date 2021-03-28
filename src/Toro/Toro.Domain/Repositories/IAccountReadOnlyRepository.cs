using System;
using System.Threading.Tasks;
using Toro.Domain.Models;

namespace Toro.Domain.Repositories
{
    public interface IAccountReadOnlyRepository
    {
        Task<Account> GetAccount(Guid accountId);
        Task<Account> GetAccount(string accountNumber);
    }
}
