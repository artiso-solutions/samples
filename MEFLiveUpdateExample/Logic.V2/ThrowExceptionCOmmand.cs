using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
   using System.Windows.Input;

   class ThrowExceptionCommand : ICommand
   {
      public bool CanExecute(object parameter)
      {
         return true;
      }

      public void Execute(object parameter)
      {
         throw new NotImplementedException();
      }

      public event EventHandler CanExecuteChanged;
   }
}
