using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace STColorPerception.Util
{
  class ScaleCompensationConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      if (value == DependencyProperty.UnsetValue)
        return 0;

      double scale = (double)value;
      double size = Double.Parse(parameter as String);

      if (scale == 0)
        return 0;
      return size / scale;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
