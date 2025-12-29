using System;

public sealed class RelayCommand
{
    private readonly Action _execute;
    private readonly Func<bool> _canExecute;

    public RelayCommand(Action execute, Func<bool> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute()
    {
        return _canExecute == null || _canExecute();
    }

    public void Execute()
    {
        if (!CanExecute()) return;
        _execute();
    }
}
