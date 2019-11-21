using System.ComponentModel.DataAnnotations.Schema;
namespace LinqDbQueryTest
{
    [Table ("aaa")]
    public class Teachers
    {
        public string Id { get; set; }

        public string School { get; set; }

        public string Job { get; set; }
    }
}