using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Services
{
    public class StockService : IStockService
    {
        private readonly WebAppContext _context;

        public StockService(WebAppContext context)
        {
            _context = context;
        }

        public async Task AddStock(Stock newStock)
        {
            _context.Stock.Add(newStock);
            await _context.SaveChangesAsync();
        }

        //public async Task<IActionResult> Details(string id)

        public async Task<Stock> GetStock(string symbol)
        {
            Stock stock = await _context.Stock.FirstOrDefaultAsync(s => s.Symbol == symbol);
            return stock;
        }

        public async Task UpdateStockDetails(string sSymbol, string sName, double sPrice, double sChange)
        {
            Stock stock = await _context.Stock.FirstOrDefaultAsync(s => s.Symbol == sSymbol);
            stock.Name = sName;
            stock.Price = sPrice;
            stock.Change = sChange;
            await _context.SaveChangesAsync();
        }

        public async Task<double> GetPrice(string symbol)
        {
            Stock stock = await _context.Stock.FirstOrDefaultAsync(s => s.Symbol == symbol);
            return stock.Price;
        }

    }
}
