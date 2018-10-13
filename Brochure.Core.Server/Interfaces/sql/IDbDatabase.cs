using System.Threading.Tasks;

namespace Brochure.Core.Server
{
    public interface IDbDatabase
    {
        #region Database
        #region Async
        Task<bool> IsExistDataBaseAsync (string databaseName);
        Task<long> CreateDataBaseAsync (string databaseName);
        Task<long> DeleteDataBaseAsync (string databaseName);
        #endregion
        #endregion
    }
}