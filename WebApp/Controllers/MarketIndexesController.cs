using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class MarketIndexesController : Controller
    {
        private readonly WebAppContext _context;

        public MarketIndexesController(WebAppContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> Search()
        {
            return View(await _context.Index.ToListAsync());
        }

        [HttpPost]
        public IActionResult Search(string name)
        {

            // Get indices and search them
            var all_indices = from u in _context.Index select u;

            // Return all if fields empty
            if (String.IsNullOrEmpty(name))
            {
                return View(all_indices);
            }

            var found_indices = new List<MarketIndex>();

            if (!String.IsNullOrEmpty(name))
            {
                // Remove trailing \t
                name = name.Replace("\t", String.Empty);
                var matched_indices = all_indices.Where(u => (u.Name.Contains(name)));
                var matched_indices_list = new List<MarketIndex>(matched_indices);

                found_indices = found_indices.Concat(matched_indices_list).ToList();
            }

            // Return found indices
            return View(found_indices);
        }

        // GET: MarketIndexes
        [Authorize(Policy = "Administrator")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Index.ToListAsync());
        }

        // GET: MarketIndexes/Details/5
        [Authorize(Policy = "Administrator")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marketIndex = await _context.Index
                .FirstOrDefaultAsync(m => m.Name == id);
            if (marketIndex == null)
            {
                return NotFound();
            }

            return View(marketIndex);
        }

        // GET: MarketIndexes/Create
        [Authorize(Policy = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: MarketIndexes/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] MarketIndex marketIndex)
        {
            if (ModelState.IsValid)
            {
                _context.Add(marketIndex);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(marketIndex);
        }

        // GET: MarketIndexes/Edit/5
        [Authorize(Policy = "Administrator")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marketIndex = await _context.Index.FindAsync(id);
            if (marketIndex == null)
            {
                return NotFound();
            }
            return View(marketIndex);
        }

        // POST: MarketIndexes/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name")] MarketIndex marketIndex)
        {
            if (id != marketIndex.Name)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(marketIndex);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarketIndexExists(marketIndex.Name))
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
            return View(marketIndex);
        }

        // GET: MarketIndexes/Delete/5
        [Authorize(Policy = "Administrator")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marketIndex = await _context.Index
                .FirstOrDefaultAsync(m => m.Name == id);
            if (marketIndex == null)
            {
                return NotFound();
            }

            return View(marketIndex);
        }

        // POST: MarketIndexes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var marketIndex = await _context.Index.FindAsync(id);
            _context.Index.Remove(marketIndex);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarketIndexExists(string id)
        {
            return _context.Index.Any(e => e.Name == id);
        }
    }
}
