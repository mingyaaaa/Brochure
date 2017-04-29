using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using Brochure.Core.Extends;
using Brochure.Core.Helper;

namespace Brochure.Core.Query
{
    public class WhereBuild : IQuery
    {
        private IDocument dic = new RecordDocument();
        private string _resultStr = " where 1=1";
        private const string PreString = ConstString.SqlServerPre;
        private string _tableName;
        public WhereBuild And(EntrieyQuery query)
        {
            _tableName = query.GetTableName();
            _resultStr = _resultStr + $" and {query}";
            dic.Merger(query.GetDocument());
            return this;
        }

        public WhereBuild Or(EntrieyQuery query)
        {
            _tableName = query.GetTableName();
            _resultStr = _resultStr + $" or {query}";
            dic.Merger(query.GetDocument());
            return this;
        }
        public override string ToString() => _resultStr;

        public IDocument GetDocument()
        {
            return dic;
        }

        public string GetTableName()
        {
            return _tableName;
        }
    }

    public class EntrieyQuery : IQuery
    {
        private string _result = string.Empty;
        private IDocument _doc;
        private IEntrity _entrity;
        public EntrieyQuery(string str, IDocument doc, IEntrity entrity)
        {
            _result = str;
            _doc = doc;
            _entrity = entrity;
        }

        public IDocument GetDocument()
        {
            return _doc;
        }

        public string GetTableName()
        {
            return _entrity.TableName;
        }

        public override string ToString()
        {
            return _result;
        }
    }



    public class DeleteBuild : IQuery
    {
        private string _result = "delete from {0} ";
        private readonly WhereBuild _whereBuild;
        private readonly string _tableName;
        public DeleteBuild(WhereBuild whereBuild)
        {
            _tableName = whereBuild.GetTableName();
            _result = string.Format(_result, _tableName);
            _whereBuild = whereBuild;
        }
        public override string ToString()
        {
            return _result + _whereBuild;
        }
        public IDocument GetDocument()
        {
            return _whereBuild.GetDocument();
        }

        public string GetTableName()
        {
            return _tableName;
        }
    }
    public class UpdateBuild : IQuery
    {
        private string _result = "update {0} set ";
        private readonly WhereBuild _whereBuild;
        private readonly string _tableName;
        public UpdateBuild(WhereBuild whereBuild)
        {
            _tableName = whereBuild.GetTableName();
            _result = string.Format(_result, _tableName);
            _whereBuild = whereBuild;
        }
        public override string ToString()
        {
            return _result + _whereBuild;
        }
        public IDocument GetDocument()
        {
            return _whereBuild.GetDocument();
        }

        public string GetTableName()
        {
            return _tableName;
        }
    }

    public class SelectBuild : IQuery
    {
        private string _result = "select {0} from  {1}";
        private readonly IQuery _whereBuild;
        public SelectBuild(IQuery whereBuild)
        {
            _whereBuild = whereBuild;
        }
        public override string ToString()
        {
            return _result + _whereBuild;
        }
        public IDocument GetDocument()
        {
            return _whereBuild.GetDocument();
        }

        public string GetTableName()
        {
            return "";
        }
    }

    public class InsertBuid : IQuery
    {
        private readonly string _result = "Insert into {0}({1}) values({2})";
        private const string PreString = ConstString.SqlServerPre;
        private readonly IDocument _dic;
        private readonly string _tableName;
        public InsertBuid(IEntrity entrity)
        {
            _dic = entrity.AsDocument();
            var arr = _dic.Keys.AddPreString(PreString);
            _tableName = entrity.TableName;
            _dic.Remove(ConstString.T);
            if (_dic[ConstString.Id].To<Guid>() == Guid.Empty)
                _dic[ConstString.Id] = Guid.NewGuid();
            _result = string.Format(_result, _tableName, _dic.Keys.ToString(","), arr.ToString(","));
        }
        public override string ToString()
        {
            return _result;
        }
        public IDocument GetDocument()
        {
            return _dic;
        }

        public string GetTableName()
        {
            return _tableName;
        }
    }
}
