using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Abstract
{
    public struct StringJoin
    {
        private StringBuilder joinBuilder;
        private string spit;

        public StringJoin(string str, string spit)
        {
            joinBuilder = new StringBuilder(str);
            this.spit = spit;
        }

        public StringJoin(string spit)
        {
            joinBuilder = new StringBuilder();
            this.spit = spit;
        }

        public void Join(string str)
        {
            if (joinBuilder.Length == 0)
                joinBuilder.Append(str);
            else
                joinBuilder.Append(spit).Append(str);
        }

        public override string ToString()
        {
            return joinBuilder.ToString();
        }
    }
}