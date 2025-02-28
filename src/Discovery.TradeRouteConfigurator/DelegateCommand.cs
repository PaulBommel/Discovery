using System;
using System.Windows.Input;

namespace Discovery.TradeRouteConfigurator
{
    public class DelegateCommand(Action<object> executeDelegate, Func<object, bool> canExecuteDelegate) : ICommand
    {
        private readonly Action<object> _executeDelegate = executeDelegate;
        private readonly Func<object, bool> _canExecuteDelegate = canExecuteDelegate;

        public DelegateCommand(Action<object> executeDelegate)
            : this(executeDelegate, null) { }

        public void Execute(object parameter)
        {
            _executeDelegate(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteDelegate == null || _canExecuteDelegate(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
