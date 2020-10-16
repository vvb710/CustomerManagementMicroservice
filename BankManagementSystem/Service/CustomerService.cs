using BankManagementSystem.Models;
using BankManagementSystem.Repository;
using System.Collections.Generic;

namespace BankManagementSystem.Service
{
    public class CustomerService : ICustomerService
    {
        private IRepositoryWrapper _repo;

        public CustomerService(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        public List<CustomerDetails> GetAllCustomerDetails()
        {
            return _repo.Customer.GetAll();
        }

        public CustomerDetails GetDetailsById(int id)
        {
            return _repo.Customer.GetById(id);
        }

        public CustomerDetails CreateCustomer(CustomerDetails customerDetails)
        {
            var customer = _repo.Customer.Insert(customerDetails);
            _repo.Customer.Save();
            return customer;
        }

        public CustomerDetails UpdateCustomer(CustomerDetails customerDetails)
        {
            var updatedCustomer = _repo.Customer.Update(customerDetails);
            _repo.Customer.Save();
            return updatedCustomer;
        }

        public void DeleteCustomer(int id)
        {
            _repo.Customer.Delete(id);
            _repo.Customer.Save();
        }

        public bool IsCustomerRegistered(int id)
        {
            return _repo.Customer.IsCustomerExists(id);
        }

        public List<CustomerDetails> SearchCustomerByName(string name)
        {
            return _repo.Customer.SearchByCustomerName(name);
        }
    }
}
