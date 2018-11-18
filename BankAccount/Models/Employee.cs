using System;
using System.Collections.Generic;

namespace BankAccount.Models
{
    public partial class Employee
    {
        public int EmployeeId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
        public string Permissions { get; set; }
    }
}
