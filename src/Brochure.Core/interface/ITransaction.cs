using System;
using System.Data.Common;

namespace Brochure.Core
{
    public interface ITransaction : IDisposable
    {
        void Rollback();
        void Commit();
    }
}