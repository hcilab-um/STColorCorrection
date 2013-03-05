using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PerceptionLib
{
  public class Cs200Connection
  {

    /// <summary>
    /// function to connect cs200
    /// </summary>
    /// <returns>1 if connected sucessfully ,0 if not</returns>
    public static int ConnectToCS200()
    {
      int i = CaptureEngine.end_usb(0);
      StringBuilder returnString = new StringBuilder(250);
      int numOfDevices = CaptureEngine.get_num();

      //to check if the device is connected
      if (numOfDevices > 0)
      {
        int returnvar = CaptureEngine.int_usb(0);
        string cRemote = "RMT,1\r\n";
        int length = cRemote.Length;

        int r = CaptureEngine.write64_usb(0, cRemote, 1, length);
        r = CaptureEngine.read64_usb(0, returnString, 1, 250);
        return returnvar;
      }
      else
        return 1;
    }


    public static CIEXYZ StartAVGMeasureXYZ()
    {
      CIEXYZ xyz1 = StartMeasureXYZ();
      CIEXYZ xyz2 = StartMeasureXYZ();
      CIEXYZ xyz3 = StartMeasureXYZ();

      CIEXYZ xyz = new CIEXYZ ((xyz1.X+xyz2.X+xyz3.X)/3,(xyz1.Y+xyz2.Y+xyz3.Y)/3,(xyz1.Z+xyz2.Z+xyz3.Z)/3);
      
      
      return xyz;
    }
    /// <summary>
    /// function to get XYZ vales from CS200
    /// </summary>
    /// <returns></returns>
    public static CIEXYZ StartMeasureXYZ()
    {

      StringBuilder returnString = new StringBuilder(250);
        //measure
        string cMeasure = "MES,1\r\n";
        int length = cMeasure.Length;

        int r = CaptureEngine.write64_usb(0, cMeasure, 1, length);
        r = CaptureEngine.read64_usb(0, returnString, 1, 250);

        string waittime = returnString.ToString(5,2);


        int t = Convert.ToInt32(waittime);
        t = t * 1000;
        t = t - 500;

        System.Threading.Thread.Sleep(t);

        cMeasure = "MDR,3\r\n";
        length = cMeasure.Length;

        r = CaptureEngine.write64_usb(0, cMeasure, 1, length);
        r = CaptureEngine.read64_usb(0, returnString, 1, 250);

        string error = returnString.ToString(0, 4);
        while (error == "ER02")
        {
          System.Threading.Thread.Sleep(300);
          cMeasure = "MDR,3\r\n";
          length = cMeasure.Length;

          r = CaptureEngine.write64_usb(0, cMeasure, 1, length);
          r = CaptureEngine.read64_usb(0, returnString, 1, 250);
          error = returnString.ToString(0, 4);
        }

        string op = returnString.ToString();
        string x = returnString.ToString(27, 10);
        string y = returnString.ToString(39, 10);
        string z = returnString.ToString(51, 10);

        double X = Convert.ToDouble(x) / 100;
        double Y = Convert.ToDouble(y) / 100;
        double Z = Convert.ToDouble(z) / 100;

        CIEXYZ xyz = new CIEXYZ(X, Y, Z);

        return xyz;
     
    }

    // to disconnect 
    public static int DisconnectCS200()
    {
      StringBuilder returnString = new StringBuilder(250);
      string cMeasure = "RMT,0\r\n";
      int length = cMeasure.Length;

      int r = CaptureEngine.write64_usb(0, cMeasure, 1, length);
      r = CaptureEngine.read64_usb(0, returnString, 1, 250);
      
      int i = CaptureEngine.end_usb(0);
      return i;
    }
   
  }
}
