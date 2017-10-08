using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Brochure.Core.Extends;

namespace Brochure.WPF.Client.Converter
{
    /// <summary>
    ///
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var b = value.As<bool>();
            if (b)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}