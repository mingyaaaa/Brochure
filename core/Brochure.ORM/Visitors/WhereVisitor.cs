﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace Brochure.ORM.Visitors
{
    /// <summary>
    /// The where visitor.
    /// </summary>
    public class WhereVisitor : ORMVisitor, ICloneable
    {
        private readonly IEnumerable<IFuncVisit> _funcVisits;

        /// <summary>
        /// Initializes a new instance of the <see cref="WhereVisitor"/> class.
        /// </summary>
        /// <param name="dbPrivoder">The db privoder.</param>
        /// <param name="dbOption">The db option.</param>
        /// <param name="funcVisits">The func visits.</param>
        public WhereVisitor(IDbProvider dbPrivoder, DbOption dbOption, IEnumerable<IFuncVisit> funcVisits) : base(dbPrivoder, dbOption, funcVisits)
        {
            _funcVisits = funcVisits;
        }

        /// <summary>
        /// Adds the paramters.
        /// </summary>
        /// <param name="pams">The pams.</param>
        public void AddParamters(IEnumerable<IDbDataParameter> pams)
        {
            this.Parameters.AddRange(pams ?? new List<IDbDataParameter>());
        }

        /// <summary>
        /// Visits the binary.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>An Expression.</returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            var left = AddBrackets(GetSql(node.Left));
            var exType = node.NodeType;
            var right = AddBrackets(GetSql(node.Right));
            sql = _dbPrivoder.GetOperateSymbol(left, exType, right);
            return node;
        }

        /// <summary>
        /// Gets the sql.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>An object.</returns>
        public override object GetSql(Expression expression = null)
        {
            if (expression != null)
            {
                return base.GetSql(expression);
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
        private object AddBrackets(object obj)
        {
            if (obj is string)
            {
                var str = obj.ToString();
                if (str.Contains("and") || str.Contains("or"))
                    return $"({str})";
                return str;
            }
            return obj;
        }

        /// <summary>
        /// Clones the.
        /// </summary>
        /// <returns>An object.</returns>
        public object Clone()
        {
            return new WhereVisitor(_dbPrivoder, dbOption, _funcVisits);
        }
    }
}