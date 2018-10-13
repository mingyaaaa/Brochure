using Irony.Parsing;

namespace Brochure.Core.Server
{
    public interface IDbConnect
    {
        IDbTableBase GetTableHub ();
        IDbDatabase GetBatabaseHub ();
        IDbData GetDataHub (string tableName, bool isBeginTransaction = false);
        IDbData GetDataHub<T> (bool isBeginTransaction = false) where T : EntityBase;
        IDbColumns GetColumnsHub (string tableName);
        IDbColumns GetColumnsHub<T> () where T : EntityBase;

    }
    public interface IDbTransaction
    {
        void BeginTransaction ();
        void Commit ();
        void Rollback ();
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