using System.Threading.Tasks;

namespace Brochure.Core.Server
{
    public interface IDbColumns
    {
        #region Columns
        #region Async
        /// <summary>
        /// 判断指定表，指定列是否存在
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列名</param>
        /// <returns></returns>
        Task<bool> IsExistColumnAsync (string columnName);

        /// <summary>
        /// 向指定表中添加列
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="typeName">列的类型</param>
        /// <param name="isNotNull">是否为空</param>
        /// <returns></returns>
        Task<long> AddColumnsAsync (string columnName, string typeName, bool isNotNull);
        /// <summary>
        /// 删除指定表的指定列
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <returns></returns>
        Task<long> DeleteColumnAsync (string columnName);
        /// <summary>
        /// 修改制定表的制定列的类型
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="typeName">类型明</param>
        /// <param name="isNotNull">是否可以为空</param>
        /// <returns></returns>
        Task<long> UpdateColumnAsync (string columnName, string typeName, bool isNotNull);

        /// <summary>
        /// 对指定的表名，指定的列名 重命名
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="newcolumnName">新名称</param>
        /// <param name="typeName">类型</param>
        /// <returns></returns>
        Task<long> RenameColumnAsync (string columnName, string newcolumnName, string typeName);
        #endregion
        #endregion

    }
}