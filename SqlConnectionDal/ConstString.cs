namespace ConnectionDal
{
    public class ConstString
    {
#if Sql
        public const string PreParamString = "@";
#elif Oracle
        public const string PreParamString = "@";
#elif MySql
        public const string PreParamString = "@";
#endif
        public const string T = "TableName";
        public const string Id = "Id";
        public const string ConnectionString = "ConnectionStrings";
        public const string Max = "Max";
        public const string Min = "Min";
    }
}
