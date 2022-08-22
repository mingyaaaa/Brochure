using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.ORM.MySql
{
    internal class InsertManySql : ISql
    {
        private readonly string _databaseName;

        public InsertManySql(IEnumerable<object> objs, string databaseName = "")
        {
            if (objs.Count() == 0)
                throw new Exception("没有插入数据");
            Datas = objs;
            _databaseName = databaseName;
        }

        public string Database { get; set; }

        public IEnumerable<object> Datas { get; }
    }
}