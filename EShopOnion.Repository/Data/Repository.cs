using EShopOnion.DataAccess.Entities;
using EShopOnion.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EShopOnion.Repository.Data
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;
        private readonly DbSet<T> _entities;

        public Repository(StoreContext dbContext)
        {
            _dbContext = dbContext;
            _entities = _dbContext.Set<T>();
        }
        public virtual T GetById(int id)
        {
            return _entities.Find(id);
        }
        public virtual IEnumerable<T> List()
        {
            return _entities.AsEnumerable();
        }
        public virtual IEnumerable<T> List(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _entities
                   .Where(predicate)
                   .AsEnumerable();
        }
        public void Create(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _entities.Add(entity);
            _dbContext.SaveChanges();
        }
        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
        public void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _entities.Remove(entity);
            _dbContext.SaveChanges();
        }
    }
}
