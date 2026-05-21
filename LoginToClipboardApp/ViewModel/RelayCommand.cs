using System.Windows.Input;

namespace LoginClipboardApp.ViewModel
{
    public class RelayCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool>? canExecute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action execute)
        {
            this.execute = execute;
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object? parameter)
        {
            return this.canExecute?.Invoke() != false;
        }

        public void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                this.execute();
            }
        }
    }
}
