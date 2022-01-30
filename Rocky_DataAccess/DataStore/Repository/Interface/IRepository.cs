using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rocky_DataAccess.DataStore.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        public T Find(int id);
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
            string includeProperties = null,
            bool isTracking = true
            );
        public T FirstOrDefault(
             Expression<Func<T, bool>> filter = null,
             string includeProperties = null,
             bool isTracking = true
             );

        public void Add(T entity);
        public void Remove(T entity);
        public void Save();

    }
}
