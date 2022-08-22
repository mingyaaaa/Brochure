namespace Brochure.Abstract.Models
{
    /// <summary>
    /// The result.
    /// </summary>
    public class Result
    {
        private readonly object? _data;

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
            _data = data;
        }

        /// <summary>
        /// Gets the msg.
        /// </summary>
        public string Msg { get; protected set; }

        /// <summary>
        /// Gets the code.
        /// </summary>
        public int Code { get; protected set; }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>A T? .</returns>
        public T? GetData<T>()
        {
            return (T?)_data;
        }

        /// <summary>
        /// Gets a value indicating whether is success.
        /// </summary>
        public virtual bool IsSuccess => Code == 0;

        /// <summary>
        /// Gets the o k.
        /// </summary>
        public static Result OK => new Result(0, string.Empty);
    }

    /// <summary>
    /// The result.
    /// </summary>
    public class Result<T> : Result
    {
        private readonly T? _data;

        public Result(int code, string msg) : base(code, msg)
        {
            this.Code = code;
            this.Msg = msg;
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
        /// Gets the data.
        /// </summary>
        public T? Data => _data;
    }
}