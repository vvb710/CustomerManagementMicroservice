using BankManagementSystem.Models;
using System.Collections.Generic;

namespace BankManagementSystem.Service
{
    public interface IQuoteService
    {
        List<QuoteDetails> GetAllQuoteDetails();

        QuoteDetails GetQuoteDetailsDetailsById(int id);

        QuoteDetails CreateQuote(QuoteDetails customerDetails);

        public QuoteDetails UpdateQuote(QuoteDetails customerDetails);

        public void DeleteQuote(int id);

        public IEnumerable<QuoteDetails> GetQuoteDetailsByCustomerId(int customerId);

        public void DeleteQuoteDetailsByCustomerId(IEnumerable<QuoteDetails> quoteDetails);

        public bool IsQuoteExists(int id);
    }
}
