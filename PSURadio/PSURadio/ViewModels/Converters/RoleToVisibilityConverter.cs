using System;
using Xamarin.Forms;

public class RoleToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        // Получите роль пользователя из value (например, строка "admin", "radio_host" и т. д.)
        string role = value as string;

        // Проверьте роль пользователя и верните соответствующее значение видимости
        if (role == "admin" || role == "radio_host")
            return true;
        else
            return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
