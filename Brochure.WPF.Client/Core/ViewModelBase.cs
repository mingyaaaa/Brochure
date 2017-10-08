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
    /// <summary>
    ///
    /// </summary>
    public abstract class ViewModelBase : NotifyPropertyChanged
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        public virtual void LoadDataAsync(params object[] obj)
        {
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void LoadDataAsync()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void InitCommand()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        public virtual void InitCommand(params object[] obj)
        {
        }
    }
}