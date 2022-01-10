using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
            if (!_context.Stock.Any())
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


                if (!_context.Index.Any())
                {
                    var indicesInitializing = new List<string>{ "S&P 500", "Dow Jones", "NASDAQ" };

                    foreach (var i in indicesInitializing)
                    {
                        _context.Index.Add(new MarketIndex { Name = i });


                        await _context.SaveChangesAsync();

                    };
                    { 

                    }
                }
            }

                using (WebClient client = new WebClient())
                {
                    var symbols = (from s in _context.Stock select s.Symbol).ToList();

                    foreach (var symbol in symbols)

                    {
                        string QUERY_URL = "https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=" + symbol + "&apikey=XXX";
                        string QUERY_URL2 = "https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords=" + symbol + "&apikey=XXX";

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

                        var stock = await _stockService.GetStock(s_symbol);
                        if (stock != null)
                        {
                            await _stockService.UpdateStockDetails(s_symbol, s_name, s_price, s_change);
                        }


                    }


                }

                await _context.SaveChangesAsync();

                var allIndices =
                    from i in _context.Index
                    select i.Name;
                allIndices = allIndices.Distinct();
                ViewBag.allIndices = allIndices.ToList();

                var allCategories = new List<string> { "Technology", "Parmaceutical", "Airline", "Vehicle manufacturer" };
                ViewBag.allCategories = allCategories.ToList();
                //await _context.Stock.ToListAsync()
                return View(await _context.Stock.ToListAsync());

            
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Search()
        {
            return View(await _context.Stock.ToListAsync());
        }

        [HttpPost]
        public IActionResult Search(string symbol, string stockname)
        {

            // Get users and search them
            var all_stocks = from u in _context.Stock select u;

            // Return all if fields empty
            if (String.IsNullOrEmpty(symbol) && (String.IsNullOrEmpty(stockname)))
            {
                return View(all_stocks);
            }

            var found_stocks = new List<Stock>();

            // Symbol or Stock name
            if (!String.IsNullOrEmpty(symbol))
            {
                // Remove trailing \t
                symbol = symbol.Replace("\t", String.Empty);
                var matched_symbols = all_stocks.Where(u => (u.Symbol.Contains(symbol)));
                var matched_symbols_list = new List<Stock>(matched_symbols);

                found_stocks = found_stocks.Concat(matched_symbols_list).ToList();
            }

            if (!String.IsNullOrEmpty(stockname))
            {
                // Remove trailing \t
                stockname = stockname.Replace("\t", String.Empty);
                var matched_stocknames = all_stocks.Where(u => (u.Name.Contains(stockname)));
                var matched_stocknames_list = new List<Stock>(matched_stocknames);

                found_stocks = found_stocks.Concat(matched_stocknames_list).ToList();
            }

            // Return found users
            return View(found_stocks);
        }

        // GET: Stocks/Details/5
        [Authorize]
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
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stocks/Create

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
        [Authorize(Policy = "Administrator")]
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
        [Authorize(Policy = "Administrator")]
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


        public ActionResult SearchStocks(double? lowPrice, double? highPrice, string change, string category, string indexName)
        {
            var allIndices =
                from i in _context.Index
                select i.Name;
            allIndices = allIndices.Distinct();
            ViewBag.allIndices = allIndices.ToList();

            var allCategories = new List<string> { "Technology", "Parmaceutical", "Airline", "Vehicle manufacturer" };
            ViewBag.allCategories = allCategories.ToList();
            List<Stock> filteredStocks = new List<Stock>();
            filteredStocks = ASearch(lowPrice, highPrice, change, category, indexName);
            ViewData["vStocks"] = filteredStocks.ToList();

            return View(filteredStocks.ToList());
        }

        public List<Stock> ASearch(double? lowPrice, double? highPrice, string? change, string category, string indexName)
        {
            var stocks = 
                from s in _context.Stock
                select s;

            if (lowPrice != null)
            {
                stocks = stocks.Where(s => s.Price >= lowPrice);
            }
            if (highPrice != null)
            {
                stocks = stocks.Where(s => s.Price <= highPrice);
            }
            if (!String.IsNullOrEmpty(change))
            {
                if (change == "dec")
                {
                    stocks = stocks.Where(s => s.Change <= 0);
                }
                if (change == "inc")
                {
                    stocks = stocks.Where(s => s.Change > 0);
                }
            }
            if (!String.IsNullOrEmpty(category))
            {
                stocks = stocks.Where(s => s.Category == category);
            }
            if (!String.IsNullOrEmpty(indexName))
            {
                var index = _context.Index.FirstOrDefault(i => i.Name == indexName);
                stocks = stocks.Include("Indices").Where(s => s.Indices.Contains(index));
            }

            return stocks.ToList();

        }

    }
}
