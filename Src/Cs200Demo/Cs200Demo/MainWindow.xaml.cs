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
        StringBuilder returnString = new StringBuilder(250);
        
        ConnectionToCS200.end_usb(0);
        int numOfDevices = ConnectionToCS200.get_num();
        
        DateTime Start1 = DateTime.Now;
        ConnectionToCS200.int_usb(0);
        string time1 = DateTime.Now.Subtract(Start1).ToString();


        Start1 = DateTime.Now;
        string cRemote = "RMT,1\r\n";
        int length = cRemote.Length;

        int r = ConnectionToCS200.write64_usb(0, cRemote, 1, length);
        r = ConnectionToCS200.read64_usb(0, returnString, 1, 250);
        string time7 = DateTime.Now.Subtract(Start1).ToString();

        
        
        DateTime Start2 = DateTime.Now;
        string op1 = fun();
       //ConnectionToCS200.end_usb(0);
        string time2 = DateTime.Now.Subtract(Start1).ToString();
        
      // ConnectionToCS200.int_usb(0);
        string op2 = fun();
       //ConnectionToCS200.end_usb(0);
        
      //ConnectionToCS200.int_usb(0);
       string op3 = fun();
       //ConnectionToCS200.end_usb(0);

       //ConnectionToCS200.int_usb(0);
       string op4 = fun();
       ConnectionToCS200.end_usb(0);

       ConnectionToCS200.int_usb(0);
       string op5 = fun();
       ConnectionToCS200.end_usb(0);

       ConnectionToCS200.int_usb(0);
       string op6 = fun();
       ConnectionToCS200.end_usb(0);
       
        Start2 = DateTime.Now;
       string cMeasure = "RMT,0\r\n";
       length = cMeasure.Length;

       r = ConnectionToCS200.write64_usb(0, cMeasure, 1, length);
       r = ConnectionToCS200.read64_usb(0, returnString, 1, 250);
       string time4 = DateTime.Now.Subtract(Start2).ToString();

        //System.Threading.Thread.Sleep(250);

        //cMeasure = "MES,1\r\n";
        //length = cMeasure.Length;

        //r = ConnectionToCS200.write64_usb(0, cMeasure, 1, length);
        //r = ConnectionToCS200.read64_usb(0, returnString, 1, 250);
        //System.Threading.Thread.Sleep(1000);

        ////store in location 100
        //cMeasure = "MEM,100\r\n";
        //length = cMeasure.Length;

        //r = ConnectionToCS200.write64_usb(0, cMeasure, 1, length);
        //r = ConnectionToCS200.read64_usb(0, returnString, 1, 250);
        //System.Threading.Thread.Sleep(500);
        ////read from loaction 100
        //cMeasure = "SDR,100,3\r\n";
        //length = cMeasure.Length;

        //r = ConnectionToCS200.write64_usb(0, cMeasure, 1, length);
        //r = ConnectionToCS200.read64_usb(0, returnString, 1, 250);

        ////delet aLL MEMORY
        //cMeasure = "MAD\r\n";
        //length = cMeasure.Length;

        //r = ConnectionToCS200.write64_usb(0, cMeasure, 1, length);
        //r = ConnectionToCS200.read64_usb(0, returnString, 1, 250);
        
        //////return from remote mode
        //cMeasure = "MDR,0\r\n";
        //length = cMeasure.Length;

        //r = ConnectionToCS200.write64_usb(0, cMeasure, 1, length);
        //r = ConnectionToCS200.read64_usb(0, returnString, 1, 250);

        
        
      }
      catch (Exception ex)
      {

      }
    }

    public static string fun()
    {
      
      
      StringBuilder returnString = new StringBuilder(250);

        //string returnString = string.Empty;
      //get into remote mode


     // System.Threading.Thread.Sleep(100);
      //measure
      //cRemote = "SPR\r\n";
      //length = cRemote.Length;

      //r = ConnectionToCS200.write64_usb(0, cRemote, 1, length);
      //r = ConnectionToCS200.read64_usb(0, returnString, 1, 250);
      //DateTime Start2 = DateTime.Now;
      string cMeasure = "MES,1\r\n";
      int length = cMeasure.Length;

      int r = ConnectionToCS200.write64_usb(0, cMeasure, 1, length);
      r = ConnectionToCS200.read64_usb(0, returnString, 1, 250);
      
      string waittime =returnString.ToString(5, 2);

      
      int t= Convert.ToInt32(waittime);
      t=t*1000;
      t=t-500;
      
      System.Threading.Thread.Sleep(t);
     
      cMeasure = "MDR,3\r\n";
      length = cMeasure.Length;

      r = ConnectionToCS200.write64_usb(0, cMeasure, 1, length);
      r = ConnectionToCS200.read64_usb(0, returnString, 1, 250);
      
      string error = returnString.ToString(0, 4);
      while (error == "ER02")
      {
        System.Threading.Thread.Sleep(300);
        cMeasure = "MDR,3\r\n";
        length = cMeasure.Length;

        r = ConnectionToCS200.write64_usb(0, cMeasure, 1, length);
        r = ConnectionToCS200.read64_usb(0, returnString, 1, 250);
        error = returnString.ToString(0, 4);
      }
      
      string op = returnString.ToString();
      string x = returnString.ToString(27, 10);
      string y = returnString.ToString(39, 10);
      string z = returnString.ToString(51, 10);

      double X = Convert.ToDouble(x) / 100;
      double Y = Convert.ToDouble(y) / 100;
      double Z = Convert.ToDouble(z) / 100;
      string time3 = DateTime.Now.Subtract(Start2).ToString();

      

      
      return op;
    }
       
  }
}
