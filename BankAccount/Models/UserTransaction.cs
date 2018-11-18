using System;
using System.Collections.Generic;

namespace BankAccount.Models
{
    public partial class UserTransaction
    {
        public int TransactionId { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExecutionDate { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }

        public Client Recipient { get; set; }
        public Client Sender { get; set; }
    }
}
