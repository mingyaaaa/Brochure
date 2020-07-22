using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brochure.ORM
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<T> Get (IQuery query);

        Task<T> Get (string id);

        Task<int> Update (IQuery query, T entity);

        Task<int> Update (string id, T entity);

        Task<bool> Delete (IQuery query);

        Task<bool> Delete (string id);

        Task<bool> DeleteMany (IEnumerable<string> ids);

        Task<IEnumerable<T>> List (IQuery query);

        Task<IEnumerable<T>> List (IEnumerable<string> ids);

        Task<int> Insert (T entity);

        Task<T> InsetAndGet (T entity);

        Task<int> Insert (IEnumerable<T> entity);
    }
}