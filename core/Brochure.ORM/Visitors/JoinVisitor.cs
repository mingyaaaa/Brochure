using System;
using System.Linq.Expressions;

namespace Brochure.ORM.Visitors
{
    public class JoinVisitor : ORMVisitor
    {
        private string tableName;

        public JoinVisitor (IDbProvider dbPrivoder, DbOption dbOption) : base (dbPrivoder, dbOption) { }

        public void SetTableName (Type tableType)
        {
            tableName = TableUtlis.GetTableName (tableType);
        }
        protected override Expression VisitBinary (BinaryExpression node)
        {
            var left = GetSql (node.Left);
            var right = GetSql (node.Right);
            sql = $"join [{tableName}] on {left} = {right}";
            return node;
        }
    }
}