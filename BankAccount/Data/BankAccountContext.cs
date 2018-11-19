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



        public void test()
        {
            var UserTransactions2 = this.UserTransaction
                .FromSql("exec dbo.GetTransactionById 10")
                .ToList();

            var Client = this.Client
                .FromSql("exec dbo.GetClientById 10")
                .ToList();

            var ClientDetail = this.ClientDetail
               .FromSql("exec dbo.GetClientById 10")
               .ToList();
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
                .FromSql("exec dbo.GetClientByLoginPassword @login @password", Login, Password)
                .First();

            ClientDetail ClientDetail = this.ClientDetail
               .FromSql("exec dbo.GetClientByLoginPassword @ID", Login, Password)
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
                .FromSql("exec dbo.GetEmployeeByLoginPassword @login @password", Login, Password)
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
