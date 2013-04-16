using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace PredictionGraphs
{
  public class SubstractionConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      if (values.Length != 2 || values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
        return 0.0;

      var factor1 = Double.Parse(values[0].ToString());
      var factor2 = Double.Parse(values[1].ToString());
      var constant = Double.Parse(parameter.ToString());
 
      return factor1 - factor2 - constant;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
