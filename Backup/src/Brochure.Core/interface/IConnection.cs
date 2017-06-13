using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Brochure.Core.Abstract;

namespace Brochure.Core
{
    public interface IConnection : IDisposable
    {
        IDocument Insert(IEntrity entrity);
        long Delete(BaseBuild build);
        long DeleteById<T>(Guid id) where T : IEntrity;
        long DeleteById(IEntrity entrity);
        long Update(BaseBuild updateBuild);
        List<IDocument> SearchProcedure(string procedureName, IParameter outParameter = null, params IParameter[] inParameter);
        T GetInfoById<T>(Guid id) where T : IEntrity;
        List<IDocument> Search(BaseBuild selectBuild);
        Task<IDocument> InsertAsync(IEntrity entrity);
        Task<long> DeleteAsync(BaseBuild build);
        Task<long> UpdateaAsync(BaseBuild updateBuild);
        Task<T> GetInfoByIdAsync<T>(Guid id) where T : IEntrity;
        Task<List<IDocument>> SearchAsync(BaseBuild selectBuild);
        Task<List<IDocument>> SearchProcedureAsync(string procedureName, IParameter outParameter = null, params IParameter[] inParameter);
        void Close();
        void Commit();
    }
}