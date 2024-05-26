using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace AcademyManager.Models
{
    public class ImageConverter : IValueConverter
    {
        private byte[] _ouput;
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = value as string;
            if (input == null) return null;
            try
            {
                _ouput = Convert.FromBase64String(input);
            }
            catch (FormatException)
            {
                return input;
            }
            using (MemoryStream ms = new MemoryStream(_ouput))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
