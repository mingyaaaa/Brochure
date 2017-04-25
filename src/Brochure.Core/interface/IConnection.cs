using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Brochure.Core.Query;

namespace Brochure.Core
{
    public interface IConnection : IDisposable
    {
        ITransaction Transaction { get; }
        IDocument Insert(IEntrity entrity);
        long Delete<T>(QueryBuild build, IEntrity entrity = null) where T : IEntrity;
        long DeleteById<T>(Guid id) where T : IEntrity;
        long DeleteById(IEntrity entrity);
        long Update(IEntrity obj);
        T GetInfoById<T>(string id);
        T Search<T>(string str);
        Task<IDocument> InsertAsync(IEntrity entrity);
        Task<long> DeleteAsync(IEntrity entrity);
        Task<long> UpdateaAsync(IEntrity entrity);
        Task<T> GetInfoByIdAsync<T>(Guid id);
        Task<T> SearchAsync<T>(string str);
        void Close();
        void Commit();
    }
}