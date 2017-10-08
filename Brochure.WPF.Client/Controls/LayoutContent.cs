using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace Brochure.WPF.Client.Controls
{
    /// <summary>
    ///
    /// </summary>
    [ContentProperty("Content")]
    public class LayoutContent : DependencyObject
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(LayoutContent), new PropertyMetadata(string.Empty));

        /// <summary>
        ///
        /// </summary>
        public string PropertyType
        {
            get => GetValue(TitleProperty)?.ToString();
            set => SetValue(TitleProperty, value);
        }
    }
}