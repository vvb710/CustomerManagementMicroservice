using BankManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace BankManagementSystem.Repository
{
    public class CustomerDetailsRepository : GenericRepository<CustomerDetails>, ICustomerDetailsRepository
    {
        public CustomerDetailsRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public bool IsCustomerExists(int id)
        {
            var customers = _dbContext.CustomerDetails.Where(c => c.Id == id);
            if (customers.Any())
            {
                return true;
            }
            return false;
        }

        public List<CustomerDetails> SearchByCustomerName(string name)
        {
            return _dbContext.CustomerDetails.Where(c => c.Name == name).ToList();
        }
    }
}