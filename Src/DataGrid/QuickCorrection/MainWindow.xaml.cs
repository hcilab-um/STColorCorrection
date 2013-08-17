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

    private static DataTable profile = new DataTable();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {

    }

    private void btn_Start_Click(object sender, RoutedEventArgs e)
    {
      Matrix3D navigationMatrix = new Matrix3D();
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
      //System.Drawing.Color foreground = System.Drawing.Color.FromArgb(150, 150, 150);
      //PerceptionLib.CIEXYZ background = new CIEXYZ(0.2146, 0.43125, 0.07595); //RGB: 0	199	0 - greenish

      /////////////////
      //code for comarison
      //populate template in datagrid
      PopulateGrid(@"C:\Users\jhincapie\Desktop\Projects\STColorCorrection\Data\PROFILE\Comparison_Template.txt");
      DataTable template = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        template = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
      }));

      DataRow newRow;

      // to create a random number
      Random randomGenerater = new Random();
      for (int i = 0; i < 1000; i++)
      {
        Byte[] rgb = new Byte[3];
        randomGenerater.NextBytes(rgb);
        System.Drawing.Color foreground = System.Drawing.Color.FromArgb(rgb[0], rgb[1], rgb[2]);

        PerceptionLib.CIEXYZ background = new CIEXYZ(0, 0, 0);
        background.X = randomGenerater.NextDouble() * 0.9504;
        background.Y = randomGenerater.NextDouble() * 1.0000;
        background.Z = randomGenerater.NextDouble() * 1.0888;

        try
        {
          Bin foregroundBin = FindForegroundBin(p3700, navigationMatrix, foreground);
          PerceptionLib.Color fg = new PerceptionLib.Color();

          //fg measured LAB value
          fg.LA = foregroundBin.measuredLAB.X;
          fg.A = foregroundBin.measuredLAB.Y;
          fg.B = foregroundBin.measuredLAB.Z;

          Stopwatch stop1 = new Stopwatch();
          stop1.Start();
          Bin corretedColorBF = null;
          corretedColorBF = BruteForceCorrection(p3700, navigationMatrix, foreground, background);
          stop1.Stop();

          Stopwatch stop2 = new Stopwatch();
          stop2.Start();
          Bin corretedColorQCHS = QuickCorrection(p3700, navigationMatrix, foreground, background, HalfTheStep);
          stop2.Stop();

          Stopwatch stop3 = new Stopwatch();
          stop3.Start();
          Bin corretedColorQCD = QuickCorrection(p3700, navigationMatrix, foreground, background, DecreaseTheStep);
          stop3.Stop();

          newRow = template.NewRow();
          newRow["FgR"] = foreground.R.ToString();
          newRow["FgG"] = foreground.G.ToString();
          newRow["FgB"] = foreground.B.ToString();

          newRow["BgX"] = background.X.ToString();
          newRow["BgY"] = background.Y.ToString();
          newRow["BgZ"] = background.Z.ToString();

          newRow["FgL"] = fg.LA.ToString();
          newRow["FgA"] = fg.A.ToString();
          newRow["Fg_B"] = fg.B.ToString();

          newRow["BFL"] = corretedColorBF.binLAB.X.ToString();
          newRow["BFA"] = corretedColorBF.binLAB.Y.ToString();
          newRow["BFB"] = corretedColorBF.binLAB.Z.ToString();
          newRow["BFDis"] = corretedColorBF.distance.ToString();
          newRow["BFTime"] = stop1.ElapsedTicks.ToString();

          newRow["QCHSL"] = corretedColorQCHS.binLAB.X.ToString();
          newRow["QCHSA"] = corretedColorQCHS.binLAB.Y.ToString();
          newRow["QCHSB"] = corretedColorQCHS.binLAB.Z.ToString();
          newRow["QCHSCycles"] = corretedColorQCHS.cycles;
          newRow["QCHSDis"] = corretedColorQCHS.distance.ToString();
          newRow["QCHSError"] = corretedColorBF.distance == 0 ? corretedColorQCHS.distance : Math.Abs((corretedColorBF.distance - corretedColorQCHS.distance) / corretedColorBF.distance) * 100;
          newRow["QCHSTime"] = stop2.ElapsedTicks.ToString();
          newRow["QCHSRatio"] = (stop1.ElapsedTicks / stop2.ElapsedTicks).ToString();

          newRow["QCDL"] = corretedColorQCD.binLAB.X.ToString();
          newRow["QCDA"] = corretedColorQCD.binLAB.Y.ToString();
          newRow["QCDB"] = corretedColorQCD.binLAB.Z.ToString();
          newRow["QCDCycles"] = corretedColorQCD.cycles;
          newRow["QCDDis"] = corretedColorQCD.distance.ToString();
          newRow["QCDError"] = corretedColorBF.distance == 0 ? corretedColorQCD.distance : Math.Abs((corretedColorBF.distance - corretedColorQCD.distance) / corretedColorBF.distance) * 100;
          newRow["QCDTime"] = stop3.ElapsedTicks.ToString();
          newRow["QCDRatio"] = (stop1.ElapsedTicks / stop3.ElapsedTicks).ToString();

          template.Rows.Add(newRow);
          //  Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = Template.DefaultView));
          //  Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
        }


        catch
        { Console.WriteLine(""); }
      }

      Dispatcher.Invoke(new Action(() =>
        {
          dtgrid_corrDisplay.ItemsSource = template.DefaultView;
          dtgrid_corrDisplay.Items.Refresh();
        }));
    }

    //BP Algorithm
    private Bin BruteForceCorrection(Bin[, ,] profile, Matrix3D navigationMatrix, System.Drawing.Color foreground, CIEXYZ background)
    {

      Bin closestBin = null;
      Bin foregroundBin = FindForegroundBin(profile, navigationMatrix, foreground);

      for (int indexL = 0; indexL < RANGEL; indexL++)
      {
        for (int indexA = 0; indexA < RANGEA; indexA++)
        {
          for (int indexB = 0; indexB < RANGEB; indexB++)
          {
            Bin actualBin = profile[indexL, indexA, indexB];
            if (actualBin.isEmpty)
              continue;

            CalculateCorrectionAccuracy(actualBin, background, foregroundBin);
            if (closestBin == null)
              closestBin = actualBin;
            else if (actualBin.distance < closestBin.distance)
              closestBin = actualBin;
          }
        }
      }

      return closestBin;
    }

    public delegate Vector3D ReduceStep(Vector3D step);

    //actual algorithm
    private Bin QuickCorrection(Bin[, ,] profile, Matrix3D navigationMatrix, System.Drawing.Color foreground, CIEXYZ background, ReduceStep reduceStep)
    {
      //1- Converts the foreground to how the display shows it
      Bin foregroundBin = FindForegroundBin(profile, navigationMatrix, foreground);

      //2- general parameters
      Point3D origin = navigationMatrix.Transform(new Point3D(50, 0, 0));
      Vector3D step = navigationMatrix.Transform(new Vector3D(50, 100, 110));

      //3- finds the correction accuracy of the current bin
      Bin originBin = GetProfileBin(profile, origin);
      CalculateCorrectionAccuracy(originBin, background, foregroundBin);
      int cycles = 0;

      //--- Iterative Version ---//
      while (Math.Abs(step.X) > 1 || Math.Abs(step.Y) > 1 && Math.Abs(step.Z) > 1)
      {
        cycles++;

        //4- finds the correction accuracy of the 6 samplers and whether they are better than the origin
        Point3D top = new Point3D(origin.X + step.X, origin.Y, origin.Z);
        Point3D bottom = new Point3D(origin.X - step.X, origin.Y, origin.Z);
        Point3D left = new Point3D(origin.X, origin.Y - step.Y, origin.Z);
        Point3D right = new Point3D(origin.X, origin.Y + step.Y, origin.Z);
        Point3D forward = new Point3D(origin.X, origin.Y, origin.Z - step.Z);
        Point3D backward = new Point3D(origin.X, origin.Y, origin.Z + step.Z);

        Bin[] samples = new Bin[6];
        samples[0] = GetProfileBin(profile, top, Location.Top);
        samples[1] = GetProfileBin(profile, bottom, Location.Bottom);
        samples[2] = GetProfileBin(profile, left, Location.Left);
        samples[3] = GetProfileBin(profile, right, Location.Right);
        samples[4] = GetProfileBin(profile, forward, Location.Forward);
        samples[5] = GetProfileBin(profile, backward, Location.Backward);

        int countSamplesClosestThanOrigin = 0;
        for (int index = 0; index < samples.Length; index++)
        {
          CalculateCorrectionAccuracy(samples[index], background, foregroundBin);
          if (samples[index].distance >= originBin.distance)
            continue;
          samples[index].isMoreAccurateThanOrigin = true;
          countSamplesClosestThanOrigin++;
        }

        //5- if the origin is the most accurate, it halves the step and checks again
        if (countSamplesClosestThanOrigin == 0)
        {
          if (step.X == 1 && step.Y == 1 && step.Z == 1)
            return originBin;

          step = reduceStep(step);
          continue;
        }

        //6- if there is at least one sample more accurate, it moves the origin in that direction, maintains the step and checks again
        else
        {
          //6.1 calculates weights
          double totalimprovements = 0;
          for (int index = 0; index < samples.Length; index++)
          {
            if (!samples[index].isMoreAccurateThanOrigin)
              continue;
            totalimprovements += (originBin.distance - samples[index].distance);
          }
          for (int index = 0; index < samples.Length; index++)
          {
            if (!samples[index].isMoreAccurateThanOrigin)
              continue;
            samples[index].weight = (originBin.distance - samples[index].distance) / totalimprovements;
          }

          //6.2 calculates displacement
          Vector3D displacement = new Vector3D(0, 0, 0);
          for (int index = 0; index < samples.Length; index++)
          {
            if (!samples[index].isMoreAccurateThanOrigin)
              continue;
            displacement = displacement + (samples[index].binLAB - origin) * samples[index].weight;
          }

          displacement.X = displacement.X > 0 ? Math.Ceiling(displacement.X) : Math.Floor(displacement.X);
          displacement.Y = displacement.Y > 0 ? Math.Ceiling(displacement.Y) : Math.Floor(displacement.Y);
          displacement.Z = displacement.Z > 0 ? Math.Ceiling(displacement.Z) : Math.Floor(displacement.Z);

          //6.3 pokes new origin
          Point3D newOriginLoc = origin + displacement;
          Bin newOriginBin = GetProfileBin(profile, newOriginLoc);
          while (newOriginBin.isEmpty)
          {
            //6.4 moves half the magnitude in the given direction
            displacement.X = Math.Round(displacement.X / 2, MidpointRounding.ToEven);
            displacement.Y = Math.Round(displacement.Y / 2, MidpointRounding.ToEven);
            displacement.Z = Math.Round(displacement.Z / 2, MidpointRounding.ToEven);

            newOriginLoc = origin + displacement;
            newOriginBin = GetProfileBin(profile, newOriginLoc);
          }

          if (origin == newOriginLoc) // it's the same location then just reduces the step
            step = reduceStep(step);
          else if (originBin.distance < newOriginBin.distance) // the current location is still better, then just reduces the step
            step = reduceStep(step);
          else // the new location is better thus is moves there
          {
            origin = newOriginLoc;
            originBin = newOriginBin;
            CalculateCorrectionAccuracy(originBin, background, foregroundBin);
          }
        }
      }

      originBin.cycles = cycles;
      return originBin;
    }

    private Vector3D HalfTheStep(Vector3D step)
    {
      if (Math.Abs(step.X) > 1)
        step.X = Math.Round(step.X / 2, MidpointRounding.AwayFromZero);
      if (Math.Abs(step.Y) > 1)
        step.Y = Math.Round(step.Y / 2, MidpointRounding.AwayFromZero);
      if (Math.Abs(step.Z) > 1)
        step.Z = Math.Round(step.Z / 2, MidpointRounding.AwayFromZero);
      return step;
    }

    private static Vector3D DecreaseTheStep(Vector3D step)
    {
      if (Math.Abs(step.X) > 1)
        step.X = step.X > 0 ? step.X - 1 : step.X + 1;
      if (Math.Abs(step.Y) > 1)
        step.Y = step.Y > 0 ? step.Y - 1 : step.Y + 1;
      if (Math.Abs(step.Z) > 1)
        step.Z = step.Z > 0 ? step.Z - 1 : step.Z + 1;
      return step;
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

    private PerceptionLib.Color FindForegroundBin(System.Drawing.Color foregroundRGB)
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

      foregroundLAB.LA = (double)binL;
      foregroundLAB.A = (double)binA;
      foregroundLAB.B = (double)binB;

      return foregroundLAB;
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
      returnBin.isMoreAccurateThanOrigin = false;
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
      public bool isMoreAccurateThanOrigin;
      public int cycles;
      public double distance;
      public Location Location;
      public double weight;

      public Bin(int l, int a, int b)
      {
        binLAB = new Point3D(l, a, b);
        isEmpty = true;
        distance = Double.MaxValue;
        isMoreAccurateThanOrigin = false;
      }

      public override string ToString()
      {
        return String.Format("Coordinates: {0}, IsEmpty: {1}, Location: {2}", binLAB, isEmpty, Location);
      }
    }

    public enum Location { Top, Bottom, Left, Right, Forward, Backward }
    private void PopulateGrid(string fileName)
    {
      //DataTable table = CSV.GetDataTableFromCSV(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\color.txt");
      DataTable table = PerceptionLib.CSV.GetDataTableFromCSV(@fileName);
      if (table.Columns.Count == 0)
        System.Windows.MessageBox.Show("Error!");
      else
        dtgrid_corrDisplay.ItemsSource = table.DefaultView;

      dtgrid_corrDisplay.AutoGenerateColumns = true;
    }

    public double GetRandomNumber(double minimum, double maximum)
    {
      Random random = new Random();
      return random.NextDouble() * (maximum - minimum) + minimum;
    }


  }
}
