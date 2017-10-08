using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Brochure.WPF.Client.Controls
{
    /// <summary>
    ///
    /// </summary>

    public class LayoutRoot
    {
        /// <summary>
        ///
        /// </summary>
        public LayoutRoot()
        {
            RightSide = new LayoutAnchorSide();
            LeftSide = new LayoutAnchorSide();
            TopSide = new LayoutAnchorSide();
            BottomSide = new LayoutAnchorSide();
            RootPanel = new LayoutPanel(new LayoutDocumentPane());
        }

        /// <summary>
        ///
        /// </summary>
        public LayoutPanel RootPanel { get; set; }

        /// <summary>
        ///
        /// </summary>
        public LayoutAnchorSide BottomSide { get; set; }

        /// <summary>
        ///
        /// </summary>
        public LayoutAnchorSide TopSide { get; set; }

        /// <summary>
        ///
        /// </summary>
        public LayoutAnchorSide LeftSide { get; set; }

        /// <summary>
        ///
        /// </summary>
        public LayoutAnchorSide RightSide { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DockManager Manager { get; set; }
    }
}