using System;
using Brochure.Core.Extends;

namespace Brochure.Core.Query
{
    public class WhereBuild : BaseBuild
    {
        public WhereBuild() : base(" where 1=1 ")
        {

        }
        public BaseBuild And(EntrieyQuery query)
        {
            TableName = query.GetTableName();
            ResultStr = ResultStr + $" and {query}";
            Dic.Merger(query.GetDocument());
            return this;
        }
        public BaseBuild Or(EntrieyQuery query)
        {
            TableName = query.GetTableName();
            ResultStr = ResultStr + $" or {query}";
            Dic.Merger(query.GetDocument());
            return this;
        }
    }

    public class EntrieyQuery : BaseBuild
    {
        public EntrieyQuery(string str, IDocument doc, IEntrity entrity) : base("")
        {
            ResultStr = str;
            Dic = doc;
            TableName = entrity.TableName;
        }
    }

    public class ParamBuild : BaseBuild
    {
        public ParamBuild(object obj) : base("")
        {
            Dic = obj.AsDocument();
            SetResultStr();
        }
        public ParamBuild(IDocument doc) : base("")
        {
            Dic = doc;
            SetResultStr();
        }

        private void SetResultStr()
        {
            foreach (var item in Dic.Keys)
            {
                ResultStr = ResultStr + $" {item}={ConstString.SqlServerPre + item}, ";
            }
            ResultStr = ResultStr.TrimEnd().TrimEnd(',');
        }
        public override string ToString()
        {
            return ResultStr;
        }
    }

    public class DeleteBuild : BaseBuild
    {
        private readonly BaseBuild _whereBuild;
        public DeleteBuild(BaseBuild whereBuild) : base("delete from {0}")
        {
            TableName = whereBuild.GetTableName();
            ResultStr = string.Format(ResultStr, TableName);
            _whereBuild = whereBuild;
        }

        public override string ToString()
        {
            return ResultStr + _whereBuild;
        }

        public override IDocument GetDocument()
        {
            Dic.Merger(_whereBuild.GetDocument());
            return base.GetDocument();
        }
    }

    public class UpdateBuild : BaseBuild
    {
        private readonly BaseBuild _paramBuild;
        private readonly BaseBuild _whereBuild;
        public UpdateBuild(ParamBuild paramBuild, BaseBuild whereBuild) : base("update {0} set ")
        {
            _paramBuild = paramBuild;
            TableName = whereBuild.GetTableName();
            ResultStr = string.Format(ResultStr, TableName);
            _whereBuild = whereBuild;
        }

        public override IDocument GetDocument()
        {
            Dic.Merger(_whereBuild.GetDocument());
            Dic.Merger(_paramBuild.GetDocument());
            return base.GetDocument();
        }

        public override string ToString()
        {
            return ResultStr + _paramBuild + _whereBuild;
        }
    }

    public class SelectBuild
    {
        private string _result = "select {0} from  {1}";
        private readonly BaseBuild _whereBuild;
        public SelectBuild(BaseBuild whereBuild)
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

    public class InsertBuid : BaseBuild
    {

        private const string PreString = ConstString.SqlServerPre;

        public InsertBuid(IEntrity entrity) : base("Insert into {0}({1}) values({2})")
        {
            Dic = entrity.AsDocument();
            var arr = Dic.Keys.AddPreString(PreString);
            TableName = entrity.TableName;
            Dic.Remove(ConstString.T);
            if (Dic[ConstString.Id].To<Guid>() == Guid.Empty)
                Dic[ConstString.Id] = Guid.NewGuid();
            ResultStr = string.Format(ResultStr, TableName, Dic.Keys.ToString(","), arr.ToString(","));
        }
    }
}
