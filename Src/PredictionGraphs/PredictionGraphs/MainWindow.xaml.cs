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
    
		public DataTable DisplayData
    {
      get { return displayData; }
      set 
      {
        displayData = value;
        OnPropertyChanged("DisplayData");
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
        }
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
