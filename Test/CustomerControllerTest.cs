using BankManagementSystem.Controllers;
using BankManagementSystem.Logging;
using BankManagementSystem.Models;
using BankManagementSystem.Repository;
using BankManagementSystem.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace Test
{
    public class CustomerControllerTest
    {
        private Mock<ICustomerService> _customerService { get; set; }
        private Mock<IQuoteService> _quoteService { get; set; }
        private Mock<ILoggerManager> _loggerService { get; set; }

        private void CreateMockServiceInstances()
        {
            _customerService = new Mock<ICustomerService>();
            _quoteService = new Mock<IQuoteService>();
            _loggerService = new Mock<ILoggerManager>();
        }

        #region Postive scenarios
        [Fact]
        public void GetMethodWithoutParameter_ShouldReturnListOfCustomerDetails()
        {
            //Arrange
            CreateMockServiceInstances();

            _customerService.Setup(service => service.GetAllCustomerDetails()).Returns(GetAllCustomerDetails());
            var controller = new CustomerController(_customerService.Object, _quoteService.Object, _loggerService.Object);

            //Act
            var result = controller.Get();

            //Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ObjectResult>(result);
            var customerDetails = actionResult.Value as List<CustomerDetails>;
            Assert.NotNull(customerDetails);
            Assert.Equal(2, customerDetails.Count);
            Assert.Equal((int)HttpStatusCode.OK, actionResult.StatusCode.Value);
        }

        [Fact]
        public void GetMethodWithParameterId_ShouldReturnCustomerDetails()
        {
            // Arrange            
            CreateMockServiceInstances();

            _customerService.Setup(service => service.GetDetailsById(It.IsAny<int>())).Returns(GetAllCustomerDetails()[0]);
            var controller = new CustomerController(_customerService.Object, _quoteService.Object, _loggerService.Object);

            // Act
            var result = controller.Get(100);

            // Assert            
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ObjectResult>(result);
            var customerDetails = actionResult.Value as CustomerDetails;
            Assert.NotNull(customerDetails);
            Assert.Equal("customer1", customerDetails.Name);
            Assert.Equal("customer1@mail.com", customerDetails.Email);
            Assert.Equal((int)HttpStatusCode.OK, actionResult.StatusCode.Value);
        }

        [Fact]
        public void GetMethodWithParameterName_ShouldReturnCustomerDetails()
        {
            // Arrange            
            CreateMockServiceInstances();

            _customerService.Setup(service => service.SearchCustomerByName(It.IsAny<string>())).Returns(GetAllCustomerDetails());
            var controller = new CustomerController(_customerService.Object, _quoteService.Object, _loggerService.Object);

            // Act
            var result = controller.Get("customer1");

            // Assert            
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ObjectResult>(result);
            var customerDetails = actionResult.Value as List<CustomerDetails>;
            Assert.NotNull(customerDetails);
            Assert.Equal(2, customerDetails.Count);
            Assert.Equal((int)HttpStatusCode.OK, actionResult.StatusCode.Value);
        }

        [Fact]
        public void PostMethod_ShouldReturnExpectedResult()
        {
            CreateMockServiceInstances();

            _customerService.Setup(service => service.CreateCustomer(It.IsAny<CustomerDetails>())).Returns(GetAllCustomerDetails()[0]);
            var controller = new CustomerController(_customerService.Object, _quoteService.Object, _loggerService.Object);

            // Act
            var response = controller.Post(GetAllCustomerDetails()[0]);

            // Assert  
            Assert.NotNull(response);
            var actionResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal($"Customer details has been created succesfully with id : {GetAllCustomerDetails()[0].Id}", actionResult.Value);
            Assert.Equal((int)HttpStatusCode.Created, actionResult.StatusCode.Value);
        }

        [Fact]
        public void PutMethod_ShouldReturnExpectedResult()
        {
            var updatedDetails = GetAllCustomerDetails()[0];
            updatedDetails.Country = "Sri Lanka";
            CreateMockServiceInstances();
            _customerService.Setup(service => service.UpdateCustomer(It.IsAny<CustomerDetails>())).Returns(updatedDetails);
            _customerService.Setup(service => service.IsCustomerRegistered(It.IsAny<int>())).Returns(true);
            var controller = new CustomerController(_customerService.Object, _quoteService.Object, _loggerService.Object);

            // Act
            var result = controller.Put(updatedDetails);

            // Assert  
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ObjectResult>(result);
            var details = actionResult.Value as CustomerDetails;
            Assert.NotNull(details);
            Assert.Equal("Sri Lanka", details.Country);
            Assert.Equal((int)HttpStatusCode.OK, actionResult.StatusCode.Value);
        }

        [Fact]
        public void DeleteMethod_ShouldReturnExpectedResult()
        {
            CreateMockServiceInstances();
            _customerService.Setup(service => service.DeleteCustomer(It.IsAny<int>()));
            _quoteService.Setup(service => service.DeleteQuoteDetailsByCustomerId(GetQuteDetails()));
            _customerService.Setup(service => service.IsCustomerRegistered(It.IsAny<int>())).Returns(true);
            var controller = new CustomerController(_customerService.Object, _quoteService.Object, _loggerService.Object);

            var id = 100;
            // Act
            var result = controller.Delete(id);

            // Assert 
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NoContent, actionResult.StatusCode.Value);
            Assert.Equal($"Customer with id {id} has been deleted succesfully with associated quote details to customer!!", actionResult.Value);
        }

        #endregion

        #region Negative scenarios

        [Fact]
        public void GetMethodWithOutParameter_ShouldReturnNotFoundResult()
        {
            // Arrange
            CreateMockServiceInstances();
            _customerService.Setup(service => service.GetAllCustomerDetails()).Returns((List<CustomerDetails>)null);
            var controller = new CustomerController(_customerService.Object, _quoteService.Object, _loggerService.Object);

            // Act
            var response = controller.Get();

            //Assert
            Assert.NotNull(response);
            var actionResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal("No customers available", actionResult.Value);
            Assert.Equal((int)HttpStatusCode.NotFound, actionResult.StatusCode.Value);
        }

        [Fact]
        public void GetMethodWithParameterId_ShouldReturnNotFoundResult()
        {
            // Arrange   
            CreateMockServiceInstances();
            var controller = new CustomerController(_customerService.Object, _quoteService.Object, _loggerService.Object);

            var id = 1000;
            // Act
            var result = controller.Get(id);

            // Assert            
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal($"No customer found with id : {id}", actionResult.Value);
            Assert.Equal((int)HttpStatusCode.NotFound, actionResult.StatusCode.Value);
        }

        [Fact]
        public void GetMethodWithParameterName_ShouldReturnNotFoundResult()
        {
            // Arrange            
            CreateMockServiceInstances();
            var controller = new CustomerController(_customerService.Object, _quoteService.Object, _loggerService.Object);

            var customerName = "demo customer";
            // Act
            var result = controller.Get(customerName);

            // Assert            
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal($"No customers found with name : {customerName}", actionResult.Value);
            Assert.Equal((int)HttpStatusCode.NotFound, actionResult.StatusCode.Value);
        }

        [Fact]
        public void PutMethod_ShouldReturnNotFoundResult()
        {
            // Arrange        
            CreateMockServiceInstances();
            var controller = new CustomerController(_customerService.Object, _quoteService.Object, _loggerService.Object);
            _customerService.Setup(service => service.IsCustomerRegistered(It.IsAny<int>())).Returns(false);
            var updatedCustomerDetails = GetCustomerDetailsForNegativeScenarios();
            updatedCustomerDetails.Country = "China";

            // Act
            var result = controller.Put(updatedCustomerDetails);

            // Assert   
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, actionResult.StatusCode.Value);
        }

        [Fact]
        public void DeleteMethod_ShouldReturnNotFoundResult()
        {
            // Arrange            
            CreateMockServiceInstances();
            _customerService.Setup(service => service.DeleteCustomer(It.IsAny<int>()));
            _customerService.Setup(service => service.IsCustomerRegistered(It.IsAny<int>())).Returns(false);
            var controller = new CustomerController(_customerService.Object, _quoteService.Object, _loggerService.Object);

            var id = 100;

            // Act
            var result = controller.Delete(id) ;

            // Assert            
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, actionResult.StatusCode.Value);
        }
        #endregion

        #region private method
        private List<CustomerDetails> GetAllCustomerDetails()
        {
            return new List<CustomerDetails>
            {
                new CustomerDetails(){ Id = 100, UserName="customer1@123",Name="customer1",Email="customer1@mail.com",Password="customer1Pass@321",DateOfBirth=new DateTime(1999,10,29),State="Ohio",ContactNumber="9826374890",Address="1/234 , Customer street 1",PermanentAccountNumber="Pan321",Country="USA",AccountType="Saving account"},
                new CustomerDetails(){ Id = 101, UserName="customer2@123",Name="customer2",Email="customer2@mail.com",Password="customer2Pass@321",DateOfBirth=new DateTime(1999,4,29),State="New York",ContactNumber="9236719374",Address="1/321 , Customer street 2",PermanentAccountNumber="Pan213",Country="USA",AccountType="Saving account"}
            };
        }

        private IEnumerable<QuoteDetails> GetQuteDetails()
        {
            var quoteDetails = new List<QuoteDetails>
            {
                new QuoteDetails
                {
                    Id = 2000,
                    CustomerId = 100,
                    ContributionAmount = 200,
                    StartDate = new DateTime(2020, 09, 1),
                    EndDate = new DateTime(2020, 10, 10),
                    MaturityAmount = 500
                },
                new QuoteDetails
                {
                    Id = 2001,
                    CustomerId = 100,
                    ContributionAmount = 200,
                    StartDate = new DateTime(2020, 09, 1),
                    EndDate = new DateTime(2020, 10, 10),
                    MaturityAmount = 500
                }
            };
            return quoteDetails;
        }

        private CustomerDetails GetCustomerDetailsForNegativeScenarios()
        {
            return new CustomerDetails() { Id = 150, UserName = "customer1@123", Name = "customer1", Email = "customer1@mail.com", Password = "customer1Pass@321", DateOfBirth = new DateTime(1999, 10, 29), State = "Ohio", ContactNumber = "9826374890", Address = "1/234 , Customer street 1", PermanentAccountNumber = "Pan321", Country = "USA", AccountType = "Saving account" };
        }
        #endregion
    }
}
