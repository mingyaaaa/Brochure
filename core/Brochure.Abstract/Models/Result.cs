using System;

namespace Brochure.Abstract.Models
{
    public interface IResult
    {

        string Msg { get; }

        int Code { get; }

        object Data { get; }


    }

    public interface IResult<T> : IResult
    {
        T GetData();
    }

    public class Result : IResult
    {

        public Result(int code, string msg)
        {
            this.Msg = msg;
            this.Code = code;
        }
        public Result(object data, int code = 0, string msg = "") : this(code, msg)
        {
            Data = data;
        }
        public string Msg { get; }

        public int Code { get; }

        public object Data { get; }

        public static IResult OK => new Result(0, "");

    }

    public static class ResultExtensions {

        public static T GetData<T>(this IResult result)
        {
            return (T)result.Data;
        }
    }


    public class Result<T> : IResult<T>
    { 
        private readonly T _data;
        public Result(int code,string msg)
        {
            this.Msg = msg;
            this.Code = code;
        }
        public Result(T data, int code=0, string msg=""):this(code,msg)
        { 
            _data = data;
        }

        public string Msg { get ; }
        public int Code { get ;  }
        public object Data => _data;


        public T GetData()
        {
            return _data;
        }

    }
}