using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace LinqDbQuery.Visitors
{
    public class WhereVisitor : ORMVisitor
    {

        public WhereVisitor (IDbProvider dbPrivoder, IEnumerable<IDbDataParameter> pams = null) : base (dbPrivoder)
        {
            this.Parameters.AddRange (pams??new List<IDbDataParameter> ());
        }

        protected override Expression VisitBinary (BinaryExpression node)
        {
            var left = AddBrackets (GetSql (node.Left));
            var exType = node.NodeType;
            var right = AddBrackets (GetSql (node.Right));
            sql = _dbPrivoder.GetOperateSymbol (left, exType, right);
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
                sql = $"where {sql}";
                return sql;
            }
        }

        /// <summary>
        /// 添加括号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private object AddBrackets (object obj)
        {
            if (obj is string)
            {
                var str = obj.ToString ();
                if (str.Contains ("and") || str.Contains ("or"))
                    return $"({str})";
                return str;
            }
            return obj;
        }
    }
}