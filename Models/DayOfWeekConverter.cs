using System.Globalization;
using System.Windows.Data;

namespace AcademyManager.Models
{
    public class DayOfWeekConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Dictionary<DayOfWeek, string> dayMapping = new Dictionary<DayOfWeek, string>()
            {
                {DayOfWeek.Monday, "Thứ Hai" },
                {DayOfWeek.Tuesday, "Thứ Ba" },
                {DayOfWeek.Wednesday, "Thứ Tư" },
                {DayOfWeek.Thursday, "Thứ Năm" },
                {DayOfWeek.Friday, "Thứ Sáu" },
                {DayOfWeek.Saturday, "Thứ Bảy" },
                {DayOfWeek.Sunday, "Chủ Nhật" }
            };

            if (value is DayOfWeek day) return dayMapping[day];
            return value;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
