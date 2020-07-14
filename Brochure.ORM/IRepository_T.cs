using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brochure.ORM
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<T> Get (IQuery query);

        Task<T> Get (Guid id);

        Task<T> Update (IQuery query, T entity);

        Task<T> Update (Guid id, T entity);

        Task<bool> Delete (IQuery query);

        Task<bool> Delete (Guid id);

        Task<IEnumerable<T>> List (IQuery query);

        Task<IEnumerable<T>> List (IEnumerable<Guid> ids);
    }
}