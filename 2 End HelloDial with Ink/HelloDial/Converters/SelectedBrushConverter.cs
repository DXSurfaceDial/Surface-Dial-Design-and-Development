using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace HelloDial.Converters
{
    public class SelectedBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, 0xE5, 0xE5, 0xE5));

            int selectedIndex = (int)value;

            int ourIndex = int.Parse(parameter.ToString());

            if (selectedIndex == ourIndex)
                brush.Color = Color.FromArgb(255, 0x35, 0x35, 0x35);

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
