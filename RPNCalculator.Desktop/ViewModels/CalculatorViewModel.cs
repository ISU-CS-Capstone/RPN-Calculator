using System.ComponentModel;
using System.Windows.Input;

namespace RPNCalculator.Desktop.ViewModels;

public class CalculatorViewModel : INotifyPropertyChanged
{
    private string _output;

    public string Output
    {
        get => _output;
        set
        {
            _output = value;
            OnPropertyChanged(nameof(Output));
        }
    }

    // Example of a command for a digit button
    public ICommand DigitCommand { get; }

    public CalculatorViewModel()
    {
        // Initialize commands
        DigitCommand = new RelayCommand(ExecuteDigitCommand);
        // Initialize other properties and commands
    }

    private void ExecuteDigitCommand(object parameter)
    {
        // Call the method in your class that handles the digit button click
        // Assuming `parameter` is the digit or operation passed from the button
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

// RelayCommand is a simple implementation of ICommand
// It allows you to pass the action to execute
public class RelayCommand : ICommand
{
    private Action<object> _execute;
    private Func<object, bool> _canExecute;

    public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute == null || _canExecute(parameter);
    }

    public void Execute(object parameter)
    {
        _execute(parameter);
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}
