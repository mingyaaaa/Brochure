using System.Collections.Generic;

namespace Brochure.ORM.Visitors
{
    public class HavingVisitor : ORMVisitor
    {
        public HavingVisitor(IDbProvider dbProvider, DbOption dbOption, IEnumerable<IFuncVisit> funcVisits) : base(dbProvider, dbOption, funcVisits) { }
    }
}