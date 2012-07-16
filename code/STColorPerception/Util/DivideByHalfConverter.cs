using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace STColorPerception.Util
{
  public class DivideByHalfConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      if (value == DependencyProperty.UnsetValue)
        return 0;

      double inputV = (double)value;
      double sign = Double.Parse(parameter as String);
      return inputV / 2 * sign;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
