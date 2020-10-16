using BankManagementSystem.Models;
using System.Collections.Generic;

namespace BankManagementSystem.Service
{
    public interface ICustomerService
    {
        List<CustomerDetails> GetAllCustomerDetails();

        CustomerDetails GetDetailsById(int id);

        CustomerDetails CreateCustomer(CustomerDetails customerDetails);

        public CustomerDetails UpdateCustomer(CustomerDetails customerDetails);

        public void DeleteCustomer(int id);

        public bool IsCustomerRegistered(int id);

        public List<CustomerDetails> SearchCustomerByName(string name);
    }
}
