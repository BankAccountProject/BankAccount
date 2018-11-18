using System;
using System.Collections.Generic;

namespace BankAccount.Models
{
    public partial class AddressBook
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int InscribedId { get; set; }
        public string Description { get; set; }

        public Client Inscribed { get; set; }
        public Client Owner { get; set; }
    }
}
