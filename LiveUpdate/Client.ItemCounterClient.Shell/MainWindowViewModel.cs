using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBaseLibrary;

namespace Shell
{
    public class MainWindowViewModel : ViewModelBase
    {
       public MainWindowViewModel()
       {
       }

       private string errorMessage;
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                if (errorMessage == value)
                {
                    return;
                }

                errorMessage = value;
                OnPropertyChanged();
            }
        }
    }
}
