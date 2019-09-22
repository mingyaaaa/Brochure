namespace LinqDbQuery.Visitors
{
    public class OrderVisitor : ORMVisitor
    {
        public OrderVisitor (IDbProvider dbProvider) : base (dbProvider)
        { }
    }
}