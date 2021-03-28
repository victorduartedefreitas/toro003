using System.Threading.Tasks;
using Toro.Domain.Models;

namespace Toro.Domain.Services
{
    public interface IAccountService
    {
        Task<DefaultServiceReturn> Deposit(TransactionEvent transaction);
    }
}
