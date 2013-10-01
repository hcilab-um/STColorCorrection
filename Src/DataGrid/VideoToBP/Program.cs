using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Timers;
using System.Windows.Media.Media3D;
using QuickCorrection;
using GenericParsing;
using System.Data;
using PerceptionLib;

namespace VideoToBP
{
  class Program
  {

    private const String PATH_TO_VIDEO = @"\..\..\videoBP-clawmachine.mp4";
    private const String PATH_TO_BIN_PROFILE = @"C:\Users\jhincapie\Desktop\Projects\STColorCorrection\Data\PROFILE\p3700.csv";

    private static Matrix3D navigationMatrix = Matrix3D.Identity;
    private static Bin[, ,] displayProfile = null;

    private static Capture capture = null;

    static void Main(string[] args)
    {
      navigationMatrix = new Matrix3D();
      navigationMatrix.Translate(new Vector3D(0, 100, 110));
      navigationMatrix.Scale(new Vector3D((double)1 / 5, (double)1 / 5, (double)1 / 5));

      displayProfile = new Bin[Bin.RANGEL, Bin.RANGEA, Bin.RANGEB];
      for (int l = 0; l < Bin.RANGEL; l++)
        for (int a = 0; a < Bin.RANGEA; a++)
          for (int b = 0; b < Bin.RANGEB; b++)
            displayProfile[l, a, b] = new Bin(l, a, b);

      PopulateProfile(displayProfile, navigationMatrix);

      String path = Environment.CurrentDirectory + PATH_TO_VIDEO;
      if (!System.IO.File.Exists(path))
        return;


      //Opens the movie file
      capture = new Capture(path);
      double fps = capture.GetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FPS);

      //Reads frame by frame
      Timer timer = new Timer(1000 / fps);
      timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
      timer.Start();

      Console.Read();
    }

    private static void PopulateProfile(Bin[, ,] dProfile, Matrix3D navigationMatrix)
    {
      DataTable dataFile = new DataTable();
      try
      {
        // add the csv bin file
        using (GenericParserAdapter parser = new GenericParserAdapter(PATH_TO_BIN_PROFILE))
        {
          System.Data.DataSet dsResult = parser.GetDataSet();
          dataFile = dsResult.Tables[0];
        }
      }
      catch
      { }

      for (int i = 1; i < dataFile.Rows.Count; i++)
      {
        //lab vale as got form profile index
        Point3D labBin = new Point3D();
        labBin.X = Convert.ToDouble(dataFile.Rows[i][0].ToString());
        labBin.Y = Convert.ToDouble(dataFile.Rows[i][1].ToString());
        labBin.Z = Convert.ToDouble(dataFile.Rows[i][2].ToString());

        //trasfered points
        Point3D labCoordinate = navigationMatrix.Transform(labBin);

        //gets the bin to fill up
        Bin actualBin = GetProfileBin(dProfile, labCoordinate);

        //bin RGB Value
        actualBin.binRGB.X = Convert.ToByte(dataFile.Rows[i][9].ToString());
        actualBin.binRGB.Y = Convert.ToByte(dataFile.Rows[i][10].ToString());
        actualBin.binRGB.Z = Convert.ToByte(dataFile.Rows[i][11].ToString());

        //Measure Lab Values
        actualBin.measuredLAB.X = Convert.ToDouble(dataFile.Rows[i][3].ToString());
        actualBin.measuredLAB.Y = Convert.ToDouble(dataFile.Rows[i][4].ToString());
        actualBin.measuredLAB.Z = Convert.ToDouble(dataFile.Rows[i][5].ToString());

        //measured XYZ Values
        actualBin.measuredXYZ.X = Convert.ToDouble(dataFile.Rows[i][6].ToString());
        actualBin.measuredXYZ.Y = Convert.ToDouble(dataFile.Rows[i][7].ToString());
        actualBin.measuredXYZ.Z = Convert.ToDouble(dataFile.Rows[i][8].ToString());

        //is empty check
        actualBin.isEmpty = false;
      }
    }

    private static Bin outOfBounds = new Bin(-1, -1, -1);
    private static Bin GetProfileBin(Bin[, ,] profile, Point3D coordinates, Location location = Location.Top)
    {
      if (coordinates.X < 0 || coordinates.X >= Bin.RANGEL)
        return outOfBounds;
      if (coordinates.Y < 0 || coordinates.Y >= Bin.RANGEA)
        return outOfBounds;
      if (coordinates.Z < 0 || coordinates.Z >= Bin.RANGEB)
        return outOfBounds;

      Bin returnBin = profile[(int)coordinates.X, (int)coordinates.Y, (int)coordinates.Z];
      returnBin.location = location;
      returnBin.isMoreAccurateThanOrigin = false;
      return returnBin;
    }

    static void timer_Elapsed(object sender, ElapsedEventArgs e)
    {
      Image<Bgr, Byte> frame = capture.QueryFrame();
      Image<Bgr, Byte> clone;
      if (frame == null)
        (sender as Timer).Enabled = false;
      else
      {
        frame = frame.Resize(0.5, Emgu.CV.CvEnum.INTER.CV_INTER_LANCZOS4);
        clone = frame.Clone();

        ApplyBinProfileColors(clone);

        CvInvoke.cvShowImage("MOVIE", frame);
        CvInvoke.cvShowImage("RECOLOR", clone);
        CvInvoke.cvWaitKey(1);
      }
    }

    private static void ApplyBinProfileColors(Image<Bgr, byte> frame)
    {
      for (int col = 0; col < frame.Width; col++)
        for (int row = 0; row < frame.Height; row++)
        {
          Bin fgBin = GetBinForForeground(frame[row, col]);
          frame[row, col] = new Bgr(fgBin.binRGB.Z, fgBin.binRGB.Y, fgBin.binRGB.X);
        }
    }

    private static Bin GetBinForForeground(Bgr foregroundBGR)
    {
      double foregroundX, foregroundY, foregroundZ;
      #region Convert Foreground to XYZ
      // by the formula given the the web page http://www.brucelindbloom.com/index.html [XYZ]=[M][RGB]
      //In order to properly use this matrix, the RGB values must be linear and in the nominal range [0.0, 1.0].
      // RGB values may first need conversion (for example, dividing by 255 and then raising them to a power)
      // Where M for D65:	 0.4124564  0.3575761  0.1804375
      //0.2126729  0.7151522  0.0721750
      //0.0193339  0.1191920  0.9503041

      //// to make rgb values linear red, green, blue values
      double rLinear = (double)foregroundBGR.Red / 255.0;
      double gLinear = (double)foregroundBGR.Green / 255.0;
      double bLinear = (double)foregroundBGR.Blue / 255.0;

      // convert to a sRGB form
      double sR, sG, sB;

      if (rLinear > 0.04045)
        sR = Math.Pow(((rLinear + 0.055) / 1.055), 2.2);
      else
        sR = rLinear / 12.92;

      if (gLinear > 0.04045)
        sG = Math.Pow(((gLinear + 0.055) / 1.055), 2.2);
      else
        sG = gLinear / 12.92;

      if (bLinear > 0.04045)
        sB = Math.Pow(((bLinear + 0.055) / 1.055), 2.2);
      else
        sB = bLinear / 12.92;


      foregroundX = (sR * 0.4124564 + sG * 0.3575761 + sB * 0.1804375);
      foregroundY = (sR * 0.2126729 + sG * 0.7151522 + sB * 0.0721750);
      foregroundZ = (sR * 0.0193339 + sG * 0.1191920 + sB * 0.9503041);
      #endregion

      double foregroundL, foregroundA, foregroundB;
      #region Converts Foreground to LAB
      double Fx, Fy, Fz;

      double xr = foregroundX / CIEXYZ.D65.X;
      double yr = foregroundY / CIEXYZ.D65.Y;
      double zr = foregroundZ / CIEXYZ.D65.Z;

      Fx = PerceptionLib.Color.FX(xr);
      Fy = PerceptionLib.Color.FX(yr);
      Fz = PerceptionLib.Color.FX(zr);

      foregroundL = PerceptionLib.Color.Lxyz(Fy);
      foregroundA = 500 * (Fx - Fy);
      foregroundB = 200 * (Fy - Fz);
      #endregion

      int binL, binA, binB;
      #region Find Foreground Bin -- Receives the color's LAB and sets the limits
      binL = ((int)(Math.Round(foregroundL / 5.0)) * 5);
      binA = ((int)(Math.Round(foregroundA / 5.0)) * 5);
      binB = ((int)(Math.Round(foregroundB / 5.0)) * 5);
      if (binL < 0)
        binL = 0;
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
      #endregion

      return GetProfileBin(displayProfile, navigationMatrix.Transform(new Point3D(binL, binA, binB)));
    }

  }
}
