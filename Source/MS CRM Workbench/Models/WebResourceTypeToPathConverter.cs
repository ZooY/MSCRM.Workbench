using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;


namespace PZone.Models
{
    public class EnumImageToPathConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (targetType != typeof(ImageSource))
                throw new InvalidOperationException("Converter can only convert to value of type ImageSource.");
            if (value == null)
                return null;

            var memInfo = value.GetType().GetMember(value.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(ImageAttribute), false);
            var attribute = attributes.FirstOrDefault() as ImageAttribute;
            if (attribute == null)
                return null;
            return (ImageSource)new ImageSourceConverter().ConvertFromString("pack://application:,,,"+attribute.Path);
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}