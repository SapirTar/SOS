using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Services
{
    public interface IOrderService
    {
        Task AddOrder(Order newOrder);

    }
}