namespace Brochure.Core.Querys
{
    public class QueryOperation
    {

    }
    public class QueryOperationType
    {
        public const string Eq = "=";
        public const string In = "in";
        public const string Like = "Like";
        public const string NotIn = "NotIn";
        public const string NotEq = "NotEq";
        public const string NotLike = "Not Like";

        public const string Betweent = "between {0} and {1}";

        public const string Gt = ">";
        public const string Gte = ">=";
        public const string Lt = "<";
        public const string Lte = "<=";

        public const string And = "and";
        public const string Or = "or";
    }
}