using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfBaseLibrary
{
   using System.Windows.Input;

   public class RelayCommand : ICommand
   {
      private readonly Func<object, bool> canExecute;

      private readonly Action<object> execute;
      public RelayCommand(Func<object, bool> canExecute, Action<object> execute)
      {
         this.canExecute = canExecute;
         this.execute = execute;
      }

      public RelayCommand(Action<object> execute)
      {

         this.execute = execute;
      }
      public event EventHandler CanExecuteChanged;

      public void Execute(object parameter)
      {
         execute(parameter);
      }

      public bool CanExecute(object parameter)
      {
         if (canExecute != null)
         {
            return canExecute(parameter);
         }

         return true;
      }
   }
}
