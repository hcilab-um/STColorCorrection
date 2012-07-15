using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PerceptionLib
{


  public class Color : INotifyPropertyChanged
  {

      /// <summary>
      /// RGB structure.
      /// </summary>
      public struct RGB
      {
          /// <summary>
          /// Gets an empty RGB structure;
          /// </summary>
          public static readonly RGB Empty = new RGB();

          private int red;
          private int green;
          private int blue;

           /// <summary>
          /// Gets or sets red value.
          /// </summary>
          public int Red
          {
              get
              {
                  return red;
              }
              set
              {
                  red = (value > 255) ? 255 : ((value < 0) ? 0 : value);
              }
          }

          /// <summary>
          /// Gets or sets red value.
          /// </summary>
          public int Green
          {
              get
              {
                  return green;
              }
              set
              {
                  green = (value > 255) ? 255 : ((value < 0) ? 0 : value);
              }
          }

          /// <summary>
          /// Gets or sets red value.
          /// </summary>
          public int Blue
          {
              get
              {
                  return blue;
              }
              set
              {
                  blue = (value > 255) ? 255 : ((value < 0) ? 0 : value);
              }
          }

          public RGB(int R, int G, int B)
          {
              this.red = (R > 255) ? 255 : ((R < 0) ? 0 : R);
              this.green = (G > 255) ? 255 : ((G < 0) ? 0 : G);
              this.blue = (B > 255) ? 255 : ((B < 0) ? 0 : B);
          }

       }


      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      /// <summary>
      /// Structure to define CIE XYZ.
      /// </summary>
      public struct CIEXYZ
      {
          /// <summary>
          /// Gets an empty CIEXYZ structure.
          /// </summary>
          public static readonly CIEXYZ Empty = new CIEXYZ();
          /// <summary>
          /// Gets the CIE D65 lighting's white structure coordinates.
          /// </summary>
        //  public static readonly CIEXYZ D50 = new CIEXYZ(0.9505, 1.0, 1.0890);
          public static readonly CIEXYZ D65 = new CIEXYZ(0.3127, 0.3290, 0.3583);


          private double x;
          private double y;
          private double z;

          /// <summary>
          /// Gets or sets X component.
          /// </summary>
          public double X
          {
              get
              {
                  return this.x;
              }
              set
              {
                  this.x = (value > 0.3127) ? 0.3127 : ((value < 0) ? 0 : value);
              }
          }

          /// <summary>
          /// Gets or sets Y component.
          /// </summary>
          public double Y
          {
              get
              {
                  return this.y;
              }
              set
              {
                  this.y = (value > 0.3290) ? 0.3290 : ((value < 0) ? 0 : value);
              }
          }

          /// <summary>
          /// Gets or sets Z component.
          /// </summary>
          public double Z
          {
              get
              {
                  return this.z;
              }
              set
              {
                  this.z = (value > 0.3583) ? 0.3583 : ((value < 0) ? 0 : value);
              }
          }

          public CIEXYZ(double x, double y, double z)
          {
              this.x = (x > 0.9505) ? 0.9505 : ((x < 0) ? 0 : x);
              this.y = (y > 1.0) ? 1.0 : ((y < 0) ? 0 : y);
              this.z = (z > 1.089) ? 1.089 : ((z < 0) ? 0 : z);
          }

        

      }

     ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Structure to define L u' v'
    /// </summary>
      public struct CIElUV
      {
          /// <summary>
          /// Gets an empty CIEXYZ structure.
          /// </summary>
          public static readonly CIElUV Empty = new CIElUV();

          private double l;
          private double u;
          private double v;
          private double u1;
          private double v1;
          private double ur1;
          private double vr1;


        
          /// <summary>
          /// Gets or sets L component.
          /// </summary>
          public double L
          {
              get
              {
                  return this.l;
              }
              set
              {
                  this.l = value;
              }
          }

          /// <summary>
          /// Gets or sets u component.
          /// </summary>
          public double U
          {
              get
              {
                  return this.u;
              }
              set
              {
                  this.u = value;
              }
          }

          /// <summary>
          /// Gets or sets v component.
          /// </summary>
          public double V
          {
              get
              {
                  return this.v;
              }
              set
              {
                  this.v = value;
              }
          }

          /// <summary>
          /// Gets or sets u component.
          /// </summary>
          public double U1
          {
              get
              {
                  return this.u1;
              }
              set
              {
                  this.u1 = value;
              }
          }

          /// <summary>
          /// Gets or sets v component.
          /// </summary>
          public double V1
          {
              get
              {
                  return this.v1;
              }
              set
              {
                  this.v1 = value;
              }
          }
          public double Ur1
          {
              get
              {
                  return this.ur1;
              }
              set
              {
                  this.ur1 = value;
              }
          }

          /// <summary>
          /// Gets or sets v component.
          /// </summary>
          public double Vr1
          {
              get
              {
                  return this.vr1;
              }
              set
              {
                  this.vr1 = value;
              }
          }

          public CIElUV(double l, double u, double v, double u1, double v1, double ur1, double vr1)
          {
              this.l = l;
              this.u = u;
              this.v = v;
              this.u1 = u1;
              this.v1 = v1;
              this.ur1 = ur1;
              this.vr1 = vr1;
          }


          //private double l, u, v;
          //public double L
          //{
          //    get { return l; }
          //    set
          //    {
          //        l = value;
          //        OnPropertyChanged("L");
          //    }
          //}

          //public double U
          //{
          //    get { return u; }
          //    set
          //    {
          //        u = value;
          //        OnPropertyChanged("U");
          //    }
          //}

          //public double V
          //{
          //    get { return v; }
          //    set
          //    {
          //        v = value;
          //        OnPropertyChanged("V");
          //    }
          //}

          //public event PropertyChangedEventHandler PropertyChanged;
          //private void OnPropertyChanged(String name)
          //{
          //    if (PropertyChanged != null)
          //        PropertyChanged(this, new PropertyChangedEventArgs(name));
          //}

      }

      //public event PropertyChangedEventHandler PropertyChanged;
    //private void OnPropertyChanged(String name)
    //{
    //  if (PropertyChanged != null)
    //    PropertyChanged(this, new PropertyChangedEventArgs(name));
    //}

    //public System.Windows.Media.Color ToRBG()
    //{
    //  return System.Windows.Media.Colors.Black;
    //}


      /// <summary>
      /// Convertion from RGB to XYZ
      /// </summary>
      /// <param name="cRGB"></param>
      /// <returns></returns>
    public static CIEXYZ RGB_XYZ(int red, int green, int blue)
    {
        // by the formula given the the web page http://www.brucelindbloom.com/index.html [XYZ]=[M][RGB]
        //In order to properly use this matrix, the RGB values must be linear and in the nominal range [0.0, 1.0].
        // RGB values may first need conversion (for example, dividing by 255 and then raising them to a power)
        // Where M for D65:	 0.4124564  0.3575761  0.1804375
                             //0.2126729  0.7151522  0.0721750
                             //0.0193339  0.1191920  0.9503041

        // to make rgb values linear red, green, blue values
        double rLinear = (double)red / 255.0;
        double gLinear = (double)green / 255.0;
        double bLinear = (double)blue / 255.0;

        // convert to a sRGB form
        double r = (rLinear > 0.04045) ? Math.Pow((rLinear + 0.055) / (1 + 0.055), 2.2) : (rLinear / 12.92);
        double g = (gLinear > 0.04045) ? Math.Pow((gLinear + 0.055) / (1 + 0.055), 2.2) : (gLinear / 12.92);
        double b = (bLinear > 0.04045) ? Math.Pow((bLinear + 0.055) / (1 + 0.055), 2.2) : (bLinear / 12.92);
        
        
        // converts and returs as a struct
         return new CIEXYZ( (r*0.4124 + g*0.3576 + b*0.1805),
                            (r*0.2126 + g*0.7152 + b*0.0722),
                            (r*0.0193 + g*0.1192 + b*0.9505) );

        //CIEXYZ XYZ = CIEXYZ.Empty;
        //XYZ.X = (r * 0.4124 + g * 0.3576 + b * 0.1805);
        //XYZ.Y = (r * 0.2126 + g * 0.7152 + b * 0.0722);
        //XYZ.Z = (r * 0.0193 + g * 0.1192 + b * 0.9505);

        //return XYZ;


    }
     
      
      /// <summary>
      /// convertion from RGB to LUV via XYZ
      /// </summary>
      /// <param name="red"></param>
      /// <param name="green"></param>
      /// <param name="blue"></param>
      /// <returns></returns>

    public static CIElUV RGB_Luv(int red, int green, int blue,int White_Light)
    {
        CIEXYZ XYZ = CIEXYZ.Empty;
        XYZ = RGB_XYZ(red, green, blue);
        return XYZ_Luv(XYZ.X, XYZ.Y, XYZ.Z,White_Light);
    }




    private static double Lxyz(double e)
    {
        return ((e > 0.008856) ? (116*Math.Pow(e, (1.0 / 3.0)))-16 : (903.3*e));
    }

    public static CIElUV XYZ_Luv(double x, double y, double z, int White_Light)
    {
        double yr;
        
        CIElUV luv = CIElUV.Empty;
        luv.U1 = (4 * x) / (x + (15 * y) + (3 * z));
        luv.V1 = (9 * y) / (x + (15 * y) + (3 * z));

        yr = y / CIEXYZ.D65.Y;

        luv.Ur1 = (4 * CIEXYZ.D65.X) / (x + (15 * CIEXYZ.D65.Y) + (3 * CIEXYZ.D65.Z));
        luv.Vr1 = (9 * CIEXYZ.D65.Y) / (x + (15 * CIEXYZ.D65.Y) + (3 * CIEXYZ.D65.Z));

        luv.L = Lxyz(yr);
        luv.U = (13 * luv.L) / (luv.U1 - luv.Ur1);
        luv.V = (13 * luv.L) / (luv.V1 - luv.Vr1);

        return luv;
    }

  }

}
