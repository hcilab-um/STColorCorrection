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

    public static readonly DependencyProperty DistanceProperty = DependencyProperty.Register("Distance", typeof(String), typeof(ColumnColor));
    public String Distance
    {
      get { return (String)GetValue(DistanceProperty); }
      set { SetValue(DistanceProperty, value); }
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
      if (e.Property == DistanceProperty || e.Property == DataContextProperty)
      {
        if (DataContext == null || !(DataContext is DataTable))
          return;

        String varName = String.Format("Dist_{0}", Distance);

        SolidColorBrush bgColor = TestBg as SolidColorBrush;
        DataView view = new DataView(DataContext as DataTable);
        String filter = String.Format("{0} = '0x{1:x2}{2:x2}{3:x2}'", view.Table.Columns[0].ColumnName, bgColor.Color.R, bgColor.Color.G, bgColor.Color.B);
        view.RowFilter = filter;

        cHistogram.Children.Clear();
        foreach (DataRowView row in view)
        {
          Rectangle measurement = new Rectangle();
          measurement.Width = cHistogram.ActualWidth;
          measurement.Height = 1;
          measurement.Opacity = 0.1;
          measurement.Fill = Brushes.Blue;
          Canvas.SetLeft(measurement, 0);
          Canvas.SetTop(measurement, CalculateDistanceFromTop(row, varName, 1));
          cHistogram.Children.Add(measurement);
        }
      }
    }

    private double CalculateDistanceFromTop(DataRowView row, string varName, double markerHeight)
    {
      String distanceValue = row[varName] as String;
      if (distanceValue == null || distanceValue.Length == 0)
      {
        distanceValue = "5.000";
        //throw new ArgumentException("DataSet missing data!");
        Console.WriteLine("DataSet missing data!");
      }

      double distance = Double.Parse(distanceValue);
      double columnHeight = cHistogram.ActualHeight;

      if (distance > maxValue)
        return -10;

      var location = distance * columnHeight / maxValue;
      location -= markerHeight / 2;

      return columnHeight - location;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(String name)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(name));
    }

  }
}
