using Brochure.ORM.Database;
using Brochure.ORM.Visitors;

namespace Brochure.ORM
{
    /// <summary>
    /// The db context.
    /// </summary>
    public abstract class DbContext
    {
        private readonly DbDatabase dbDatabase;
        private readonly DbTable dbTable;
        private readonly DbColumns dbColumns;
        private readonly DbIndex dbIndex;
        private readonly IDbProvider dbProvider;
        private readonly DbOption dbOption;
        private readonly DbData dbData;
        private readonly IVisitProvider visitProvider;

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
            IDbProvider dbProvider, IVisitProvider visitProvider)
        {
            this.dbData = dbData;
            this.dbIndex = dbIndex;
            this.dbColumns = dbColumns;
            this.dbTable = dbTable;
            this.dbDatabase = dbDatabase;
            this.dbProvider = dbProvider;
            this.dbOption = dbOption;
            this.visitProvider = visitProvider;
        }

        /// <summary>
        /// Gets the db option.
        /// </summary>
        /// <returns>A DbOption.</returns>
        public DbOption GetDbOption()
        {
            return dbOption;
        }

        /// <summary>
        /// Gets the db data.
        /// </summary>
        /// <returns>A DbData.</returns>
        public DbData GetDbData()
        {
            return this.dbData;
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <returns>A DbDatabase.</returns>
        public DbDatabase GetDatabase()
        {
            return dbDatabase;
        }

        /// <summary>
        /// Gets the db table.
        /// </summary>
        /// <returns>A DbTable.</returns>
        public DbTable GetDbTable()
        {
            return dbTable;
        }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <returns>A DbColumns.</returns>
        public DbColumns GetColumns()
        {
            return dbColumns;
        }

        /// <summary>
        /// Gets the db index.
        /// </summary>
        /// <returns>A DbIndex.</returns>
        public DbIndex GetDbIndex()
        {
            return dbIndex;
        }
        /// <summary>
        /// Gets the db provider.
        /// </summary>
        /// <returns>An IDbProvider.</returns>
        public IDbProvider GetDbProvider()
        {
            return this.dbProvider;
        }

        /// <summary>
        /// Gets the visit provider.
        /// </summary>
        /// <returns>An IVisitProvider.</returns>
        public IVisitProvider GetVisitProvider()
        {
            return visitProvider;
        }
    }
}