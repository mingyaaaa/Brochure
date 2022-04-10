namespace Plugin.Abstract.RequestModel
{
    /// <summary>
    /// The req login model.
    /// </summary>
    public class ReqLoginModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReqLoginModel"/> class.
        /// </summary>
        public ReqLoginModel()
        {
        }

        /// <summary>
        /// Gets or sets the login type.
        /// </summary>
        public string? LoginType { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the login name.
        /// </summary>
        public string? LoginName { get; set; }
    }
}