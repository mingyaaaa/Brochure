using Brochure.Abstract.PluginDI;
using Brochure.ORM.Database;
using Brochure.ORM.Visitors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Brochure.ORM
{
    /// <summary>
    /// The db context.
    /// </summary>
    public abstract class DbContext : IDisposable, IAsyncDisposable
    {
        public DbDatabase Database { get; }
        public DbTable Tables { get; }
        public DbColumns Columns { get; }
        public DbIndex Indexs { get; }
        public IDbProvider DbProvider { get; }
        public DbOption DbOption { get; }
        public DbData Datas { get; }
        public IVisitProvider VisitProvider { get; }

        private IDbConnection _connection;

        internal static IServiceProvider ServiceProvider { get; set; }
        private IServiceScope _serviceScope;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContext"/> class.
        /// </summary>
        /// <param name="dbDatabase">The db database.</param>
        /// <param name="dbTable">The db table.</param>
        /// <param name="dbColumns">The db columns.</param>
        /// <param name="dbIndex">The db index.</param>
        /// <param name="dbData">The db data.</param>
        /// <param name="dbOption">The db option.</param>
        /// <param name="dbProvider">The db provider.</param>
        /// <param name="visitProvider">The visit provider.</param>
        public DbContext(DbDatabase dbDatabase,
            DbTable dbTable, DbColumns dbColumns,
            DbIndex dbIndex, DbData dbData,
            DbOption dbOption,
            IDbProvider dbProvider, IVisitProvider visitProvider, IConnectFactory connectFactory)
        {
            this.Datas = dbData;
            this.Indexs = dbIndex;
            this.Columns = dbColumns;
            this.Tables = dbTable;
            this.Database = dbDatabase;
            this.DbProvider = dbProvider;
            this.DbOption = dbOption;
            this.VisitProvider = visitProvider;
            _connection = connectFactory.CreateAndOpenConnection();
        }

        public DbContext()
        {
            _serviceScope = ServiceProvider.CreateScope();
            this.Datas = _serviceScope.ServiceProvider.GetRequiredService<DbData>();
            this.Indexs = _serviceScope.ServiceProvider.GetRequiredService<DbIndex>();
            this.Columns = _serviceScope.ServiceProvider.GetRequiredService<DbColumns>();
            this.Tables = _serviceScope.ServiceProvider.GetRequiredService<DbTable>();
            this.Database = _serviceScope.ServiceProvider.GetRequiredService<DbDatabase>();
            this.DbProvider = _serviceScope.ServiceProvider.GetRequiredService<IDbProvider>();
            this.DbOption = _serviceScope.ServiceProvider.GetRequiredService<DbOption>();
            this.VisitProvider = _serviceScope.ServiceProvider.GetRequiredService<IVisitProvider>();
            var connectionFactory = _serviceScope.ServiceProvider.GetRequiredService<IConnectFactory>();
            _connection = connectionFactory.CreateAndOpenConnection();
        }

        public ValueTask DisposeAsync()
        {
            _serviceScope.Dispose();
            _connection.Dispose();
            return ValueTask.CompletedTask;
        }

        public void Dispose()
        {
            _serviceScope.Dispose();
            _connection.Dispose();
        }
    }
}