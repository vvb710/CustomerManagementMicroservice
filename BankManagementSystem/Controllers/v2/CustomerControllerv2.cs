using System.Linq;
using System.Net;
using BankManagementSystem.Logging;
using BankManagementSystem.Models;
using BankManagementSystem.Repository;
using BankManagementSystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace BankManagementSystem.Controllers
{
    [Route("api/v{version:apiVersion}/customer")]
    [ApiVersion("2.0")]
    [ApiExplorerSettings(GroupName = "v2")]
    [ApiController]
    public class CustomerControllerV2 : ControllerBase
    {
        private ICustomerService _customerService;

        private IQuoteService _quoteService;

        private readonly ILoggerManager _logger;

        public CustomerControllerV2(ICustomerService customerService, IQuoteService quoteService, ILoggerManager logger)
        {
            _customerService = customerService;
            _quoteService = quoteService;
            _logger = logger;
        }

        /// <summary>
        /// GET: api/customer
        /// </summary>
        /// <returns>List of customers</returns>
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInfo("Fetching all the Customer details");

            var customerDetails = _customerService.GetAllCustomerDetails();

            if (customerDetails == null || customerDetails.Count() == 0)
            {
                return StatusCode((int)HttpStatusCode.NotFound, "No customers available");
            }

            _logger.LogInfo($"Returning {customerDetails.Count()} customers.");
            return StatusCode((int)HttpStatusCode.OK, customerDetails);
        }

        /// <summary>
        /// GET api/Customer/{id}
        /// </summary>
        /// <param name="id">CustomerId</param>
        /// <returns>a specific customer</returns>
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            _logger.LogInfo($"Feteching details for customer with id {id}");

            var customerDetail = _customerService.GetDetailsById(id);
            if (customerDetail == null)
            {
                _logger.LogInfo($"No customer found with id : {id}");
                return StatusCode((int)HttpStatusCode.NotFound, $"No customer found with id : {id}");
            }
            _logger.LogInfo($"Customer found with id : {id}");
            return StatusCode((int)HttpStatusCode.OK, customerDetail);
        }

        /// <summary>
        /// Create/Register a customer
        /// POST api/Customer
        /// </summary>
        /// <param name="customerDetails">CustomerDetails</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] CustomerDetails customerDetails)
        {
            _logger.LogInfo("Creating an customer.....");
            var customer = _customerService.CreateCustomer(customerDetails);
            _logger.LogInfo($"Created customer succesfully with customer id : {customer.Id}...!!");
            return StatusCode((int)HttpStatusCode.Created, $"Customer details has been created succesfully with id : {customer.Id}");
        }

        /// <summary>
        /// Update customer details by Id
        /// PUT api/Customer/5
        /// </summary>
        /// <param name="id">customerid</param>
        /// <param name="customerDetails">updated customerDetails</param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put([FromBody] CustomerDetails customerDetails)
        {
            _logger.LogInfo($"Updating customer with Id : {customerDetails.Id}");

            var isCustomerRegistered = _customerService.IsCustomerRegistered(customerDetails.Id);
            if (isCustomerRegistered)
            {
                var updatedDetails = _customerService.UpdateCustomer(customerDetails);
                _logger.LogInfo($"Updating customer successfully...!!");
                return StatusCode((int)HttpStatusCode.OK, updatedDetails);
            }
            _logger.LogInfo($"Customer with Id : {customerDetails.Id} not available for update");
            return StatusCode((int)HttpStatusCode.NotFound, $"Customer with Id : {customerDetails.Id} not available for update");
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
            _logger.LogInfo($"Initiatig delete process for customer with Id : {id}");

            var isCustomerRegistered = _customerService.IsCustomerRegistered(id);
            if (isCustomerRegistered)
            {
                var quoteDetails = _quoteService.GetQuoteDetailsByCustomerId(id);
                _quoteService.DeleteQuoteDetailsByCustomerId(quoteDetails);
                _customerService.DeleteCustomer(id);
                _logger.LogInfo($"Customer with id {id} has been deleted succesfully");
                return StatusCode((int)HttpStatusCode.NoContent, $"Customer with id {id} has been deleted succesfully with associated quote details to customer!!");
            }
            _logger.LogInfo($"Customer with Id : {id} not available for delete");
            return StatusCode((int)HttpStatusCode.NotFound, $"Customer with Id : {id} not available for delete");
        }
    }
}
