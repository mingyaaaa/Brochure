namespace Brochure.ORM.Visitors
{
    public class HavingVisitor : ORMVisitor
    {
        public HavingVisitor (IDbProvider dbProvider, DbOption dbOption) : base (dbProvider, dbOption) { }
    }
}