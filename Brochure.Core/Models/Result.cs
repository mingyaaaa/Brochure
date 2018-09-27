namespace Brochure.Core.Models
{
    public class Result
    {
        public Result()
        {
        }

        public Result(string msg)
        {

        }

        public Result(int code, string msg)
        {

        }
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
