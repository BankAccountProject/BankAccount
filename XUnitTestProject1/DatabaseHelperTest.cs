using BankAccount.Helpers;
using System;
using Xunit;

namespace XUnitTestProject1
{
    public class DatabaseHelperTest
    {
        [Theory]
        [InlineData("password", "password")]
        [InlineData("password3", "password3")]
        public void Passing_Passwords_Are_Equal(string pass, string confPass)
        {
            // Arrange
            string password = pass;
            string confirmPassword = confPass;

            // Act
            bool result = DatabaseHelper.PasswordAreEqual(password, confirmPassword);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("99 2000 0001 2345 6789 1234 4550")]
        [InlineData("99200000012345678912344550")]
        public void Passing_Check_Number_Of_Digits(string accountNumber)
        {
            // Act
            bool result = DatabaseHelper.CheckNumberOfDigits(accountNumber);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("99 2000 0001 2345 6789 1234 4550")]
        [InlineData("99200000012345678912344550")]
        public void Passing_Check_Bank_Account_If_Number(string accountNumber)
        {
            // Act
            bool result = DatabaseHelper.CheckBankAccountIfNumber(accountNumber);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("aododod90@")]
        [InlineData("aAAAAd90@")]
        [InlineData("aodod--==od90@")]
        public void Passing_Validate_Password(string password)
        {
            // a-z V A-Z, 0-9, symbol-ASCII
            // Act
            bool result = DatabaseHelper.ValidatePassword(password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Not_Passing_Check_Bank_Account_If_Number()
        {
            // Arrange 
            string accountNumber = "99 2000 0001 2345 6789 1234 455g";

            // Act
            bool result = DatabaseHelper.CheckBankAccountIfNumber(accountNumber);

            // Assert
            Assert.False(result);
        }
    }
}
