using System;
using System.Collections.Generic;

namespace BankAccount.Models
{
    public partial class Client
    {
        public Client()
        {
            AddressBookInscribed = new HashSet<AddressBook>();
            AddressBookOwner = new HashSet<AddressBook>();
            UserTransactionRecipient = new HashSet<UserTransaction>();
            UserTransactionSender = new HashSet<UserTransaction>();
        }

        public int ClientId { get; set; }
        public string Permisions { get; set; }
        public string AccountNumber { get; set; }
        public decimal AccountBalance { get; set; }
        public decimal Interest { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }

        public ClientDetail ClientDetail { get; set; }
        public ICollection<AddressBook> AddressBookInscribed { get; set; }
        public ICollection<AddressBook> AddressBookOwner { get; set; }
        public ICollection<UserTransaction> UserTransactionRecipient { get; set; }
        public ICollection<UserTransaction> UserTransactionSender { get; set; }
    }
}
