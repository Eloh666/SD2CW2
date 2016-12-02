using CourseworkTwoMetro.ViewModels.Utils;

namespace CourseworkTwoMetro.ViewModels
{
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified November
    /// Simple view model class that inherits from the property changed notifier
    /// and adds some common logic/booleans to use to display spinner
    /// when an asynchronous method is being executed
    /// </summary>
    public abstract class FormWithSpinnerViewModel : PropertyChangedNotifier
    {
        private bool _loading;
        private bool _loadingFailed;
        private bool _loadingSuccess;

        protected FormWithSpinnerViewModel()
        {
            this._loading = false;
            this._loadingFailed = false;
            this._loadingSuccess = false;
        }
        public bool NotLoading => !this._loading;
        public bool Loading
        {
            get { return this._loading; }
            set
            {
                this._loading = value;
                OnPropertyChangedEvent(null);
            }
        }

        public bool LoadingFailed
        {
            get { return this._loadingFailed; }
            set
            {
                this._loadingFailed = value;
                OnPropertyChangedEvent(null);
            }
        }

        public bool LoadingSuccess
        {
            get { return this._loadingSuccess; }
            set
            {
                this._loadingSuccess = value;
                OnPropertyChangedEvent(null);
            }
        }

        public bool PreloadOnly => this.NotLoading && !this.LoadingSuccess && !this.LoadingFailed;
    }
}