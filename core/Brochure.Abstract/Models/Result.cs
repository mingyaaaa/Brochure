using System;

namespace Brochure.Abstract.Models
{
    public interface IResult
    {
        /// <summary>
        /// Gets the msg.
        /// </summary>
        string Msg { get; }

        /// <summary>
        /// Gets the code.
        /// </summary>
        int Code { get; }

        /// <summary>
        /// Gets the data.
        /// </summary>
        object Data { get; }
    }

    public interface IResult<T> : IResult
    {
        new T Data { get; }
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

        /// <summary>
        /// Gets the msg.
        /// </summary>
        public string Msg { get; }

        /// <summary>
        /// Gets the code.
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// Gets the data.
        /// </summary>
        public object Data { get; }

        /// <summary>
        /// Gets the o k.
        /// </summary>
        public static IResult OK => new Result(0, "");
    }

    public static class ResultExtensions
    {
        public static T GetData<T>(this IResult result)
        {
            return (T)result.Data;
        }
    }

    public class Result<T> : IResult<T>
    {
        private readonly T _data;

        public Result(int code, string msg)
        {
            this.Msg = msg;
            this.Code = code;
        }

        public Result(T data, int code = 0, string msg = "") : this(code, msg)
        {
            _data = data;
        }

        /// <summary>
        /// Gets the msg.
        /// </summary>
        public string Msg { get; }

        /// <summary>
        /// Gets the code.
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// Gets the data.
        /// </summary>
        public T Data => _data;

        /// <summary>
        /// Gets the data.
        /// </summary>
        object IResult.Data => _data;
    }
}