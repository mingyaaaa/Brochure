using System;
using System.Collections.Generic;
using Brochure.Core;

namespace Brochure.WPF.Client
{
    public interface IMenus : ISingleton
    {
        void AddMenuItem(MenuItem menu);
        void RemoveMenuItem(Guid key);
        List<MenuItem> GetMenuItems();
        MenuItem GetMenuItem(Guid key);
    }
}
