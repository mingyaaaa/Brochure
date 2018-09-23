using System;

namespace Brochure.Core.Interfaces
{
    public interface ILog
    {
        void Error(string message, Exception e);
        void Warn(string message);
        void Info(string message);
    }
}
