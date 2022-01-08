using System;
using System.Net;
using System.Net.NetworkInformation;

namespace Brochure.Utils
{
    /// <summary>
    /// The system util.
    /// </summary>
    public class SystemUtil : ISystemUtil
    {
        /// <summary>
        /// Gets the usefull port.
        /// </summary>
        /// <returns>An int.</returns>
        public int GetUsefullPort ()
        {
            Random random = new Random ();
            var port = random.Next (3000, 10000);
            while (!IsUsePort (port))
                port = random.Next (3000, 10000);
            return port;
        }

        /// <summary>
        /// Are the use port.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns>A bool.</returns>
        public bool IsUsePort (int port)
        {
            bool inUse = false;
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties ();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners ();
            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == port)
                {
                    inUse = true;
                    break;
                }
            }
            return inUse;
        }
    }
}