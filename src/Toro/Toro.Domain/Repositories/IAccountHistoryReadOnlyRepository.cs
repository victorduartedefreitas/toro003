using System;
using System.Threading.Tasks;
using Toro.Domain.Models;

namespace Toro.Domain.Repositories
{
    public interface IAccountHistoryReadOnlyRepository
    {
        Task<AccountHistory> GetHistoryAsync(Guid accountId);
    }
}
