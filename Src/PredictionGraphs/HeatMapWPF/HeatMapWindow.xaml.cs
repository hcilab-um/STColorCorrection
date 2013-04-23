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
using System.ComponentModel;
using HeatMapWPF.Properties;

namespace HeatMapWPF
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window, INotifyPropertyChanged
  {
    const int BACKGROUND_COUNT = 23;
    HeatMapData[,] heatMap = new HeatMapData[HeatMapData.GRID_SIZE, HeatMapData.GRID_SIZE];
    CheckBox[] checkBoxes = new CheckBox[BACKGROUND_COUNT];
    Ellipse[] backgroundMarkers = new Ellipse[BACKGROUND_COUNT];
    DataView dataView;

    public MainWindow()
    {
      InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      InitCheckBoxes();
      InitBackgroundPositions();
      InitMap();
    }

    private void InitCheckBoxes()
    {
      int index = 0;
      checkBoxes[index] = cb_1;
      index++;
      checkBoxes[index] = cb_2;
      index++;
      checkBoxes[index] = cb_3;
      index++;
      checkBoxes[index] = cb_4;
      index++;
      checkBoxes[index] = cb_5;
      index++;
      checkBoxes[index] = cb_6;
      index++;
      checkBoxes[index] = cb_7;
      index++;
      checkBoxes[index] = cb_8;
      index++;
      checkBoxes[index] = cb_9;
      index++;
      checkBoxes[index] = cb_10;
      index++;
      checkBoxes[index] = cb_11;
      index++;
      checkBoxes[index] = cb_12;
      index++;
      checkBoxes[index] = cb_13;
      index++;
      checkBoxes[index] = cb_14;
      index++;
      checkBoxes[index] = cb_15;
      index++;
      checkBoxes[index] = cb_16;
      index++;
      checkBoxes[index] = cb_17;
      index++;
      checkBoxes[index] = cb_18;
      index++;
      checkBoxes[index] = cb_19;
      index++;
      checkBoxes[index] = cb_20;
      index++;
      checkBoxes[index] = cb_21;
      index++;
      checkBoxes[index] = cb_22;
      index++;
      checkBoxes[index] = cb_23;
    }

    private void InitMap()
    {
      for (int row = 0; row < HeatMapData.GRID_SIZE; row++)
      {
        for (int col = 0; col < HeatMapData.GRID_SIZE; col++)
        {
          heatMap[row, col] = new HeatMapData(row, col);
        }
      }
    }

    private void InitBackgroundPositions()
    {
      string[] splitValues;
      double dataX, dataY;
      double graphX, graphY;
      int col, row, index;
      foreach (CheckBox checkBox in checkBoxes)
      {
        splitValues = checkBox.Tag.ToString().Split(',');
        dataX = Double.Parse(splitValues[0]);
        dataY = Double.Parse(splitValues[1]);
        splitValues = checkBox.Name.ToString().Split('_');
        index = int.Parse(splitValues[1]) - 1;
        col = (int)((dataX + 100) / HeatMapData.DataWidth);
        row = HeatMapData.GRID_SIZE - (int)((dataY + 100) / HeatMapData.DataHeight + 1);
        graphY = (HeatMapData.GRID_SIZE - row - 1) * HeatMapData.GraphHeight;
        graphX = col * HeatMapData.GraphWidth;

        graphX = cvHeatMap.ActualWidth / Settings.Default.DataMapWidth * dataX + cvHeatMap.ActualWidth / 2;
        graphY = cvHeatMap.ActualHeight / Settings.Default.DataMapHeight * dataY + cvHeatMap.ActualHeight / 2;

        backgroundMarkers[index] = new Ellipse();
        backgroundMarkers[index].Width = 20;
        backgroundMarkers[index].Height = 20;
        backgroundMarkers[index].Fill = checkBox.Background;
        backgroundMarkers[index].Stroke = Brushes.Black;
        backgroundMarkers[index].StrokeThickness = 2;

        Canvas.SetLeft(backgroundMarkers[index], graphX - backgroundMarkers[index].Width/2);
        Canvas.SetBottom(backgroundMarkers[index], graphY - backgroundMarkers[index].Height/ 2);
      }
    }

    private void CorrectMap()
    {
      bool atLeastOneCorrected = false;

      foreach (HeatMapData heatMapData in heatMap)
      {
        if (heatMapData.DataSize != 0)
          continue;
        atLeastOneCorrected = atLeastOneCorrected || GetSurroundedAverage(heatMapData);
      }

      if (atLeastOneCorrected)
        CorrectMap();
    }

    private bool GetSurroundedAverage(HeatMapData heatMapData)
    {
      int row = heatMapData.Row;
      int col = heatMapData.Column;
      List<HeatMapData> surroundedList = new List<HeatMapData>();
      if (row != 0)
      {
        if (heatMap[row - 1, col].DataSize != 0)
        {
          surroundedList.Add(heatMap[row - 1, col]);
        }
      }

      if (row != HeatMapData.GRID_SIZE - 1)
      {
        if (heatMap[row + 1, col].DataSize != 0)
        {
          surroundedList.Add(heatMap[row + 1, col]);
        }
      }

      if (col != 0)
      {
        if (heatMap[row, col - 1].DataSize != 0)
        {
          surroundedList.Add(heatMap[row, col - 1]);
        }
      }

      if (col != HeatMapData.GRID_SIZE - 1)
      {
        if (heatMap[row, col + 1].DataSize != 0)
        {
          surroundedList.Add(heatMap[row, col + 1]);
        }
      }

      if (surroundedList.Count < 3)
        return false;


      heatMapData.DataSize = surroundedList.Sum(hmd => hmd.DataSize);
      heatMapData.DataSum = surroundedList.Sum(hmd => hmd.DataSum);
      return true;
    }


    private void DrawMap()
    {
      foreach (HeatMapData heatMapDataq in heatMap)
      {
        if (heatMapDataq != null && cbShowHeatMap.IsChecked.Value)
          cvHeatMap.Children.Add(heatMapDataq.DrawRectangle());
      }

      string[] splitValues;
      int index;
      foreach (CheckBox checkBox in checkBoxes)
      {
        splitValues = checkBox.Name.ToString().Split('_');
        index = int.Parse(splitValues[1]) - 1;
        if (checkBox.IsChecked == true)
          cvHeatMap.Children.Add(backgroundMarkers[index]);
        else
          cvHeatMap.Children.Remove(backgroundMarkers[index]);
      }

    }

    private void Button_Click_Select(object sender, RoutedEventArgs e)
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
          PopulateFromAndTo(dataView);
        }
      }
    }

    private void PopulateFromAndTo(DataView dataView)
    {
      foreach (DataColumn column in dataView.Table.Columns)
      {
        if (column.ColumnName.EndsWith("_L"))
        {
          String variable = column.ColumnName.Substring(0, column.ColumnName.Length - 2);
          variable = variable.Trim();
          cbFrom.Items.Add(variable);
          cbTo.Items.Add(variable);
        }
      }
    }

    private void Button_Click_Refresh(object sender, RoutedEventArgs e)
    {
      if (dataView == null)
      {
        MessageBox.Show("Need to import data!");
        return;
      }

      if (cbFrom.SelectedIndex == -1 || cbTo.SelectedIndex == -1)
      {
        MessageBox.Show("Please select from and to variables");
        return;
      }

      cvHeatMap.Children.Clear();
      foreach (HeatMapData heatMapData in heatMap)
      {
        heatMapData.DataSize = 0;
        heatMapData.DataSum = 0;
        heatMapData.IsOutsideGammut = false;
      }

      foreach (DataRowView dataRowView in dataView)
      {
        var x_value = Double.Parse(dataRowView[Settings.Default.XColumnName].ToString());
        var y_value = Double.Parse(dataRowView[Settings.Default.YColumnName].ToString());
        int col = (int)((HeatMapData.GRID_SIZE * (x_value + 100) / Settings.Default.DataMapWidth));
        int row = HeatMapData.GRID_SIZE - (int)((HeatMapData.GRID_SIZE * (y_value + 100) / Settings.Default.DataMapHeight)) - 1;
        var heatMapData = heatMap[row, col];

        bool isOutsideGammut = GetGammutFlag(dataRowView);
        if ((cbGammutBoth.IsSelected || cbGammutOut.IsSelected) && isOutsideGammut)
        {
          heatMapData.DataSum += CalculateDistance(dataRowView, cbFrom.SelectedItem as String, cbTo.SelectedItem as String);
          heatMapData.DataSize++;
        }
        else if ((cbGammutBoth.IsSelected || cbGammutIn.IsSelected) && !isOutsideGammut)
        {
          heatMapData.DataSum += CalculateDistance(dataRowView, cbFrom.SelectedItem as String, cbTo.SelectedItem as String);
          heatMapData.DataSize++;
        }
      }

      CorrectMap();
      DrawMap();
    }

    private static bool GetGammutFlag(DataRowView dataRowView)
    {
      String gammutFlag = dataRowView["GammutFlag"] as String;
      if (gammutFlag == "0")
        return false;
      return true;
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

    private void CheckBox_Click(object sender, RoutedEventArgs e)
    {
      if (dataView == null)
      {
        MessageBox.Show("Need to import data!");
        CheckBox cb = (CheckBox)sender;
        cb.IsChecked = false;
        return;
      }

      String filter = null;
      SolidColorBrush solidColorBrush;
      dataView.RowFilter = "";
      foreach (CheckBox checkBox in checkBoxes)
      {
        if (checkBox.IsChecked == true)
        {
          solidColorBrush = checkBox.Background as SolidColorBrush;
          if (filter == null)
            filter = String.Format("{0} = '0x{1:x2}{2:x2}{3:x2}'", Settings.Default.BgColumn, solidColorBrush.Color.R, solidColorBrush.Color.G, solidColorBrush.Color.B);
          else
            filter += " OR " + String.Format("{0} = '0x{1:x2}{2:x2}{3:x2}'", Settings.Default.BgColumn, solidColorBrush.Color.R, solidColorBrush.Color.G, solidColorBrush.Color.B);
        }
      }
      dataView.RowFilter = filter;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(String name)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(name));
    }
  }
}
