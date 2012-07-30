using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace STColorPerception.Util
{
  public class ColorMarginConverter : IMultiValueConverter
  {
    private const double IMAGE_SIDE = 1945;//2048;
    private const double ZERO_XY = 103;
    private const double IMAGE_GRAPH_SIDE = IMAGE_SIDE - 2 * ZERO_XY;

    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      if (values.Length != 4 ||
        values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue ||
        values[2] == DependencyProperty.UnsetValue || values[3] == DependencyProperty.UnsetValue)
        return new Thickness(0, 0, 0, 0);

      double u = (double)values[0];
      double v = (double)values[1];
      double width = (double)values[2];
      double height = (double)values[3];

      double scaleW = width / IMAGE_SIDE;
      double scaleH = height / IMAGE_SIDE;

      if(scaleH == 0 || scaleW == 0)
        return new Thickness(0, 0, 0, 0);

      double graphW = IMAGE_GRAPH_SIDE * scaleW;
      double graphH = IMAGE_GRAPH_SIDE * scaleH;

      double left = graphW * u / 0.6;
      double bottom = graphH * v / 0.6;
      return new Thickness(left, 0, 0, bottom);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
