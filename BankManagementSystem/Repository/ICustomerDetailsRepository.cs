using BankManagementSystem.Models;
using System.Collections.Generic;

namespace BankManagementSystem.Repository
{
    public interface ICustomerDetailsRepository : IGenericRepository<CustomerDetails>
    {
        bool IsCustomerExists(int id);

        public List<CustomerDetails> SearchByCustomerName(string name);
    }
}
