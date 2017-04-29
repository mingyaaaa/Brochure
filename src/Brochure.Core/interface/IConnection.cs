using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Brochure.Core.Query;

namespace Brochure.Core
{
    public interface IConnection : IDisposable
    {
        ITransaction Transaction { get; }
        IDocument Insert(InsertBuid query);
        long Delete(DeleteBuild build);
        long DeleteById<T>(Guid id) where T : IEntrity;
        long DeleteById(IEntrity entrity);
        long Update(UpdateBuild updateBuild);
        T GetInfoById<T>(string id);
        IDocument GetInfoById(string id);
        IDocument Search(string str);
        Task<IDocument> InsertAsync(IEntrity entrity);
        Task<long> DeleteAsync(IEntrity entrity);
        Task<long> UpdateaAsync(IDocument entrity);
        Task<T> GetInfoByIdAsync<T>(Guid id);
        Task<IDocument> GetInfoByIdAsync(Guid id);
        Task<IDocument> SearchAsync(string str);
        void Close();
        void Commit();
    }
}