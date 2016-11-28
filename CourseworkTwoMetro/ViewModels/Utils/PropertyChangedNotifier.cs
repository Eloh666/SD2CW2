using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CourseworkTwoMetro.ViewModels.Utils
{
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified October 
    /// Basic implementation fo the INotifyPropertyChanged interface.
    /// The viewmodels can simply extend this class to avoid having to reimplement the interface every time.
    /// </summary>
    public class PropertyChangedNotifier : INotifyPropertyChanged
    {
        public void RefreshView()
        {
            OnPropertyChangedEvent(null);
        }
        // implements the INotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChangedEvent([CallerMemberName] string propName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        protected bool SetProperty<T>(ref T storage, T value,
            [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return false;
            storage = value;
            OnPropertyChangedEvent(propertyName);
            return true;
        }
    }
}