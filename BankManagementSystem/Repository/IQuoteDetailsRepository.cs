using BankManagementSystem.Models;
using System.Collections.Generic;

namespace BankManagementSystem.Repository
{
    public interface IQuoteDetailsRepository : IGenericRepository<QuoteDetails>
    {
        public IEnumerable<QuoteDetails> GetQuoteDetailsByCustomerId(int customerId);

        bool IsQuoteExists(int id);
    }
}
