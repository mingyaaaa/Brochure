using System;
using System.Windows.Input;

namespace Brochure.WPF.Client.Command
{
    /// <summary>
    ///
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _action;
        private readonly Func<object, bool> _func;

        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        /// <param name="func"></param>
        public RelayCommand(Action<object> action, Func<object, bool> func)
        {
            _action = action;
            _func = func;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        /// <param name="func"></param>
        public RelayCommand(Action action, Func<bool> func)
        {
            _action += t =>
            {
                action.Invoke();
            };
            _func += t => func.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        public RelayCommand(Action<object> action)
        {
            _action = action;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        public RelayCommand(Action action)
        {
            _action += t => action.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            if (_func == null)
                return true;
            return _func.Invoke(parameter);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _action?.Invoke(parameter);
        }

        /// <inheritdoc />
        public event EventHandler CanExecuteChanged;
    }
}