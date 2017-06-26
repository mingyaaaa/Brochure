using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Brochure.WPF.Client
{
    public class MenuItem
    {
        /// <summary>
        /// 菜单的唯一标识
        /// </summary>
        public Guid Key { get; set; }
        //
        public string Title { get; set; }

        public Guid GroupKey { get; set; }

        public List<MenuItem> Childs { get; set; }

        public int Order { get; set; }

        public bool IsShowSeparator { get; set; }

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
