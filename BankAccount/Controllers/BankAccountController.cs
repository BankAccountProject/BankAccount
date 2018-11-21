using BankAccount.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAccount.Controllers
{
    public class BankAccountController: Controller
    {
        private readonly BankAccountContext _context;
        private Client client;
        public BankAccountController(BankAccountContext context)
        {
            _context = context;
            client = new Client();
            client.ClientId = 18;
        }

        public IActionResult AllTransactionByClientId()
        {
            return View(_context.GetTransactionByClientId(client.ClientId));
        }

        public IActionResult TransactionDetails(int id)
        {

            return View(_context.GetTransactionById(id));
        }
    }
}
