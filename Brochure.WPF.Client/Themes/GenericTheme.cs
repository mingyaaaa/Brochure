using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Brochure.WPF.Client.Abstract;

namespace Brochure.WPF.Client.Themes
{
    /// <summary>
    ///
    /// </summary>
    public class GenericTheme : Theme
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override Uri GetReourceUri()
        {
            return new Uri("/Brochure.WPF.Client;Component/Themes/generic.xaml", UriKind.RelativeOrAbsolute);
        }
    }
}