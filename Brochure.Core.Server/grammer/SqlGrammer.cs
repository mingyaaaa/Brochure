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
            var not_in = ToTerm ("notin");
            var and = ToTerm ("and");
            var or = ToTerm ("or");
            var between = ToTerm ("between");
            var notbetween = ToTerm ("notbetween");
            var like = ToTerm ("like");
            var notlike = ToTerm ("notlike");
            var not = ToTerm ("not");
            var _null = ToTerm ("null");
            var _is = ToTerm ("is");
            var comma = ToTerm (",");

            var in_stmt = new NonTerminal ("In");
            var not_in_stmt = new NonTerminal ("NotIn");
            var eq = new NonTerminal ("OperationExp");
            var between_stmt = new NonTerminal ("Between");
            var not_between_stmt = new NonTerminal ("NotBetween");
            var like_stmt = new NonTerminal ("Like");
            var not_like_stmt = new NonTerminal ("NotLike");
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

            #region notin
            not_in_stmt.Rule = identifier + not_in + "(" + valueListstmt + ")";
            #endregion

            #region EqOperation
            eq.Rule = identifier + OprExp + valuestmt;
            #endregion

            #region like

            like_stmt.Rule = identifier + like + valuestmt;

            #endregion

            #region not like

            not_like_stmt.Rule = identifier + notlike + valuestmt;

            #endregion

            #region Betweent

            between_stmt.Rule = identifier + between + valuestmt + "and" + valuestmt;

            #endregion
            #region NotBetweent

            not_between_stmt.Rule = identifier + notbetween + valuestmt + "and" + valuestmt;

            #endregion

            #region IsNull

            nullstmt.Rule = identifier + _is + _null;

            #endregion

            #region Is Not null

            notnullstmt.Rule = identifier + _is + not + _null;

            #endregion
            var logicOper = new NonTerminal ("LogicOper");

            logicOper.Rule = ToTerm ("and") |"or";
            expressstmt.Rule = in_stmt | not_in_stmt | eq | like_stmt | not_like_stmt | between_stmt | not_between_stmt | nullstmt | notnullstmt | parExp | logicExp;
            logicExp.Rule = expressstmt + logicOper + expressstmt;
            parExp.Rule = "(" + expressstmt + ")";
            RegisterBracePair ("(", ")");
            MarkPunctuation ("(", ")");
            MarkTransient (stmtlist, valuestmt, expressstmt, parExp, OprExp);
            this.Root = expressstmt;
        }
    }
}