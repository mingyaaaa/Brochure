using System;
using System.Collections.Generic;

namespace Brochure.WPF.Client.Core.Menus
{
    /// <summary>
    ///
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// 菜单的唯一标识
        /// </summary>
        public Guid Key { get; set; }

        //
        /// <summary>
        ///
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Guid GroupKey { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<MenuItem> Childs { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool IsShowSeparator { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Dictionary<Guid, List<MenuItem>> Groups
        {
            get
            {
                var dic = new Dictionary<Guid, List<MenuItem>>();
                foreach (var item in Childs)
                {
                    if (!dic.ContainsKey(item.GroupKey))
                    {
                        dic[item.GroupKey] = new List<MenuItem>();
                        continue;
                    }
                    dic[item.GroupKey].Add(item);
                }
                return dic;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="title"></param>
        /// <param name="groupKey"></param>
        /// <param name="order"></param>
        public MenuItem(string title, Guid groupKey, int order = int.MinValue)
        {
            Key = Guid.NewGuid();
            Title = title;
            GroupKey = groupKey;
            Order = order;
            Childs = new List<MenuItem>();
        }
    }
}