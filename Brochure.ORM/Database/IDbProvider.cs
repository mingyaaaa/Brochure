using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Brochure.ORM
{
    public interface IDbProvider
    {
        bool IsUseParamers { get; }
        string GetParamsSymbol ();
        IDbDataParameter GetDbDataParameter ();
        string GetOperateSymbol (object left, ExpressionType expressionType, object right);
        string GetObjectType (object type);
        IDbConnection GetDbConnection ();
        ExpressionVisitor GetVisitor ();
        TypeMap GetTypeMap ();
    }
}