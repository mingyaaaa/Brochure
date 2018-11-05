using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Common;
using Brochure.Core.Server.Enums.sql;

namespace Brochure.Core.Server
{
    public interface IClient
    {

        TypeMap TypeMap { get; }

        ISqlParse SqlParse { get; }

        string DatabaseName { get; set; }
        /// <summary>
        /// 获取数据库的操作中心
        /// </summary>
        /// <returns></returns>
        Task<IDatabaseHub> GetDatabaseHubAsync();
        /// <summary>
        ///  获取数据表的操作中心
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        Task<IDataTableHub> GetDataTableHubAsync(string databaseName = null);
        /// <summary>
        /// 获取数据操作中心
        /// </summary>
        /// <returns></returns>
        Task<IDataHub> GetDataHubAsync<T>(IDbTransaction dbTransaction = null) where T : EntityBase;
        /// <summary>
        /// 获取数据操作中心
        /// </summary>
        /// <returns></returns>
        Task<IDataHub> GetDataHubAsync(string tableName, IDbTransaction dbTransaction = null);

        void SetDatabase(string databaseName);

        IDbTransaction BeginTransaction();
    }

    public interface IDatabaseHub : IDisposable
    {
        IClient Client { get; }
        /// <summary>
        /// 判断数据库是否存在
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        Task<bool> IsExistDataBaseAsync(string databaseName);
        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        Task<long> CreateDataBaseAsync(string databaseName);
        /// <summary>
        /// 删除数据库
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        Task<long> DeleteDataBaseAsync(string databaseName);

        /// <summary>
        /// 切换数据库
        /// </summary>
        /// <returns></returns>
        Task<long> ChangeDatabaseAsync(string databaseName);

    }

    public interface IDataTableHub : IDisposable
    {
        IClient Client { get; }
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
        Task<long> CreateTableAsync<T>() where T : EntityBase;
        /// <summary>
        /// 删除表 
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        Task<long> DeleteTableAsync(string tableName);
        /// <summary>
        /// 修改表名
        /// </summary>
        /// <param name="olderName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Task<long> UpdateTableNameAsync(string olderName, string tableName);

        /// <summary>
        /// 注册表数据
        /// </summary>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        Task<long> RegistTableAsync<T>() where T : EntityBase;

    }

    public interface IDataHub : IDisposable
    {
        /// <summary>
        /// 客户端
        /// </summary>
        IClient Client { get; }
        /// <summary>
        /// 表名
        /// </summary>
        string TableName { get; }

        #region Columns
        /// <summary>
        /// 判断指定表，指定列是否存在
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列名</param>
        /// <returns></returns>
        Task<bool> IsExistColumnAsync(string columnName);

        /// <summary>
        /// 向指定表中添加列
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="typeName">列的类型</param>
        /// <param name="isNotNull">是否为空</param>
        /// <returns></returns>
        Task<long> AddColumnsAsync(string columnName, string typeName, bool isNotNull);
        /// <summary>
        /// 删除指定表的指定列
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <returns></returns>
        Task<long> DeleteColumnAsync(string columnName);
        /// <summary>
        /// 修改制定表的制定列的类型
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="typeName">类型明</param>
        /// <param name="isNotNull">是否可以为空</param>
        /// <returns></returns>
        Task<long> UpdateColumnAsync(string columnName, string typeName, bool isNotNull);

        /// <summary>
        /// 对指定的表名，指定的列名 重命名
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="newcolumnName">新名称</param>
        /// <param name="typeName">类型</param>
        /// <returns></returns>
        Task<long> RenameColumnAsync(string columnName, string newcolumnName, string typeName);
        #endregion

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
        Task<long> InserManyAsync(IEnumerable<EntityBase> entities);
        /// <summary>
        /// 删除一个数据
        /// </summary>
        /// <param name="id">主建Id</param>
        Task<long> DeleteAsync(Guid id);
        /// <summary>
        /// 删除多个数据
        /// </summary>
        /// <param name="ids">主建集合</param>
        Task<long> DeleteAsync(IEnumerable<Guid> ids);
        /// <summary>
        /// 删除多个数据
        /// </summary>
        /// <param name="ids">主建集合</param>
        Task<long> DeleteAsync(Query query);
        /// <summary>
        /// 更新一个数据
        /// </summary>
        /// <param name="id">主建</param>
        /// <param name="doc">需要更新的数据</param>
        Task<long> UpdateAsync(Guid id, IRecord doc);
        /// <summary>
        /// 更新一个数据
        /// </summary>
        /// <param name="query">对应的查询条件{field:value}</param>
        /// <param name="doc">需要更新的数据</param>
        Task<long> UpdateAsync(Query query, IRecord doc);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        Task<List<IRecord>> GetListAsync(SearchParams searchParams);

        Task<List<IRecord>> GetListGroupByAsync(List<Aggregate> aggregates, SearchParams searchParams, params string[] groupField);

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        Task<long> GetCountAsync(Query query);
        /// <summary>
        /// 获取单个信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        Task<IRecord> GetInfoAsync(Guid guid);

        #region SQLIndex
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ColumnNames">列名</param>
        /// <param name="indexName">索引名称</param>
        /// <param name="sqlIndex">索引类型</param>
        /// <returns></returns>
        Task<long> CreateIndexAsync(string[] ColumnNames, string indexName, string sqlIndex);
        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="indexName">索引名称</param>
        /// <returns></returns>
        Task<long> DeleteIndexAsync(string indexName);
        #endregion
        #endregion
    }

    public interface IDbTransaction
    {
        DbTransaction Transaction { get; }
        void Commit();
        void Rollback();
    }

    public interface IDbParams
    {
        string ParamSymbol { get; }
        string Sql { get; set; }
        IRecord Params { get; }
    }

    public interface ISqlParse
    {
        IDbParams Parse(ParseTree tree);
    }

}
