using System.ComponentModel;

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
        // implements the INotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChangedEvent(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}