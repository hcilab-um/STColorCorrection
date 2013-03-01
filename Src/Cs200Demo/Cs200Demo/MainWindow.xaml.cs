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

namespace Cs200Demo
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      // on winodos loas trhis happens
      InitializeComponent();
    }
    /// <summary>
    /// button click function
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Click(object sender, RoutedEventArgs e)
    {
      // Note before you tru to run this program pls inclue the path of the Kmsecs200.dll in project ->properties-> build Events 
      try
      {
        //ConnectionToCS200.end_usb(0);
        int numOfDevices = ConnectionToCS200.get_num();
        ConnectionToCS200.int_usb(0);


        String cRemote = "RMT,1\r\n";
       // int r is usied to see the error code

        // while i debug r retus the same value as the strlength !! (in this case 7)
        int r = ConnectionToCS200.write64_usb(0, cRemote, 1, 7);

        String returnString = String.Empty;
        // r used to return -3
        r = ConnectionToCS200.read64_usb(0, returnString, 1, 250);

        String cMeasure = "MES,1\r\n";
        // r was 7
        r = ConnectionToCS200.write64_usb(0, cMeasure, 1, 7);

        //r was -3
        r = ConnectionToCS200.read64_usb(0, returnString, 1, 250);

        System.Threading.Thread.Sleep(10000);

        cMeasure = "MDR,0\r\n";

        // r was 7
        r = ConnectionToCS200.write64_usb(0, cMeasure, 1, 7);

        //r was -3
        r = ConnectionToCS200.read64_usb(0, returnString, 1, 250);

        r = ConnectionToCS200.end_usb(0);
      }
      catch (Exception ex)
      {

      }
    }

    
  }
}
