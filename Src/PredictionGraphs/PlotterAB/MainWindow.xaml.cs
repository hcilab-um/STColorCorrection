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
          PopulateComboBoxes();
        }
      }
    }

    private void PopulateComboBoxes()
    {
      foreach (DataColumn column in dataView.Table.Columns)
      {
        if (column.ColumnName.EndsWith("_L"))
        {
          String variable = column.ColumnName.Substring(0, column.ColumnName.Length - 2);
          variable = variable.Trim();
          cbA.Items.Add(variable);
        }
      }
    }

    private void DrawPoints()
    {
      if (dataView == null)
        return;
      if (cbA.SelectedItem == null)
        return;

      cvHeatMap.Children.Clear();
      foreach (DataRowView row in dataView)
      {
        double dataX = Double.Parse(row[(cbA.SelectedItem as String) + "_a"] as String);
        double dataY = Double.Parse(row[(cbA.SelectedItem as String) + "_b"] as String);

        double graphX = cvHeatMap.ActualWidth / 200 * dataX + cvHeatMap.ActualWidth / 2;
        double graphY = cvHeatMap.ActualHeight / 200 * dataY + cvHeatMap.ActualHeight / 2;

        Ellipse circle = new Ellipse();
        circle.Width = sSize.Value;
        circle.Height = sSize.Value;
        
        circle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + (row["Bg_RRGGBB"] as String).Substring(2).ToUpper()));
        circle.Stroke = Brushes.Black;
        circle.StrokeThickness = 2;

        Canvas.SetLeft(circle, graphX - circle.Width / 2);
        Canvas.SetBottom(circle, graphY - circle.Height / 2);
        cvHeatMap.Children.Add(circle);
      }
    }

    private void cbA_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      DrawPoints();
    }

    private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
      DrawPoints();
    }
  }
}
