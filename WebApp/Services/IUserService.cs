using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Services
{
    public interface IUserService
    {
        Task<User> GetUser(string username);
        int GetStocksAmount(string username, string symbol);
        Task AddStockToList(Stock stock, string username);

    }
}