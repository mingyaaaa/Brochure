using Brochure.Abstract;
using System.Linq;

namespace Brochure.Extensions
{
    /// <summary>
    /// The i record extends.
    /// </summary>
    public static class IRecordExtends
    {
        /// <summary>
        /// Merges the.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="newRecord">The new record.</param>
        /// <param name="isOverride">If true, is override.</param>
        /// <returns>An IRecord.</returns>
        public static IRecord Merge(this IRecord record, IRecord newRecord, bool isOverride = true)
        {
            var keys = newRecord.Keys.ToList();
            foreach (var item in keys)
            {
                if (record.ContainsKey(item) && !isOverride)
                    continue;
                record[item] = newRecord[item];
            }
            return record;
        }

        /// <summary>
        /// Updates the merge.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="newRecord">The new record.</param>
        /// <returns>An IRecord.</returns>
        public static IRecord UpdateMerge(this IRecord record, IRecord newRecord)
        {
            var keys = record.Keys.ToList();
            foreach (var item in keys)
            {
                if (newRecord.ContainsKey(item))
                    record[item] = newRecord[item];
            }
            return record;
        }
    }
}