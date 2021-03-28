using System.Threading.Tasks;
using Toro.Domain.Models;

namespace Toro.Domain.Repositories
{
    public interface IAccountHistoryWriteOnlyRepository
    {
        Task SaveAsync(AccountHistory accountHistory);
    }
}
