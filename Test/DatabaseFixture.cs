using BankManagementSystem.Data;
using BankManagementSystem.Models;
using BankManagementSystem.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    public class DatabaseFixture : IDisposable
    {
        private List<CustomerDetails> CustomerDetails { get; set; }

        public IApplicationDbContext dbContext;

        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<CustomerDetailsDbContext>().UseInMemoryDatabase(databaseName: "CustomerDB").Options;
            dbContext = new CustomerDetailsDbContext(options);

            dbContext.CustomerDetails.Add(new CustomerDetails() { Id = 200, UserName = "customeruser1@123", Name = "customername1", Email = "customerEmail1@mail.com", Password = "customerPassword1@321", DateOfBirth = new DateTime(1999, 10, 29), State = "Ohio", ContactNumber = "9826374890", Address = "1/234 , Customer street 1", PermanentAccountNumber = "Pan321", Country = "USA", AccountType = "Saving account" });
            dbContext.CustomerDetails.Add(new CustomerDetails() { Id = 201, UserName = "customeruser2@123", Name = "customername2", Email = "customerEmail2@mail.com", Password = "customerPassword2@321", DateOfBirth = new DateTime(1995, 03, 29), State = "Madagascar", ContactNumber = "9036478936", Address = "1/234 , Customer street 2", PermanentAccountNumber = "Pan123321", Country = "Africa", AccountType = "Current account" });
            dbContext.CustomerDetails.Add(new CustomerDetails() { Id = 202, UserName = "customeruser3@123", Name = "customername3", Email = "customerEmail3@mail.com", Password = "customerPassword3@321", DateOfBirth = new DateTime(1987, 07, 14), State = "Miami", ContactNumber = "9857893645", Address = "1/234 , Customer street 3", PermanentAccountNumber = "Pan321231", Country = "USA", AccountType = "Saving account" });

            dbContext.SaveChanges();
        }

        public void Dispose()
        {
            CustomerDetails = null;
            dbContext = null;
        }
    }
}
