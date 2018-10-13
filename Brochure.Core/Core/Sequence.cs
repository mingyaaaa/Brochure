using System.Collections.Generic;
using System.Linq;

namespace Brochure.Core
{
    public class Sequence
    {
        public IDictionary<string, int> seqDic;
        public Sequence ()
        {
            seqDic = new Dictionary<string, int> ();
        }
        public string GetSequence (string str)
        {
            var count = 0;
            if (seqDic.ContainsKey (str))
            {
                count = seqDic[str] + 1;
            }
            seqDic[str] = count;
            return str + seqDic[str];
        }
        public static string GetRecordSequence (IRecord record, string str)
        {
            var dic = new Dictionary<string, int> ();
            foreach (var item in record.Keys.ToList ())
                dic.Add (item, 0);
            var count = 0;
            if (dic.ContainsKey (str))
            {
                count = dic[str] + 1;
            }
            dic[str] = count;
            return str + dic[str];
        }
    }
}