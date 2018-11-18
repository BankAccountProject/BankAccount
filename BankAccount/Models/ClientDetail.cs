using System;
using System.Collections.Generic;

namespace BankAccount.Models
{
    public partial class ClientDetail
    {
        public int ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Pesel { get; set; }
        public string Idnumber { get; set; }
        public string PhoneNumber { get; set; }

        public Client Client { get; set; }
    }
}
