using System.Threading.Tasks;
using Toro.Domain.Models;

namespace Toro.Domain.Repositories
{
    public interface IAccountWriteOnlyRepository
    {
        Task Save(Account account);
    }
}
