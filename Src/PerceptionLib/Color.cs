using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace PerceptionLib
{
  // class to get RBG 
  public class RGBValue
  {
    private byte r, g, b;

    public byte R
    {
      get
      {
        return this.r;
      }
      set
      {
        this.r = value;
      }
    }

    public byte G
    {
      get
      {
        return this.g;
      }
      set
      {
        this.g = value;
      }
    }

    public byte B
    {
      get
      {
        return this.b;
      }
      set
      {
        this.b = value;
      }
    }

  }

  /// <summary>
  /// to define CIE XYZ.
  /// </summary>
  public class CIEXYZ
  {

    /// <summary>
    /// Gets the CIE D65 lighting's white structure coordinates.
    /// </summary>
    //  public static readonly CIEXYZ D50 = new CIEXYZ(0.9505, 1.0, 1.0890);

    // this is wrong as pointed out by David as its for xyz rather than XYZ
    //public static readonly CIEXYZ D65 = new CIEXYZ(0.3127, 0.3290, 0.3583);

    // the new D65 XYZ as provided by David and also cross verified with wikipedia
    public static readonly CIEXYZ D65 = new CIEXYZ(0.9504, 1.0000, 1.0888);

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
        this.x = (value > 0.9504) ? 0.9504 : ((value < 0) ? 0 : value);
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
        this.y = (value > 1.0000) ? 1.0000 : ((value < 0) ? 0 : value);
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
        this.z = (value > 1.0888) ? 1.0888 : ((value < 0) ? 0 : value);
      }
    }

    public CIEXYZ(double pX, double pY, double pZ)
    {
      X = pX;
      Y = pY;
      Z = pZ;
    }
  }
 
  
  
  public class Color : INotifyPropertyChanged
  {

    private double u, v, uR, vR;
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
      get { return (4 * CIEXYZ.D65.X) / (CIEXYZ.D65.X + (15 * CIEXYZ.D65.Y) + (3 * CIEXYZ.D65.Z));  }
      
    }

    public double VR
    {
      get { return (9 * CIEXYZ.D65.Y) / (CIEXYZ.D65.X + (15 * CIEXYZ.D65.Y) + (3 * CIEXYZ.D65.Z)); }
  
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

    


    public static Color ToLUV(System.Drawing.Color cRGB)
    {
      Color rColor = new Color();
      CIEXYZ xyz = RGBToXYZ(cRGB);

      rColor.UP = (4 * xyz.X) / (xyz.X + (15 * xyz.Y) + (3 * xyz.Z));
      rColor.VP = (9 * xyz.Y) / (xyz.X + (15 * xyz.Y) + (3 * xyz.Z));

      //rColor.UR = (4 * CIEXYZ.D65.X) / (CIEXYZ.D65.X + (15 * CIEXYZ.D65.Y) + (3 * CIEXYZ.D65.Z));
      //rColor.VR = (9 * CIEXYZ.D65.Y) / (CIEXYZ.D65.X + (15 * CIEXYZ.D65.Y) + (3 * CIEXYZ.D65.Z));

      double yr = xyz.Y / CIEXYZ.D65.Y;
      rColor.L = Lxyz(yr);
      rColor.U = (13 * rColor.L) * (rColor.UP - rColor.UR);
      rColor.V = (13 * rColor.L) * (rColor.VP - rColor.VR);
      return rColor;
    }
    
    private static double Lxyz(double e)
    {
      return ((e > 0.008856) ? (116 * Math.Pow(e, (1.0 / 3.0))) - 16 : (903.3 * e));
    }

    private static CIEXYZ RGBToXYZ(System.Drawing.Color cRGB)
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

      return new CIEXYZ((r * 0.4124564 + g * 0.3575761 + b * 0.1804375),
                         (r * 0.2126729 + g * 0.7151522 + b * 0.0721750),
                         (r * 0.0193339 + g * 0.1191920 + b * 0.9503041));
    }


    /// <summary>
    /// Function to go from LUV to RGB
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public RGBValue ToRBG(Color PassedLUV)
    {
      CIEXYZ xyz = LUVToXYZ(PassedLUV);
      RGBValue rgb = new RGBValue();

      rgb.R = (byte)(xyz.X * 3.2404542 + (xyz.Y * (-0.9692660)) + xyz.Z * 0.0556434);
      rgb.G = (byte)((xyz.X * (-1.5371385)) + xyz.Y * 1.8760108 + (xyz.Z * -0.2040259));
      rgb.B = (byte)((xyz.X * (-0.4985314)) + xyz.Y * 0.0415560 + xyz.Z * 1.0572252);
      
      return rgb; 
    }
    
    /// <summary>
    /// Function to Convert LUV to XyZ
    /// </summary>
    /// <param name="PassedLUV"></param>
    /// <returns></returns>
    private static CIEXYZ LUVToXYZ(Color PassedLUV)
    {
      double x,y,z,a, b, c, d;

      y = GetY(PassedLUV.L);

      a = (1 / 3) * ((52 * PassedLUV.L) / (PassedLUV.U + (13 * PassedLUV.L * PassedLUV.UR)) - 1);
      b = -5 * y;
      c = -1 / 3;
      d = y * ((39 * PassedLUV.L) / (PassedLUV.V + (13 * PassedLUV.L * PassedLUV.VR)));

      x = (d - b) / (a - c);
      z = (x * a) + b;
      CIEXYZ xyz = new CIEXYZ(x,y,z);
                 
      return xyz ;
    }

    // function to get y value
    private static double GetY(double l)
    {
      return (l > (0.008856 * 903.3)) ? Math.Pow(((l + 16) / 116),3) : (l/903.3);
    }
     

  }

}
