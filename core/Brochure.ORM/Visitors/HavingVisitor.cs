using System.Collections.Generic;

namespace Brochure.ORM.Visitors
{
    /// <summary>
    /// The having visitor.
    /// </summary>
    public class HavingVisitor : ORMVisitor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HavingVisitor"/> class.
        /// </summary>
        /// <param name="dbProvider">The db provider.</param>
        /// <param name="dbOption">The db option.</param>
        /// <param name="funcVisits">The func visits.</param>
        public HavingVisitor(IDbProvider dbProvider, DbOption dbOption, IEnumerable<IFuncVisit> funcVisits) : base(dbProvider, dbOption, funcVisits) { }
    }
}