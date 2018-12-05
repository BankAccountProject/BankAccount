using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace BankAccount.Models
{
    public partial class BankAccountContext : DbContext
    {
        public virtual DbSet<AddressBook> AddressBook { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<ClientDetail> ClientDetail { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<UserTransaction> UserTransaction { get; set; }

        public BankAccountContext(DbContextOptions<BankAccountContext> options)
            : base(options)
        {
        }

        public void ActivateClientById(int clientId)
        {
            var ClientId = new SqlParameter("clientId", clientId);
            this.Database.ExecuteSqlCommand("dbo.ActivateClientById @clientId",
                              ClientId);
            return;
        }

        public void DeactivateClientById(int clientId)
        {
            var ClientId = new SqlParameter("clientId", clientId);
            this.Database.ExecuteSqlCommand("dbo.DeactivateClientById @clientId",
                              ClientId);
            return;
        }

        public Client GetClientById(int id)
        {
            var ID = new SqlParameter("ID", id);

            Client Client = this.Client
                .FromSql("exec dbo.GetClientById @ID", ID)
                .First();

            ClientDetail ClientDetail = this.ClientDetail
               .FromSql("exec dbo.GetClientById @ID", ID)
               .First();

            return Client;
        }

        public Client GetClientByLoginPassword(string login, string password)
        {
            var Login = new SqlParameter("login", login);
            var Password = new SqlParameter("password", password);

            Client Client = this.Client
                .FromSql("exec dbo.GetClientByLoginPassword @login, @password", Login, Password)
                .First();
            var Id = new SqlParameter("ID", Client.ClientId);
            ClientDetail ClientDetail = this.ClientDetail
               .FromSql("exec dbo.GetClientById @ID", Id)
               .First();

            return Client;
        }

        public Client GetClientByAccountNumber(string accountNumber)
        {
            var AccountNumber = new SqlParameter("accountNumber", accountNumber);
     

            Client Client = this.Client
                .FromSql("exec dbo.GetClientByAccountNumber @accountNumber", AccountNumber)
                .First();
            var Id = new SqlParameter("ID", Client.ClientId);
            ClientDetail ClientDetail = this.ClientDetail
               .FromSql("exec dbo.GetClientById @ID", Id)
               .First();

            return Client;
        }
        

        public Employee GetEmployeeById(int id)
        {
            var ID = new SqlParameter("ID", id);

            Employee Employee = this.Employee
                .FromSql("exec dbo.GetEmployeeById @ID", ID)
                .First();

            return Employee;
        }

        public Employee GetEmployeeByLoginPassword(string login, string password)
        {
            var Login = new SqlParameter("login", login);
            var Password = new SqlParameter("password", password);

            Employee Employee = this.Employee
                .FromSql("exec dbo.GetEmployeeByLoginPassword @login, @password", Login, Password)
                .First();

            return Employee;
        }

        public UserTransaction GetTransactionById(int id)
        {
            var ID = new SqlParameter("ID", id);

            UserTransaction UserTransaction = this.UserTransaction
                .FromSql("exec dbo.GetTransactionById @ID", ID)
                .First();

            return UserTransaction;
        }

        public List<UserTransaction> GetTransactionByClientId(int id)
        {
            var ID = new SqlParameter("ID", id);

            List<UserTransaction> UserTransactions = this.UserTransaction
                .FromSql("exec dbo.GetTransactionByClientId @ID", ID)
                .ToList();

            foreach (var UserTransaction in UserTransactions)
            {
                Client recipient = GetClientById(UserTransaction.RecipientId);
                UserTransaction.Recipient = recipient;

                Client sender = GetClientById(UserTransaction.SenderId);
                UserTransaction.Sender = sender;
            }

            return UserTransactions;
        }

        public List<UserTransaction> GetTransactionByRecipientId(int id)
        {
            var ID = new SqlParameter("ID", id);

            List<UserTransaction> UserTransactions = this.UserTransaction
                .FromSql("exec dbo.GetTransactionByRecipientId @ID", ID)
                .ToList();

            return UserTransactions;
        }

        public List<UserTransaction> GetTransactionBySenderId(int id)
        {
            var ID = new SqlParameter("ID", id);

            List<UserTransaction> UserTransactions = this.UserTransaction
                .FromSql("exec dbo.GetTransactionBySenderId @ID", ID)
                .ToList();

            return UserTransactions;
        }

        public List<AddressBook> GetAddressBookByOwnerId(int id)
        {
            var ID = new SqlParameter("ID", id);

            List<AddressBook> AddressBooks = this.AddressBook
                .FromSql("exec dbo.GetAddressBookByOwnerId @ID", ID)
                .ToList();

            foreach(var addressBook in AddressBooks)
            {
                Client client = GetClientById(addressBook.InscribedId);
                addressBook.Inscribed = client;
            }

            return AddressBooks;
        }

        public void NewAddressBook(int ownerId, int inscribedId, string description)
        {
            var OwnerId = new SqlParameter("ownerId", ownerId);
            var InscribedId = new SqlParameter("inscribedId", inscribedId);
            var Description = new SqlParameter("description", description);
            this.Database.ExecuteSqlCommand("exec dbo.NewAddressBook @ownerId, @inscribedId, @description",
                              OwnerId, InscribedId, Description);
        }

        public void NewClient(string permisions, string accountNumber, decimal accountBalance, decimal interest, string login, string password, bool active)
        {
            var Permisions = new SqlParameter("permisions", permisions);
            var AccountNumber = new SqlParameter("accountNumber", accountNumber);
            var AccountBalance = new SqlParameter("accountBalance", accountBalance);
            var Interest = new SqlParameter("interest", interest);
            var Login = new SqlParameter("login", login);
            var Password = new SqlParameter("password", password);
            var Active = new SqlParameter("active", active);
            this.Database.ExecuteSqlCommand("dbo.NewClient @permisions, @accountNumber, @accountBalance, @interest, @login, @password, @active",
                              Permisions, AccountNumber, AccountBalance, Interest, Login, Password, Active);
            return;
        }

        public void NewClientDetail(int clientId, string firstName, string lastName, DateTime dateOfBirth, string pesel, string idNumber, string phoneNumber)
        {
            var ClientId = new SqlParameter("clientId", clientId);
            var FirstName = new SqlParameter("firstName", firstName);
            var LastName = new SqlParameter("lastName", lastName);
            var DateOfBirth = new SqlParameter("dateOfBirth", dateOfBirth);
            var Pesel = new SqlParameter("pesel", pesel);
            var IdNumber = new SqlParameter("idNumber", idNumber);
            var PhoneNumber = new SqlParameter("phoneNumber", phoneNumber);
            this.Database.ExecuteSqlCommand("dbo.NewClient @clientId, @firstName, @lastName, @dateOfBirth, @pesel, @idNumber, @phoneNumber",
                              ClientId, FirstName, LastName, DateOfBirth, Pesel, IdNumber, PhoneNumber);
            return;
        }

        public void NewTransaction(int senderId, int recipientId, decimal amount, DateTime executionDate, string title, string status)
        {
            var SenderId = new SqlParameter("senderId", senderId);
            var RecipientId = new SqlParameter("recipientId", recipientId);
            var Amount = new SqlParameter("amount", amount);
            var ExecutionDate = new SqlParameter("executionDate", executionDate);
            var Title = new SqlParameter("title", title);
            var Status = new SqlParameter("status", status);
            this.Database.ExecuteSqlCommand("dbo.NewTransaction @senderId, @recipientId, @amount, @executionDate, @title, @status",
                              SenderId, RecipientId, Amount, ExecutionDate, Title, Status);
            return;
        }

        public void UpdateClientDateOfBirth(int clientId, DateTime dateOfBirth)
        {
            var ClientId = new SqlParameter("clientId", clientId);
            var DateOfBirth = new SqlParameter("dateOfBirth", dateOfBirth);

            this.Database.ExecuteSqlCommand("dbo.UpdateClientDateOfBirth @clientId, @dateOfBirth",
                              ClientId, DateOfBirth);
            return;
        }

        public void UpdateClientDetail(int clientId, string lastName, DateTime dateOfBirth, string pesel, string phoneNumber)
        {
            var ClientId = new SqlParameter("clientId", clientId);
            var LastName = new SqlParameter("lastName", lastName);
            var DateOfBirth = new SqlParameter("dateOfBirth", dateOfBirth);
            var Pesel = new SqlParameter("pesel", pesel);
            var PhoneNumber = new SqlParameter("phoneNumber", phoneNumber);
            this.Database.ExecuteSqlCommand("dbo.UpdateClientDetail @clientId, @lastName, @dateOfBirth, @pesel, @phoneNumber",
                              ClientId, LastName, DateOfBirth, Pesel, PhoneNumber);
            return;
        }

        public void UpdateClientDetailFirstName(int clientId, string firstName)
        {
            var ClientId = new SqlParameter("clientId", clientId);
            var FirstName = new SqlParameter("firstName", firstName);
            
            this.Database.ExecuteSqlCommand("dbo.UpdateClientDetailFirstName @clientId, @firstName",
                              ClientId, FirstName);
            return;
        }

        public void UpdateClientDetailLastName(int clientId, string lastName)
        {
            var ClientId = new SqlParameter("clientId", clientId);
            var LastName = new SqlParameter("lastName", lastName);

            this.Database.ExecuteSqlCommand("dbo.UpdateClientDetailLastName @clientId, @lastName",
                              ClientId, LastName);
            return;
        }

        public void UpdateClientPasswordById(int clientId, string newPassword)
        {
            var ClientId = new SqlParameter("clientId", clientId);
            var NewPassword = new SqlParameter("newPassword", newPassword);

            this.Database.ExecuteSqlCommand("dbo.UpdateClientPasswordById @clientId, @newPassword",
                              ClientId, NewPassword);
            return;
        }

        public void UpdateClientPasswordByLogin(string login, string newPassword)
        {
            var Login = new SqlParameter("login", login);
            var NewPassword = new SqlParameter("newPassword", newPassword);

            this.Database.ExecuteSqlCommand("dbo.UpdateClientPasswordByLogin @login, @newPassword",
                              Login, NewPassword);
            return;
        }

        public void UpdateClientPesel(int clientId, string pesel)
        {
            var ClientId = new SqlParameter("clientId", clientId);
            var Pesel = new SqlParameter("pesel", pesel);
            this.Database.ExecuteSqlCommand("dbo.UpdateClientPesel @clientId, @pesel",
                              ClientId, Pesel);
            return;
        }

        public void UpdateClientPhoneNumber(int clientId, string phoneNumber)
        {
            var ClientId = new SqlParameter("clientId", clientId);
            var PhoneNumber = new SqlParameter("phoneNumber", phoneNumber);
            this.Database.ExecuteSqlCommand("dbo.UpdateClientPhoneNumber @clientId, @phoneNumber",
                              ClientId, PhoneNumber);
            return;
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddressBook>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(30);

                entity.Property(e => e.InscribedId).HasColumnName("InscribedID");

                entity.Property(e => e.OwnerId).HasColumnName("OwnerID");

                entity.HasOne(d => d.Inscribed)
                    .WithMany(p => p.AddressBookInscribed)
                    .HasForeignKey(d => d.InscribedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AddressBook_Client_Inscribed");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.AddressBookOwner)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AddressBook_Client_Owner");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasIndex(e => e.AccountNumber)
                    .HasName("idx_AccountNumber")
                    .IsUnique();

                entity.HasIndex(e => e.Login)
                    .HasName("indx_Login")
                    .IsUnique();

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.AccountBalance).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.AccountNumber)
                    .IsRequired()
                    .HasMaxLength(26);

                entity.Property(e => e.Interest).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Permisions)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<ClientDetail>(entity =>
            {
                entity.HasKey(e => e.ClientId);

                entity.HasIndex(e => e.Pesel)
                    .HasName("indx_Pesel")
                    .IsUnique();

                entity.Property(e => e.ClientId)
                    .HasColumnName("ClientID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.FirstName).HasMaxLength(30);

                entity.Property(e => e.Idnumber)
                    .HasColumnName("IDNumber")
                    .HasMaxLength(9);

                entity.Property(e => e.LastName).HasMaxLength(30);

                entity.Property(e => e.Pesel).HasMaxLength(11);

                entity.Property(e => e.PhoneNumber).HasMaxLength(9);

                entity.HasOne(d => d.Client)
                    .WithOne(p => p.ClientDetail)
                    .HasForeignKey<ClientDetail>(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClientDetail_Client");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Permissions)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<UserTransaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.ExecutionDate).HasColumnType("datetime");

                entity.Property(e => e.RecipientId).HasColumnName("RecipientID");

                entity.Property(e => e.SenderId).HasColumnName("SenderID");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Recipient)
                    .WithMany(p => p.UserTransactionRecipient)
                    .HasForeignKey(d => d.RecipientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Client_Recipient");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.UserTransactionSender)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Client_Sender");
            });
        }
    }
}
