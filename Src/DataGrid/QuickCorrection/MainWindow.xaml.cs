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
using PerceptionLib;
using System.Windows.Threading;
using GenericParsing;
using System.Data;
using System.Windows.Media.Media3D;
using System.Diagnostics;

namespace QuickCorrection
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {

    public const int RANGEL = 21;
    public const int RANGEA = 41;
    public const int RANGEB = 45;

    public MainWindow()
    {
      InitializeComponent();
    }

    private DataTable profile = new DataTable();

    //1- Create the transformation matrix
    private Matrix3D navigationMatrix = new Matrix3D();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      navigationMatrix.Translate(new Vector3D(0, 100, 110));
      navigationMatrix.Scale(new Vector3D((double)1 / 5, (double)1 / 5, (double)1 / 5));

      //2- Load the profile in a three dimensional array
      Bin[, ,] p3700 = new Bin[RANGEL, RANGEA, RANGEB];
      for (int l = 0; l < RANGEL; l++)
        for (int a = 0; a < RANGEA; a++)
          for (int b = 0; b < RANGEB; b++)
            p3700[l, a, b] = new Bin(l, a, b);

      try
      {
        // add the csv bin file
        using (GenericParserAdapter parser = new GenericParserAdapter(@"C:\Users\jhincapie\Desktop\Projects\STColorCorrection\Data\PROFILE\p3700.csv"))
        {
          System.Data.DataSet dsResult = parser.GetDataSet();
          profile = dsResult.Tables[0];
        }
      }
      catch
      { }

      for (int i = 1; i < profile.Rows.Count; i++)
      {
        //lab vale as got form profile index
        Point3D labBin = new Point3D();
        labBin.X = Convert.ToDouble(profile.Rows[i][0].ToString());
        labBin.Y = Convert.ToDouble(profile.Rows[i][1].ToString());
        labBin.Z = Convert.ToDouble(profile.Rows[i][2].ToString());

        if (labBin.X == 65 && labBin.Y == 0 && labBin.Z == 0)
        {
          Console.WriteLine("a");
        }

        //trasfered points
        Point3D labCoordinate = navigationMatrix.Transform(labBin);

        //gets the bin to fill up
        Bin actualBin = GetProfileBin(p3700, labCoordinate);

        //bin RGB Value
        actualBin.binRGB.X = Convert.ToByte(profile.Rows[i][9].ToString());
        actualBin.binRGB.Y = Convert.ToByte(profile.Rows[i][10].ToString());
        actualBin.binRGB.Z = Convert.ToByte(profile.Rows[i][11].ToString());

        //Measure Lab Values
        actualBin.measuredLAB.X = Convert.ToDouble(profile.Rows[i][3].ToString());
        actualBin.measuredLAB.Y = Convert.ToDouble(profile.Rows[i][4].ToString());
        actualBin.measuredLAB.Z = Convert.ToDouble(profile.Rows[i][5].ToString());

        //measured XYZ Values
        actualBin.measuredXYZ.X = Convert.ToDouble(profile.Rows[i][6].ToString());
        actualBin.measuredXYZ.Y = Convert.ToDouble(profile.Rows[i][7].ToString());
        actualBin.measuredXYZ.Z = Convert.ToDouble(profile.Rows[i][8].ToString());

        //is empty check
        actualBin.isEmpty = false;
      }

      //3- Get the parameters: foreground and background
      System.Drawing.Color foreground = System.Drawing.Color.FromArgb(150, 150, 150);
      PerceptionLib.CIEXYZ background = new CIEXYZ(0.2146, 0.43125, 0.07595); //RGB: 0	199	0 - greenish

      //5- Calls the correction alrorithm
      try
      {
        Point3D origin = new Point3D(50, 0, 0);
        Vector3D step = new Vector3D(50, 100, 110);

        Stopwatch stop = new Stopwatch();
        stop.Start();
        Bin corretedColor = null;
        for (int i = 0; i < 640; i++)
          for (int j = 0; j < 480; j++)
            corretedColor = QuickCorrection(p3700, foreground, background, navigationMatrix.Transform(origin), navigationMatrix.Transform(step));
        stop.Stop();
        Console.WriteLine("Finished");
      }
      catch
      { Console.WriteLine(""); }
    }

    //actual algorithm
    private Bin QuickCorrection(Bin[, ,] profile, System.Drawing.Color foreground, CIEXYZ background, Point3D origin, Vector3D step)
    {
      //1- Converts the foreground to how the display shows it
      Bin foregroundBin = FindForegroundBin(profile, navigationMatrix, foreground);

      Bin originBin = GetProfileBin(profile, origin);
      CalculateCorrectionAccuracy(originBin, background, foregroundBin);

      Point3D top = new Point3D(origin.X + step.X, origin.Y, origin.Z);
      Point3D bottom = new Point3D(origin.X - step.X, origin.Y, origin.Z);
      Point3D left = new Point3D(origin.X, origin.Y - step.Y, origin.Z);
      Point3D right = new Point3D(origin.X, origin.Y + step.Y, origin.Z);
      Point3D forward = new Point3D(origin.X, origin.Y, origin.Z - step.Z);
      Point3D backward = new Point3D(origin.X, origin.Y, origin.Z + step.Z);

      List<Bin> samples = new List<Bin>();
      samples.Add(GetProfileBin(profile, top, Location.Top));
      samples.Add(GetProfileBin(profile, bottom, Location.Bottom));
      samples.Add(GetProfileBin(profile, left, Location.Left));
      samples.Add(GetProfileBin(profile, right, Location.Right));
      samples.Add(GetProfileBin(profile, forward, Location.Forward));
      samples.Add(GetProfileBin(profile, backward, Location.Backward));
      samples.ForEach(sample => CalculateCorrectionAccuracy(sample, background, foregroundBin));

      var closests = samples.Where(sample => sample.distance < originBin.distance);
      if (closests.Count() == 0)
      {
        if (step.X == 1 && step.Y == 1 && step.Z == 1)
          return originBin;

        if (step.X > 1)
          step.X = Math.Round(step.X / 2, MidpointRounding.AwayFromZero);
        if (step.Y > 1)
          step.Y = Math.Round(step.Y / 2, MidpointRounding.AwayFromZero);
        if (step.Z > 1)
          step.Z = Math.Round(step.Z / 2, MidpointRounding.AwayFromZero);
        return QuickCorrection(profile, foreground, background, origin, step);
      }
      else
      {
        //calculates weights
        double totalimprovements = 0;
        foreach (var sample in closests)
          totalimprovements += (originBin.distance - sample.distance);
        foreach (var sample in closests)
          sample.weight = (originBin.distance - sample.distance) / totalimprovements;

        //calculates displacement
        Vector3D displacement = new Vector3D(0, 0, 0);
        foreach (var sample in closests)
          displacement = displacement + (sample.binLAB - origin) * sample.weight;
        displacement.X = Math.Round(displacement.X, MidpointRounding.AwayFromZero);
        displacement.Y = Math.Round(displacement.Y, MidpointRounding.AwayFromZero);
        displacement.Z = Math.Round(displacement.Z, MidpointRounding.AwayFromZero);

        //pokes new origin
        Point3D newOriginLoc = origin + displacement;
        Bin newOrigin = GetProfileBin(profile, newOriginLoc);
        while (newOrigin.isEmpty)
        {
          displacement.X = Math.Round(displacement.X / 2, MidpointRounding.AwayFromZero);
          displacement.Y = Math.Round(displacement.Y / 2, MidpointRounding.AwayFromZero);
          displacement.Z = Math.Round(displacement.Z / 2, MidpointRounding.AwayFromZero);

          newOriginLoc = origin + displacement;
          newOrigin = GetProfileBin(profile, newOriginLoc);
        }

        return QuickCorrection(profile, foreground, background, newOriginLoc, step);
      }
    }

    private void CalculateCorrectionAccuracy(Bin actualBin, CIEXYZ background, Bin foregroundBin)
    {
      if (actualBin.isEmpty)
        return;

      Point3D predictionXYZ = AddXYZ(actualBin.measuredXYZ, new Point3D(background.X, background.Y, background.Z));
      PerceptionLib.Color predictionLAB = PerceptionLib.Color.ToLAB(new CIEXYZ(predictionXYZ.X, predictionXYZ.Y, predictionXYZ.Z));
      actualBin.distance = DistanceLAB(foregroundBin.measuredLAB, new Point3D(predictionLAB.L, predictionLAB.A, predictionLAB.B));
    }

    //to find fg bin
    private Bin FindForegroundBin(Bin[, ,] profile, Matrix3D navigationMatrix, System.Drawing.Color foregroundRGB)
    {
      PerceptionLib.Color foregroundLAB = PerceptionLib.Color.ToLAB(foregroundRGB);

      int binL = ((int)(Math.Round(foregroundLAB.LA / 5.0)) * 5);
      int binA = ((int)(Math.Round(foregroundLAB.A / 5.0)) * 5);
      int binB = ((int)(Math.Round(foregroundLAB.B / 5.0)) * 5);
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

      Bin foregroundBin = GetProfileBin(profile, navigationMatrix.Transform(new Point3D(binL, binA, binB)));

      return foregroundBin;
    }

    private Bin GetProfileBin(Bin[, ,] profile, Point3D coordinates, Location location = Location.Top)
    {
      Bin outOfBounds = new Bin(-1, -1, -1);
      outOfBounds.Location = location;

      if (coordinates.X < 0 || coordinates.X >= RANGEL)
        return outOfBounds;
      if (coordinates.Y < 0 || coordinates.Y >= RANGEA)
        return outOfBounds;
      if (coordinates.Z < 0 || coordinates.Z >= RANGEB)
        return outOfBounds;

      Bin returnBin = profile[(int)coordinates.X, (int)coordinates.Y, (int)coordinates.Z];
      returnBin.Location = location;
      return returnBin;
    }

    // bin distance call function
    private double DistanceLAB(Point3D colorLab1, Point3D colorLab2)
    {
      double l, a, b, result, L1, L2, A1, A2, B1, B2;

      L1 = colorLab1.X;
      L2 = colorLab2.X;
      A1 = colorLab1.Y;
      A2 = colorLab2.Y;
      B1 = colorLab1.Z;
      B2 = colorLab2.Z;

      l = L1 - L2;
      a = A1 - A2;
      b = B1 - B2;
      l = l * l;
      a = a * a;
      b = b * b;
      result = l + a + b;

      double distance = Math.Sqrt(result);
      return distance;
    }

    // adding xyz
    private Point3D AddXYZ(Point3D colorXYZ1, Point3D colorXYZ2)
    {
      double X1, X2, Y1, Y2, Z1, Z2, resultX, resultY, resultZ;

      X1 = colorXYZ1.X;
      Y1 = colorXYZ1.Y;
      Z1 = colorXYZ1.Z;

      X2 = colorXYZ2.X;
      Y2 = colorXYZ2.Y;
      Z2 = colorXYZ2.Z;

      resultX = X1 + X2;
      resultY = Y1 + Y2;
      resultZ = Z1 + Z2;

      return new Point3D(resultX, resultY, resultZ);
    }

    class Bin
    {
      public Point3D binLAB;
      public Point3D binRGB;
      public Point3D measuredLAB;
      public Point3D measuredXYZ;

      public bool isEmpty;
      public double distance;
      public Location Location;
      public double weight;

      public Bin(int l, int a, int b)
      {
        binLAB = new Point3D(l, a, b);
        isEmpty = true;
        distance = Double.MaxValue;
      }

      public override string ToString()
      {
        return String.Format("Coordinates: {0}, IsEmpty: {1}", binLAB, isEmpty);
      }
    }

    public enum Location { Top, Bottom, Left, Right, Forward, Backward }

  }
}
