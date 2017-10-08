using System;
using System.Collections.Generic;
using Brochure.Core;
using Brochure.WPF.Client.Core.Menus;

namespace Brochure.WPF.Client
{
    /// <summary>
    ///
    /// </summary>
    public interface IMenus : ISingleton
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="menu"></param>
        void AddMenuItem(MenuItem menu);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        void RemoveMenuItem(Guid key);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        List<MenuItem> GetMenuItems();

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        MenuItem GetMenuItem(Guid key);
    }
}