using System;
using System.Collections.Generic;
using System.Text;
using Brochure.Core.Query;

namespace Brochure.Core
{

    public abstract class BaseBuild
    {
        protected IDocument Dic = new RecordDocument();
        protected string ResultStr;
        protected string TableName;

        protected BaseBuild(string resultStr)
        {
            ResultStr = resultStr;
        }

        public virtual IDocument GetDocument()
        {
            return Dic;
        }

        public virtual string GetTableName()
        {
            return TableName;
        }

        public override string ToString() => ResultStr;
    }
}
