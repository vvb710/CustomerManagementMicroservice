using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankManagementSystem.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        List<T> GetAll();

        T GetById(object id);

        T Insert(T entity);

        T Update(T entity);

        void Delete(object id);

        void Save();

        public void DeleteRange(IEnumerable<T> entities);
    }
}
