using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using VMA.Enums;

namespace VMA.Converters
{
    public class NotificationTypeToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case NotificationType.Success:
                    return new SolidColorBrush(Colors.Green);
                case NotificationType.Warning:
                    return new SolidColorBrush(Colors.OrangeRed);
                case NotificationType.Failure:
                    return new SolidColorBrush(Colors.Red);
                case NotificationType.Alert:
                    return new SolidColorBrush(Colors.Orange);
                default:
                    return new SolidColorBrush(Colors.White);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
