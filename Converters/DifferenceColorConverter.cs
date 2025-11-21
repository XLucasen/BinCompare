using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BinCompare
{
    /// <summary>
    /// 差异颜色转换器
    /// </summary>
    public class DifferenceColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool hasDifference && hasDifference)
            {
                return new SolidColorBrush(Color.FromRgb(209, 52, 56)); // 红色
            }
            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 差异背景转换器
    /// </summary>
    public class DifferenceBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool hasDifference && hasDifference)
            {
                return new SolidColorBrush(Color.FromRgb(255, 200, 200)); // 浅红色
            }
            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 十六进制模式转换器
    /// </summary>
    public class HexModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isHexMode)
            {
                return isHexMode ? "十六进制" : "二进制";
            }
            return "未知";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
