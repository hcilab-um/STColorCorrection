using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace PredictionGraphs
{
  class DistanceToLocationConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      String varName = values[1] as String;
      double distance = Double.Parse((values[0] as System.Data.DataRowView)[varName] as String);
      double maxValue = (double)values[2];
      double columnHeight = (double)values[3];
      double markerHeight = (double)values[4];

      if (distance > maxValue)
        return new Thickness(0, -10, 0, 0); //draws it outside of the viewport

      var location = distance * columnHeight / maxValue;
      location -= markerHeight / 2;

      return new Thickness(0, columnHeight - location, 0, 0);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
