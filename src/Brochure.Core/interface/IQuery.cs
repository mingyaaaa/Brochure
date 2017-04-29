using System;
using System.Collections.Generic;
using System.Text;

namespace Brochure.Core
{
    public interface IQuery
    {
        IDocument GetDocument();
        string GetTableName();
    }
}
