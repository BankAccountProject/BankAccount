using BankAccount.Helpers;
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

        [Route("/NewTransaction")]
        public IActionResult NewTransaction(UserTransaction userTransaction)
        {
            if (userTransaction.Recipient != null)
            {
                var accountNumber = userTransaction.Recipient.AccountNumber;
                Client recipient = _context.GetClientByAccountNumber(accountNumber);
                int senderId = client.ClientId;
                int recipientId = recipient.ClientId;

                decimal amount = userTransaction.Amount;
                DateTime executionDate = DateTime.Now;
                string title = userTransaction.Title;
                string status = "Sent";

                if (DatabaseHelper.CheckBankAccountIfNumber(accountNumber) )
                { 
                    try
                    {
                        _context.NewTransaction(senderId, recipientId, amount, executionDate, title, status);
                        ModelState.Clear();
                        ViewBag.statusType = "alert-success";
                        ViewBag.statusInfo = "Poprawnie wysłano przelew";
                    } catch (Exception e)
                    {
                        ViewBag.statusType = "alert-danger";
                        ViewBag.statusInfo = "Błąd, nie wysłano przelewu";
                    }
                    
                    
                } else
                {
                    ViewBag.statusType = "alert-warning";
                    ViewBag.statusInfo = "Zły numer konta";
                }
            }
            
            ViewBag.addressBooks = _context.GetAddressBookByOwnerId(client.ClientId);
            return View();
        }
    }
}
