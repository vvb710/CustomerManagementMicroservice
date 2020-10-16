using BankManagementSystem.Extensions;
using BankManagementSystem.Models;
using BankManagementSystem.Repository;
using System.Collections.Generic;

namespace BankManagementSystem.Service
{
    public class QuoteService : IQuoteService
    {
        private IRepositoryWrapper _repo;

        public QuoteService(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        public List<QuoteDetails> GetAllQuoteDetails()
        {
            return _repo.Quote.GetAll();
        }

        public QuoteDetails GetQuoteDetailsDetailsById(int id)
        {
            return _repo.Quote.GetById(id);
        }

        public QuoteDetails CreateQuote(QuoteDetails quoteDetails)
        {
            quoteDetails.MaturityAmount = MaturityCalculation.CalculateMaturityAmount(quoteDetails);
            var quote = _repo.Quote.Insert(quoteDetails);
            _repo.Customer.Save();
            return quote;
        }

        public QuoteDetails UpdateQuote(QuoteDetails quoteDetails)
        {
            quoteDetails.MaturityAmount = MaturityCalculation.CalculateMaturityAmount(quoteDetails);
            var updatedQuote = _repo.Quote.Update(quoteDetails);
            _repo.Quote.Save();
            return updatedQuote;
        }

        public void DeleteQuote(int id)
        {
            _repo.Quote.Delete(id);
            _repo.Quote.Save();
        }

        public IEnumerable<QuoteDetails> GetQuoteDetailsByCustomerId(int customerId)
        {
            return _repo.Quote.GetQuoteDetailsByCustomerId(customerId);
        }

        public void DeleteQuoteDetailsByCustomerId(IEnumerable<QuoteDetails> quoteDetails)
        {
            _repo.Quote.DeleteRange(quoteDetails);
            _repo.Quote.Save();
        }

        public bool IsQuoteExists(int id)
        {
            return _repo.Quote.IsQuoteExists(id);
        }
    }
}
