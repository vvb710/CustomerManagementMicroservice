using BankManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankManagementSystem.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public ApplicationDbContext _dbContext { get; set; }

        private DbSet<T> _table;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _table = dbContext.Set<T>();
        }

        public void Delete(object id)
        {
            T exists = _table.Find(id);
            _table.Remove(exists);
        }

        public List<T> GetAll()
        {
            return _table.ToList();
        }

        public T GetById(object id)
        {
            return _table.Find(id);
        }

        public T Insert(T entity)
        {
            _table.Add(entity);
            return entity;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public T Update(T entity)
        {
            _table.Update(entity);
            return entity;
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _table.RemoveRange(entities);
        }
    }
}
