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

namespace PredictionGraphs
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window, INotifyPropertyChanged
  {

    private DataTable displayData;
    private String filter;
    
		public DataTable DisplayData
    {
      get { return displayData; }
      set 
      {
        displayData = value;
        OnPropertyChanged("DisplayData");
      }
    }

    public String Filter
    {
      get { return filter; }
      set 
      {
        filter = value;
        OnPropertyChanged("Filter");
      }
    }

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
          DisplayData = dsResult.Tables[0];
          PopulateFromAndTo();
        }
      }
    }

    private void PopulateFromAndTo()
    {
      cbFrom.Items.Clear();
      cbTo.Items.Clear();
      foreach (DataColumn column in DisplayData.Columns)
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

    private Dictionary<String, String> filterValues = new Dictionary<string, string>();
    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (DisplayData == null)
        return;

      ComboBox source = (ComboBox)sender;
      String columnName = source.Tag as String;
      if (!DisplayData.Columns.Contains(columnName))
      {
        MessageBox.Show(String.Format("Cannot filter by {0}", columnName));
        return;
      }

      String value = (source.SelectedItem as ComboBoxItem).Tag as String;
      if (value != null)
        filterValues[columnName] = value;
      else if(filterValues.ContainsKey(columnName))
        filterValues.Remove(columnName);

      String tmpFilter = String.Empty;
      foreach (String key in filterValues.Keys)
      {
        if (tmpFilter.Length != 0)
          tmpFilter += " AND ";
        tmpFilter += String.Format("{0} = '{1}'", key, filterValues[key]);
      }
      Filter = tmpFilter;
    }

    private void Intensity_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(String name) 
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(name));
    }
  }
}
