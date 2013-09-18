using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cudafy;
using Cudafy.Host;
using Cudafy.Types;
using Cudafy.Translator;

using System.Data;
using GenericParsing;

namespace CudafyByExample
{

  class bf
  {

    public const int ImagreHeight = 800;

    public const int ImagreWidth = 600;

    public const int ImagreDimention = ImagreHeight * ImagreWidth;

    [Cudafy]
    public const double TRUE = 1.0;
    [Cudafy]
    public const double FALSE = -1.0;

    /// <summary>
    /// This struct is only used to return all the data to the testing 
    /// algorithm. 
    /// </summary>
    [Cudafy]
    public struct TestingStructure
    {
        public double Given_R;
        public double Given_G;
        public double Given_B;
        public double distance;
        public double execution_time;
    }


    [Cudafy]
    public struct ProfileStrucuture
    {
      public double L ;
      public double A ;
      public double B ;
      //public double Given_R;
      //public double Given_G;
      //public double Given_B;
      public double ML ;
      public double MA ;
      public double MB ;
      public double MX ;
      public double MY ;
      public double MZ ;
      public double isempty;
      public double isMoreAccurateThanOrigin;     
                
    }

    [Cudafy]
    public struct BackGroundStrucuture
    {
      public double X;
      public double Y;
      public double Z;
      
    }

    [Cudafy]
    public struct ForeGroundStrucuture
    {
      public byte R;
      public byte G;
      public byte B;

    }

    public const int LAxis = 21;
    public const int AAxis = 41;
    public const int BAxis = 45;


    [Cudafy]
    public static double FX(double e)
    {
      if (e > 0.008856)
        e = Math.Pow(e,(1.0/3.0));
      
      else
        e = ((903.3 * e) + 16) / 116;

      return e;
    }

    [Cudafy]
    public static double Lxyz(double e)
    {
      if (e > 0.008856)
        e = (116 * Math.Pow(e, 0.333333333333333)) - 16;
	  else
	  e= (double)(903.3 * e);
	  return e;
    }
    
    [Cudafy]
    public static BackGroundStrucuture RGBToXYZ_St(ForeGroundStrucuture RGB )
    {
      // by the formula given the the web page http://www.brucelindbloom.com/index.html [XYZ]=[M][RGB]
      //In order to properly use this matrix, the RGB values must be linear and in the nominal range [0.0, 1.0].
      // RGB values may first need conversion (for example, dividing by 255 and then raising them to a power)
      // Where M for D65:	 0.4124564  0.3575761  0.1804375
      //0.2126729  0.7151522  0.0721750
      //0.0193339  0.1191920  0.9503041

        //// to make rgb values linear red, green, blue values
        BackGroundStrucuture XYZ = new BackGroundStrucuture();
        double rLinear = (double)(RGB.R / 255.0);
        double gLinear = (double)(RGB.G / 255.0);
        double bLinear = (double)(RGB.B / 255.0);

        // convert to a sRGB form

        //double r =  Math.pow((rLinear ), 2.2) ;
        //double g =  Math.pow((gLinear ), 2.2) ;
        //double b = Math.pow((bLinear ), 2.2) ;
        double r, g, b;

        if (rLinear > 0.04045)
        {
            r = (double)((double)(rLinear + 0.055) / 1.055);
            r = Math.Pow(r, (double)2.200000);
        }
        else
            r = (double)(rLinear / 12.92);

        if (gLinear > 0.04045)
        {
            g = (double)((double)(gLinear + 0.055) / 1.055);
            g = Math.Pow((double)g, (double)2.2);
        }
        else
            g = (double)(gLinear / 12.92);

        if (bLinear > 0.04045)
        {
            b = (double)((double)(bLinear + 0.055) / 1.055);
            b = Math.Pow((double)b, (double)2.2);
        }
        else
            b = (double)(bLinear / 12.92);


        XYZ.X = (double)(r * 0.4124564 + g * 0.3575761 + b * 0.1804375);
        XYZ.Y = (double)(r * 0.2126729 + g * 0.7151522 + b * 0.0721750);
        XYZ.Z = (double)(r * 0.0193339 + g * 0.1191920 + b * 0.9503041);

        if (XYZ.X > 0.9504)
            XYZ.X = 0.9504F;
        else if (XYZ.X < 0)
            XYZ.X = 0;
        else
            XYZ.X = XYZ.X;

        if (XYZ.Y > 1)
            XYZ.Y = 1;
        else if (XYZ.Y < 0)
            XYZ.Y = 0;
        else
            XYZ.Y = XYZ.Y;

        if (XYZ.Z > 1.0888)
            XYZ.Z = 1.0888F;
        else if (XYZ.Z < 0)
            XYZ.Z = 0;
        else
            XYZ.Z = XYZ.Z;

        return XYZ;

    }
    
    [Cudafy]
     public static ProfileStrucuture ToLAB(ForeGroundStrucuture cRGB)
    {
      double Fx, Fy, Fz;
      
      BackGroundStrucuture xyz = RGBToXYZ_St(cRGB);

      double xr = (double)(xyz.X / 0.9504f);
      double yr = (double)(xyz.Y / 1.0000f);
      double zr = (double)(xyz.Z / 1.0888f);

      if (xr > 0.008856)
          xr = Math.Pow(xr, (1.0 / 3.0));

      else
          xr = ((903.3 * xr) + 16) / 116;

      if (yr > 0.008856)
          yr = Math.Pow(yr, (1.0 / 3.0));

      else
          yr = ((903.3 * yr) + 16) / 116;


      if (zr > 0.008856)
          zr = Math.Pow(zr, (1.0 / 3.0));

      else
          zr = ((903.3 * zr) + 16) / 116;

      Fx = xr;
      Fy = yr;
      Fz = zr;

      ProfileStrucuture rColor= new ProfileStrucuture();

      if (yr > 0.008856)
          yr = (116 * Math.Pow(yr, 0.333333333333333)) - 16;
      else
          yr = (double)(903.3 * yr);
      rColor.L = yr;

      rColor.A = 500 * (Fx - Fy);
      rColor.B = 200 * (Fy - Fz);

      return rColor;
    }

    [Cudafy]
    public static ProfileStrucuture XYZtoLAB_st(BackGroundStrucuture xyz)
    {
        double Fx, Fy, Fz;

        double xr = (double)(xyz.X / 0.9504f);
        double yr = (double)(xyz.Y / 1.0000f);
        double zr = (double)(xyz.Z / 1.0888f);

        if (xr > 0.008856)
            xr = Math.Pow(xr, (1.0 / 3.0));

        else
            xr = ((903.3 * xr) + 16) / 116;

        if (yr > 0.008856)
            yr = Math.Pow(yr, (1.0 / 3.0));

        else
            yr = ((903.3 * yr) + 16) / 116;


        if (zr > 0.008856)
            zr = Math.Pow(zr, (1.0 / 3.0));

        else
            zr = ((903.3 * zr) + 16) / 116;

        Fx = xr;
        Fy = yr;
        Fz = zr;

        ProfileStrucuture rColor = new ProfileStrucuture();

        if (yr > 0.008856)
            yr = (116 * Math.Pow(yr, 0.333333333333333)) - 16;
        else
            yr = (double)(903.3 * yr);
        rColor.L = yr;

        rColor.A = 500 * (Fx - Fy);
        rColor.B = 200 * (Fy - Fz);

        return rColor;
    }

    //[Cudafy]
    //private static ProfileStrucuture GetProfileBin( int binL, int binA, int binB, ProfileStrucuture[ , , ] profile_GPU1)
    //{
    //  ProfileStrucuture outOfBounds = new ProfileStrucuture();
    //  outOfBounds.L = -1;
    //  outOfBounds.A = -1;
    //  outOfBounds.B = -1;

    //  outOfBounds.isempty = 0;
    //  if (binL < 0 ||binL >= 21)
    //    return outOfBounds;
    //  if (binA < 0 || binA >= 41)
    //    return outOfBounds;
    //  if (binB < 0 || binB >= 45)
    //    return outOfBounds;

    //  ProfileStrucuture returnBin = new ProfileStrucuture();

        
    //   returnBin = profile_GPU1[binL, binA, binB];

    //  return returnBin;
    //}

    // constant memory
    //private static ProfileStrucuture GetProfileBin(int binL, int binA, int binB)
    //{
    //  ProfileStrucuture outOfBounds = new ProfileStrucuture();
    //  outOfBounds.L = -1;
    //  outOfBounds.A = -1;
    //  outOfBounds.B = -1;

    //  outOfBounds.isempty = 0;
    //  if (binL < 0 || binL >= 21)
    //    return outOfBounds;
    //  if (binA < 0 || binA >= 41)
    //    return outOfBounds;
    //  if (binB < 0 || binB >= 45)
    //    return outOfBounds;

    //  ProfileStrucuture returnBin = new ProfileStrucuture();


    //  returnBin = profile_GPU[binL, binA, binB];

    //  return returnBin;
    //}

    [Cudafy]
    private static int multipleOf5(double value)
    {
      // x.y = Round (x /5 + y ) * 5

      double real_part = Math.Truncate(value);
      double decimal_part = ((value - (int)value));

      return (int)Math.Round(real_part / 5.0 + decimal_part) * 5;
    }

    //[Cudafy]
    //private static ProfileStrucuture FindForegroundBin(ForeGroundStrucuture foregorungRGB_GPU, ProfileStrucuture[, ,] profile_GPU)
    //{
    //  ProfileStrucuture foregroundLAB = ToLAB( foregorungRGB_GPU);

    //  int binL = ((int)Math.Round(foregroundLAB.L / 5.0)) * 5;
    //  int binA = ((int)Math.Round(foregroundLAB.A / 5.0)) * 5;
    //  int binB = ((int)Math.Round(foregroundLAB.B / 5.0)) * 5;

    //  if (binL < 0)
    //    binL = 0;
    //  if (binL > 100)
    //    binL = 100;
    //  if (binA < -86.17385493791946)
    //    binA = -85;
    //  if (binA > 98.2448002875424)
    //    binA = 100;
    //  if (binB < -107.8619171648283)
    //    binB = -110;
    //  if (binB > 94.47705120353054)
    //    binB = 95;

    //  binL =(int)(binL * 0.2) + 0;
    //  binA =(int)(binA * 0.2) + 20;
    //  binB = (int)(binB * 0.2) + 22;

    //  ProfileStrucuture foregroundBin = GetProfileBin(binL, binA, binB, profile_GPU);

    //  return foregroundBin;
    //}

    // constant memory
    //private static ProfileStrucuture FindForegroundBin(ForeGroundStrucuture foregorungRGB_GPU)
    //{
    //  ProfileStrucuture foregroundLAB = ToLAB(foregorungRGB_GPU);

    //  int binL = ((int)Math.Round(foregroundLAB.L / 5.0)) * 5;
    //  int binA = ((int)Math.Round(foregroundLAB.A / 5.0)) * 5;
    //  int binB = ((int)Math.Round(foregroundLAB.B / 5.0)) * 5;

    //  if (binL < 0)
    //    binL = 0;
    //  if (binL > 100)
    //    binL = 100;
    //  if (binA < -86.17385493791946)
    //    binA = -85;
    //  if (binA > 98.2448002875424)
    //    binA = 100;
    //  if (binB < -107.8619171648283)
    //    binB = -110;
    //  if (binB > 94.47705120353054)
    //    binB = 95;

    //  binL = (int)(binL * 0.2) + 0;
    //  binA = (int)(binA * 0.2) + 20;
    //  binB = (int)(binB * 0.2) + 22;

    //  ProfileStrucuture foregroundBin = GetProfileBin(binL, binA, binB);

    //  return foregroundBin;
    //}

    [Cudafy]
    private static BackGroundStrucuture  addXYZ_st(double X1,double Y1,double Z1,BackGroundStrucuture XYZbg)
    {
      BackGroundStrucuture XYZ;
      XYZ.X = X1 + XYZbg.X;
      XYZ.Y = Y1 + XYZbg.Y;
      XYZ.Z = Z1 + XYZbg.Z;

      //if (XYZ.X > 0.9504)
      //  XYZ.X = 0.9504f;
      //else if (XYZ.X < 0)
      //  XYZ.X = 0;
      //else
      //  XYZ.X = XYZ.X;

      //if (XYZ.Y > 1)
      //  XYZ.Y = 1;
      //else if (XYZ.Y < 0)
      //  XYZ.Y = 0;
      //else
      //  XYZ.Y = XYZ.Y;

      //if (XYZ.Z > 1.0888)
      //  XYZ.Z = 1.0888f;
      //else if (XYZ.Z < 0)
      //  XYZ.Z = 0;
      //else
      //  XYZ.Z = XYZ.Z;

      return XYZ;	  
   }

      ///
      /// ORIGINAL BRUTE FORCE ALGORITHM!
      ///

    //[Cudafy]
    //public static void Bruteforce(GThread thread, ProfileStrucuture[, ,] profile_GPU, ForeGroundStrucuture[] foregorungRGB_GPU, BackGroundStrucuture[] BackgroundXYZ_GPU, ProfileStrucuture[] ptr)
    //{
    //    // map from threadIdx/BlockIdx to pixel position
    //    int x = thread.threadIdx.x + thread.blockIdx.x * thread.blockDim.x;
    //    int y = thread.threadIdx.y + thread.blockIdx.y * thread.blockDim.y;
    //    int offset = x + y * thread.blockDim.x * thread.gridDim.x;
    //    double ox = (x - 1.0 / 2.0);
    //    double oy = (y - 1.0 / 2.0);

    //    double closestColor = double.MaxValue;

    //    double diffL = 0.0;
    //    double diffA = 0.0;
    //    double diffB = 0.0;

    //    int BestL = 0;
    //    int BestA = 0;
    //    int BestB = 0;

    //    ProfileStrucuture returnBin = new ProfileStrucuture();
    //    ProfileStrucuture foregroundLAB = ToLAB(foregorungRGB_GPU[offset]);

    //    int binL = ((int)Math.Round(foregroundLAB.L / 5.0)) * 5;
    //    int binA = ((int)Math.Round(foregroundLAB.A / 5.0)) * 5;
    //    int binB = ((int)Math.Round(foregroundLAB.B / 5.0)) * 5;

    //    if (binL > 100)
    //        binL = 100;
    //    if (binA < -86.17385493791946)
    //        binA = -85;
    //    if (binA > 98.2448002875424)
    //        binA = 100;
    //    if (binB < -107.8619171648283)
    //        binB = -110;
    //    if (binB > 94.47705120353054)
    //        binB = 95;

    //    binL = (int)(binL * 0.2) + 0;
    //    binA = (int)(binA * 0.2) + 20;
    //    binB = (int)(binB * 0.2) + 22;


    //    returnBin.MX = profile_GPU[binL, binA, binB].MX;
    //    returnBin.MY = profile_GPU[binL, binA, binB].MY;
    //    returnBin.MZ = profile_GPU[binL, binA, binB].MZ;

    //    double closestBinDistance;
    //    for (int indexL = 0; indexL < 21; indexL++)
    //    {
    //        for (int indexA = 0; indexA < 41; indexA++)
    //        {
    //            for (int indexB = 0; indexB < 45; indexB++)
    //            {
    //                ProfileStrucuture actualBin = profile_GPU[indexL, indexA, indexB];
    //                if (actualBin.isempty != 1)
    //                    continue;
    //                BackGroundStrucuture PredictionXYZ = addXYZ_st(actualBin.MX, actualBin.MY, actualBin.MZ, BackgroundXYZ_GPU[offset]);

    //                ProfileStrucuture PredictionlAB = XYZtoLAB_st(PredictionXYZ);

    //                diffL = PredictionlAB.L - returnBin.ML;
    //                diffA = PredictionlAB.A - returnBin.MA;
    //                diffB = PredictionlAB.B - returnBin.MB;

    //                //diffL = PredictionXYZ.X - returnBin.MX;
    //                //diffA = PredictionXYZ.Y - returnBin.MY;
    //                //diffB = PredictionXYZ.Z - returnBin.MZ;

    //                diffL = diffL * diffL;
    //                diffA = diffA * diffA;
    //                diffB = diffB * diffB;
                    
    //                closestBinDistance = Math.Sqrt(diffL + diffA + diffB);

    //                if (closestBinDistance >= closestColor)
    //                    continue;
    //                else
    //                {
    //                    closestColor = closestBinDistance;
    //                    BestL = indexL;
    //                    BestA = indexA;
    //                    BestB = indexB;
    //                }

    //            }
    //        }
    //    }
    //    ProfileStrucuture ValueToReturn = profile_GPU[BestL, BestA, BestB];
    //    ptr[offset] = ValueToReturn;

    //}

    [Cudafy]
    public static void Bruteforce(GThread thread, ProfileStrucuture[, ,] profile_GPU, ForeGroundStrucuture[] foregorungRGB_GPU, BackGroundStrucuture[] BackgroundXYZ_GPU, TestingStructure[] ptr)
    {

        // map from threadIdx/BlockIdx to pixel position
        int x = thread.threadIdx.x + thread.blockIdx.x * thread.blockDim.x;
        int y = thread.threadIdx.y + thread.blockIdx.y * thread.blockDim.y;
        int offset = x + y * thread.blockDim.x * thread.gridDim.x;
        double ox = (x - 1.0 / 2.0);
        double oy = (y - 1.0 / 2.0);

        double closestColor = double.MaxValue;

        double diffL = 0.0;
        double diffA = 0.0;
        double diffB = 0.0;

        int BestL = 0;
        int BestA = 0;
        int BestB = 0;

        ProfileStrucuture returnBin = new ProfileStrucuture();
        ProfileStrucuture foregroundLAB = ToLAB(foregorungRGB_GPU[offset]);

        int binL = ((int)Math.Round(foregroundLAB.L / 5.0)) * 5;
        int binA = ((int)Math.Round(foregroundLAB.A / 5.0)) * 5;
        int binB = ((int)Math.Round(foregroundLAB.B / 5.0)) * 5;

        if (binL > 100)
            binL = 100;
        if (binA < -86.17385493791946)
            binA = -85;
        if (binA > 98.2448002875424)
            binA = 100;
        if (binB < -107.8619171648283)
            binB = -110;
        if (binB > 94.47705120353054)
            binB = 95;

        binL = (int)(binL * 0.2) + 0;
        binA = (int)(binA * 0.2) + 20;
        binB = (int)(binB * 0.2) + 22;


        returnBin.ML = profile_GPU[binL, binA, binB].ML;
        returnBin.MA = profile_GPU[binL, binA, binB].MA;
        returnBin.MB = profile_GPU[binL, binA, binB].MB;

        double closestBinDistance;
        for (int indexL = 0; indexL < 21; indexL++)
        {
            for (int indexA = 0; indexA < 41; indexA++)
            {
                for (int indexB = 0; indexB < 45; indexB++)
                {
                    ProfileStrucuture actualBin = profile_GPU[indexL, indexA, indexB];
                    if (actualBin.isempty == TRUE)
                        continue;
                    BackGroundStrucuture PredictionXYZ = addXYZ_st(actualBin.MX, actualBin.MY, actualBin.MZ, BackgroundXYZ_GPU[offset]);

                    ProfileStrucuture PredictionlAB = XYZtoLAB_st(PredictionXYZ);

                    diffL = PredictionlAB.L - returnBin.ML;
                    diffA = PredictionlAB.A - returnBin.MA;
                    diffB = PredictionlAB.B - returnBin.MB;

                    //diffL = PredictionXYZ.X - returnBin.MX;
                    //diffA = PredictionXYZ.Y - returnBin.MY;
                    //diffB = PredictionXYZ.Z - returnBin.MZ;

                    diffL = diffL * diffL;
                    diffA = diffA * diffA;
                    diffB = diffB * diffB;

                    closestBinDistance = Math.Sqrt(diffL + diffA + diffB);

                    if (closestBinDistance >= closestColor)
                        continue;
                    else
                    {
                        closestColor = closestBinDistance;
                        BestL = indexL;
                        BestA = indexA;
                        BestB = indexB;
                    }

                }
            }
        }
        ProfileStrucuture return_bin = profile_GPU[BestL, BestA, BestB];
        TestingStructure ValueToReturn = new TestingStructure();
        ValueToReturn.Given_R = return_bin.L;
        ValueToReturn.Given_G = return_bin.A;
        ValueToReturn.Given_B = return_bin.B;
        ValueToReturn.distance = closestColor;
        ptr[offset] = ValueToReturn;

    }

    //public static void Execute()
    //{

    //  //cuda intializer
    //  CudafyModule km = CudafyModule.TryDeserialize();
    //  if (km == null || !km.TryVerifyChecksums())
    //  {
    //   // km = CudafyTranslator.Cudafy((typeof(ForeGroundStrucuture)), (typeof(BackGroundStrucuture)), typeof(Color));
    //      km = CudafyTranslator.Cudafy(typeof(ProfileStrucuture),typeof(ForeGroundStrucuture), typeof(BackGroundStrucuture), typeof(Color));        
    //    km.TrySerialize();
    //  }

    //  CudafyTranslator.GenerateDebug = true;
    //  // cuda or emulator
    //  GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId);
    //  //GPGPU gpu = CudafyHost.GetDevice(eGPUType.Emulator);
    //  gpu.LoadModule(km);

    //  ProfileStrucuture[] distance_CPU = new ProfileStrucuture[786432];
      
    //  // allocate memory on the GPU for the bitmap (same size as ptr)
      

    //  DataTable profile = new DataTable();
    //  try
    //  {
    //    // add the csv bin file
    //    using (GenericParserAdapter parser = new GenericParserAdapter(@"C:\lev\STColorCorrection\Data\PROFILE\p3700.csv"))
    //    {
    //      System.Data.DataSet dsResult = parser.GetDataSet();
    //      profile = dsResult.Tables[0];
    //    }
    //  }
    //  catch(Exception ex)
    //  {Console.WriteLine(ex); }
      

    //  // allocate temp memory, initialize it, copy to constant memory on the GPU
    //  // L 0-21 A 0-41 B 0-45
   
    // ProfileStrucuture[ , , ] profiles_CPU = new ProfileStrucuture[21,41,45];
    // ForeGroundStrucuture[] foregorungRGB_CPU = new ForeGroundStrucuture[786432];
    // BackGroundStrucuture[] BackgroundXYZ_CPU = new BackGroundStrucuture[786432];
     
    // for (int indexL = 0; indexL < 21; indexL++)
    //  {
    //    for (int indexA = 0; indexA < 41; indexA++)
    //    {
    //      for (int indexB = 0; indexB < 45; indexB++)
    //      {
    //        profiles_CPU[indexL, indexA, indexB].L = indexL;
    //        profiles_CPU[indexL, indexA, indexB].A = indexA;
    //        profiles_CPU[indexL, indexA, indexB].B = indexB;
    //        //profiles_CPU[indexL, indexA, indexB].Given_R = 0;
    //        //profiles_CPU[indexL, indexA, indexB].Given_G = 0;
    //        //profiles_CPU[indexL, indexA, indexB].Given_B = 0;
    //        profiles_CPU[indexL, indexA, indexB].ML = 0;
    //        profiles_CPU[indexL, indexA, indexB].MA = 0;
    //        profiles_CPU[indexL, indexA, indexB].MB = 0;
    //        profiles_CPU[indexL, indexA, indexB].MX = 0;
    //        profiles_CPU[indexL, indexA, indexB].MY = 0;
    //        profiles_CPU[indexL, indexA, indexB].MZ = 0;
            
    //        profiles_CPU[indexL, indexA, indexB].isempty = -1;
    //        profiles_CPU[indexL, indexA, indexB].isMoreAccurateThanOrigin = -1;
    //      }
    //    }
    //   }
        
    //  int lvalue, avalue, bvalue;
    //  try
    //  {
    //  for (int i = 1; i < profile.Rows.Count; i++)
    //  {
    //      lvalue=Convert.ToInt32 (profile.Rows[i][0].ToString());
    //      avalue = Convert.ToInt32(profile.Rows[i][1].ToString());
    //      bvalue= Convert.ToInt32(profile.Rows[i][2].ToString());

    //      lvalue=(int)(lvalue*0.2);
    //      avalue=(int)(avalue*0.2)+20;
    //      bvalue=(int)(bvalue*0.2)+22;
            
    //      profiles_CPU[lvalue, avalue, bvalue].L = lvalue;
    //      profiles_CPU[lvalue, avalue, bvalue].A = avalue;
    //      profiles_CPU[lvalue, avalue, bvalue].B = bvalue;
          
    //      //profiles_CPU[lvalue, avalue, bvalue].Given_R = (double)Convert.ToByte(profile.Rows[i][9].ToString());
    //      //profiles_CPU[lvalue, avalue, bvalue].Given_G = (double)Convert.ToByte(profile.Rows[i][10].ToString());
    //      //profiles_CPU[lvalue, avalue, bvalue].Given_B = (double)Convert.ToByte(profile.Rows[i][11].ToString());

    //      profiles_CPU[lvalue, avalue, bvalue].ML = (double)Convert.ToDouble(profile.Rows[i][3].ToString());
    //      profiles_CPU[lvalue, avalue, bvalue].MA = (double)Convert.ToDouble(profile.Rows[i][4].ToString());
    //      profiles_CPU[lvalue, avalue, bvalue].MB = (double)Convert.ToDouble(profile.Rows[i][5].ToString());

    //      profiles_CPU[lvalue, avalue, bvalue].MX = (double)Convert.ToDouble(profile.Rows[i][6].ToString());
    //      profiles_CPU[lvalue, avalue, bvalue].MY = (double)Convert.ToDouble(profile.Rows[i][7].ToString());
    //      profiles_CPU[lvalue, avalue, bvalue].MZ = (double)Convert.ToDouble(profile.Rows[i][8].ToString());
            

    //      profiles_CPU[lvalue, avalue, bvalue].isempty = 1;
         
    //  }

    //  }
    //  catch (Exception ex)
    //  { Console.WriteLine(ex); }
      
    //  try
    //  {
    //  for (int i = 0; i < 786432; i++)
    //  {
    //    foregorungRGB_CPU[i].R = 1;
    //    foregorungRGB_CPU[i].G = 1;
    //    foregorungRGB_CPU[i].B = 255;

    //    BackgroundXYZ_CPU[i].X = 0.629866667F;
    //    BackgroundXYZ_CPU[i].Y = 0.653533333F;
    //    BackgroundXYZ_CPU[i].Z = 0.684966667F;
    //  }
    //  }
    //  catch (Exception ex)
    //  { Console.WriteLine(ex); }

    //  ProfileStrucuture[, ,] profile_GPU = gpu.CopyToDevice(profiles_CPU);

    //  for (int i = 0; i < 1; i++)
    //  {
    //      // capture the start time
    //      gpu.StartTimer();
    //      ForeGroundStrucuture[] foregorungRGB_GPU = gpu.CopyToDevice(foregorungRGB_CPU);
    //      BackGroundStrucuture[] BackgroundXYZ_GPU = gpu.CopyToDevice(BackgroundXYZ_CPU);
          

    //      //out put
    //      ProfileStrucuture[] distance_GPU = gpu.Allocate(distance_CPU);

    //      // generate a bitmap from our sphere data
    //      //Image size: 1024 x 768

    //      //dim3 grids = new dim3(1, 1);
    //      //dim3 threads = new dim3(1,1);

    //      dim3 grids = new dim3(1024 / 16, 768 / 16);
    //      dim3 threads = new dim3(16, 16);

    //      gpu.Launch(grids, threads, ((Action<GThread, ProfileStrucuture[, ,], ForeGroundStrucuture[], BackGroundStrucuture[], ProfileStrucuture[]>)Bruteforce), profile_GPU, foregorungRGB_GPU, BackgroundXYZ_GPU, distance_GPU);

    //      //gpu.Launch(grids, threads, ((Action<GThread, ForeGroundStrucuture[], BackGroundStrucuture[], double[]>)Bruteforce), foregorungRGB_GPU, BackgroundXYZ_GPU, distance_GPU);

    //      // copy our bitmap back from the GPU for display
    //      gpu.CopyFromDevice(distance_GPU, distance_CPU);


    //      // get stop time, and display the timing results
    //      double elapsedTime = gpu.StopTimer();
    //      Console.WriteLine("Time to generate: {0} ms", elapsedTime);
    //      gpu.Free(foregorungRGB_GPU);
    //      gpu.Free(BackgroundXYZ_GPU);
    //      gpu.Free(distance_GPU);
    //  }

      
    //  gpu.FreeAll();

    //  }

    public static TestingStructure[] CorrectColour(System.Drawing.Color rgb, double X, double Y, double Z)
    {

        //set these to constant if you want testing

        //rgb = System.Drawing.Color.FromArgb(65, 108, 20);
        //X = 0.613829950099918;
        //Y = 0.938638756488747;
        //Z = 1.08019833591292;



      //cuda intializer
      CudafyModule km = CudafyModule.TryDeserialize();
      if (km == null || !km.TryVerifyChecksums())
      {
       // km = CudafyTranslator.Cudafy((typeof(ForeGroundStrucuture)), (typeof(BackGroundStrucuture)), typeof(Color));
          km = CudafyTranslator.Cudafy(typeof(ProfileStrucuture),typeof(ForeGroundStrucuture), typeof(BackGroundStrucuture), typeof(TestingStructure), typeof(bf));        
        km.TrySerialize();
      }

      CudafyTranslator.GenerateDebug = true;
      // cuda or emulator
      GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId);
      //sGPGPU gpu = CudafyHost.GetDevice(eGPUType.Emulator);
      gpu.LoadModule(km);
      Console.WriteLine("Running brute force correction using {0}", gpu.GetDeviceProperties(false).Name);

      TestingStructure[] distance_CPU = new TestingStructure[1];
      
      // allocate memory on the GPU for the bitmap (same size as ptr)
      

      DataTable profile = new DataTable();
      try
      {
        // add the csv bin file
        using (GenericParserAdapter parser = new GenericParserAdapter(@"C:\lev\STColorCorrection\Data\PROFILE\p3700.csv"))
        {
          System.Data.DataSet dsResult = parser.GetDataSet();
          profile = dsResult.Tables[0];
        }
      }
      catch(Exception ex)
      {Console.WriteLine(ex); }
      

      // allocate temp memory, initialize it, copy to constant memory on the GPU
      // L 0-21 A 0-41 B 0-45
   
     ProfileStrucuture[ , , ] profiles_CPU = new ProfileStrucuture[21,41,45];
     ForeGroundStrucuture[] foregorungRGB_CPU = new ForeGroundStrucuture[1];
     BackGroundStrucuture[] BackgroundXYZ_CPU = new BackGroundStrucuture[1];
     
     for (int indexL = 0; indexL < 21; indexL++)
      {
        for (int indexA = 0; indexA < 41; indexA++)
        {
          for (int indexB = 0; indexB < 45; indexB++)
          {
            profiles_CPU[indexL, indexA, indexB].L = indexL;
            profiles_CPU[indexL, indexA, indexB].A = indexA;
            profiles_CPU[indexL, indexA, indexB].B = indexB;
            //profiles_CPU[indexL, indexA, indexB].Given_R = 0;
            //profiles_CPU[indexL, indexA, indexB].Given_G = 0;
            //profiles_CPU[indexL, indexA, indexB].Given_B = 0;
            profiles_CPU[indexL, indexA, indexB].ML = 0;
            profiles_CPU[indexL, indexA, indexB].MA = 0;
            profiles_CPU[indexL, indexA, indexB].MB = 0;
            profiles_CPU[indexL, indexA, indexB].MX = 0;
            profiles_CPU[indexL, indexA, indexB].MY = 0;
            profiles_CPU[indexL, indexA, indexB].MZ = 0;
            
            profiles_CPU[indexL, indexA, indexB].isempty = TRUE;
            profiles_CPU[indexL, indexA, indexB].isMoreAccurateThanOrigin = -1;
          }
        }
       }
        
      int lvalue, avalue, bvalue;
      try
      {
      for (int i = 1; i < profile.Rows.Count; i++)
      {
          lvalue=Convert.ToInt32 (profile.Rows[i][0].ToString());
          avalue = Convert.ToInt32(profile.Rows[i][1].ToString());
          bvalue= Convert.ToInt32(profile.Rows[i][2].ToString());

          lvalue=(int)(lvalue*0.2);
          avalue=(int)(avalue*0.2)+20;
          bvalue=(int)(bvalue*0.2)+22;
            
          profiles_CPU[lvalue, avalue, bvalue].L = lvalue;
          profiles_CPU[lvalue, avalue, bvalue].A = avalue;
          profiles_CPU[lvalue, avalue, bvalue].B = bvalue;
          
          //profiles_CPU[lvalue, avalue, bvalue].Given_R = (double)Convert.ToByte(profile.Rows[i][9].ToString());
          //profiles_CPU[lvalue, avalue, bvalue].Given_G = (double)Convert.ToByte(profile.Rows[i][10].ToString());
          //profiles_CPU[lvalue, avalue, bvalue].Given_B = (double)Convert.ToByte(profile.Rows[i][11].ToString());

          profiles_CPU[lvalue, avalue, bvalue].ML = (double)Convert.ToDouble(profile.Rows[i][3].ToString());
          profiles_CPU[lvalue, avalue, bvalue].MA = (double)Convert.ToDouble(profile.Rows[i][4].ToString());
          profiles_CPU[lvalue, avalue, bvalue].MB = (double)Convert.ToDouble(profile.Rows[i][5].ToString());

          profiles_CPU[lvalue, avalue, bvalue].MX = (double)Convert.ToDouble(profile.Rows[i][6].ToString());
          profiles_CPU[lvalue, avalue, bvalue].MY = (double)Convert.ToDouble(profile.Rows[i][7].ToString());
          profiles_CPU[lvalue, avalue, bvalue].MZ = (double)Convert.ToDouble(profile.Rows[i][8].ToString());
            

          profiles_CPU[lvalue, avalue, bvalue].isempty = FALSE;
         
      }

      }
      catch (Exception ex)
      { Console.WriteLine(ex); }

      //foreground and background image inicialization
      #region
      try
      {
          for (int i = 0; i < 1; i++)
          {
              foregorungRGB_CPU[i].R = rgb.R;
              foregorungRGB_CPU[i].G = rgb.G;
              foregorungRGB_CPU[i].B = rgb.B;

              BackgroundXYZ_CPU[i].X = X;
              BackgroundXYZ_CPU[i].Y = Y;
              BackgroundXYZ_CPU[i].Z = Z;
          }
      }
      catch (Exception ex)
      { Console.WriteLine(ex); }
      #endregion

      ProfileStrucuture[, ,] profile_GPU = gpu.CopyToDevice(profiles_CPU);


        // capture the start time
        gpu.StartTimer();
        ForeGroundStrucuture[] foregorungRGB_GPU = gpu.CopyToDevice(foregorungRGB_CPU);
        BackGroundStrucuture[] BackgroundXYZ_GPU = gpu.CopyToDevice(BackgroundXYZ_CPU);
          

        //out put
        TestingStructure[] distance_GPU = gpu.Allocate(distance_CPU);

        // generate a bitmap from our sphere data
        //Image size: 1024 x 768

        dim3 grids = new dim3(1, 1);
        dim3 threads = new dim3(1,1);

        //dim3 grids = new dim3(1024 / 16, 768 / 16);
        //dim3 threads = new dim3(16, 16);

        gpu.Launch(grids, threads, ((Action<GThread, ProfileStrucuture[, ,], ForeGroundStrucuture[], BackGroundStrucuture[], TestingStructure[]>)Bruteforce), profile_GPU, foregorungRGB_GPU, BackgroundXYZ_GPU, distance_GPU);

        //gpu.Launch(grids, threads, ((Action<GThread, ForeGroundStrucuture[], BackGroundStrucuture[], double[]>)Bruteforce), foregorungRGB_GPU, BackgroundXYZ_GPU, distance_GPU);

        // copy our bitmap back from the GPU for display
        gpu.CopyFromDevice(distance_GPU, distance_CPU);


        // get stop time, and display the timing results
        double elapsedTime = gpu.StopTimer();
        distance_CPU[0].execution_time = elapsedTime;
        Console.WriteLine("Time to generate: {0} ms", elapsedTime);

        gpu.Free(foregorungRGB_GPU);
        gpu.Free(BackgroundXYZ_GPU);
        gpu.Free(distance_GPU);
        gpu.FreeAll();
        return distance_CPU;
    }

    }
  }
  


