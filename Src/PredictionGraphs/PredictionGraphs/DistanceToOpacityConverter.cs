using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace PredictionGraphs
{
  class DistanceToOpacityConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      String varName = values[1] as String;
      var distance = Double.Parse((values[0] as System.Data.DataRowView)[varName] as String);
      var max = Double.Parse(parameter as String);
      var opacity = 1 - distance / max;
      return opacity;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
