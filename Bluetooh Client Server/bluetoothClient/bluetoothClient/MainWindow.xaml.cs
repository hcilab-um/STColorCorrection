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
using System.ComponentModel;
using InTheHand.Net.Sockets;
using InTheHand.Net;
using System.IO;
//using System.Drawing;
using System.Threading;

namespace bluetoothClient
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window, INotifyPropertyChanged
  {
    private Bluetooth bluetoothObj;
    private BackgroundWorker workerNetwork = new BackgroundWorker();

    private String hexColor = String.Empty;
    public string HexColor
    {
      get { return hexColor; }
      set 
      {
        hexColor = value;
        OnPropertyChanged("HexColor");
      }
    }

    public MainWindow()
    {
      InitializeComponent();
      bluetoothObj = new Bluetooth(Properties.Settings.Default.applicationGuid, this);
      bluetoothObj.Init();
      workerNetwork.DoWork += new DoWorkEventHandler(WorkerNetwork_DoWork);
      
    }

    private void Connect_Click(object sender, RoutedEventArgs e)
    {
        
      workerNetwork.RunWorkerAsync();
    }

    void WorkerNetwork_DoWork(object sender, DoWorkEventArgs e)
    {
      if (!bluetoothObj.IsConnected)
        return;

      try
      {
          string temp;
        //Sends the RGB color
        HexColor = bluetoothObj.ReciveColor();
        temp = bluetoothObj.ReciveColor();
      }
      catch (Exception exception)
      {
        Console.WriteLine(exception.Message);
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged(String name)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(name));
    }
  }
}