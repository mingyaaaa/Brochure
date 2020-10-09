using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brochure.ORM.Database;
using Brochure.ORM.Querys;

namespace Brochure.ORM
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : EntityBase
    {
        protected readonly DbContext context;
        protected readonly DbData dbData;
        public RepositoryBase (DbContext context)
        {
            this.context = context;
            dbData = context.GetDbData ();
        }
        public Task<bool> Delete (IWhereQuery query)
        {
            var r = dbData.Delete<T> (query);
            return Task.FromResult (r > 0);
        }

        public async Task<bool> Delete (string id)
        {
            var query = new Query<T> (context.GetDbProvider ());
            var q = query.WhereAnd (t => t.Id == id) as IWhereQuery;
            return await Delete (q);
        }

        public async Task<bool> DeleteMany (IEnumerable<string> ids)
        {
            var query = new Query<T> (context.GetDbProvider ());
            var q = query.WhereAnd (t => ids.Contains (t.Id)) as IWhereQuery;
            return await Delete (q);
        }

        public async Task<T> Get (IQuery query)
        {
            var t = dbData.Query<T> (query);
            return t.FirstOrDefault ();
        }

        public async Task<T> Get (string id)
        {
            var query = new Query<T> (context.GetDbProvider ());
            var q = query.WhereAnd (t => t.Id == id);
            return await Get (q);
        }

        public Task<int> Insert (T entity)
        {
            var r = dbData.Insert (entity);
            return Task.FromResult (r);
        }

        public Task<int> Insert (IEnumerable<T> entity)
        {
            var r = dbData.InsertMany (entity);
            return Task.FromResult (r);
        }

        public async Task<T> InsetAndGet (T entity)
        {
            var r = await Insert (entity);
            if (r <= 0)
                return null;
            var e = await Get (entity.Id);
            return e;
        }

        public Task<IEnumerable<T>> List (IQuery query)
        {
            var t = dbData.Query<T> (query);
            return Task.FromResult (t);
        }

        public async Task<IEnumerable<T>> List (IEnumerable<string> ids)
        {
            var query = new Query<T> (context.GetDbProvider ());
            var q = query.WhereAnd (t => ids.Contains (t.Id));
            return await List (q);
        }

        public Task<int> Update (IWhereQuery query, T entity)
        {
            if (query == null)
                throw new Exception ("query不能为null");
            var t = dbData.Update<T> (entity, query);
            return Task.FromResult (t);
        }

        public async Task<int> Update (string id, T entity)
        {
            var query = new Query<T> (context.GetDbProvider ());
            var q = query.WhereAnd (t => t.Id == id) as IWhereQuery;
            return await Update (q, entity);
        }
    }
}