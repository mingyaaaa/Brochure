using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brochure.WPF.Client.Enums;

namespace Brochure.WPF.Client.Controls
{
    /// <summary>
    ///
    /// </summary>
    public class LayoutAnchorSide
    {
        /// <summary>
        ///
        /// </summary>
        public LayoutAnchorSide()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public void UpdateSide()
        {
            if (Root.LeftSide == this)
                Side = AnchorSideEnum.Left;
            else if (Root.TopSide == this)
                Side = AnchorSideEnum.Top;
            else if (Root.RightSide == this)
                Side = AnchorSideEnum.Right;
            else if (Root.BottomSide == this)
                Side = AnchorSideEnum.Bottom;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public AnchorSideEnum Side { get; set; }

        /// <summary>
        ///
        /// </summary>
        public LayoutRoot Root { get; set; }
    }
}