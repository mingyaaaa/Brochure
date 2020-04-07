namespace LinqDbQuery.Visitors
{
    public class HavingVisitor : ORMVisitor
    {
        public HavingVisitor (IDbProvider dbProvider) : base (dbProvider)
        { }
    }
}