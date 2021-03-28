using System;
using System.Threading.Tasks;
using Toro.Domain.Models;

namespace Toro.Domain.Repositories
{
    public interface IAccountReadOnlyRepository
    {
        Task<Account> GetAccountAsync(Guid accountId);
        Task<Account> GetAccountAsync(string accountNumber);
    }
}
