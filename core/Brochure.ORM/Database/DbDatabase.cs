using System.Threading.Tasks;
namespace Brochure.ORM.Database
{
    /// <summary>
    /// The db database.
    /// </summary>
    public abstract class DbDatabase
    {
        protected DbOption Option;
        private readonly DbSql _dbSql;
        private readonly IConnectFactory connectFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbDatabase"/> class.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="dbSql">The db sql.</param>
        /// <param name="connectFactory">The connect factory.</param>
        protected DbDatabase(DbOption option, DbSql dbSql, IConnectFactory connectFactory)
        {
            Option = option;
            this._dbSql = dbSql;
            this.connectFactory = connectFactory;
        }

        /// <summary>
        /// Creates the database async.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A Task.</returns>
        public Task<long> CreateDatabaseAsync(string databaseName)
        {
            return Task.Run<long>(() => CreateDatabase(databaseName));
        }

        /// <summary>
        /// Tries the create database async.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A Task.</returns>
        public async Task<long> TryCreateDatabaseAsync(string databaseName)
        {
            var exist = await IsExistDataBaseAsync(databaseName);
            if (!exist)
                return await CreateDatabaseAsync(databaseName);
            return -1;
        }


        /// <summary>
        /// Creates the database.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A long.</returns>
        public virtual long CreateDatabase(string databaseName)
        {
            var sql = _dbSql.GetCreateDatabaseSql(databaseName);
            var connection = connectFactory.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = sql;
            return command.ExecuteNonQuery();
        }



        /// <summary>
        /// Deletes the database async.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A Task.</returns>
        public Task<long> DeleteDatabaseAsync(string databaseName)
        {
            return Task.Run<long>(() => DeleteDatabase(databaseName));
        }

        /// <summary>
        /// Tries the delete database async.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A Task.</returns>
        public async Task<long> TryDeleteDatabaseAsync(string databaseName)
        {
            var exist = await IsExistDataBaseAsync(databaseName);
            if (exist)
                return await DeleteDatabaseAsync(databaseName);
            return -1;
        }


        /// <summary>
        /// Deletes the database.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A long.</returns>
        public virtual long DeleteDatabase(string databaseName)
        {
            var connection = connectFactory.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = _dbSql.GetDeleteDatabaseSql(databaseName);
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Are the exist data base async.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A Task.</returns>
        public Task<bool> IsExistDataBaseAsync(string databaseName)
        {
            return Task.Run(() => IsExistDataBase(databaseName));
        }

        /// <summary>
        /// Are the exist data base.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A bool.</returns>
        public virtual bool IsExistDataBase(string databaseName)
        {
            var connection = connectFactory.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = _dbSql.GetDataBaseNameCountSql(databaseName);
            var rr = (int)command.ExecuteScalar();
            return rr >= 1;
        }

        /// <summary>
        /// Changes the database.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        public virtual void ChangeDatabase(string databaseName)
        {
            var connection = connectFactory.CreateConnection();
            connection.ChangeDatabase(databaseName);
        }
    }
}