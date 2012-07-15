using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace STColorPerception.Util
{
  class FixTranslationConverter : IValueConverter
  {
    private const double IMAGE_SIDE = 2048;
    private const double ZERO_XY = 102;

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      if (value == DependencyProperty.UnsetValue)
        return 0;

      double actual = (double)value;
      double scale = actual / IMAGE_SIDE;
      double sign = Double.Parse(parameter as String);

      return ZERO_XY * scale * sign;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
