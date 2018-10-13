using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brochure.Core.Server
{
    public interface IDbData
    {
        #region Data

        #region Async
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        Task<long> InserOneAsync (EntityBase entity);
        /// <summary>
        /// 添加多个数据
        /// </summary>
        /// <param name="entitys">实体对象集合</param>
        Task<long> InserManyAsync (IEnumerable<EntityBase> entities);
        /// <summary>
        /// 删除一个数据
        /// </summary>
        /// <param name="id">主建Id</param>
        Task<long> DeleteAsync (Guid id);
        /// <summary>
        /// 删除多个数据
        /// </summary>
        /// <param name="ids">主建集合</param>
        Task<long> DeleteAsync (IEnumerable<Guid> ids);
        /// <summary>
        /// 删除多个数据
        /// </summary>
        /// <param name="ids">主建集合</param>
        Task<long> DeleteAsync (Query query);
        /// <summary>
        /// 更新一个数据
        /// </summary>
        /// <param name="id">主建</param>
        /// <param name="doc">需要更新的数据</param>
        Task<long> UpdateAsync (Guid id, IRecord doc);
        /// <summary>
        /// 更新一个数据
        /// </summary>
        /// <param name="query">对应的查询条件{field:value}</param>
        /// <param name="doc">需要更新的数据</param>
        Task<long> UpdateAsync (Query query, IRecord doc);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        Task<List<IRecord>> GetListAsync (SearchParams searchParams);

        Task<List<IRecord>> GetListGroupByAsync (List<Aggregate> aggregates, SearchParams searchParams, params string[] groupField);

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        Task<long> GetCountAsync (Query query);
        /// <summary>
        /// 获取单个信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        Task<IRecord> GetInfoAsync (Guid guid);
        #endregion

        #region SQLIndex
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ColumnNames">列名</param>
        /// <param name="indexName">索引名称</param>
        /// <param name="sqlIndex">索引类型</param>
        /// <returns></returns>
        Task<long> CreateIndexAsync (string[] ColumnNames, string indexName, string sqlIndex);
        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="indexName">索引名称</param>
        /// <returns></returns>
        Task<long> DeleteIndexAsync (string indexName);
        #endregion
        #endregion
    }
}