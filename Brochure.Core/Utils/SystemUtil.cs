using System;
using System.Net;
using System.Net.NetworkInformation;

namespace Brochure.Core.Utils
{
    public static class SystemUtil
    {
        public static int GetUsefullPort()
        {
            Random random = new Random();
            var port = random.Next(3000, 10000);
            while (!IsUsePort(port))
                port = random.Next(3000, 10000);
            return port;
        }
        public static bool IsUsePort(int port)
        {
            bool inUse = false;
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();
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
