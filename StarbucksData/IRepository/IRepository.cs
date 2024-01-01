using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StarbucksData.IRepository
{
    public interface IRepository<T> where T : class
    {
        T GetFirstorDefault(Expression<Func<T, bool>> filter, string? includeproperties = null, bool tracked = true);

        Task<T> GetFirstorDefaultAsync(Expression<Func<T, bool>> filter, string? includeproperties = null, bool tracked = true);

        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeproperties = null);

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeproperties = null);

        void Add(T entity);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);
    }
}
