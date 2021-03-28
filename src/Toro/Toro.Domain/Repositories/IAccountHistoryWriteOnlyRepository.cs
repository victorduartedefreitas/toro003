using System.Threading.Tasks;
using Toro.Domain.Models;

namespace Toro.Domain.Repositories
{
    public interface IAccountHistoryWriteOnlyRepository
    {
        Task Save(AccountHistory accountHistory);
    }
}
