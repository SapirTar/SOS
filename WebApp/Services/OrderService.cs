using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly WebAppContext _context;

        public OrderService(WebAppContext context)
        {
            _context = context;
        }

        public async Task AddOrder(Order newOrder)
        {
            _context.Order.Add(newOrder);
            await _context.SaveChangesAsync();
        }

    }
}
