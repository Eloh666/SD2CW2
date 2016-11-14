using CourseworkTwoMetro.ViewModels.Utils;

namespace CourseworkTwoMetro.ViewModels
{
    public abstract class FormWithSpinnerViewModel : PropertyChangedNotifier
    {
        private bool _loading;
        
        protected FormWithSpinnerViewModel()
        {
            this._loading = false;
        }
        public bool NotLoading => !this._loading;
        public bool Loading
        {
            get { return this._loading; }
            set
            {
                this._loading = value;
                OnPropertyChangedEvent("Loading");
            }
        }
    }
}