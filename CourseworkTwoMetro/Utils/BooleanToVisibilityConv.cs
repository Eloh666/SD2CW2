using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CourseworkOneMetro.ViewModels.Utils
{
    /// <summary>
    /// Implementation of a class to convert a moolean into a visibility property using the IValueConverter interface
    /// REFERENCE: this is not entirely my implementation, MSDN documentation as well as guides have been used
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public sealed class BoolToVisibilityConv : IValueConverter
    {
        public Visibility TrueValue { get; set; }
        public Visibility FalseValue { get; set; }

        public BoolToVisibilityConv()
        {
            // sets defaults
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Hidden;
        }

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return null;
            return (bool)value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (Equals(value, TrueValue))
                return true;
            if (Equals(value, FalseValue))
                return false;
            return null;
        }
    }
}