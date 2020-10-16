using BankManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace BankManagementSystem.Repository
{
    public class QuoteDetailsRepository : GenericRepository<QuoteDetails>, IQuoteDetailsRepository
    {
        public QuoteDetailsRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public IEnumerable<QuoteDetails> GetQuoteDetailsByCustomerId(int customerId)
        {
            var quoteDetails = _dbContext.QuoteDetails.Where(c => c.CustomerId == customerId);
            return quoteDetails;
        }

        public bool IsQuoteExists(int id)
        {
            var quote = _dbContext.QuoteDetails.Where(c => c.Id == id);
            if (quote.Any())
            {
                return true;
            }
            return false;
        }
    }
}