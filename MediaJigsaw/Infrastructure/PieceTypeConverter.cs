using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using MediaJigsaw.Models;

namespace MediaJigsaw.Infrastructure
{
    [ValueConversion(typeof(PieceType), typeof(String))]
    public class PieceTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enum.GetName(value.GetType(), value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PieceType outType;
            if(Enum.TryParse(value.ToString(), out outType))
            {
                return outType;
            }
            return null;
        }
    }
}
