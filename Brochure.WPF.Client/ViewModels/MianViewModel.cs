using System.Collections.ObjectModel;
using Brochure.WPF.Client.Core.Menus;

namespace Brochure.WPF.Client.ViewModels
{
    /// <summary>
    ///
    /// </summary>
    public class MianViewModel : ViewModelBase
    {
        /// <summary>
        ///
        /// </summary>
        public ObservableCollection<MenuItem> Menus { get; set; }

        /// <inheritdoc />
        public override void LoadDataAsync()
        {
        }

        /// <inheritdoc />
        public override void InitCommand()
        {
        }
    }
}