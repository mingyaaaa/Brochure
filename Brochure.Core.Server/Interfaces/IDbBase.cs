using Brochure.Core.Interfaces;
using Brochure.Core.Server.Abstracts;
using Brochure.Core.Server.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brochure.Core.Server.Interfaces
{
    public interface IDbBase
    {
        #region Data
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        Task<long> InserOneAsync(EntityBase entity);
        /// <summary>
        /// 添加多个数据
        /// </summary>
        /// <param name="entitys">实体对象集合</param>
        Task<long> InserManyAsync(IEnumerable<EntityBase> entities, bool isTransaction);
        /// <summary>
        /// 删除一个数据
        /// </summary>
        /// <param name="id">主建Id</param>
        Task<long> DeleteOneAsync(Guid id);
        /// <summary>
        /// 删除多个数据
        /// </summary>
        /// <param name="ids">主建集合</param>
        Task<long> DeleteManyAsync(Guid[] ids, bool isTransaction);
        /// <summary>
        /// 更新一个数据
        /// </summary>
        /// <param name="id">主建</param>
        /// <param name="doc">需要更新的数据</param>
        Task<long> UpdateOneAsync(Guid id, IBDocument doc);
        /// <summary>
        /// 更新一个数据
        /// </summary>
        /// <param name="query">对应的查询条件{field:value}</param>
        /// <param name="doc">需要更新的数据</param>
        Task<long> UpdateOneAsync(IBDocument query, IBDocument doc);
        /// <summary>
        /// 更新各个Id对应的数据
        /// </summary>
        /// <param name="docs">包含Id的数据</param>
        Task<long> UpdateManyAsync(List<IBDocument> docs, bool isTransaction);
        /// <summary>
        /// 更新数据  如果不存在就添加  存在就更新
        /// </summary>
        /// <param name="entity">包含Id的实体数据</param>
        Task<long> UpdateOrAddAsync(EntityBase entity);
        #endregion
        #region Table
        /// <summary>
        /// 判断表是否存在
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        Task<long> IsExistTable(string tableName);
        /// <summary>
        /// 创建表 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<long> CreateTable(EntityBase entity);
        /// <summary>
        /// 删除表 
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        Task<long> DeleteTable(string tableName);
        #endregion
        #region Colomns
        /// <summary>
        /// 判断指定表，指定列是否存在
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="colomnName">列名</param>
        /// <returns></returns>
        Task<long> IsExist(string tableName, string colomnName);
        /// <summary>
        /// 向指定表中添加列
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="colomnName">列名</param>
        /// <param name="typeName">列的类型</param>
        /// <returns></returns>
        Task<long> AddColomns(string tableName, string colomnName, string typeName, bool IsNotNull);
        /// <summary>
        /// 删除指定表的指定列
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="colomnName">列名</param>
        /// <returns></returns>
        Task<long> DeleteColomn(string tableName, string colomnName);
        /// <summary>
        /// 修改制定表的制定列的类型
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="colomnName">列名</param>
        /// <param name="typeName">类型明</param>
        /// <param name="IsNotNull">是否可以为空</param>
        /// <returns></returns>
        Task<long> UpdateColomn(string tableName, string colomnName, string typeName, bool IsNotNull);
        /// <summary>
        /// 对指定的表名，指定的列名 重命名
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="colomnName">列名</param>
        /// <param name="newColomnName">新名称</param>
        /// <returns></returns>
        Task<long> RenameColomn(string tableName, string colomnName, string newColomnName);
        #endregion
        #region SQLIndex
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="colomnNames">列名</param>
        /// <param name="indexName">索引名称</param>
        /// <param name="sqlIndex">索引类型</param>
        /// <returns></returns>
        Task<long> CreateIndex(string tableName, string[] colomnNames, string indexName, SQLIndex sqlIndex);
        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="indexName">索引名称</param>
        /// <returns></returns>
        Task<long> DeleteIndex(string tableName, string indexName);

        #endregion
        #region Table
        /// <summary>
        /// 注册表数据
        /// </summary>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        Task<long> RegistTable(params string[] tableNames);
        #endregion
    }
}