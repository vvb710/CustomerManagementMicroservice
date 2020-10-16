using BankManagementSystem.Data;
using BankManagementSystem.Models;
using BankManagementSystem.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Test
{
    public class CustomerDetailsRepositoryTest : IClassFixture<DatabaseFixture>
    {
        private readonly ICustomerDetailsRepository _repo;

        DatabaseFixture _fixture;

        public CustomerDetailsRepositoryTest(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _repo = new CustomerDetailsRepository(_fixture.dbContext);
        }

        [Fact]
        public void GetAllCustomer_ShouldReturnListOfAllMovies()
        {
            //Act
            var actual = _repo.GetAllCustomerDetails();

            //Assert
            Assert.IsAssignableFrom<List<CustomerDetails>>(actual);
            Assert.NotNull(actual);
            Assert.True(actual.Count() > 0);
            Assert.Equal(3, actual.Count());
        }

        [Fact]
        public void GetCustomerById_ShouldReturnCustomerDetails()
        {
            //Arrange
            var context = MockDbRepository();
            var service = new CustomerDetailsRepository(context.Object);
            var id = 200;

            //Act
            var customerDetails = service.GetCustomerDetailsById(id);

            // Assert            
            Assert.NotNull(customerDetails);
            Assert.Equal("customername1", customerDetails.Name);
        }

        [Fact]
        public void GetCustomerByIdNotPresent_ShouldNotCustomerDetails()
        {
            //Arrange
            var context = MockDbRepository();
            var service = new CustomerDetailsRepository(context.Object);
            var id = 5000;

            //Act
            var customerDetails = service.GetCustomerDetailsById(id);

            // Assert            
            Assert.Null(customerDetails);
        }

        [Fact]
        public void GetCustomerByNameNotPresent_ShouldNotCustomerDetails()
        {
            //Arrange
            var context = MockDbRepository();
            var service = new CustomerDetailsRepository(context.Object);
            var name = "Virat Kholi";

            //Act
            var customerDetails = service.GetCustomerDetailsByName(name);

            // Assert            
            Assert.Empty(customerDetails);
        }

        [Fact]
        public void GetCustomerByName_ShouldReturnCustomerDetails()
        {
            //Arrange
            var context = MockDbRepository();
            var service = new CustomerDetailsRepository(context.Object);
            var customerName = "customername3";

            //Act
            var customerDetails = service.GetCustomerDetailsByName(customerName);

            // Assert            
            Assert.NotNull(customerDetails);
            Assert.Equal(2, customerDetails.Count);
            foreach (var customerDetail in customerDetails)
            {
                Assert.Equal(customerName, customerDetail.Name);
            }
        }

        [Fact]
        public void PostMethod_ShouldCreateCustomer()
        {
            // Arrange
            var data = new List<CustomerDetails> {
                new CustomerDetails { Id = 10, UserName = "customeruser4@123", Name = "customername3", Email = "customerEmail4@mail.com", Password = "customerPassword3@321", DateOfBirth = new DateTime(1987, 07, 14), State = "Miami", ContactNumber = "9857893645", Address = "1/234 , Customer street 3", PermanentAccountNumber = "Pan321231", Country = "USA", AccountType = "Saving account" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<CustomerDetails>>();
            mockSet.As<IQueryable<CustomerDetails>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<CustomerDetails>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<CustomerDetails>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<CustomerDetails>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.Setup(c => c.CustomerDetails).Returns(mockSet.Object);

            // Act
            var service = new CustomerDetailsRepository(mockContext.Object);

            // Act
            service.AddCustomerDetail(data.First());
            var customerDetails = service.GetCustomerDetailsById(data.First().Id);


            // Assert
            Assert.Equal(data.First().Name, customerDetails.Name);
            Assert.Equal(data.First().Id, customerDetails.Id);
            Assert.Equal(data.First().UserName, customerDetails.UserName);
        }


        private Mock<IApplicationDbContext> MockDbRepository()
        {
            var customerDetails = GetFakeData().AsQueryable();
            var mockDbSet = new Mock<DbSet<CustomerDetails>>();
            mockDbSet.As<IQueryable<CustomerDetails>>().Setup(m => m.Provider).Returns(customerDetails.Provider);
            mockDbSet.As<IQueryable<CustomerDetails>>().Setup(m => m.Expression).Returns(customerDetails.Expression);
            mockDbSet.As<IQueryable<CustomerDetails>>().Setup(m => m.ElementType).Returns(customerDetails.ElementType);

            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.Setup(c => c.CustomerDetails).Returns(mockDbSet.Object);
            return mockContext;
        }

        private List<CustomerDetails> GetCustomerDetail()
        {
            var a = GetFakeData();
            var b = new CustomerDetails()
            {
                Id = 2000,
                UserName = "customer2000@123",
                Name = "customer1",
                Email = "customer2000@mail.com",
                Password = "customer2000Pass@321",
                DateOfBirth = new DateTime(1999, 10, 29),
                State = "UP",
                ContactNumber = "9826374890",
                Address = "1/234 , Customer street 1",
                PermanentAccountNumber = "Pan321",
                Country = "India",
                AccountType = "Saving account"
            };
            a.Add(b);
            return a;
        }

        private List<CustomerDetails> GetFakeData()
        {
            var customerDetails = new List<CustomerDetails>();
            customerDetails.Add(new CustomerDetails() { Id = 200, UserName = "customeruser1@123", Name = "customername1", Email = "customerEmail1@mail.com", Password = "customerPassword1@321", DateOfBirth = new DateTime(1999, 10, 29), State = "Ohio", ContactNumber = "9826374890", Address = "1/234 , Customer street 1", PermanentAccountNumber = "Pan321", Country = "USA", AccountType = "Saving account" });
            customerDetails.Add(new CustomerDetails() { Id = 201, UserName = "customeruser2@123", Name = "customername2", Email = "customerEmail2@mail.com", Password = "customerPassword2@321", DateOfBirth = new DateTime(1995, 03, 29), State = "Madagascar", ContactNumber = "9036478936", Address = "1/234 , Customer street 2", PermanentAccountNumber = "Pan123321", Country = "Africa", AccountType = "Current account" });
            customerDetails.Add(new CustomerDetails() { Id = 202, UserName = "customeruser3@123", Name = "customername3", Email = "customerEmail3@mail.com", Password = "customerPassword3@321", DateOfBirth = new DateTime(1987, 07, 14), State = "Miami", ContactNumber = "9857893645", Address = "1/234 , Customer street 3", PermanentAccountNumber = "Pan321231", Country = "USA", AccountType = "Saving account" });
            customerDetails.Add(new CustomerDetails() { Id = 203, UserName = "customeruser4@123", Name = "customername3", Email = "customerEmail4@mail.com", Password = "customerPassword3@321", DateOfBirth = new DateTime(1987, 07, 14), State = "Miami", ContactNumber = "9857893645", Address = "1/234 , Customer street 3", PermanentAccountNumber = "Pan321231", Country = "USA", AccountType = "Saving account" });

            return customerDetails;
        }
    }
}
