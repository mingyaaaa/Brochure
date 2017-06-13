using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Brochure.Core.Extends;
using Brochure.Core.Helper;
using Brochure.WPF.Client.Annotations;

namespace Brochure.WPF.Client
{
    public abstract class ViewModelBase : NotifyPropertyChanged
    {
        public virtual void LoadDataAsync(params object[] obj)
        {
        }
        public virtual void LoadDataAsync()
        {
        }
        public virtual void InitCommand()
        {
        }
        public virtual void InitCommand(params object[] obj)
        {
        }
    }
}
