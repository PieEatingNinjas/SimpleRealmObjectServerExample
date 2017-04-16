using System;
using System.Globalization;
using Xamarin.Forms;

namespace SimpleRealmObjectServerExample
{
    public class IsCompleteToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool &&((bool)value))
            {
                return Color.Green;
            }
            else
            {
                return Color.Red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
