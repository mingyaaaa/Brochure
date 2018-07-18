using System;
using Irony.Parsing;

namespace Brochure.Core.Server
{
    public class SqlGrammer : Grammar
    {
        public SqlGrammer () : base (false)
        {
            var number = new NumberLiteral ("NumberValue"); //表示数字
            number.DefaultIntTypes = new [] { TypeCode.Int32, TypeCode.Int64, TypeCode.Int16 };
            number.DefaultFloatType = TypeCode.Decimal;
            var identifier = new IdentifierTerminal ("Identifier"); //表示变量
            var str = new StringLiteral ("StringValue", "'", StringOptions.AllowsDoubledQuote);
            var timeValue = new QuotedValueLiteral ("DateTimeValue", "Date(", ")", TypeCode.String);
            //key
            var _in = ToTerm ("in");
            var and = ToTerm ("and");
            var or = ToTerm ("or");
            var between = ToTerm ("between");
            var like = ToTerm ("like");
            var not = ToTerm ("not");
            var _null = ToTerm ("null");
            var _is = ToTerm ("is");
            var comma = ToTerm (",");

            var in_stmt = new NonTerminal ("In");
            var eq = new NonTerminal ("OperationExp");
            var betweenstmt = new NonTerminal ("Between");
            var likestmt = new NonTerminal ("Like");
            var notnullstmt = new NonTerminal ("NotNull");
            var nullstmt = new NonTerminal ("Null");
            var stmtlist = new NonTerminal ("stmtlist");
            var stmt = new NonTerminal ("stmt");
            var valuestmt = new NonTerminal ("valuestmt");
            var valueListstmt = new NonTerminal ("ValueList");
            var expressstmt = new NonTerminal ("Express");
            var parExp = new NonTerminal ("parExp");
            var logicExp = new NonTerminal ("LogicExp");
            var OprExp = new NonTerminal ("OprExp");
            OprExp.Rule = ToTerm ("=") |">" |"<" |">=" |"<=" |"!=";
            valueListstmt.Rule = MakePlusRule (valueListstmt, comma, valuestmt);
            valuestmt.Rule = number | str | timeValue;

            #region in
            in_stmt.Rule = identifier + _in + "(" + valueListstmt + ")";
            #endregion

            #region EqOperation
            eq.Rule = identifier + OprExp + valuestmt;
            #endregion

            #region like

            likestmt.Rule = identifier + like + valuestmt;

            #endregion

            #region Betweent

            betweenstmt.Rule = identifier + valuestmt + "and" + valuestmt;

            #endregion

            #region IsNull

            nullstmt.Rule = identifier + _is + _null;

            #endregion

            #region Is Not null

            notnullstmt.Rule = identifier + _is + not + _null;

            #endregion
            var logicOper = new NonTerminal ("LogicOper");

            logicOper.Rule = ToTerm ("and") |"or";
            expressstmt.Rule = in_stmt | eq | likestmt | betweenstmt | nullstmt | notnullstmt | parExp | logicExp;
            logicExp.Rule = expressstmt + logicOper + expressstmt;
            parExp.Rule = "(" + expressstmt + ")";
            RegisterBracePair ("(", ")");
            MarkPunctuation ("(", ")");
            MarkTransient (stmtlist, valuestmt, expressstmt, parExp, OprExp);
            this.Root = expressstmt;
        }
    }
}