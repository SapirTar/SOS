using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Services
{
    public interface IStockService
    {
        Task<Stock> GetStock(string symbol);
        Task UpdateStockDetails(string sSymbol, string sName, double sPrice, double sChange);
        Task AddStock(Stock newStock);
        Task<double> GetPrice(string symbol);
    }
}