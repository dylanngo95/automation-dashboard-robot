using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Data.Infrastructure
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {

        private AutomationDashboardDbContext dbContext;
        private readonly IDbSet<T> dbSet;

        protected IDbFactory DbFactory {
            get;
            private set;
        }

        protected AutomationDashboardDbContext DbContext {
            get { return dbContext ?? (dbContext = DbFactory.Init()); }
        }

        public RepositoryBase(IDbFactory dbFactory) {
            this.DbFactory = dbFactory;
            dbSet = DbContext.Set<T>();
        }

        public virtual T Add(T entity)
        {
            return dbSet.Add(entity);
        }

        public virtual T Delete(T entity)
        {
            return dbSet.Remove(entity);
        }

        public virtual T Delete(int id)
        {
            var entity = dbSet.Find(id);
            if (entity != null)
                return dbSet.Remove(entity);
            return null;
        }

        public virtual void DeleteMulti(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> results = dbSet.Where<T>(where).AsEnumerable();
            foreach (T item in results)
                dbSet.Remove(item);
        }

        public virtual IEnumerable<T> GetAll(string[] includes = null)
        {
            if (includes != null && includes.Count() > 0) {
                var query = dbContext.Set<T>().Include(includes.First());
                foreach (var item in includes.Skip(1)) {
                    query = query.Include(item);
                }
                return query.AsQueryable();
            }

            return dbContext.Set<T>().AsQueryable();
        }

        public IEnumerable<T> GetMulti(Expression<Func<T, bool>> expression, string[] includes = null)
        {
            if (includes != null && includes.Count() > 0) {
                var query = dbContext.Set<T>().Include(includes.First());
                foreach (var item in includes.Skip(1))
                    query = query.Include(item);
                return query.Where<T>(expression).AsQueryable();
            }

            return dbContext.Set<T>().Where<T>(expression).AsQueryable<T>();
        }

        public virtual IEnumerable<T> GetMultiPaging(Expression<Func<T, bool>> predicate, out int total, int index = 0, int size = 50, string[] includes = null)
        {
            int skipCount = index * size;
            IQueryable<T> _resetSet;

            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Count() > 0)
            {
                var query = dbContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                _resetSet = predicate != null ? query.Where<T>(predicate).AsQueryable() : query.AsQueryable();
            }
            else
            {
                _resetSet = predicate != null ? dbContext.Set<T>().Where<T>(predicate).AsQueryable() : dbContext.Set<T>().AsQueryable();
            }

            _resetSet = skipCount == 0 ? _resetSet.Take(size) : _resetSet.Skip(skipCount).Take(size);
            total = _resetSet.Count();
            return _resetSet.AsQueryable();
        }

        public T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null)
        {
            if (includes != null && includes.Count() > 0)
            {
                var query = dbContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.FirstOrDefault(expression);
            }
            return dbContext.Set<T>().FirstOrDefault(expression);
        }

        public virtual T GetSingleById(int id)
        {
            return dbSet.Find(id);
        }

        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual T UpdateResult(T entity)
        {
            dbSet.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }
    }
}
