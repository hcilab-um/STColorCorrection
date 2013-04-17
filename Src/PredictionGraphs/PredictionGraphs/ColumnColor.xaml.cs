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

    public static readonly DependencyProperty ModelProperty = DependencyProperty.Register("Model", typeof(String), typeof(ColumnColor));
    public String Model
    {
      get { return (String)GetValue(ModelProperty); }
      set { SetValue(ModelProperty, value); }
    }

    public static readonly DependencyProperty BgTypeProperty = DependencyProperty.Register("BgType", typeof(String), typeof(ColumnColor));
    public String BgType
    {
      get { return (String)GetValue(BgTypeProperty); }
      set { SetValue(BgTypeProperty, value); }
    }

    private String distanceVarName;
    public String DistanceVarName
    {
      get { return distanceVarName; }
      set
      {
        distanceVarName = value;
        OnPropertyChanged("DistanceVarName");
      }
    }

    private double measureHeight;
    public double MeasureHeight 
    {
      get { return measureHeight; }
      set
      {
        measureHeight = value;
        OnPropertyChanged("MeasureHeight");
      }
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
      if (e.Property == ModelProperty || e.Property == BgTypeProperty || e.Property == DataContextProperty)
      {
        if (DataContext == null || !(DataContext is DataTable))
          return;

        DistanceVarName = String.Format("Dist_{0}_{1}", Model, BgType);

        SolidColorBrush bgColor = TestBg as SolidColorBrush;
        DataView view = new DataView(DataContext as DataTable);
        String filter = String.Format("{0} = '0x{1:x2}{2:x2}{3:x2}'", view.Table.Columns[0].ColumnName, bgColor.Color.R, bgColor.Color.G, bgColor.Color.B);
        view.RowFilter = filter;
        view.Sort = String.Format("Dist DESC");
        lbChart.ItemsSource = view;
        
        MeasureHeight = lbChart.ActualHeight / view.Count;
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(String name)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(name));
    }

  }
}
