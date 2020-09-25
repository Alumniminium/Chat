using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace GUI.UI.Converters
{
    public sealed class StringToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,object parameter, CultureInfo culture)
        {
            try
            {
                return new Bitmap(value.ToString());
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType,object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
