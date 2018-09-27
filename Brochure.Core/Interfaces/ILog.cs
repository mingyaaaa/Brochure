using System;

namespace Brochure.Core.Interfaces
{
    public interface ILog
    {
        void Error(string message, Exception e);
        void Warning(string message);
        void Info(string message);
        void Debug(string message);
    }
}
