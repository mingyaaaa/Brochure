using System.Linq;
using Brochure.Core.Interfaces;

namespace Brochure.Core.Extends
{
    public static class IRecordExtends
    {
        public static IRecord Merge (this IRecord record, IRecord newRecord, bool isOverride = true)
        {
            var keys = newRecord.Keys.ToList ();
            foreach (var item in keys)
            {
                if (record.ContainsKey (item) && !isOverride)
                    continue;
                record[item] = newRecord[item];
            }
            return record;
        }
        public static IRecord UpdateMerge (this IRecord record, IRecord newRecord)
        {
            var keys = record.Keys.ToList ();
            foreach (var item in keys)
            {
                if (newRecord.ContainsKey (item))
                    record[item] = newRecord[item];
            }
            return record;
        }
    }
}