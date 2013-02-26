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
using System.Drawing;
using System.Threading;

namespace server_bluetoothexample
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private Bluetooth bluetoothObj;
    private BackgroundWorker workerNetwork;
    private string hexcolor;

    public MainWindow()
    {
      InitializeComponent();

      bluetoothObj = new Bluetooth(Properties.Settings.Default.applicationGuid, this);
      bluetoothObj.Init();

      workerNetwork = new BackgroundWorker();
      workerNetwork.DoWork += new DoWorkEventHandler(WorkerNetwork_DoWork);
    }



    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      //_sensor.Dispose();
      bluetoothObj.Dispose();
    }


    /// <summary>
    /// function to convert RGB to HEX
    /// </summary>
    /// <returns></returns>
    public string rgbToHex()
    {
      string hexcolor_temp;
      byte R, G, B;
      R = Convert.ToByte(txt_R.Text.ToString());
      G = Convert.ToByte(txt_G.Text.ToString());
      B = Convert.ToByte(txt_B.Text.ToString());

      hexcolor_temp = ColorTranslator.FromHtml(String.Format("#{0:X2}{1:X2}{2:X2}", R, G, B)).Name.Remove(0, 2);
      return hexcolor_temp;
    }


    /// <summary>
    /// worker which sends the RGB value to the phone
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void WorkerNetwork_DoWork(object sender, DoWorkEventArgs e)
    {
      if (!bluetoothObj.IsConnected)
        return;
      if (hexcolor == null || hexcolor == String.Empty)
        return;

      try
      {
        //Sends the RGB color
        bluetoothObj.SendRGB(hexcolor);
      }
      catch (Exception exception)
      {
        Console.WriteLine(exception.Message);
      }
    }

    private void btn_Send_Click(object sender, RoutedEventArgs e)
    {

      hexcolor = rgbToHex();
      workerNetwork.RunWorkerAsync();

      //if (!workerNetwork.IsBusy)
      
      // show the rgb color being sent in the text block
      //txtb_hexColor.Text = hexcolor;
      //WorkerNetwork_DoWork();

    }
  }
}
