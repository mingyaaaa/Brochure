using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Brochure.WPF.Client.Views;

namespace Brochure.WPF.Client.Core
{
    /// <summary>
    ///
    /// </summary>

    public class DockPanleManager : Control
    {
        static DockPanleManager()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public DockPanleManager()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register("Layout", typeof(LeftAndRightDockPanle), typeof(DockPanleManager), new PropertyMetadata(null));

        /// <summary>
        ///
        /// </summary>
        public LeftAndRightDockPanle Layout
        {
            get { return GetValue(LayoutProperty) as LeftAndRightDockPanle; }
            set { SetValue(LayoutProperty, value); }
        }
    }
}