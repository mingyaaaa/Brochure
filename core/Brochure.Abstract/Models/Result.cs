using System;

namespace Brochure.Abstract.Models
{
    /// <summary>
    /// The result.
    /// </summary>
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

    /// <summary>
    /// The result.
    /// </summary>
    public interface IResult<T> : IResult
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        new T Data { get; }
    }

    /// <summary>
    /// The result.
    /// </summary>
    public class Result : IResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="msg">The msg.</param>
        public Result(int code, string msg)
        {
            this.Msg = msg;
            this.Code = code;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="code">The code.</param>
        /// <param name="msg">The msg.</param>
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

    /// <summary>
    /// The result extensions.
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>A T.</returns>
        public static T GetData<T>(this IResult result)
        {
            return (T)result.Data;
        }
    }

    /// <summary>
    /// The result.
    /// </summary>
    public class Result<T> : IResult<T>
    {
        private readonly T _data;

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="msg">The msg.</param>
        public Result(int code, string msg)
        {
            this.Msg = msg;
            this.Code = code;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="code">The code.</param>
        /// <param name="msg">The msg.</param>
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