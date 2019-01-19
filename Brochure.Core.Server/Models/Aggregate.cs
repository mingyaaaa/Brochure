namespace Brochure.Core.Server
{
    public class Aggregate
    {
        public static string Sum(string field)
        {
            return $"Sum({field})";
        }

        public static string Count(string field)
        {
            return $"Count({field})";
        }

        public static string Avg(string field)
        {
            return $"avg({field})";
        }

        public static string Max(string field)
        {
            return $"max({field})";
        }

        public static string Min(string field)
        {
            return $"min({field})";
        }
    }
}
