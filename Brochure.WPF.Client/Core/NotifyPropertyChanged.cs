using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Brochure.Core.Helper;
using Brochure.WPF.Client.Annotations;

namespace Brochure.WPF.Client
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void Set<T>(Expression<Func<object>> exp, ref T field, T value)
        {
            if (field.Equals(value)) return;
            field = value;
            OnPropertyChanged(ObjectHelper.GetPropertyName(exp));
        }
    }
}