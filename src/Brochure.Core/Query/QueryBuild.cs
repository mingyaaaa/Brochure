using System;
using System.Collections.Generic;
using Brochure.Core.Abstract;
using Brochure.Core.Extends;

namespace Brochure.Core.Query
{
    public class WhereBuild : BaseBuild
    {
        public WhereBuild() : base(" where 1=1 ")
        {

        }
        public WhereBuild And(EntrieyQuery query)
        {
            TableName = query.GetTableName();
            ResultStr = ResultStr + $" and {query}";
            Dic.Merger(query.GetDocument());
            return this;
        }
        public WhereBuild Or(EntrieyQuery query)
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

    public class UpdateParamBuild : BaseBuild
    {
        public UpdateParamBuild(object obj) : base("")
        {
            Dic = obj.AsDocument();
            SetResultStr();
        }
        public UpdateParamBuild(IDocument doc) : base("")
        {
            Dic = doc;
            SetResultStr();
        }
        public UpdateParamBuild(BaseBuild build) : base("")
        {
            Dic = build.GetDocument();
            TableName = build.GetTableName();
            ResultStr = build.ToString();
            SetResultStr();
        }
        private void SetResultStr()
        {
            foreach (var item in Dic.Keys)
            {
                ResultStr = ResultStr + $" {TableName}.{item}={ConstString.SqlServerPre + item}, ";
            }
            ResultStr = ResultStr.TrimEnd().TrimEnd(',');
        }
        public override string ToString()
        {
            return ResultStr;
        }
    }

    public class SelectParamBuild : BaseBuild
    {
        public static string SearchAll = " * ";
        public List<string> ParamList = new List<string>();
        public SelectParamBuild(bool isSearchAll) : base("")
        {
            if (isSearchAll)
            {
                ResultStr = SearchAll;
            }
        }
        public SelectParamBuild(List<string> param) : base("")
        {
            ResultStr = ResultStr + param.ToString(",");
            ParamList.AddRange(param);
        }

        public void Add(List<string> param)
        {
            ResultStr = ResultStr + "," + param.ToString(",");
            ParamList.AddRange(param);
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
        public UpdateBuild(UpdateParamBuild paramBuild, BaseBuild whereBuild) : base("update {0} set ")
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

    public class SelectBuild : BaseBuild
    {
        private readonly SelectParamBuild _paramBuild;
        public SelectBuild(IEntrity entrity, SelectParamBuild param, WhereBuild whereBuild = null) : base("select {0} from  {1}")
        {
            _paramBuild = param;
            ResultStr = string.Format(ResultStr, _paramBuild, entrity.TableName);
            if (whereBuild != null)
                ResultStr = ResultStr + whereBuild;
            if (param.ToString() == SelectParamBuild.SearchAll)
            {
                _paramBuild.ParamList.AddRange(entrity.AsDocument().Keys);
            }
        }

        public SelectBuild LeftJoin(IEntrity entrity, string str1, string str2)
        {
            ResultStr = $" left join {entrity.TableName} on {str1}={str2} ";
            return this;
        }

        public List<string> GetParamList()
        {
            return _paramBuild.ParamList;
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
