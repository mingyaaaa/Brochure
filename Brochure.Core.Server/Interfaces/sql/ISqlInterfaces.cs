using System.Collections.Generic;
using Brochure.Core.Abstracts;
using Brochure.Core.Interfaces;

namespace Brochure.Core.Server.Interfaces
{
    public interface IDbConnect
    {
        IDbTableBase GetTableHub ();
        IDbDatabase GetBatabaseHub ();
        IDbData GetDataHub (string tableName);
        void Commit ();
        void Rollback ();
        void BeginTransaction ();
    }
    public interface IDbParams
    {
        string ParamSymbol { get; }
        string Sql { get; }
        IBDocument Params { get; }
    }

    public interface IDbField
    {

    }
}