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

    public int gmt=0;

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

  public class LABbin
  {
      public double L { get; set; }
      public double A { get; set; }
      public double B { get; set; }

  }

  public class RGBbin
  {
      public byte R { get; set; }
      public byte G { get; set; }
      public byte B { get; set; }

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
      /// <summary>
      /// same function as above but this does it from XYZ
      /// </summary>
      /// <param name="xyz"></param>
      /// <returns></returns>
    public static Color ToLUV(CIEXYZ xyz)
    {
        Color rColor = new Color();
        Color rColorlab = new Color();
        //CIEXYZ xyz = RGBToXYZ(cRGB);

        rColor.UP = (4 * xyz.X) / (xyz.X + (15 * xyz.Y) + (3 * xyz.Z));
        rColor.VP = (9 * xyz.Y) / (xyz.X + (15 * xyz.Y) + (3 * xyz.Z));

        //rColor.UR = (4 * CIEXYZ.D65.X) / (CIEXYZ.D65.X + (15 * CIEXYZ.D65.Y) + (3 * CIEXYZ.D65.Z));
        //rColor.VR = (9 * CIEXYZ.D65.Y) / (CIEXYZ.D65.X + (15 * CIEXYZ.D65.Y) + (3 * CIEXYZ.D65.Z));

        double yr = xyz.Y / CIEXYZ.D65.Y;
        rColor.L = Lxyz(yr);
        rColor.U = (13 * rColor.L) * (rColor.UP - rColor.UR);
        rColor.V = (13 * rColor.L) * (rColor.VP - rColor.VR);
        rColorlab = ToLAB(xyz);
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
  
    public static Color ToLAB(CIEXYZ xyz)
    {
        double Fx, Fy, Fz;
        Color rColor = new Color();
        //CIEXYZ xyz = RGBToXYZ(cRGB);

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

      //double r =  Math.Pow((rLinear ), 2.2) ;
      //double g =  Math.Pow((gLinear ), 2.2) ;
      //double b = Math.Pow((bLinear ), 2.2) ;
         double r,g,b;
   
        if ( rLinear > 0.04045 )
               r =  Math.Pow(((rLinear+ 0.055 ) / 1.055 ) , 2.2) ;
        else
                r=rLinear / 12.92;

        if (gLinear > 0.04045)
            g = Math.Pow(((gLinear + 0.055) / 1.055), 2.2);
        else
            g = gLinear / 12.92;

        if (bLinear > 0.04045)
            b = Math.Pow(((bLinear + 0.055) / 1.055), 2.2);
        else
            b = bLinear / 12.92;      
        

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
/// funtion for LAb To RGB
/// </summary>
/// <param name="PassedLab"></param>
/// <returns></returns>
    public static RGBValue ToRBGFromLAB(Color PassedLab)
    {
        int tempr, tempg, tempb;
        CIEXYZ xyz = LABToXYZ(PassedLab);


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

        tempr = (int)Math.Round(Clinear[0] * 255.0);
        tempg = (int)Math.Round(Clinear[1] * 255.0);
        tempb = (int)Math.Round(Clinear[2] * 255.0);


        rgb.R = (byte)tempr;
        rgb.G = (byte)tempg;
        rgb.B = (byte)tempb;
        return rgb;
    }
  
    public static RGBValue ToRBG(CIEXYZ xyz)
    {
        double tempr, tempg, tempb;
        //CIEXYZ xyz = LUVToXYZ(PassedLUV);


        RGBValue rgb = new RGBValue();

        double[] Clinear = new double[3];
        double[] linear = new double[3];

        Clinear[0] = (xyz.X) * 3.2404542 - (xyz.Y) * 1.5371385 - (xyz.Z) * 0.4985314; // red
        Clinear[1] = -(xyz.X) * 0.9692660 + (xyz.Y) * 1.8760108 + (xyz.Z) * 0.0415560; // green
        Clinear[2] = (xyz.X) * 0.0556434 - (xyz.Y) * 0.2040259 + (xyz.Z) * 1.0572252; // blue


        //  if ( var_R > 0.0031308 ) var_R = 1.055 * ( var_R ^ ( 1 / 2.4 ) ) - 0.055
        //else                     var_R = 12.92 * var_R
        //if ( var_G > 0.0031308 ) var_G = 1.055 * ( var_G ^ ( 1 / 2.4 ) ) - 0.055
        //else                     var_G = 12.92 * var_G
        //if ( var_B > 0.0031308 ) var_B = 1.055 * ( var_B ^ ( 1 / 2.4 ) ) - 0.055
        //else                     var_B = 12.92 * var_B



        if (Clinear[0] > 0.0031308)
            Clinear[0] = 1.055 * Math.Pow(Clinear[0], (1 / (2.2))) - 0.055;
        else
            Clinear[0] = 12.92 * Clinear[0];

        if (Clinear[1] > 0.0031308)
            Clinear[1] = 1.055 * Math.Pow(Clinear[1], (1 / (2.2))) - 0.055;
        else
            Clinear[1] = 12.92 * Clinear[1];
        if (Clinear[2] > 0.0031308)
            Clinear[2] = 1.055 * Math.Pow(Clinear[2], (1 / (2.2))) - 0.055;
        else
            Clinear[2] = 12.92 * Clinear[2];
         

        ////gamma companding
        //for (int i = 0; i < 3; i++)
        //{
        //    if (Clinear[i] <= 0.0031308)
        //    {
        //        linear[i] = Clinear[i] * 12.92;
        //    }

        //    else
        //    {
        //        linear[i] = (1.055 * Math.Pow(Clinear[i], (1.0 / 2.2))) - 0.055;
        //        //linear[i] = ( Math.Pow((Clinear[i]), (1.0 / 2.2)));
                
        //    }
        //}

        tempr = (Math.Round(Clinear[0] * 255));
        tempg = (Math.Round(Clinear[1] * 255));
        tempb = (Math.Round(Clinear[2] * 255));
        if (tempr > 255 || tempg > 255 || tempb > 255)
            rgb.gmt = 1;
          else
              rgb.gmt = 1;

        if (tempr > 255)
        {
            tempr = 255;
        }
        if (tempg > 255)
        {
            tempg = 255;
        }
        if (tempb > 255)
        {
            tempb = 255;
        }
        if (tempr < 0)
        {
            tempr = 0;
        }
        if (tempg < 0)
        {
            tempg = 0;
        }
        if (tempb < 0)
        {
            tempb = 0;
        }
        rgb.R = (byte)(tempr);
        rgb.G = (byte)(tempg);
        rgb.B = (byte)(tempb);
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
      /// Funtion to convert Lab To XYZ
      /// </summary>
      /// <param name="passedLAB"></param>
      /// <returns></returns>
    private static CIEXYZ LABToXYZ(Color passedLAB)
    {
        double X, Y, Z, xr, yr, zr, Xr, Yr, Zr, Fx, Fy, Fz;

        Fy = (double)((double)(passedLAB.L + 16)) / 116;

        Fx = (double)((double)(passedLAB.A / 500)) + Fy;

        Fz = (double) Fy-((double)(passedLAB.B / 200));

        xr = (Math.Pow(Fx, 3) > (0.008856)) ? Math.Pow(Fx, 3) : ((double)((double)(116 * Fx - 16) / 903.3));

        yr = (passedLAB.L > (0.008856 * 903.3)) ? Math.Pow(Fy, 3) : ((double)(passedLAB.L/ 903.3));

        zr = (Math.Pow(Fz, 3) > (0.008856)) ? Math.Pow(Fz, 3) : ((double)((double)(116 * Fz - 16) / 903.3));

       //for D65
        Xr = 0.9504;

        Yr = 1;

        Zr = 1.0888;

        X = (double)xr * Xr;
        Y = (double)yr * Yr;
        Z = (double)zr * Zr;

        CIEXYZ xyz = new CIEXYZ(X, Y, Z);

        return xyz;
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

    public static double ColorDistanceCalAB(Color Color1, Color Color2)
    {
        double l, u, v, result;
        l = Color1.L - Color2.L;
        u = Color1.A - Color2.A;
        v = Color1.B - Color2.B;
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
      /// <summary>
      /// funtion to get RGB data which are taken out from the LAB binning
      /// </summary>
      /// <returns></returns>
    public static List<RGBbin> RGBbinedData()
    {

        List<LABbin> bin = binning();
        int count = bin.Count();

        List<RGBbin> BinRGB = new List<RGBbin>();
       // byte[][] BinRGB = new byte[count][];
        
        RGBValue temp = new RGBValue();
        Color lab= new Color();

       // int binCount=0;

        for (int i = 0; i < count; i++)
        {
                      
                lab.L = bin[i].L;
                lab.A = bin[i].A;
                lab.B = bin[i].B;


                temp = ToRBGFromLAB(lab);

                if (temp.R < 0 || temp.R > 255 || temp.B < 0 || temp.B > 255 || temp.G < 0 || temp.G > 255)
                {
                    continue;
                }
                else
                {
                    //BinRGB[binCount].R = (byte)temp.R;
                    //BinRGB[binCount].G= (byte)temp.G;
                    //BinRGB[binCount].B= (byte)temp.B;
                    //binCount++;

                    BinRGB.Add(new RGBbin
                    {
                        R = (byte)temp.R,
                        G= (byte)temp.G,
                        B= (byte)temp.B

                    });
                }
                     
        }

        return BinRGB;

    }

      /// <summary>
      /// funtion to bin lab values in a bin unit of 5
      /// </summary>
      /// <returns></returns>
    public static List<LABbin> binning()
      {
          double l=0, a=-100, b=-100;
          int cnt = 0;

          //Color bin = new Color();
         // double[][] binedLabValues = new double[33641][];

          List<LABbin> binedLabValues = new List<LABbin>();

          for (l = 0; l < 101; l = l + 10)
          {
            
              if (l == 0 && a == -100 && b == -100)
              {
                  //bin.L = L;
                  //bin.A = a;
                  //bin.B = b;
                  binedLabValues.Add(new LABbin
                  {
                      L=l,A=a,B=b
                                      
                  });
                  
                  
                  
              }
              else
              {
                  for (a = -100; a < 101; a = a + 10)
                  {
                      for (b = -100; b < 101; b = b + 10)
                      {
                          //bin.L = L;
                          //bin.A = a;
                          //bin.B = b;
                          binedLabValues.Add(new LABbin
                          {
                              L = l,
                              A = a,
                              B = b

                          });
                      }
                  }

              }
          }

          return binedLabValues;
      }

      /// <summary>
      /// function to create Hexdecimal values from RGB
      /// </summary>
      /// <param name="cRGB"></param>
      /// <returns></returns>
    public String RGBtoHEX(System.Drawing.Color cRGB)
    {
        return ColorTranslator.FromHtml(String.Format("#{0:X2}{1:X2}{2:X2}", cRGB.R, cRGB.G, cRGB.B)).Name.Remove(0, 2);

    }
  }

}
