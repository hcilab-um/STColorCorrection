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

namespace PredictionGraphs
{
  /// <summary>
  /// Interaction logic for ColumnColor.xaml
  /// </summary>
  public partial class ColumnColor : UserControl
  {

    public static readonly DependencyProperty TestBgProperty = DependencyProperty.Register("TestBg", typeof(Brush), typeof(ColumnColor));
    public Brush TestBg
    {
      get { return (Brush)GetValue(TestBgProperty); }
      set { SetValue(TestBgProperty, value); }
    }

    public ColumnColor()
    {
      InitializeComponent();
      TestBg = Brushes.Red;
    }
  }
}
