using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class StocksController : Controller
    {
        private readonly WebAppContext _context;
        private readonly IStockService _stockService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;

        public StocksController(WebAppContext context, IStockService stockService, IUserService userService, IOrderService orderService)
        {
            _context = context;
            _stockService = stockService;
            _userService = userService;

        }

        // GET: Stocks
        public async Task<IActionResult> Index()
        {
            if (!_context.Stock.Any())
            {
                IntialStockTable();
            }
            {/*var symbol1s = new List<string>()
                    {
                        "IBM",
                        "MSFT",
                        "Teva",
                        "UAL",
                        "ELALF",
                        "M1RN34.SAO",
                        "PFE",
                        "TSLA",
                        "GOOGL",
                        "BMWYY"
                    };*/
                }       
            
            
            return View(await _context.Stock.ToListAsync());
        }

        public async void IntialStockTable()
        {
            var Initializing = new List<List<string>>();
            Initializing.Add(new List<string> { "IBM", "Technology" });
            Initializing.Add(new List<string> { "Teva", "Parmaceutical" });
            Initializing.Add(new List<string> { "UAL", "Airline" });
            Initializing.Add(new List<string> { "ELALF", "Airline" });
            Initializing.Add(new List<string> { "PFE", "Parmaceutical" });
            Initializing.Add(new List<string> { "MSFT", "Technology" });
            Initializing.Add(new List<string> { "M1RN34.SAO", "Parmaceutical" });
            Initializing.Add(new List<string> { "GOOGL", "Technology" });
            Initializing.Add(new List<string> { "TSLA", "Vehicle manufacturer" });
            Initializing.Add(new List<string> { "BMWYY", "Vehicle manufacturer" });

            foreach (var ls in Initializing)
            {
                await _stockService.AddStock(new Stock { Symbol = ls[0], Name = "", Price = 1, Change = 1, Category = ls[1] });
            };

            using (WebClient client = new WebClient())
            {
                var symbols = (from s in _context.Stock select s.Symbol).ToList();

<<<<<<< HEAD
                for (int i = 0; i < symbols.Count; i++)
||||||| 3ebe208
                for (int i = 6; i < 9; i++)
=======
                for (int i = 0; i < symbols.Count(); i++)
>>>>>>> 22cdbe55593c829e523d63c41cf1dd672d4e619f
                {
                    string QUERY_URL = "https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=" + symbols[i] + "&apikey=8SIHG57EBHLCEKEN";
                    string QUERY_URL2 = "https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords=" + symbols[i] + "&apikey=8SIHG57EBHLCEKEN";

                    Uri queryUri = new Uri(QUERY_URL);
                    Uri queryUri2 = new Uri(QUERY_URL2);

                    dynamic json_data = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(client.DownloadString(queryUri));
                    dynamic json_data2 = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(client.DownloadString(queryUri2));

                    dynamic jsi = json_data["Global Quote"];
                    dynamic jsi2 = json_data2["bestMatches"];


                    string data = jsi.GetRawText();
                    Dictionary<string, string> dic = JsonSerializer.Deserialize<Dictionary<string, string>>(data);
                    string data2 = jsi2.GetRawText();
                    var results = JsonSerializer.Deserialize<List<dynamic>>(data2);
                    Dictionary<string, string> result = JsonSerializer.Deserialize<Dictionary<string, string>>(results[0].GetRawText());
                    string s_symbol = dic["01. symbol"];
                    string s_name = result["2. name"];
                    double s_price = Convert.ToDouble(dic["05. price"]);
                    double s_change = Convert.ToDouble(dic["09. change"]);

                    //var s = new Stock { Symbol = s_symbol, Name = s_name, Price = s_price, Change = s_change, Category = s_category };
                    var stock = await _stockService.GetStock(s_symbol);
                    if (stock != null)
                    {
                        await _stockService.UpdateStockDetails(s_symbol, s_name, s_price, s_change);
                    }
                    Thread.Sleep(500);
                }
            }
        }

        /*public ActionResult Trade(string submit, string symbol)
        {
            switch (submit)
            {
                case "Buy":
                    return (Buy(symbol));
                case "Sell":
                    return (Sell(symbol));
            }
        }*/
        public int GetStocksAmount(string symbol, string username)
        {
            return _userService.GetStocksAmount(username, symbol);
        }
        [HttpPost]
        public ActionResult Buy(string symbol)
        {
            return RedirectToAction("Buy", "Orders", symbol);
        }

        [HttpPost]
        public ActionResult Sell(Order order)
        {

            return RedirectToAction("Index", "Stocks");
        }

        // GET: Stocks/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stock
                .FirstOrDefaultAsync(m => m.Symbol == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // GET: Stocks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Symbol,Name,Price,Change,Category")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stock);
        }

        // GET: Stocks/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stock.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return View(stock);
        }

        // POST: Stocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Symbol,Name,Price,Change,Category")] Stock stock)
        {
            if (id != stock.Symbol)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockExists(stock.Symbol))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(stock);
        }

        // GET: Stocks/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stock
                .FirstOrDefaultAsync(m => m.Symbol == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var stock = await _context.Stock.FindAsync(id);
            _context.Stock.Remove(stock);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockExists(string id)
        {
            return _context.Stock.Any(e => e.Symbol == id);
        }
    }
}
