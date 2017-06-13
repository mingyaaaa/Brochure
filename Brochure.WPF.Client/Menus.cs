﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Brochure.Core.Extends;
using Brochure.Core.implement;

namespace Brochure.WPF.Client
{
    public class Menus : Singleton, IMenus
    {
        public List<MenuItem> Items = new List<MenuItem>();

        public void AddMenuItem(MenuItem menu)
        {
            Items.Add(menu);

        }
        public void RemoveMenuItem(Guid key)
        {
            RemoveMenuItem(Items, key);
        }

        public List<MenuItem> GetMenuItems()
        {
            OrderMenuItem();
            return Items;
        }

        public MenuItem GetMenuItem(Guid key)
        {
            MenuItem result = null;
            Stack<MenuItem> stack = new Stack<MenuItem>();
            foreach (var item in Items)
            {
                stack.Push(item);
            }
            while (stack.Count > 0)
            {
                var temp = stack.Pop();
                if (temp.Key == key)
                {
                    result = temp;
                    break;
                }
                foreach (var item in temp.Childs)
                    stack.Push(item);
            }
            return result;
        }

        private void RemoveMenuItem(List<MenuItem> items, Guid key)
        {
            MenuItem result = null;
            foreach (var item in items)
            {
                if (item.Childs.Count == 0)
                {
                    if (key == item.Key)
                        result = item;
                }
                if (item.Childs.Count > 0)
                    RemoveMenuItem(item.Childs, key);
            }
            if (result != null)
                items.Remove(result);
        }

        private void OrderMenuItem()
        {
            Stack<MenuItem> stack = new Stack<MenuItem>();
            foreach (var item in Items)
                stack.Push(item);
            while (stack.Count > 0)
            {
                var menu = stack.Pop();
                if (menu.Childs.Count > 1)
                {
                    menu.Childs = menu.Childs.OrderByDescending(t => t.Order).ToList(); //位子祥排序
                    if (menu.Groups.Keys.Count > 0)
                    {
                        foreach (var key in menu.Groups.Keys)
                        {
                            var lastgroupMenuItem = menu.Groups[key].LastOrDefault();
                            if (lastgroupMenuItem == null)
                                continue;
                            lastgroupMenuItem.IsShowSeparator = true;
                        }
                    }
                }
                foreach (var t in menu.Childs)
                    stack.Push(t);
            }
        }
    }
}
