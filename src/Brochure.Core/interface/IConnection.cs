using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Brochure.Core.Query;

namespace Brochure.Core
{
    public interface IConnection : IDisposable
    {
        IDocument Insert(InsertBuid query);
        long Delete(DeleteBuild build);
        long DeleteById<T>(Guid id) where T : IEntrity;
        long DeleteById(IEntrity entrity);
        long Update(UpdateBuild updateBuild);
        List<IDocument> SearchProcedure(string procedureName, IParameter outParameter = null, params IParameter[] inParameter);
        T GetInfoById<T>(Guid id) where T : IEntrity;
        List<IDocument> Search(SelectBuild selectBuild);
        Task<IDocument> InsertAsync(IEntrity entrity);
        Task<long> DeleteAsync(DeleteBuild build);
        Task<long> UpdateaAsync(UpdateBuild updateBuild);
        Task<T> GetInfoByIdAsync<T>(Guid id) where T : IEntrity;
        Task<List<IDocument>> SearchAsync(SelectBuild selectBuild);
        Task<List<IDocument>> SearchProcedureAsync(string procedureName, IParameter outParameter = null, params IParameter[] inParameter);
        void Close();
        void Commit();
    }
}