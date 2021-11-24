namespace Brochure.ORM.Visitors
{
    /// <summary>
    /// The func visit.
    /// </summary>
    public interface IFuncVisit
    {
        /// <summary>
        /// Gets the func name.
        /// </summary>
        string FuncName { get; }

        /// <summary>
        /// Gets the excute sql.
        /// </summary>
        /// <param name="call">The call.</param>
        /// <param name="member">The member.</param>
        /// <returns>A string.</returns>
        string GetExcuteSql (object call, object member);
    }
}