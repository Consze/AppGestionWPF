using System;
using System.Windows.Input;

public class RelayCommand<T> : ICommand
{
    private readonly Action<T> _execute;
    private readonly Func<T, bool> _canExecute;

    public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter)
    {

        if (_canExecute == null) return true;

        if (parameter == null)
        {
            return _canExecute(default(T));
        }

        if (parameter is T typedParameter)
        {
            return _canExecute(typedParameter);
        }

        return false;
    }

    public void Execute(object parameter)
    {
        if (parameter == null)
        {
            _execute(default(T));
            return;
        }

        if (parameter is T typedParameter)
        {
            _execute(typedParameter);
        }
    }
}