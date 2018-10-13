using System.Threading.Tasks;

namespace Brochure.Core.Server
{
    public interface IDbTableBase
    {

        #region Table

        #region Async
        /// <summary>
        /// 判断表是否存在
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        Task<bool> IsExistTableAsync (string tableName);
        /// <summary>
        /// 创建表 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<long> CreateTableAsync<T> () where T : EntityBase;
        /// <summary>
        /// 删除表 
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        Task<long> DeleteTableAsync (string tableName);
        /// <summary>
        /// 修改表名
        /// </summary>
        /// <param name="olderName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Task<long> UpdateTableNameAsync (string olderName, string tableName);

        /// <summary>
        /// 注册表数据
        /// </summary>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        Task<long> RegistTableAsync<T> () where T : EntityBase;
        #endregion

        #endregion

    }
}