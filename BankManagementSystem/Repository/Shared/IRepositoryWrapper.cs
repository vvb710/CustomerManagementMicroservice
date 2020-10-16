namespace BankManagementSystem.Repository
{
    public interface IRepositoryWrapper
    {
        ICustomerDetailsRepository Customer { get; }

        IQuoteDetailsRepository Quote { get; }

        void Save();
    }
}
