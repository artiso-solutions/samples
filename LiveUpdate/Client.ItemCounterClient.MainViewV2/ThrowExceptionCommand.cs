using System;
using System.Windows.Input;

namespace ItemCounterClient.MainView
{
  public class ThrowExceptionCommand : ICommand
  {
    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public void Execute(object parameter)
    {
      throw new NotImplementedException();
    }
  }
}
