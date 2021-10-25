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
        private readonly object _data;

        public Result(int code, string msg)
        {
            this.Msg = msg;
            this.Code = code;
        }
        public Result(object data, int code = 0, string msg = "") : this(code, msg)
        {
            _data = data;
        }
        public string Msg { get; }

        public int Code { get; }

        public object Data { get; }

        public T GetData<T>() 
        {
           return (T)_data;
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

        public static IResult<T> OK => new Result<T>((T)(object)null);

        public T GetData()
        {
            return _data;
        }
    }
}