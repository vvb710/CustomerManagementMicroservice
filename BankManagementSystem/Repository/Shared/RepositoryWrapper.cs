namespace BankManagementSystem.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private ApplicationDbContext _dbContext;

        private ICustomerDetailsRepository _customer;

        private IQuoteDetailsRepository _quote;

        public ICustomerDetailsRepository Customer
        {
            get
            {
                if (_customer == null)
                {
                    _customer = new CustomerDetailsRepository(_dbContext);
                }
                return _customer;
            }
        }

        public IQuoteDetailsRepository Quote
        {
            get
            {
                if (_quote == null)
                {
                    _quote = new QuoteDetailsRepository(_dbContext);
                }
                return _quote;
            }
        }

        public RepositoryWrapper(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
