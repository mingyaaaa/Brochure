namespace Brochure.Utils
{
    /// <summary>
    /// The system util.
    /// </summary>
    public interface ISystemUtil
    {
        /// <summary>
        /// Gets the usefull port.
        /// </summary>
        /// <returns>An int.</returns>
        int GetUsefullPort();

        /// <summary>
        /// Are the use port.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns>A bool.</returns>
        bool IsUsePort(int port);
    }
}