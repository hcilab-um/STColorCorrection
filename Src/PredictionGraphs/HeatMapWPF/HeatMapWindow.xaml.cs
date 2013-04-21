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
		Rectangle[] backgroundRectangles = new Rectangle[BACKGROUND_COUNT];
		DataView dataView;

		public MainWindow()
		{
			InitializeComponent();
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
				index = int.Parse(splitValues[1])-1;
				col = (int)((dataX + 100) / HeatMapData.DataWidth);
				row = HeatMapData.GRID_SIZE - (int)((dataY + 100) / HeatMapData.DataHeight + 1);
				graphY = (HeatMapData.GRID_SIZE - row - 1) * HeatMapData.GraphHeight;
				graphX = col * HeatMapData.GraphWidth;
				backgroundRectangles[index] = new Rectangle(); ;
				backgroundRectangles[index].Width = HeatMapData.GraphWidth;
				backgroundRectangles[index].Height = HeatMapData.GraphHeight;
				backgroundRectangles[index].Fill = checkBox.Background;
				Canvas.SetLeft(backgroundRectangles[index], graphX);
				Canvas.SetBottom(backgroundRectangles[index], graphY);
			}
		}

		private void CorrectMap()
		{
			foreach (HeatMapData heatMapData in heatMap)
			{
				if (heatMapData.DataSize == 0)
				{
					GetSurroundedAverage(heatMapData);
				}
			}
		}

		private void GetSurroundedAverage(HeatMapData heatMapData)
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

			if (surroundedList.Count >= 3)
			{
				heatMapData.DataSize = surroundedList.Sum(hmd => hmd.DataSize);
				heatMapData.DataSum = surroundedList.Sum(hmd => hmd.DataSum);
			}
		}


		private void DrawMap()
		{
			foreach (HeatMapData heatMapDataq in heatMap)
			{
				if (heatMapDataq != null)
					cvHeatMap.Children.Add(heatMapDataq.drawRectangle());
			}
			string[] splitValues;
			int index;
			foreach (CheckBox checkBox in checkBoxes)
			{
				splitValues = checkBox.Name.ToString().Split('_');
				index = int.Parse(splitValues[1]) - 1;
				if (checkBox.IsChecked == true)
					cvHeatMap.Children.Add(backgroundRectangles[index]);
				else
					cvHeatMap.Children.Remove(backgroundRectangles[index]);
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
				}
			}
		}

		private void Button_Click_Refresh(object sender, RoutedEventArgs e)
		{
			cvHeatMap.Children.Clear();
			foreach (HeatMapData heatMapData in heatMap)
			{
				heatMapData.DataSize = 0;
				heatMapData.DataSum = 0;
			}

			foreach (DataRowView dataRowView in dataView)
			{
				var x_value = Double.Parse(dataRowView[Settings.Default.XColumnName].ToString());
				var y_value = Double.Parse(dataRowView[Settings.Default.YColumnName].ToString());
				int col = (int)((HeatMapData.GRID_SIZE * (x_value + 100) / Settings.Default.DataMapWidth));
				int row = HeatMapData.GRID_SIZE - (int)((HeatMapData.GRID_SIZE * (y_value + 100) / Settings.Default.DataMapHeight)) - 1;
				var heatMapData = heatMap[row, col];
				heatMapData.DataSum += Double.Parse(dataRowView[Settings.Default.ValueColumnName].ToString());
				heatMapData.DataSize++;
			}
			CorrectMap();
			DrawMap();
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
						filter = String.Format("{0} = '0x{1:x2}{2:x2}{3:x2}'", dataView.Table.Columns[0].ColumnName, solidColorBrush.Color.R, solidColorBrush.Color.G, solidColorBrush.Color.B);
					else
						filter += " OR " + String.Format("{0} = '0x{1:x2}{2:x2}{3:x2}'", dataView.Table.Columns[0].ColumnName, solidColorBrush.Color.R, solidColorBrush.Color.G, solidColorBrush.Color.B);
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
