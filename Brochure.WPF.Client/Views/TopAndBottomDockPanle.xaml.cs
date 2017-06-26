using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Brochure.WPF.Client.Views
{
    /// <summary>
    /// TopAndBottomDockPanle.xaml 的交互逻辑
    /// </summary>
    public partial class TopAndBottomDockPanle : UserControl
    {
        private FrameworkElement _dragcontrol;
        private FrameworkElement _entercontrol;
        /// <summary>
        /// 
        /// </summary>
        public TopAndBottomDockPanle()
        {
            InitializeComponent();
        }

        private void Border_DragEnter(object sender, DragEventArgs e)
        {
        }

        private void Border_DragOver(object sender, DragEventArgs e)
        {

        }

        private void Border_DragLeave(object sender, DragEventArgs e)
        {

        }

        private void UIElement_OnDrop(object sender, DragEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dragcontrol = sender as FrameworkElement;
            if (_dragcontrol == null)
                return;
            _dragcontrol.Visibility = Visibility.Collapsed;
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            _entercontrol = sender as FrameworkElement;
            if (_entercontrol == null || _dragcontrol == null)
                return;
            var row = Grid.GetRow(_entercontrol);
            var column = Grid.GetColumn(_entercontrol);
            Grid.SetRow(_entercontrol, Grid.GetRow(_dragcontrol));
            Grid.SetColumn(_entercontrol, Grid.GetColumn(_dragcontrol));
            Grid.SetRow(_dragcontrol, row);
            Grid.SetColumn(_dragcontrol, column);
            _dragcontrol.Visibility = Visibility.Visible;
        }

        private void UIElement_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _dragcontrol = null;
        }

        //private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        //{
        //    if (_entercontrol == null || _dragcontrol == null)
        //        return;
        //    var row = Grid.GetRow(_entercontrol);
        //    var column = Grid.GetColumn(_entercontrol);
        //    Grid.SetRow(_entercontrol, Grid.GetRow(_dragcontrol));
        //    Grid.SetColumn(_entercontrol, Grid.GetColumn(_dragcontrol));
        //    Grid.SetRow(_dragcontrol, row);
        //    Grid.SetColumn(_dragcontrol, column);
        //    _entercontrol = null;
        //}
    }
}
