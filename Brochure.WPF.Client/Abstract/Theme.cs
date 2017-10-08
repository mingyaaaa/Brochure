using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Brochure.WPF.Client.Abstract
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Theme : DependencyObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract Uri GetReourceUri();
    }
}
