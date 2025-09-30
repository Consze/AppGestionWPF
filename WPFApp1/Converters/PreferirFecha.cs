using System.Globalization;
using System.Windows.Data;

namespace WPFApp1.Converters
{
    public class PreferirFechaConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? fechaModificacion = values[0] as DateTime?;
            DateTime? fechaCreacion = values[1] as DateTime?;

            if (fechaModificacion.HasValue && fechaModificacion != DateTime.MinValue)
                return fechaModificacion.Value.ToString("dd/MM/yyyy");
            if (fechaCreacion.HasValue)
                return fechaCreacion.Value.ToString("dd/MM/yyyy");

            return DateTime.MinValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
