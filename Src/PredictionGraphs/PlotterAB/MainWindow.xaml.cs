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
using GenericParsing;
using System.Data;

namespace PlotterAB
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {

    private DataView dataView;

    public MainWindow()
    {
      InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
      if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        tbFileName.Text = ofd.FileName;
        using (GenericParserAdapter parser = new GenericParserAdapter(tbFileName.Text))
        {
          parser.Load("prediction-format.xml");
          System.Data.DataSet dsResult = parser.GetDataSet();
          dataView = dsResult.Tables[0].AsDataView();
          DrawPoints();
        }
      }
    }

    private void DrawPoints()
    {
      foreach (DataRowView row in dataView)
      {
        double dataX = Double.Parse(row["a"] as String);
        double dataY = Double.Parse(row["b"] as String);

        double graphX = cvHeatMap.ActualWidth / 200 * dataX + cvHeatMap.ActualWidth / 2;
        double graphY = cvHeatMap.ActualHeight / 200 * dataY + cvHeatMap.ActualHeight / 2;

        Ellipse circle = new Ellipse();
        circle.Width = 20;
        circle.Height = 20;
        circle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(row["RGB"] as String));
        circle.Stroke = Brushes.Black;
        circle.StrokeThickness = 2;
      }
    }
  }
}
