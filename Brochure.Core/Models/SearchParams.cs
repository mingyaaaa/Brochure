using Brochure.Core.Enums;
using Brochure.Core.Implements;
using Brochure.Core.Interfaces;
using Brochure.Core.Querys;
namespace Brochure.Core.Model
{
    public class SearchParams
    {
        public SearchParams ()
        {
            OrderField = new Record ();
            OrderField["CreateTime"] = OrderType.Desc;
        }
        public SearchParams (Query query)
        {
            Filter = query;
        }
        public SearchParams (Query query, int star, int end)
        {
            Filter = query;
            StarIndex = star;
            EndIndex = end;
        }
        /// <summary>
        /// 模糊搜索字符
        /// </summary>
        public string SearchText;
        /// <summary>
        /// 
        /// </summary>
        public Query Filter;
        /// <summary>
        /// 起始页字段 从0开始
        /// </summary>
        public int StarIndex;
        /// <summary>
        /// 结束页字段
        /// </summary>
        public int EndIndex;
        /// <summary>
        /// 排序字段 {field:1}  1-升序，2-降序
        /// </summary>
        public IRecord OrderField;
    }
}