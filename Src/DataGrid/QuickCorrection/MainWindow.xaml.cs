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
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using GenericParsing;
using System.Data;

namespace QuickCorrection
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public static int StepNumber=0;
    public MainWindow()
    {
      InitializeComponent();
    }
    
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

    private DataView dataView;
    private DataTable BinTable=new DataTable();

     //1- Create the transformation matrix
      private Matrix3D navigationMatrix = new Matrix3D();
    
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
     // var results = PerceptionLib.Color.RGBbinedData();

      navigationMatrix.Translate(new Vector3D(0, 100, 110));
      navigationMatrix.Scale(new Vector3D((double)1 / 5, (double)1 / 5, (double)1 / 5));

      //2- Load the profile in a three dimensional array
      Bin[, ,] p3700 = new Bin[21, 41, 45];

      // add the csv bin file
      using (GenericParserAdapter parser = new GenericParserAdapter(@"C:\see-through-project\gt\STColorCorrection\Src\DataGrid\DataGrid\data\p3700.csv"))
      {
        System.Data.DataSet dsResult = parser.GetDataSet();
        dataView = dsResult.Tables[0].AsDataView();
      }
      BinTable = dataView.ToTable();

        
      for (int i = 1; i < BinTable.Rows.Count; i++)
      {
       
        //lab vale as got form profile index
        Point3D LAB_Points = new Point3D();
        LAB_Points.X = Convert.ToDouble(BinTable.Rows[i][0].ToString());
        LAB_Points.Y = Convert.ToDouble(BinTable.Rows[i][1].ToString());
        LAB_Points.Z = Convert.ToDouble(BinTable.Rows[i][2].ToString());
        
        //trasfered points
        Point3D LAB = navigationMatrix.Transform(LAB_Points);

        // assing values into the aray
        
        //BinLAB VALUE
        p3700[(int)LAB.X, (int)LAB.Y, (int)LAB.Z].binLAB = LAB_Points;

        //Measure Lab Values
        p3700[(int)LAB.X, (int)LAB.Y, (int)LAB.Z].measuredLAB.X = Convert.ToDouble(BinTable.Rows[i][3].ToString());
        p3700[(int)LAB.X, (int)LAB.Y, (int)LAB.Z].measuredLAB.Y = Convert.ToDouble(BinTable.Rows[i][4].ToString());
        p3700[(int)LAB.X, (int)LAB.Y, (int)LAB.Z].measuredLAB.Z = Convert.ToDouble(BinTable.Rows[i][5].ToString());

        //measured XYZ Values
        p3700[(int)LAB.X, (int)LAB.Y, (int)LAB.Z].measuredX = Convert.ToDouble(BinTable.Rows[i][6].ToString());
        p3700[(int)LAB.X, (int)LAB.Y, (int)LAB.Z].measuredY = Convert.ToDouble(BinTable.Rows[i][7].ToString());
        p3700[(int)LAB.X, (int)LAB.Y, (int)LAB.Z].measuredZ = Convert.ToDouble(BinTable.Rows[i][8].ToString());

        //measured XYZ Values
        p3700[(int)LAB.X, (int)LAB.Y, (int)LAB.Z].measuredR = Convert.ToByte(BinTable.Rows[i][9].ToString());
        p3700[(int)LAB.X, (int)LAB.Y, (int)LAB.Z].measuredG = Convert.ToByte(BinTable.Rows[i][10].ToString());
        p3700[(int)LAB.X, (int)LAB.Y, (int)LAB.Z].measuredB = Convert.ToByte(BinTable.Rows[i][11].ToString());

        //is empty check
        p3700[(int)LAB.X, (int)LAB.Y, (int)LAB.Z].Empty = 1;
      }
      


      //3- Get the parameters: foreground and background
      System.Drawing.Color foreGround = System.Drawing.Color.FromArgb(150, 150, 150);
      PerceptionLib.CIEXYZ backGround = new CIEXYZ(20, 20, 20);

      //4- Converts the foreground to how the display shows it
      Bin foreGroundBin = FindForeGroundBin(foreGround, p3700, navigationMatrix);
      Vector3D origin1 = new Vector3D(1, 1, 1);
      int X = (int)origin1.X / 2;
      origin1.X = (double)X;

      Point3D origin = new Point3D(50, 0, 0);
      Vector3D step = new Vector3D(50, 100, 110);
      //Vector3D step = new Vector3D(25, 50, 55);
      Bin CorretedColor= QuickCorrection(p3700, foreGroundBin, backGround, navigationMatrix.Transform(origin), navigationMatrix.Transform(step));
    }

    //to find fg bin
    private Bin FindForeGroundBin(System.Drawing.Color foreGroundRGB, Bin[, ,] profile, Matrix3D navigationMatrix)
    {
      PerceptionLib.Color foreGroundLAB = PerceptionLib.Color.ToLAB(foreGroundRGB);

      int binL = ((int)(Math.Round(foreGroundLAB.LA / 5.0)) * 5);
      int binA = ((int)(Math.Round(foreGroundLAB.A / 5.0)) * 5);
      int binB = ((int)(Math.Round(foreGroundLAB.B / 5.0)) * 5);
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

      Point3D profileCoordinates = navigationMatrix.Transform(new Point3D(binL, binA, binB));
      Bin foregroundBin = profile[(int)profileCoordinates.X, (int)profileCoordinates.Y, (int)profileCoordinates.Z];

      return foregroundBin;
    }

    // bin ditacne call function
    private double DistanceLAB(Bin Color1, Bin Color2)
    {
      PerceptionLib.Color C1 = new PerceptionLib.Color();
      PerceptionLib.Color C2 = new PerceptionLib.Color();

      double l, a, b, result,L1,L2,A1,A2,B1,B2;
      L1 = Color1.measuredLAB.X;
      L2 = Color2.measuredLAB.X;
      A1 = Color1.measuredLAB.Y;
      A2 = Color2.measuredLAB.Y;
      B1 = Color1.measuredLAB.Z;
      B2 = Color2.measuredLAB.Z;

      l = L1 - L2;
      a = A1 - A2;
      b = B1 - B2;
      l = l * l;
      a = a * a;
      b = b * b;
      result = l + a + b;
            
      double Euv = Math.Sqrt(result);
      
      return Euv;
    }
   
    // adding xyz
    private Bin addXYZ(Bin Color1, CIEXYZ Color2)
    {
      double X1, X2, Y1, Y2, Z1, Z2, ResultX, ResultY, ResultZ;
      X1 = Color1.measuredX;
      Y1 = Color1.measuredY;
      Z1 = Color1.measuredZ;

      X2 = Color2.X;
      Y2 = Color2.Y;
      Z2 = Color2.Z;

      Bin returnValue = new Bin();

      
      ResultX = X1 + X2;
      ResultY = Y1 + Y2;
      ResultZ = Z1 + Z2;
      CIEXYZ ResultXYZ=new CIEXYZ(ResultX,ResultY,ResultZ);

      PerceptionLib.Color Result = new PerceptionLib.Color();

      Result = PerceptionLib.Color.ToLAB(ResultXYZ);
      returnValue.measuredX =ResultX;
      returnValue.measuredY =ResultY;
      returnValue.measuredX = ResultZ;
      returnValue.measuredLAB.X = Result.LA;
      returnValue.measuredLAB.Y = Result.A;
      returnValue.measuredLAB.Z = Result.B;

      return returnValue;
    }
    
    //actual algorithm
    private Bin QuickCorrection(Bin[, ,] profile, Bin foreGroundBin, CIEXYZ backGround, Point3D coordinate, Vector3D step)
    {
      ////////////////////
      /// juan's code
      ///// ///////////////
      //Bin anchor = GetProfileBin(profile, origin);

      //Bin PredicitionAtOrigin = addXYZ(anchor, backGround);
      //double distanceAtOrigin=DistanceLAB(foreGroundBin, PredicitionAtOrigin);
      //if (distanceAtOrigin < 2.3)
      //  return anchor;
      
      
      //Point3D top = new Point3D(origin.X + step.X, origin.Y, origin.Z);
      //Point3D bottom = new Point3D(origin.X - step.X, origin.Y, origin.Z);
      //Point3D left = new Point3D(origin.X, origin.Y - step.Y, origin.Z);
      //Point3D right = new Point3D(origin.X, origin.Y + step.Y, origin.Z);
      //Point3D forward = new Point3D(origin.X, origin.Y, origin.Z - step.Z);
      //Point3D backward = new Point3D(origin.X, origin.Y, origin.Z + step.Z);

      //Bin valueTop = GetProfileBin(profile, top);
      //Bin valueBottom = GetProfileBin(profile, bottom);
      //Bin valueLeft = GetProfileBin(profile, left);
      //Bin valueRight = GetProfileBin(profile, right);
      //Bin valueForward = GetProfileBin(profile, forward);
      //Bin valueBackwards = GetProfileBin(profile, backward);

      //List<Bin> samples = new List<Bin>();
      //samples.Add(valueTop);
      //samples.Add(valueBottom);
      //samples.Add(valueLeft);
      //samples.Add(valueRight);
      //samples.Add(valueForward);
      //samples.Add(valueBackwards);

      ////////////////////////////
      
      // to see if the first point is good
      double distanceAtOrigin = ChosenPointDistance(profile, coordinate, foreGroundBin, backGround);
      if (distanceAtOrigin < 2.3)
        return GetProfileBin(profile, coordinate);
     
     //to see in which step we are and accordingtly chose the size around to explore
     StepNumber++;
     step = step / StepNumber;


     List<Bin> samples = Tracer(coordinate, step, profile);
      List<double> distances= new List<double>();
     foreach (Bin i in samples)
     {
       if (i.Empty == 1)
       {
         double dis= ChosenPointDistance(profile, navigationMatrix.Transform(i.binLAB), foreGroundBin, backGround);
         distances.Add(dis) ;
       }
        
     }
      distances.Sort();

      if(distances[0]<2.3)
      return GetProfileBin(profile, navigationMatrix.Transform(samples[0].binLAB)); 

      else
           
     return QuickCorrection(profile,  foreGroundBin, backGround,  navigationMatrix.Transform(samples[1].binLAB), step);
    }

    //to estabish the disatnce of the the size of the points which needs to be searched around
    private List<Bin> Tracer(Point3D origin, Vector3D step, Bin[, ,] profile)
    {
      Point3D top = new Point3D(origin.X + step.X, origin.Y, origin.Z);
      Point3D bottom = new Point3D(origin.X - step.X, origin.Y, origin.Z);
      Point3D left = new Point3D(origin.X, origin.Y - step.Y, origin.Z);
      Point3D right = new Point3D(origin.X, origin.Y + step.Y, origin.Z);
      Point3D forward = new Point3D(origin.X, origin.Y, origin.Z - step.Z);
      Point3D backward = new Point3D(origin.X, origin.Y, origin.Z + step.Z);

      Bin valueTop = GetProfileBin(profile, top);
      Bin valueBottom = GetProfileBin(profile, bottom);
      Bin valueLeft = GetProfileBin(profile, left);
      Bin valueRight = GetProfileBin(profile, right);
      Bin valueForward = GetProfileBin(profile, forward);
      Bin valueBackwards = GetProfileBin(profile, backward);

      List<Bin> samples = new List<Bin>();
      samples.Add(valueTop);
      samples.Add(valueBottom);
      samples.Add(valueLeft);
      samples.Add(valueRight);
      samples.Add(valueForward);
      samples.Add(valueBackwards);

      int NoOfEmptyPoints=0;
      foreach (Bin i in samples)
      {
        if (i.Empty != 1)
          NoOfEmptyPoints++;
      }

      if (NoOfEmptyPoints < 3)
      {
        return samples;
      }
      else
      {
        step = step/ 2;
        samples = Tracer(origin, step, profile);
        return samples;
      }
            
    }

    //distance caluclatpor for a chosen point
    private double ChosenPointDistance(Bin[, ,] profile,Point3D coordinates,Bin foreGroundBin, CIEXYZ backGround)
    {
      Bin anchor = GetProfileBin(profile, coordinates);
      Bin PredicitionAtCoordinate = addXYZ(anchor, backGround);
      
      double distanceAtCoordinate = DistanceLAB(foreGroundBin, PredicitionAtCoordinate);
      return distanceAtCoordinate;
    }
    private Bin GetProfileBin(Bin[, ,] profile, Point3D coordinates)
    {
      Bin returnBin = profile[(int)coordinates.X, (int)coordinates.Y, (int)coordinates.Z];
      return returnBin;
    }

    struct Bin
    {
      public Point3D binLAB;

      public Point3D measuredLAB;

      public double measuredX;
      public double measuredY;
      public double measuredZ;

      public Byte measuredR;
      public Byte measuredG;
      public Byte measuredB;


      public int Empty;

      public double distance;
    }

  }
}
