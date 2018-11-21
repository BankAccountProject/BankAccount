using BankAccount.Models;
using BankAccount.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAccount.Controllers
{
    public class AccountController: Controller
    {
        private readonly BankAccountContext _context;
        public AccountController(BankAccountContext context)
        {
            _context = context;
        }

        [Route("/login")]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        
        public IActionResult LoggedIn(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong");
                return View("Login");
            }

            var client = _context.GetClientByLoginPassword(login.Login, login.Password);

            if (client != null)
                return RedirectToAction("AllTransactionByClientId", "BankAccount");

            else throw new Exception("Invalid login attempt");
            return View(login);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            
            return RedirectToAction("Index", "Home");
        }
        
        public ViewResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            return View(model);
        }

        [HttpGet]
        public ViewResult ManageAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ManageAccount(RegisterViewModel model)
        {
            ViewData["Client"] = _context.GetClientById(7);
            return View();
        }

    }
}
