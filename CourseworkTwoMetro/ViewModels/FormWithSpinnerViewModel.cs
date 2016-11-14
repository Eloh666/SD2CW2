using CourseworkTwoMetro.ViewModels.Utils;

namespace CourseworkTwoMetro.ViewModels
{
    public abstract class FormWithSpinnerViewModel : PropertyChangedNotifier
    {
        private bool _loading;
        private bool _loadingFailed;

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

        public bool LoadingFailed
        {
            get { return this._loadingFailed; }
            set
            {
                this._loadingFailed = value;
                OnPropertyChangedEvent("LoadingFailed");
            }
        }
    }
}