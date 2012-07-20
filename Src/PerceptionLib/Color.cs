using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PerceptionLib
{

  public class Color : INotifyPropertyChanged
  {

    private double u, v,uR,vR;
    private double l, uP, vP;

    public double L
    {
      get { return l; }
      set
      {
        l = value;
        OnPropertyChanged("L");
      }
    }

    public double UP
    {
      get { return uP; }
      set
      {
        uP = value;
        OnPropertyChanged("UP");
      }
    }

    public double VP
    {
      get { return vP; }
      set
      {
        vP = value;
        OnPropertyChanged("VP");
      }
    }
    public double UR
    {
      get { return uR; }
      set
      {
        uR = value;
        OnPropertyChanged("UR");
      }
    }

    public double VR
    {
      get { return vR; }
      set
      {
        vR = value;
        OnPropertyChanged("VR");
      }
    }
    public double U
    {
      get { return u; }
      set
      {
        u = value;
        OnPropertyChanged("U");
      }
    }

    public double V
    {
      get { return v; }
      set
      {
        v = value;
        OnPropertyChanged("V");
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(String name)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(name));
    }

    public System.Windows.Media.Color ToRBG()
    {
      return System.Windows.Media.Colors.Black;
    }

    public static Color FromRGB(System.Windows.Media.Color cRGB)
    {
      Color rColor = new Color();
      CIEXYZ xyz = RGBToXYZ(cRGB);

      rColor.UP = (4 * xyz.X) / (xyz.X + (15 * xyz.Y) + (3 * xyz.Z));
      rColor.VP = (9 * xyz.Y) / (xyz.X + (15 * xyz.Y) + (3 * xyz.Z));
      
      rColor.UR = (4 * CIEXYZ.D65.X) / (CIEXYZ.D65.X + (15 * CIEXYZ.D65.Y) + (3 * CIEXYZ.D65.Z));
      rColor.VR = (9 * CIEXYZ.D65.Y) / (CIEXYZ.D65.X + (15 * CIEXYZ.D65.Y) + (3 * CIEXYZ.D65.Z));

      double yr = xyz.Y / CIEXYZ.D65.Y;
      rColor.L = Lxyz(yr);

      return rColor;
    }

    private static double Lxyz(double e)
    {
      return ((e > 0.008856) ? (116 * Math.Pow(e, (1.0 / 3.0))) - 16 : (903.3 * e));
    }

    private static CIEXYZ RGBToXYZ(System.Windows.Media.Color cRGB)
    {
      // by the formula given the the web page http://www.brucelindbloom.com/index.html [XYZ]=[M][RGB]
      //In order to properly use this matrix, the RGB values must be linear and in the nominal range [0.0, 1.0].
      // RGB values may first need conversion (for example, dividing by 255 and then raising them to a power)
      // Where M for D65:	 0.4124564  0.3575761  0.1804375
      //0.2126729  0.7151522  0.0721750
      //0.0193339  0.1191920  0.9503041

      // to make rgb values linear red, green, blue values
      double rLinear = (double)cRGB.R / 255.0;
      double gLinear = (double)cRGB.G / 255.0;
      double bLinear = (double)cRGB.B / 255.0;

      // convert to a sRGB form
      double r = (rLinear > 0.04045) ? Math.Pow((rLinear + 0.055) / (1 + 0.055), 2.2) : (rLinear / 12.92);
      double g = (gLinear > 0.04045) ? Math.Pow((gLinear + 0.055) / (1 + 0.055), 2.2) : (gLinear / 12.92);
      double b = (bLinear > 0.04045) ? Math.Pow((bLinear + 0.055) / (1 + 0.055), 2.2) : (bLinear / 12.92);

      // converts and returs as a struct
      return new CIEXYZ((r * 0.4124 + g * 0.3576 + b * 0.1805),
                         (r * 0.2126 + g * 0.7152 + b * 0.0722),
                         (r * 0.0193 + g * 0.1192 + b * 0.9505));
    }

    /// <summary>
    /// Structure to define CIE XYZ.
    /// </summary>
    public class CIEXYZ
    {
      /// <summary>
      /// Gets an empty CIEXYZ structure.
      /// </summary>
      public static readonly CIEXYZ Empty = new CIEXYZ(0, 0, 0);
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

      public CIEXYZ(double pX, double pY, double pZ)
      {
        X = x;
        Y = y;
        Z = z;
      }
    }

  }

}
