using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class BaseNotificationClass : INotifyPropertyChanged // Class that helps property to see if it has been changed
    {                                                           
        public event PropertyChangedEventHandler PropertyChanged; // Event that is raised when a property changes

        protected virtual void OnPropertyChanged(string propertyName) // Notifies listeners that a property has changed
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // Trigger the PropertyChanged event if there are any subscribers
        }
    }
}
