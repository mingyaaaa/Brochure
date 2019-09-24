using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqDbQuery
{
    public interface IDbProvider
    {
        bool IsUseParamers { get; set; }
        string GetParamsSymbol ();
        IDbDataParameter GetDbDataParameter ();
        string GetOperateSymbol (object left, ExpressionType expressionType, object right);
        string GetObjectType (object type);
        IDbConnection GetDbConnection ();
        ExpressionVisitor GetVisitor ();
        Func<DbQueryOption> CreateOption { get; set; }
    }

    public abstract class DbSource
    {
        public DbSource ()
        {

        }
        public abstract T GetDbConnection<T> ();
    }

}