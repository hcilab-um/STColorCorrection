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
using PerceptionLib.Hacks;
using PerceptionLib;

namespace STColorPerception
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      MTObservableCollection<MeasurementPair> pairs = new MTObservableCollection<MeasurementPair>();

        // my line
      PerceptionLib.Color.CIElUV CIElUV = PerceptionLib.Color.CIElUV.Empty;
        
        
        pairs.Add(new MeasurementPair()
      {
          ColorToShow = new PerceptionLib.Color() { CIElUV.L = 0, CIElUV.U = 0, CIElUV.V = 0 },
          ColorCaptured = new PerceptionLib.Color() { CIElUV.L = 0, CIElUV.U = 0, CIElUV.V = 0 }
      });
      pairs.Add(new MeasurementPair()
      {
        ColorToShow = new PerceptionLib.Color() { L = 0, U = 0.6, V = 0 },
        ColorCaptured = new PerceptionLib.Color() { L = 0, U = 0.5, V = 0.1 }
      });
      pairs.Add(new MeasurementPair()
      {
        ColorToShow = new PerceptionLib.Color() { L = 0, U = 0, V = 0.6 },
        ColorCaptured = new PerceptionLib.Color() { L = 0, U = 0.1, V = 0.5 }
      });
      pairs.Add(new MeasurementPair()
      {
        ColorToShow = new PerceptionLib.Color() { L = 0, U = 0.6, V = 0.6 },
        ColorCaptured = new PerceptionLib.Color() { L = 0, U = 0.5, V = 0.5 }
      });
      pairs.Add(new MeasurementPair());
      pairs.Add(new MeasurementPair() { ColorToShow = new PerceptionLib.Color() { L = 0, U = 0.3, V = 0.3 } });
      cie1976C.DataContext = pairs;
    }
  }
}
