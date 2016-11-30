using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace CourseworkTwoMetro.Views
{
    /// <summary>
    /// Interaction logic for GuestEdit.xaml
    /// </summary>
    public partial class GuestEdit : MetroWindow
    {
        public GuestEdit()
        {
            InitializeComponent();
        }

        // handles the event if the key is NOT containing a number and therefore:
        // Between D0 and D9 (a standard number button
        // Between numpad0 and numpad 9
        // Filtering the view input itself, in MVVM must be handled by the view itself
        private void HandleKeydownEvent(object sender, System.Windows.Input.KeyEventArgs e)
        {
            e.Handled = !(
                (e.Key >= Key.D0 && e.Key <= Key.D9)
                ||
                (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                );
        }

    }
}
