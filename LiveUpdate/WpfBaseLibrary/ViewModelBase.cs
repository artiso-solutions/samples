using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfBaseLibrary
{
   using System.ComponentModel;
   using System.Linq.Expressions;
   using System.Runtime.CompilerServices;

   using WpfBaseLibrary.Annotations;

   public abstract class ViewModelBase : INotifyPropertyChanged
   {
      public event PropertyChangedEventHandler PropertyChanged;

      protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
      {
         if (object.Equals(storage, value)) return false;

         storage = value;
         this.OnPropertyChanged(propertyName);
         return true;
      }

      protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
         var eventHandler = this.PropertyChanged;
         if (eventHandler != null)
         {
            eventHandler(this, new PropertyChangedEventArgs(propertyName));
         }
      }
   }
}
