using System;
using System.Linq;
using System.Net;
using BankManagementSystem.Models;
using BankManagementSystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace BankManagementSystem.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        private IQuoteService _quoteService;

        private ICustomerService _customerService;

        public QuoteController(IQuoteService quoteService, ICustomerService customerService)
        {
            _quoteService = quoteService;
            _customerService = customerService;
        }

        /// <summary>
        /// GET: api/customer
        /// </summary>
        /// <returns>List of customers</returns>
        [HttpGet]
        public IActionResult Get()
        {
            var quoteDetails = _quoteService.GetAllQuoteDetails();

            if (quoteDetails == null || quoteDetails.Count() == 0)
            {
                return StatusCode((int)HttpStatusCode.NotFound, "No quotes available");
            }
            return StatusCode((int)HttpStatusCode.OK, quoteDetails);
        }

        /// <summary>
        /// GET api/Customer/{id}
        /// </summary>
        /// <param name="id">CustomerId</param>
        /// <returns>a specific customer</returns>
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var quoteDetails = _quoteService.GetQuoteDetailsDetailsById(id);
            if (quoteDetails == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound, $"No quote found with Quote Id: {id}");
            }
            return StatusCode((int)HttpStatusCode.OK, quoteDetails);
        }

        /// <summary>
        /// Create/Register a customer
        /// POST api/Customer
        /// </summary>
        /// <param name="customerDetails">CustomerDetails</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] QuoteDetails quoteDetails)
        {
            if (quoteDetails.StartDate > quoteDetails.EndDate)
            {
                return BadRequest("Start date cannot be future date");
            }
            var isCustomerAvailable = _customerService.IsCustomerRegistered(quoteDetails.CustomerId);
            if (isCustomerAvailable)
            {
                var quoteDetail = _quoteService.CreateQuote(quoteDetails);
                return StatusCode((int)HttpStatusCode.Created, $"Quote has been created succesfully for the customer. Quote Id : {quoteDetail.Id} and Maturity amount : {quoteDetail.MaturityAmount}");
            }
            return StatusCode((int)HttpStatusCode.NotFound, $"There is no cutomer available with customer Id : {quoteDetails.CustomerId} to create quote");
        }

        /// <summary>
        /// Update customer details by Id
        /// PUT api/Customer/5
        /// </summary>
        /// <param name="id">customerid</param>
        /// <param name="customerDetails">updated customerDetails</param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put([FromBody] QuoteDetails quoteDetails)
        {
            if (quoteDetails.StartDate > quoteDetails.EndDate)
            {
                return BadRequest("Start date cannot be future date");
            }
            var isCustomerAvailable = _customerService.IsCustomerRegistered(quoteDetails.CustomerId);
            if (isCustomerAvailable)
            {
                var isQuoteAvailable = _quoteService.IsQuoteExists(quoteDetails.Id);
                if (isQuoteAvailable)
                {
                    var quoteDetail = _quoteService.UpdateQuote(quoteDetails);
                    return StatusCode((int)HttpStatusCode.Created, $"Quote id: {quoteDetail.Id} has been updated succesfully with Maturity amount : {quoteDetail.MaturityAmount}");
                }
                return StatusCode((int)HttpStatusCode.NotFound, $"The cutomer - {quoteDetails.CustomerId} doesnt have quote id : {quoteDetails.Id} linked");
            }
            return StatusCode((int)HttpStatusCode.NotFound, $"There is no cutomer available with customer Id : {quoteDetails.CustomerId}");
        }

        /// <summary>
        /// Delete customer details by id
        /// DELETE api/Customer/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var isQuoteAvailable = _quoteService.IsQuoteExists(id);
            if (isQuoteAvailable)
            {
                _quoteService.DeleteQuote(id);
                return StatusCode((int)HttpStatusCode.NoContent, $"Quote - {id} has been deleted succesfully!!");
            }
            return StatusCode((int)HttpStatusCode.NotFound, $"No Quote available with id {id}");
        }

        /// <summary>
        /// GET: api/quote/test
        /// </summary>
        /// <returns>List of quote</returns>
        [HttpGet]
        [Route("test")]
        public IActionResult GetResult()
        {
            RandomException();
            var quoteDetails = _quoteService.GetAllQuoteDetails();

            if (quoteDetails == null || quoteDetails.Count() == 0)
            {
                return Ok("No records available");
            }
            return Ok(quoteDetails);
        }

        private void RandomException()
        {
            var randomNumber = new Random().Next(1, 10);
            if (randomNumber % 2 == 1)
            {
                throw new Exception("Somethong went wrong");
            }

        }
    }
}
