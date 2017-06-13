using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Brochure.WPF.Client
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _action;
        private readonly Func<object, bool> _func;

        public RelayCommand(Action<object> action, Func<object, bool> func)
        {
            _action = action;
            _func = func;
        }

        public RelayCommand(Action action, Func<bool> func)
        {
            _action += t =>
            {
                action.Invoke();
            };
            _func += t => func.Invoke();
        }

        public RelayCommand(Action<object> action)
        {
            _action = action;
        }

        public RelayCommand(Action action)
        {
            _action += t =>
            {
                action.Invoke();
            };
        }
        public bool CanExecute(object parameter)
        {
            if (_func == null)
                return true;
            return _func.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            _action?.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}
