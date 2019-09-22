using System.Linq.Expressions;

namespace LinqDbQuery.Visitors
{
    public class WhereVisitor : ORMVisitor
    {

        public WhereVisitor (IDbProvider dbPrivoder) : base (dbPrivoder)
        { }
        protected override Expression VisitBinary (BinaryExpression node)
        {
            var left = GetSql (node.Left);
            var exType = node.NodeType;
            var right = GetSql (node.Right);
            sql = _dbPrivoder.GetOperateSymbol (left, exType, right);
            sql = $"where {sql} ";
            return node;
        }
        protected override Expression VisitMethodCall (MethodCallExpression node)
        {
            base.VisitMethodCall (node);
            sql = $"where {sql}";
            return node;
        }
    }
}