using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Brochure.Core.Helper;
using Brochure.WPF.Client.Annotations;

namespace Brochure.WPF.Client
{
    /// <summary>
    ///
    /// </summary>
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        /// <summary>
        ///
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        protected void Set<T>(Expression<Func<object>> exp, ref T field, T value)
        {
            if (field.Equals(value)) return;
            field = value;
            OnPropertyChanged(ObjectHelper.GetPropertyName(exp));
        }
    }
}