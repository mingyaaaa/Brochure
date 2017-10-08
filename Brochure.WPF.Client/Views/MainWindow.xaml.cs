using Brochure.WPF.Client.Interface;
using Brochure.WPF.Client.ViewModels;

namespace Brochure.WPF.Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : WindowViewBase, IViewModel<MianViewModel>
    {
        /// <summary>
        ///
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}