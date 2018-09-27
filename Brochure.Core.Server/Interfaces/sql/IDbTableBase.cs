using System.Threading.Tasks;

namespace Brochure.Core.Server
{
    public interface IDbTableBase
    {

        #region Table
        /// <summary>
        /// 判断表是否存在
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        Task<bool> IsExistTableAsync(string tableName);
        /// <summary>
        /// 创建表 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<long> CreateTable<T>() where T : EntityBase;
        /// <summary>
        /// 删除表 
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        Task<long> DeleteTableAsync(string tableName);

        Task<long> UpdateTableName(string olderName, string tableName);

        /// <summary>
        /// 注册表数据
        /// </summary>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        Task<long> RegistTableAsync<T>() where T : EntityBase;
        #endregion

    }
}