using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.ComponentModel;
using PredictionGraphs.Properties;

namespace PredictionGraphs
{
  /// <summary>
  /// Interaction logic for ColumnColor.xaml
  /// </summary>
  public partial class ColumnColor : UserControl, INotifyPropertyChanged
  {

    public static readonly DependencyProperty TestBgProperty = DependencyProperty.Register("TestBg", typeof(Brush), typeof(ColumnColor));
    public Brush TestBg
    {
      get { return (Brush)GetValue(TestBgProperty); }
      set { SetValue(TestBgProperty, value); }
    }

    public static readonly DependencyProperty FromProperty = DependencyProperty.Register("From", typeof(String), typeof(ColumnColor));
    public String From
    {
      get { return (String)GetValue(FromProperty); }
      set { SetValue(FromProperty, value); }
    }

    public static readonly DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(String), typeof(ColumnColor));
    public String To
    {
      get { return (String)GetValue(ToProperty); }
      set { SetValue(ToProperty, value); }
    }

    public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(String), typeof(ColumnColor));
    public String Filter
    {
      get { return (String)GetValue(FilterProperty); }
      set { SetValue(FilterProperty, value); }
    }

    public static readonly DependencyProperty IntensityProperty = DependencyProperty.Register("Intensity", typeof(int), typeof(ColumnColor));
    public int Intensity
    {
      get { return (int)GetValue(IntensityProperty); }
      set { SetValue(IntensityProperty, value); }
    }

    private double maxValue;
    public double MaxValue
    {
      get { return maxValue; }
      set
      {
        maxValue = value;
        OnPropertyChanged("MaxValue");
      }
    }

    public ColumnColor()
    {
      InitializeComponent();
      TestBg = Brushes.Red;
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
      base.OnPropertyChanged(e);
      if (e.Property == FromProperty || e.Property == ToProperty || e.Property == FilterProperty || e.Property == IntensityProperty)
      {
        if (DataContext == null || !(DataContext is DataTable))
          return;
        if(From == null || To == null)
          return;

        SolidColorBrush bgColor = TestBg as SolidColorBrush;
        DataView view = new DataView(DataContext as DataTable);
        String bgFilter = String.Format("{0} = '0x{1:x2}{2:x2}{3:x2}'", Settings.Default.BgColumn, bgColor.Color.R, bgColor.Color.G, bgColor.Color.B);
        if (Filter != null && Filter.Length != 0)
          bgFilter += " AND " + Filter;
        view.RowFilter = bgFilter;

        cHistogram.Children.Clear();
        foreach (DataRowView row in view)
        {
          if (Intensity != 0)
          {
            double fg_L = Double.Parse(row["fg_L"] as String);
            if (Intensity == 1 && fg_L < 50)
              continue;
            if (Intensity == -1 && fg_L > 50)
              continue;
          }

          Rectangle measurement = new Rectangle();
          measurement.Width = cHistogram.ActualWidth;
          measurement.Height = 1;
          measurement.Opacity = 0.1;
          measurement.Fill = Brushes.Blue;
          Canvas.SetLeft(measurement, 0);
          Canvas.SetTop(measurement, CalculateDistanceFromTop(row, From, To, 1));
          cHistogram.Children.Add(measurement);
        }
      }
    }

    private double CalculateDistanceFromTop(DataRowView row, string fromName, string toName, double markerHeight)
    {
      double distance = CalculateDistance(row, fromName, toName);
      double columnHeight = cHistogram.ActualHeight;

      if (distance > maxValue)
        return -10;

      var location = distance * columnHeight / maxValue;
      location -= markerHeight / 2;

      return columnHeight - location;
    }

    private double CalculateDistance(DataRowView dataRowView, String fromVar, String toVar)
    {
      double fromL = Double.Parse(dataRowView[fromVar + "_L"] as String);
      double froma = Double.Parse(dataRowView[fromVar + "_a"] as String);
      double fromb = Double.Parse(dataRowView[fromVar + "_b"] as String);
      PerceptionLib.Color fromColor = new PerceptionLib.Color() { L = fromL, A = froma, B = fromb };

      double toL = Double.Parse(dataRowView[toVar + "_L"] as String);
      double toa = Double.Parse(dataRowView[toVar + "_a"] as String);
      double tob = Double.Parse(dataRowView[toVar + "_b"] as String);
      PerceptionLib.Color toColor = new PerceptionLib.Color() { L = toL, A = toa, B = tob };

      double distance = PerceptionLib.Color.ColorDistanceCalAB(fromColor, toColor);
      return distance;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(String name)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(name));
    }

  }
}
