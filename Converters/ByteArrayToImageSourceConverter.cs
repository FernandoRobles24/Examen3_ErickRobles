using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Examen3_ErickRobles.Converters
{
    internal class ByteArrayToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] byteArray && byteArray.Length > 0)
            {
                return ImageSource.FromStream(() => new MemoryStream(byteArray));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
