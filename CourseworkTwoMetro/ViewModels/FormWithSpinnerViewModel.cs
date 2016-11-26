using CourseworkTwoMetro.ViewModels.Utils;

namespace CourseworkTwoMetro.ViewModels
{
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
                OnPropertyChangedEvent("LoadingFailed");
            }
        }

        public bool LoadingSuccess
        {
            get { return this._loadingFailed; }
            set
            {
                this._loadingFailed = value;
                OnPropertyChangedEvent("LoadingSuccess");
            }
        }
    }
}