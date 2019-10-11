using System.Linq.Expressions;

namespace LinqDbQuery.Visitors
{
    public class WhereVisitor : ORMVisitor
    {
        public WhereVisitor (IDbProvider dbPrivoder) : base (dbPrivoder) { }

        protected override Expression VisitBinary (BinaryExpression node)
        {
            var left = AddBrackets (GetSql (node.Left).ToString ());
            var exType = node.NodeType;
            var right = AddBrackets (GetSql (node.Right).ToString ());
            sql = _dbPrivoder.GetOperateSymbol (left, exType, right);
            return node;
        }

        protected override Expression VisitMethodCall (MethodCallExpression node)
        {
            base.VisitMethodCall (node);
            sql = $"where {sql}";
            return node;
        }

        public override object GetSql (Expression expression = null)
        {
            if (expression != null)
            {
                return base.GetSql (expression);
            }
            else
            {
                sql = $"where ({sql}) ";
                return sql;
            }
        }

        /// <summary>
        /// 添加括号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private object AddBrackets (string str)
        {
            if (str.Contains ("and") || str.Contains ("or"))
                return $"({str})";
            return str;
        }
    }
}