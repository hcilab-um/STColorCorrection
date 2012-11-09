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
      // dynamic array to caluclate RGB values
   
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
    private double l, uP, vP,la,a,b;

    public double L
    {
      get { return l; }
      set
      {
        l = value;
        OnPropertyChanged("L");
      }
    }
    public double LA
    {
        get { return la; }
        set
        {
            la = value;
            OnPropertyChanged("LA");
        }
    }
    public double A
    {
        get { return a; }
        set
        {
            a = value;
            OnPropertyChanged("A");
        }
    }
    public double B
    {
        get { return b; }
        set
        {
            b = value;
            OnPropertyChanged("B");
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

    
      /// <summary>
      /// funtion to convert to luv
      /// </summary>
      /// <param name="cRGB"></param>
      /// <returns></returns>

    public static Color ToLUV(System.Drawing.Color cRGB)
    {
      Color rColor = new Color();
      Color rColorlab = new Color();
      CIEXYZ xyz = RGBToXYZ(cRGB);

      rColor.UP = (4 * xyz.X) / (xyz.X + (15 * xyz.Y) + (3 * xyz.Z));
      rColor.VP = (9 * xyz.Y) / (xyz.X + (15 * xyz.Y) + (3 * xyz.Z));

      //rColor.UR = (4 * CIEXYZ.D65.X) / (CIEXYZ.D65.X + (15 * CIEXYZ.D65.Y) + (3 * CIEXYZ.D65.Z));
      //rColor.VR = (9 * CIEXYZ.D65.Y) / (CIEXYZ.D65.X + (15 * CIEXYZ.D65.Y) + (3 * CIEXYZ.D65.Z));

      double yr = xyz.Y / CIEXYZ.D65.Y;
      rColor.L = Lxyz(yr);
      rColor.U = (13 * rColor.L) * (rColor.UP - rColor.UR);
      rColor.V = (13 * rColor.L) * (rColor.VP - rColor.VR);
      rColorlab = ToLAB(cRGB);
      rColor.LA = rColorlab.LA;
      rColor.A = rColorlab.A;
      rColor.B = rColorlab.B;
      return rColor;
    }
    private static double Lxyz(double e)
    {
        return ((e > 0.008856) ? (116 * Math.Pow(e, (1.0 / 3.0))) - 16 : (903.3 * e));
    }

      /// <summary>
      /// function to capture LAB from RGB
      /// </summary>
      /// <param name="cRGB"></param>
      /// <returns></returns>
   
    public static Color ToLAB(System.Drawing.Color cRGB)
    {
        double Fx, Fy, Fz;
        Color rColor = new Color();
        CIEXYZ xyz = RGBToXYZ(cRGB);

        double yr = xyz.Y / CIEXYZ.D65.Y;
        double xr = xyz.X / CIEXYZ.D65.X;
        double zr = xyz.Z / CIEXYZ.D65.Z;

        Fx = FX(xr);
        Fy = FX(yr);
        Fz = FX(zr);

        rColor.LA = Lxyz(yr);
        rColor.A = 500 * (Fx - Fy);
        rColor.B = 200 * (Fy - Fz);

        return rColor;
    }

    private static double FX(double e)
    {
        return ((e > 0.008856) ? (Math.Pow(e, (1.0 / 3.0))) : ((903.3 * e+16)/116));
    }
    

    
    public static CIEXYZ RGBToXYZ(System.Drawing.Color cRGB)
    {
      // by the formula given the the web page http://www.brucelindbloom.com/index.html [XYZ]=[M][RGB]
      //In order to properly use this matrix, the RGB values must be linear and in the nominal range [0.0, 1.0].
      // RGB values may first need conversion (for example, dividing by 255 and then raising them to a power)
      // Where M for D65:	 0.4124564  0.3575761  0.1804375
      //0.2126729  0.7151522  0.0721750
      //0.0193339  0.1191920  0.9503041

      //// to make rgb values linear red, green, blue values
      double rLinear = (double)cRGB.R / 255.0;
      double gLinear = (double)cRGB.G / 255.0;
      double bLinear = (double)cRGB.B / 255.0;

      // convert to a sRGB form

      double r =  Math.Pow((rLinear ), 2.2) ;
      double g =  Math.Pow((gLinear ), 2.2) ;
      double b = Math.Pow((bLinear ), 2.2) ;



      return new CIEXYZ((r * 0.4124564 + g * 0.3575761 + b * 0.1804375),
                         (r * 0.2126729 + g * 0.7151522 + b * 0.0721750),
                         (r * 0.0193339 + g * 0.1191920 + b * 0.9503041));
    }

    /// <summary>
    /// Function to go from LUV to RGB
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static RGBValue ToRBG(Color PassedLUV)
    {
        int tempr,tempg,tempb;
      CIEXYZ xyz = LUVToXYZ(PassedLUV);
      

      RGBValue rgb = new RGBValue();

      double[] Clinear = new double[3];
     
      Clinear[0] = xyz.X * 3.2404542 - xyz.Y * 1.5371385 - xyz.Z * 0.4985314; // red
      Clinear[1] = -xyz.X * 0.9692660 + xyz.Y * 1.8760108 + xyz.Z * 0.0415560; // green
      Clinear[2] = xyz.X * 0.0556434 - xyz.Y * 0.2040259 + xyz.Z * 1.0572252; // blue

      //gamma companding
      for (int i = 0; i < 3; i++)
      {

        Clinear[i] = Math.Pow(Clinear[i], (1.0 / 2.2));
      }          
        
        tempr=(int)Math.Round(Clinear[0] * 255.0);
        tempg = (int)Math.Round(Clinear[1] * 255.0);
        tempb = (int)Math.Round(Clinear[2] * 255.0);


      rgb.R = (byte)tempr;
      rgb.G = (byte)tempg;
      rgb.B = (byte)tempb;
      return rgb; 
    }
    
    /// <summary>
    /// Function to Convert LUV to XyZ
    /// </summary>
    /// <param name="PassedLUV"></param>
    /// <returns></returns>
    private static CIEXYZ LUVToXYZ(Color PassedLUV)
    {
     // by the formula given the the web page http://www.brucelindbloom.com/index.html 
      double x,y,z,a, b, c, d;

      y = GetY(PassedLUV.L);

      a = (double)(1.0 / 3.0) * ((52.0 * PassedLUV.L) / (PassedLUV.U + (13.0 * PassedLUV.L * PassedLUV.UR)) - 1.0);
      b = (double)-5.0 * y;
      c = (double)-1.0 / 3.0;
      d = (double)y * (((39.0 * PassedLUV.L) / (PassedLUV.V + (13.0 * PassedLUV.L * PassedLUV.VR))) - 5.0);

      x = (double)(d - b) / (double)(a - c);
      if (Double.IsNaN(x)) { x = 0.0; }

      z = (double)(x * a) + (double)b;
      if (Double.IsNaN(z)) { z = 0.0; }
    
      CIEXYZ xyz = new CIEXYZ(x,y,z);
             
      return xyz ;
    }

    // function to get y value
    private static double GetY(double l)
    {
      return (l > (0.008856 * 903.3)) ? Math.Pow(((l + 16.0) / 116.0),3.0) : (l/903.3);
    }
      
      
      
      /// <summary>
      /// function to find the Delta e the color difference
      /// </summary>
      /// <param name="c1"></param>
      /// <param name="c2"></param>
      /// <returns></returns>
    public double DistanceCal1(RGBValue c1 ,RGBValue  c2)
    {
        // converting color RGB rep to suit previously written TOLUV function
        System.Drawing.Color C1 = new System.Drawing.Color();
        C1 = System.Drawing.Color.FromArgb((int)c1.R, (int)c1.G, (int)c1.B);
        System.Drawing.Color C2 = new System.Drawing.Color();
        C2 = System.Drawing.Color.FromArgb((int)c2.R, (int)c2.G, (int)c2.B);
     
        Color Color1 = new Color();
        Color Color2 = new Color();
        
        //c1 & c2's LUV values
        Color1 = ToLUV(C1);
        Color2 = ToLUV(C2);
        
       double ΔEuv = Math.Pow((Math.Pow((Color1.L-Color2.L),2)+Math.Pow((Color1.U-Color2.U),2)+Math.Pow((Color1.V-Color2.V),2)),(1.0/2.0));
       return ΔEuv;
        //bool result;
        //if (ΔEuv>10)
        //    result=false;
        //else
        //    result=true;

        //return result;
    }
   
      
    /// <summary>
    /// function to cal ΔE
    /// </summary>
    /// <param name="Color1"></param>
    /// <param name="Color2"></param>
    /// <returns></returns>
    public static double ColorDistanceCal(Color Color1, Color Color2)
    {
        double l, u, v,result;
        l = Color1.L - Color2.L;
        u = Color1.U - Color2.U;
        v = Color1.V - Color2.V;
        l = l * l;
        u = u * u;
        v = v * v;
        result = l + u + v;

        double Euv = Math.Pow(result, (1.0 / 2.0));
       // double Euv = Math.Pow(Math.Pow((Color1.L - Color2.L), 2) + Math.Pow((Color1.U - Color2.U), 2) + Math.Pow((Color1.V - Color2.V), 2), 1 / 2);
        return Euv;
     }


    public RGBValue gammut(RGBValue cRGB)
    {
        // color 1
        RGBValue Color1 = new RGBValue();
        //color2
        RGBValue Color2 = new RGBValue();

        Color1.R = 0;
        Color1.G = 0;
        Color1.B = 0;

        List<RGBValue> PerceptuallyDiffRGBs = new List<RGBValue>();

        PerceptuallyDiffRGBs.Add(Color1);
        if (PerceptuallyDiffRGBs.Count>=1)
        { 

            Color2.R = (byte)(Color1.R+1);
            Color2.G = Color1.G;
            Color2.B = Color1.B;
        }


        

        return null;
    }
     

  }

}
