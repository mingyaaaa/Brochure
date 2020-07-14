using Brochure.ORM.Database;

namespace Brochure.ORM
{
    public abstract class DbContext
    {
        private readonly DbDatabase dbDatabase;
        private readonly DbTable dbTable;
        private readonly DbColumns dbColumns;
        private readonly DbIndex dbIndex;
        private readonly IDbProvider dbProvider;
        private readonly DbOption dbOption;
        private readonly DbData dbData;

        public DbContext (DbDatabase dbDatabase,
            DbTable dbTable, DbColumns dbColumns,
            DbIndex dbIndex, DbData dbData,
            DbOption dbOption,
            IDbProvider dbProvider)
        {
            this.dbData = dbData;
            this.dbIndex = dbIndex;
            this.dbColumns = dbColumns;
            this.dbTable = dbTable;
            this.dbDatabase = dbDatabase;
            this.dbProvider = dbProvider;
            this.dbOption = dbOption;
        }

        public DbOption GetDbOption ()
        {
            return dbOption;
        }

        public DbData GetDbData ()
        {
            return this.dbData;
        }

        public DbDatabase GetDatabase ()
        {
            return dbDatabase;
        }

        public DbTable GetDbTable ()
        {
            return dbTable;
        }

        public DbColumns GetColumns ()
        {
            return dbColumns;
        }

        public DbIndex GetDbIndex ()
        {
            return dbIndex;
        }
        public IDbProvider GetDbProvider ()
        {
            return this.dbProvider;
        }
    }
}