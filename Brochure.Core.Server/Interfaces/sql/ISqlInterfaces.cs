using System.Collections.Generic;
using Brochure.Core.Abstracts;
using Brochure.Core.Interfaces;
using Brochure.Core.Querys;
using Irony.Parsing;

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
        string Sql { get; set; }
        IRecord Params { get; }
    }

    public interface ISqlParse
    {
        IDbParams Parse (ParseTree tree);
    }

    public interface IDbField
    {

    }
}