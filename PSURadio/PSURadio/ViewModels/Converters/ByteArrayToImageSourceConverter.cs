using System;
using System.Globalization;
using Xamarin.Forms;

namespace PSURadio.Converters
{
    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] byteArray && byteArray.Length > 0)
            {
                return ImageSource.FromStream(() => new System.IO.MemoryStream(byteArray));
            }
            return ImageSource.FromFile("default_user.jpg"); // Путь к изображению по умолчанию
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
