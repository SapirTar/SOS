using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Services
{
    public class UserService : IUserService
    {
        private readonly WebAppContext _context;

        public UserService(WebAppContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser(string username)
        {
            User user = await _context.User.FirstOrDefaultAsync(u => u.Username == username);
            return user;
        }

        public int GetStocksAmount(string username, string symbol)
        {
            User user = _context.User.FirstOrDefault(u => u.Username == username);
            Stock stock = _context.Stock.FirstOrDefault(s => s.Symbol == symbol);
            int amount = 0;
            foreach (var s in user.OwnedStocks)
            {
                if (s.Symbol == symbol)
                {
                    amount++;
                }
            }
            return amount;
        }

        public async Task AddStockToList(Stock stock, string username)
        {
            User user = _context.User.Include(u => u.OwnedStocks).FirstOrDefault(u => u.Username == username);
            if (user.OwnedStocks == null)
            {
                user.OwnedStocks = new List<Stock>();
            }
            user.OwnedStocks.Add(stock);
            await _context.SaveChangesAsync();
        }
    }
}
