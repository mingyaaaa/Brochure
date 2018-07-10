namespace Brochure.Core.Querys
{
    public class QueryOperation
    {

    }
    public class QueryOperationType
    {
        public const string Eq = "=";
        public const string In = "In";
        public const string Like = "Like";
        public const string NotIn = "NotIn";

        public const string NotLike = "Not Like";

        public const string Betweent = "Between {0} and {1}";
        public const string NotEq = "!=";
        public const string Gt = ">";
        public const string Gte = ">=";
        public const string Lt = "<";
        public const string Lte = "<=";
        public const string And = "and";
        public const string Or = "or";
    }
}