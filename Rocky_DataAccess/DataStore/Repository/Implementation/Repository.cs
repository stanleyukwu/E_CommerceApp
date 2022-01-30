using Microsoft.EntityFrameworkCore;
using Rocky_DataAccess.DataStore.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rocky_DataAccess.DataStore.Repository
{
    public class Repository<T> : IRepository<T> where T: class

    {
        private readonly AppDbContext _context;
        internal DbSet<T> dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            this.dbSet = _context.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Find(int id)
        {
          return dbSet.Find(id);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> filter = null,
                              string includeProperties = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includProp);
                }
            }
          
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, string includeProperties = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }
            if(includeProperties != null)
            {
                foreach (var includProp in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includProp);
                }
            }
            if(orderby != null)
            {
                query = orderby(query);
            }
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }catch(DbException dbexc)
            {
                throw new Exception(dbexc.Message);
            }
        }
    }
}
