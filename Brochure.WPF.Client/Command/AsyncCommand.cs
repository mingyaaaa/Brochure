using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Brochure.WPF.Client
{
    public class AsyncCommand : ICommand
    {
        private readonly Action<object> _action;
        private readonly Func<object, Task<bool>> _func;
        private bool _funcResult;
        private bool _funcComplete = true;
        public AsyncCommand(Action<object> action, Func<object, Task<bool>> func)
        {
            _action = action;
            _func = func;
        }

        public AsyncCommand(Action action, Func<Task<bool>> func)
        {
            _action += t =>
            {
                action.Invoke();
            };
            _func += t => func.Invoke();
        }


        private async void InnerCanExecute(object parameter)
        {
            _funcComplete = false;
            _funcResult = await _func.Invoke(parameter);
            CanExecuteChanged?.Invoke(parameter, null);
            _funcComplete = true;

        }

        public void Execute(object parameter)
        {
            _action?.Invoke(parameter);
        }

        public bool CanExecute(object parameter)
        {
            if (_func == null)
                return false;
            if (_funcComplete)
                InnerCanExecute(parameter);
            return _funcResult;
        }

        public event EventHandler CanExecuteChanged;
    }
}
