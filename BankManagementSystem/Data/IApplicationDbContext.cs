using BankManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BankManagementSystem.Data
{
    public interface IApplicationDbContext<T> where T :class
    {
        public DbSet<T> details { get; set; }

        int SaveChanges();
    }
}
