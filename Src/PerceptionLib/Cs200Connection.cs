//cs200connection
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PerceptionLib
{
  public class Cs200Connection
  {
    public static int caputerIndex = 1;
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


    public static CIEXYZ StartAVGMeasureXYZ3()
    {
      double x, x1, x2, x3, y, y1, y2, y3, z, z1, z2, z3, mux, muy, muz;

      CIEXYZ xyz1 = StartMeasureXYZ();
      CIEXYZ xyz2 = StartMeasureXYZ();
      CIEXYZ xyz3 = StartMeasureXYZ();

      x = (double)((xyz1.X * 100) + (xyz2.X * 100) + (xyz3.X * 100)) / 3.0;
      y = (double)((xyz1.Y * 100) + (xyz2.Y * 100) + (xyz3.Y * 100)) / 3.0;
      z = (double)((xyz1.Z * 100) + (xyz2.Z * 100) + (xyz3.Z * 100)) / 3.0;

      x1 = Math.Pow((xyz1.X - x), 2);
      x2 = Math.Pow((xyz2.X - x), 2);
      x3 = Math.Pow((xyz3.X - x), 2);

      y1 = Math.Pow((xyz1.Y - y), 2);
      y2 = Math.Pow((xyz2.Y - y), 2);
      y3 = Math.Pow((xyz3.Y - y), 2);

      z1 = Math.Pow((xyz1.Z - z), 2);
      z2 = Math.Pow((xyz2.Z - z), 2);
      z3 = Math.Pow((xyz3.Z - z), 2);

      mux = (x1 + x2 + x3) / 3;
      muy = (y1 + y2 + y3) / 3;
      muz = (z1 + z2 + z3) / 3;

      x = (double)((xyz1.X) + (xyz2.X) + (xyz3.X)) / 3.0;
      y = (double)((xyz1.Y) + (xyz2.Y) + (xyz3.Y)) / 3.0;
      z = (double)((xyz1.Z) + (xyz2.Z) + (xyz3.Z)) / 3.0;

      CIEXYZ xyz = new CIEXYZ(x, y, z);
      xyz.stdx = Math.Sqrt(mux);
      xyz.stdy = Math.Sqrt(muy);
      xyz.stdz = Math.Sqrt(muz);

      return xyz;
    }

    public static CIEXYZ StartAVGMeasureXYZ9()
    {
      double x, y, z;

      CIEXYZ xyz1 = StartAVGMeasureXYZ3();
      CIEXYZ xyz2 = StartAVGMeasureXYZ3();
      CIEXYZ xyz3 = StartAVGMeasureXYZ3();

      x = (double)((xyz1.X) + (xyz2.X) + (xyz3.X)) / 3.0;
      y = (double)((xyz1.Y) + (xyz2.Y) + (xyz3.Y)) / 3.0;
      z = (double)((xyz1.Z) + (xyz2.Z) + (xyz3.Z)) / 3.0;

      CIEXYZ xyz = new CIEXYZ(x, y, z);
      xyz.stdx = (double)((xyz1.stdx) + (xyz2.stdx) + (xyz3.stdx)) / 3.0;
      xyz.stdy = (double)((xyz1.stdy) + (xyz2.stdy) + (xyz3.stdy)) / 3.0;
      xyz.stdz = (double)((xyz1.stdz) + (xyz2.stdz) + (xyz3.stdz)) / 3.0;

      return xyz;
    }

    public static CIEXYZ StartAVGMeasureXYZ2()
    {
      double x, x1, x2, x3, y, y1, y2, y3, z, z1, z2, z3, mux, muy, muz;

      CIEXYZ xyz1 = StartMeasureXYZ();
      CIEXYZ xyz2 = StartMeasureXYZ();
      // CIEXYZ xyz3 = StartMeasureXYZ();

      x = (double)((xyz1.X * 100) + (xyz2.X * 100)) / 2.0;
      y = (double)((xyz1.Y * 100) + (xyz2.Y * 100)) / 2.0;
      z = (double)((xyz1.Z * 100) + (xyz2.Z * 100)) / 2.0;

      x1 = Math.Pow((xyz1.X - x), 2);
      x2 = Math.Pow((xyz2.X - x), 2);
      //x3 = Math.Pow((xyz3.X - x), 2);

      y1 = Math.Pow((xyz1.Y - y), 2);
      y2 = Math.Pow((xyz2.Y - y), 2);
      //y3 = Math.Pow((xyz3.Y - y), 2);

      z1 = Math.Pow((xyz1.Z - z), 2);
      z2 = Math.Pow((xyz2.Z - z), 2);
      //z3 = Math.Pow((xyz3.Z - z), 2);

      mux = (x1 + x2) / 2;
      muy = (y1 + y2) / 2;
      muz = (z1 + z2) / 2;

      x = (double)((xyz1.X) + (xyz2.X)) / 2.0;
      y = (double)((xyz1.Y) + (xyz2.Y)) / 2.0;
      z = (double)((xyz1.Z) + (xyz2.Z)) / 2.0;

      CIEXYZ xyz = new CIEXYZ(x, y, z);
      xyz.stdx = Math.Sqrt(mux);
      xyz.stdy = Math.Sqrt(muy);
      xyz.stdz = Math.Sqrt(muz);

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

      string waittime = returnString.ToString(5, 2);


      int t = Convert.ToInt32(waittime);
      t = t * 1000;
      t = t - 500;

      ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      //take the if out for any other than phone
      //
      
      System.Threading.Thread.Sleep(t);

      cMeasure = "MDR,3\r\n";
      length = cMeasure.Length;

      r = CaptureEngine.write64_usb(0, cMeasure, 1, length);
      r = CaptureEngine.read64_usb(0, returnString, 1, 250);

      string error = returnString.ToString(0, 4);

      if (error == "ER21")
      {
        CIEXYZ xyz = new CIEXYZ(0, 0, 0);
        xyz.stdx = t;
        xyz.stdy = 0;
        xyz.stdz = 0;
        return xyz;
      }

      while (error == "ER02")
      {
        t = t + 300;
        System.Threading.Thread.Sleep(300);
        cMeasure = "MDR,3\r\n";
        length = cMeasure.Length;

        r = CaptureEngine.write64_usb(0, cMeasure, 1, length);
        r = CaptureEngine.read64_usb(0, returnString, 1, 250);
        error = returnString.ToString(0, 4);
      }

      if (error == "ER21")
      {
        CIEXYZ xyz = new CIEXYZ(0, 0, 0);
        xyz.stdx = t;
        xyz.stdy = 0;
        xyz.stdz = 0;
        return xyz;
      }
      else
      {

        string op = returnString.ToString();
        string x = returnString.ToString(27, 10);
        string y = returnString.ToString(39, 10);
        string z = returnString.ToString(51, 10);

        double X = Convert.ToDouble(x) / 100;
        double Y = Convert.ToDouble(y) / 100;
        double Z = Convert.ToDouble(z) / 100;

        CIEXYZ xyz = new CIEXYZ(X, Y, Z);
        xyz.stdx = t;
        xyz.stdy = 0;
        xyz.stdz = 0;

        return xyz;
      }

    }
    public static CIEXYZ StartMeasureXYZForPhone()
    {
      int time1 = 500;
      if (caputerIndex != 1)
      {
        System.Threading.Thread.Sleep(2350);
      }
      caputerIndex = 0;
      StringBuilder returnString = new StringBuilder(250);
      //measure
      string cMeasure = "MES,1\r\n";
      int length = cMeasure.Length;

      int r = CaptureEngine.write64_usb(0, cMeasure, 1, length);
      r = CaptureEngine.read64_usb(0, returnString, 1, 250);

      string waittime = returnString.ToString(5, 2);

      CIEXYZ xyz = new CIEXYZ(0, 0, 0);
      int t = Convert.ToInt32(waittime);
      t = t * 1000;
      t = t - 500;
      int temp = t;
     

      ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      //take the if out for any other than phone
      //
      if (t < 800)
      {

        System.Threading.Thread.Sleep(t);

        cMeasure = "MDR,3\r\n";
        length = cMeasure.Length;

        r = CaptureEngine.write64_usb(0, cMeasure, 1, length);
        r = CaptureEngine.read64_usb(0, returnString, 1, 250);

        string error = returnString.ToString(0, 4);
                
          while (error == "ER02")
          {
            t = t + 300;
            if (t < 800)
            {
              //System.Threading.Thread.Sleep(300);
              cMeasure = "MDR,3\r\n";
              length = cMeasure.Length;

              r = CaptureEngine.write64_usb(0, cMeasure, 1, length);
              r = CaptureEngine.read64_usb(0, returnString, 1, 250);
              error = returnString.ToString(0, 4);
            }
            else
            {
              int tMinus = Math.Abs(2750);
              //System.Threading.Thread.Sleep(500);
              xyz = new CIEXYZ(0, 0, 0);
              xyz.stdx = t;
              xyz.stdy = 0;
              xyz.stdz = 0;
              break;
            }


          }

          if (t < 800)
          {
          string op = returnString.ToString();
          string x = returnString.ToString(27, 10);
          string y = returnString.ToString(39, 10);
          string z = returnString.ToString(51, 10);

          double X = Convert.ToDouble(x) / 100;
          double Y = Convert.ToDouble(y) / 100;
          double Z = Convert.ToDouble(z) / 100;

          xyz = new CIEXYZ(X, Y, Z);
          xyz.stdx = t;
          xyz.stdy = 1;
          xyz.stdz = 1;
          }
        
      }
      else
      {
        System.Threading.Thread.Sleep(500);

        xyz = new CIEXYZ(-1, -1, -1);
        xyz.stdx = 0;
        xyz.stdy = 0;
        xyz.stdz = 0;

      }
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














