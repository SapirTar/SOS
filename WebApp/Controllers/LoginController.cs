﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace WebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly WebAppContext _context;


        public LoginController(WebAppContext context)
        {
            _context = context;
        }

        // GET: Login
        [HttpGet("login")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync(User given_user)
        {
            var user = _context.User.Where(u => u.Username.Equals(given_user.Username) && u.Password.Equals(given_user.Password)).FirstOrDefault();
            var is_admin = _context.User.Where(u => u.Username.Equals(given_user.Username) && u.Admin == true).FirstOrDefault();
            if (user != null)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("username", given_user.Username));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, given_user.Username));

                if (is_admin != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Starts the Startup authentication code
                await HttpContext.SignInAsync(claimsPrincipal);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Error"] = "Incorrect credentials";
                return View("Index");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        // GET: Login/MyProfile
        [Authorize]
        public async Task<IActionResult> MyProfileAsync()
        {
            var claims = User.Claims.ToList();
            var username = claims[0].Value;
            var user = _context.User.Where(u => u.Username.Equals(username)).FirstOrDefault();

            // need to initialize the list
            if (user.OwnedStocks == null)
            {
                user.OwnedStocks = new List<Stock>();
            }
            //user.OwnedStocks.Add(random_stock);
            await _context.SaveChangesAsync();

            return View(user);
        }

        public ActionResult RenderStocks()
        {
            return PartialView("StocksView");
        }

        // GET: Login/InputCC
        [Authorize]
        public IActionResult InputCC()
        {
            return View("InputCC");
        }

        [Authorize]
        public IActionResult Statistics()
        {
            var claims = User.Claims.ToList();
            var username = claims[0].Value;
            var user = _context.User.Include(u => u.OwnedStocks).Where(u => u.Username.Equals(username)).FirstOrDefault();
            return View("Statistics",user);
        }


        // POST: Login/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,Password,Email,Creditcard,Birthdate")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddCreditCard([Bind("CardNum,CVV,CardHolder")] CreditCard creditcard)
        {
            if (ModelState.IsValid)
            {
                var claims = User.Claims.ToList();
                var username = claims[0].Value;
                var user = _context.User.Include(x => x.CreditCard).Where(u => u.Username.Equals(username)).FirstOrDefault();

                // User doesn't have a CC
                if (user.CreditCard != null)
                {
                    return View("CCExists");
                }

                _context.Add(creditcard);
                
                user.CreditCard = creditcard;

                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            TempData["Message"] = "Input Error";
            return View("MyProfile");
        }

        // GET: Login/Edit/5
        [Authorize(Policy = "Administrator")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Login/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Username,Password,Email,Creditcard,Birthdate")] User user)
        {
            if (id != user.Username)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Username))
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
            return View(user);
        }

        // GET: Login/Delete/5
        [Authorize(Policy = "Administrator")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Username == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.User.Any(e => e.Username == id);
        }



    }
}
