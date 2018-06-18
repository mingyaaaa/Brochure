using Brochure.Core.Enums;
using Brochure.Core.Interfaces;
namespace Brochure.Core.Model
{
    public class SearchParams
    {
        /// <summary>
        /// 模糊搜索字符
        /// </summary>
        public string SearchText;
        /// <summary>
        /// 
        /// </summary>
        public IBDocument Filter;
        /// <summary>
        /// 起始页字段 从0开始
        /// </summary>
        public int StarPageIndex;
        /// <summary>
        /// 结束页字段
        /// </summary>
        public int EndPageIndex;
        /// <summary>
        /// 排序字段 {field:1}  1-升序，2-降序
        /// </summary>
        public IBDocument OrderField;
    }
}