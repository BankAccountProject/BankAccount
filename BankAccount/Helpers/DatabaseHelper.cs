using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BankAccount.Helpers
{
    public static class DatabaseHelper
    {
        public static bool PasswordAreEqual(string password, string confirmPassword)
        {
            if (password == confirmPassword)
                return true;
            return false;
        }

        public static string Generate26DigitAccountNumber()
        {
            Random generator = new Random();
            string accountNumber = generator.Next(100000, 999999).ToString();
            for (int i = 0; i < 3; ++i)
            {
                accountNumber += generator.Next(100000, 999999).ToString();
            }
            return accountNumber;
        }

        public static bool CheckNumberOfDigits(string accountNumber)
        {
            // delete whitespace
            accountNumber = Regex.Replace(accountNumber, @"\s+", "");
            if (accountNumber.ToCharArray().Count() == 26)
                return true;
            return false;
        }

        public static bool ValidatePassword(string password)
        {
            // a-z V A-Z, 0-9, symbol-ASCII
            bool result = password.Any(c => char.IsLetter(c)) &&
                          password.Any(c => char.IsDigit(c)) &&
                          ( password.Any(c => char.IsSymbol(c)) || password.Any(c => char.IsPunctuation(c)));
            return result;
        }

        public static bool CheckBankAccountIfNumber(string accountNumber)
        {
            accountNumber = Regex.Replace(accountNumber, @"\s+", "");
            return accountNumber.All(Char.IsDigit);
        }
    }
}
