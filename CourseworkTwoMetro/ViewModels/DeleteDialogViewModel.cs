using CourseworkTwoMetro.Managers;

namespace CourseworkTwoMetro.ViewModels
{
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified November
    /// View model for the deletatation dialog
    /// </summary>
    public class DeleteDialogViewModel : FormWithSpinnerViewModel
    {
        public string Title { get; }
        public WindowsManager Windows { get; }
        public CommandsManager Commands { get; }

        // managers singletons
        public DeleteDialogViewModel(string title, MainViewModel mainViewModel)
        {
            this.Title = title;
            this.Commands = CommandsManager.Instance(mainViewModel);
            this.Windows = WindowsManager.Instance(mainViewModel);
        }

    }
}