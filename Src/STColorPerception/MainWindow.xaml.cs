//csmainwinow
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Drawing;
using System.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using PerceptionLib.Hacks;
using PerceptionLib;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;




namespace STColorPerception
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window, INotifyPropertyChanged
  {
    private string time, time2, time3, time4;
    // color class object
    private PerceptionLib.Color colorToShow;
    private PerceptionLib.Color colorMeasured;
    private PerceptionLib.Color colorDifference;
    private PerceptionLib.Color colorBackGround;
    private PerceptionLib.Color acolor;
    private PerceptionLib.Color Bincolor;
    private PerceptionLib.Color macolor;
    private PerceptionLib.Color mBincolor;

    private PerceptionLib.CIEXYZ colorToShowXYZ;
    private PerceptionLib.CIEXYZ colorMeasuredXYZ;
    private PerceptionLib.CIEXYZ colorBackGroundXYZ;
    private PerceptionLib.CIEXYZ McolorBackGroundXYZ;
    private PerceptionLib.CIEXYZ acolorXYZ;
    private PerceptionLib.CIEXYZ BinXYZ;
    private PerceptionLib.CIEXYZ macolorXYZ;
    private PerceptionLib.CIEXYZ mBinXYZ;



    /// <summary>
    /// for CAT
    /// </summary>
    private PerceptionLib.CIEXYZ bradXYZ;
    private PerceptionLib.CIEXYZ vonXYZ;
    private PerceptionLib.CIEXYZ scalingXYZ;

    private PerceptionLib.Color bradcolor;
    private PerceptionLib.Color voncolor;
    private PerceptionLib.Color scalingcolor;

    private PerceptionLib.RGBValue bradRGB;
    private PerceptionLib.RGBValue VonRGB;
    private PerceptionLib.RGBValue scalingRGB;

    private PerceptionLib.CIEXYZ MbradXYZ;
    private PerceptionLib.CIEXYZ MvonXYZ;
    private PerceptionLib.CIEXYZ MscalingXYZ;

    private PerceptionLib.Color Mbradcolor;
    private PerceptionLib.Color Mvoncolor;
    private PerceptionLib.Color Mscalingcolor;

    private PerceptionLib.RGBValue MbradRGB;
    private PerceptionLib.RGBValue MVonRGB;
    private PerceptionLib.RGBValue MscalingRGB;

    private PerceptionLib.RGBValue acolorRGB;
    private PerceptionLib.RGBValue BincolorRGB;

    private PerceptionLib.RGBValue MacolorRGB;
    private PerceptionLib.RGBValue MBincolorRGB;

    private PerceptionLib.Color bgcolour;
    private PerceptionLib.Color mixedcolor;

    private MTObservableCollection<MeasurementPair> pairs;



    private PerceptionLib.RGBValue CalRGB;

    private int dtgridClick = 0;



    //input values
    //  rbg is input rgb which is displayed
    //  mr mg mb measured displyed colour
    //  bgr bgg bgb measured background color 
    //  mcr mcb mcg measured nacground colour

    private byte r, g, b, mr, mg, mb, bgr, bgg, bgb, mcr, mcg, mcb;
    int bgNo, fgNo;
    int BinNo = 0;



    PerceptionLib.HexRGB projectedColor, backgroungColor, capturedColor;

    double CiToCmDiff, ACToCiDiff, AcToCmDiff, BCmToCiDiff, BCmToCmDiff, VCmToCiDiff, VCmToCmDiff, SCmToCiDiff, SCmToCmDiff;
    double CiToCmStd, ACToCiStd, AcToCmStd, BCmToCiStd, BCmToCmStd, VCmToCiStd, VCmToCmStd, SCmToCiStd, SCmToCmStd;

    //EMGU CV objects
    private Image<Bgr, Byte> captureImage;
    private Image<Bgr, Byte> croppedImage;

    //Data Loader class obj
    DataTable dataTable = new DataTable();

    // this is a color object of BGR value ,.. uit  gives avg blue green and red value from cropped image.
    Bgr avgRGB;

    // obj for web cam capture
    private Capture captureDevice = new Capture();

    public PerceptionLib.HexRGB ProjectedColor
    {
      get { return projectedColor; }
      set
      {
        projectedColor = value;
        OnPropertyChanged("ProjectedColor");
      }
    }

    public PerceptionLib.HexRGB BackgroungColor
    {
      get { return backgroungColor; }
      set
      {
        backgroungColor = value;
        OnPropertyChanged("BackgroungColor");
      }
    }

    public PerceptionLib.HexRGB CapturedColor
    {
      get { return capturedColor; }
      set
      {
        capturedColor = value;
        OnPropertyChanged("CapturedColor");
      }
    }

    public PerceptionLib.Color ColorToShow
    {
      get { return colorToShow; }
      set
      {
        colorToShow = value;
        OnPropertyChanged("ColorToShow");
      }
    }
    public PerceptionLib.Color ColorMeasured
    {
      get { return colorMeasured; }
      set
      {
        colorMeasured = value;
        OnPropertyChanged("ColorMeasured");
      }
    }
    public PerceptionLib.Color ColorBackGround
    {
      get { return colorBackGround; }
      set
      {
        colorBackGround = value;
        OnPropertyChanged("ColorBackGround");
      }
    }

    public PerceptionLib.CIEXYZ ColorToShowXYZ
    {
      get { return colorToShowXYZ; }
      set
      {
        colorToShowXYZ = value;
        OnPropertyChanged("ColorToShowXYZ");
      }
    }
    public PerceptionLib.CIEXYZ ColorMeasuredXYZ
    {
      get { return colorMeasuredXYZ; }
      set
      {
        colorMeasuredXYZ = value;
        OnPropertyChanged("colorMeasuredXYZ");
      }
    }
    public PerceptionLib.CIEXYZ ColorBackGroundXYZ
    {
      get { return colorBackGroundXYZ; }
      set
      {
        colorBackGroundXYZ = value;
        OnPropertyChanged("colorBackGroundXYZ");
      }
    }

    public PerceptionLib.Color ColorDifference
    {
      get { return colorDifference; }
      set
      {
        colorDifference = value;
        OnPropertyChanged("ColorDifference");
      }
    }
    public PerceptionLib.Color BgColor
    {
      get { return bgcolour; }
      set
      {
        bgcolour = value;
        OnPropertyChanged("BgColor");
      }
    }
    public PerceptionLib.Color MixedColor
    {
      get { return mixedcolor; }
      set
      {
        mixedcolor = value;
        OnPropertyChanged("MixedColor");
      }
    }

    public PerceptionLib.Color Bradcolor
    {
      get { return bradcolor; }
      set
      {
        bradcolor = value;
        OnPropertyChanged("Bradcolor");
      }
    }
    public PerceptionLib.Color Voncolor
    {
      get { return voncolor; }
      set
      {
        voncolor = value;
        OnPropertyChanged("voncolor");
      }
    }
    public PerceptionLib.Color Scalingcolor
    {
      get { return scalingcolor; }
      set
      {
        scalingcolor = value;
        OnPropertyChanged("Scalingcolor");
      }
    }
    public PerceptionLib.Color Acolor
    {
      get { return acolor; }
      set
      {
        acolor = value;
        OnPropertyChanged("acolor");
      }
    }
    public PerceptionLib.Color BINColor
    {
      get { return Bincolor; }
      set
      {
        Bincolor = value;
        OnPropertyChanged("BINcolor");
      }
    }
    public PerceptionLib.Color MAcolor
    {
      get { return macolor; }
      set
      {
        macolor = value;
        OnPropertyChanged("MAcolor");
      }
    }
    public PerceptionLib.Color MBINColor
    {
      get { return mBincolor; }
      set
      {
        mBincolor = value;
        OnPropertyChanged("MBINcolor");
      }
    }
    public PerceptionLib.Color MBradcolor
    {
      get { return Mbradcolor; }
      set
      {
        Mbradcolor = value;
        OnPropertyChanged("Mbradcolor");
      }
    }
    public PerceptionLib.Color MVoncolor
    {
      get { return Mvoncolor; }
      set
      {
        Mvoncolor = value;
        OnPropertyChanged("Mvoncolor");
      }
    }
    public PerceptionLib.Color MScalingcolor
    {
      get { return Mscalingcolor; }
      set
      {
        Mscalingcolor = value;
        OnPropertyChanged("Mscalingcolor");
      }
    }


    public PerceptionLib.CIEXYZ BradXYZ
    {
      get { return bradXYZ; }
      set
      {
        bradXYZ = value;
        OnPropertyChanged("BradXYZ");
      }
    }
    public PerceptionLib.CIEXYZ VonXYZ
    {
      get { return vonXYZ; }
      set
      {
        vonXYZ = value;
        OnPropertyChanged("VonXYZ");
      }
    }
    public PerceptionLib.CIEXYZ ScalingXYZ
    {
      get { return scalingXYZ; }
      set
      {
        scalingXYZ = value;
        OnPropertyChanged("ScalingXYZ");
      }
    }
    public PerceptionLib.CIEXYZ AcolorXYZ
    {
      get { return acolorXYZ; }
      set
      {
        acolorXYZ = value;
        OnPropertyChanged("acolorXYZ");
      }
    }
    public PerceptionLib.CIEXYZ BINcolorXYZ
    {
      get { return BinXYZ; }
      set
      {
        BinXYZ = value;
        OnPropertyChanged("BINcolorXYZ");
      }
    }
    public PerceptionLib.CIEXYZ MAcolorXYZ
    {
      get { return macolorXYZ; }
      set
      {
        macolorXYZ = value;
        OnPropertyChanged("MAcolorXYZ");
      }
    }
    public PerceptionLib.CIEXYZ MBINcolorXYZ
    {
      get { return mBinXYZ; }
      set
      {
        mBinXYZ = value;
        OnPropertyChanged("MBINcolorXYZ");
      }
    }
    public PerceptionLib.CIEXYZ MBradXYZ
    {
      get { return MbradXYZ; }
      set
      {
        MbradXYZ = value;
        OnPropertyChanged("MbradXYZ");
      }
    }
    public PerceptionLib.CIEXYZ MVonXYZ
    {
      get { return MvonXYZ; }
      set
      {
        MvonXYZ = value;
        OnPropertyChanged("MvonXYZ");
      }
    }
    public PerceptionLib.CIEXYZ MScalingXYZ
    {
      get { return MscalingXYZ; }
      set
      {
        MscalingXYZ = value;
        OnPropertyChanged("MscalingXYZ");
      }
    }

    //  rbg is input rgb which is displayed
    //  mr mg mb measured displyed colour
    //  bgr bgg bgb measured background color 
    //  mcr mcb mcg measured nacground colour

    //Displayed RGB
    public byte R
    {
      get { return r; }
      set
      {
        r = value;
        OnPropertyChanged("R");
      }
    }

    public byte G
    {
      get { return g; }
      set
      {
        g = value;
        OnPropertyChanged("G");
      }
    }

    public byte B
    {
      get { return b; }
      set
      {
        b = value;
        OnPropertyChanged("B");
      }
    }

    // measured background   
    public byte BgR
    {
      get { return bgr; }
      set
      {
        bgr = value;
        OnPropertyChanged("BgR");
      }
    }

    public byte BgG
    {
      get { return bgg; }
      set
      {
        bgg = value;
        OnPropertyChanged("BgG");
      }
    }

    public byte BgB
    {
      get { return bgb; }
      set
      {
        bgb = value;
        OnPropertyChanged("BgB");
      }
    }

    public byte McR
    {
      get { return mcr; }
      set
      {
        mcr = value;
        OnPropertyChanged("McR");
      }
    }

    public byte McG
    {
      get { return mcg; }
      set
      {
        mcg = value;
        OnPropertyChanged("McG");
      }
    }

    public byte McB
    {
      get { return mcb; }
      set
      {
        mcb = value;
        OnPropertyChanged("McB");
      }
    }


    //measured RGB
    public byte MR
    {
      get { return mr; }
      set
      {
        mr = value;
        OnPropertyChanged("MR");
      }
    }

    public byte MG
    {
      get { return mg; }
      set
      {
        mg = value;
        OnPropertyChanged("MG");
      }
    }

    public byte MB
    {
      get { return mb; }
      set
      {
        mb = value;
        OnPropertyChanged("MB");
      }
    }

    public int BgNo
    {
      get { return bgNo; }
      set
      {
        bgNo = value;
        OnPropertyChanged("BgNo");
      }
    }

    public int FgNo
    {
      get { return fgNo; }
      set
      {
        fgNo = value;
        OnPropertyChanged("FgNo");
      }
    }


    public MainWindow()
    {
      InitializeComponent();
      //PerceptionLib.Color.RGBBinneddData_David();

      // captureDevice = new Capture();
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\color.txt");
      //ColorToShow= new PerceptionLib.Color();


      //testing rgb gamut limit
    // ColorToShowXYZ = new PerceptionLib.CIEXYZ(0.7252, 0.5394, 1.0888);
      //ColorToShow = PerceptionLib.Color.ToLAB(ColorToShowXYZ);
      //ColorToShow.L = 81.92236628;
      //ColorToShow.A = -7.659442063;
      //ColorToShow.B = 19.20678352;
      ////PerceptionLib.RGBValue a = PerceptionLib.Color.ToRBG(ColorToShowXYZ);
      //PerceptionLib.RGBValue ab = PerceptionLib.Color.ToRBGFromLAB(ColorToShow);
      
      rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(46, 60, 153));
     


     //rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(177, 44, 56));

     rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));

      //Util.CATCalulation.HueAngle(colorToShow);
     String Hex = PerceptionLib.Color.RGBtoHEX(77, 19, 18);
     

      //projectedColor = new PerceptionLib.HexRGB();
      //BackgroungColor = new PerceptionLib.HexRGB();
      //capturedColor = new PerceptionLib.HexRGB();
      //// ProjectedColor = "#ffffff";
      ////BackgroungColor="#000000";
      ////CapturedColor = "#FFFFFF";

      pairs = new MTObservableCollection<MeasurementPair>();
      cie1976C.DataContext = pairs;
      PropertyChanged += new PropertyChangedEventHandler(MainWindow_PropertyChanged);

      //ColourcaputreWorker.RunWorkerCompleted += ColourcaputreWorker_RunWorkerCompleted;
    }

    private void PopulateGrid(string fileName)
    {
      //DataTable table = CSV.GetDataTableFromCSV(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\color.txt");
      DataTable table = CSV.GetDataTableFromCSV(@fileName);
      if (table.Columns.Count == 0)
        System.Windows.MessageBox.Show("Error!");
      else
        dtgrid_corrDisplay.ItemsSource = table.DefaultView;

      dtgrid_corrDisplay.AutoGenerateColumns = true;

    }


    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      //Btn_Load_File.IsEnabled = false;
      //btn_StartMeasurment.IsEnabled = false;
      // btn_CheckMeasurment.IsEnabled = false;
      cmb_graph.IsEnabled = false;
      btn_ExportGrid.IsEnabled = false;


      // initial xy plots to cross verify the graph's accuracy 
      //   MTObservableCollection<MeasurementPair> pairs = new MTObservableCollection<MeasurementPair>();
      pairs.Add(new MeasurementPair()
      {
        ColorToShow = new PerceptionLib.Color() { L = 0, UP = 0, VP = 0 },
        ColorCaptured = new PerceptionLib.Color() { L = 0, UP = 0.1, VP = 0.2 },
        BgColor = new PerceptionLib.Color() { L = 0, UP = 0, VP = 0 }
      });


      pairs.Add(new MeasurementPair()
      {
        ColorToShow = new PerceptionLib.Color() { L = 0, UP = 0.6, VP = 0 },
        ColorCaptured = new PerceptionLib.Color() { L = 0, UP = 0.5, VP = 0.1 }
      });
      pairs.Add(new MeasurementPair()
      {
        ColorToShow = new PerceptionLib.Color() { L = 0, UP = 0, VP = 0.6 },
        ColorCaptured = new PerceptionLib.Color() { L = 0, UP = 0.1, VP = 0.5 }
      });
      pairs.Add(new MeasurementPair()
      {
        ColorToShow = new PerceptionLib.Color() { L = 0, UP = 0.6, VP = 0.6 },
        ColorCaptured = new PerceptionLib.Color() { L = 0, UP = 0.5, VP = 0.5 }
      });
      pairs.Add(new MeasurementPair());
      pairs.Add(new MeasurementPair() { BgColor = new PerceptionLib.Color() { L = 0, UP = 0.3, VP = 0.3 } });

      pairs.Add(new MeasurementPair() { BradColor = new PerceptionLib.Color() { L = 0, UP = 0.4, VP = 0.4 } });
      pairs.Add(new MeasurementPair() { VonColor = new PerceptionLib.Color() { L = 0, UP = 0.3, VP = 0.5 } });
      pairs.Add(new MeasurementPair() { Scalingcolor = new PerceptionLib.Color() { L = 0, UP = 0.4, VP = 0.2 } });

      pairs.Add(new MeasurementPair() { Acolor = new PerceptionLib.Color() { L = 0, UP = 0.2, VP = 0.2 } });
      cie1976C.DataContext = pairs;
    }


    /// <summary>
    ///   // to change all values of shown color when each time RGB text box value chabges
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void MainWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      //if ("R".Equals(e.PropertyName) || "G".Equals(e.PropertyName) || "B".Equals(e.PropertyName))
      //{
      //    //btn_StartMeasurment.IsEnabled = true;

      //    // to measure LUV from Color class
      //    ColorToShow = Util.ColorSpaceConverter.ToGetLUV(R, G, B);

      //    rec_shown.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
      //    rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
      //}
      //if ("BgR".Equals(e.PropertyName) || "BgG".Equals(e.PropertyName) || "BgB".Equals(e.PropertyName))
      //{

      //    Rectangle_Bg.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BgR, BgG, BgB));
      //}
      //if ("BgNo".Equals(e.PropertyName))
      //{
      //  cmb_graph.Items.Clear();
      //  progressBar1.Minimum = 0;
      //  progressBar1.Maximum = BgNo;
      //  progressBar1.Value = 0;

      //  for (int i = 0; i <= bgNo + 1; i++)
      //  {
      //    if (i == 0)
      //      cmb_graph.Items.Add("No_background");
      //    else if (i < bgNo)
      //      cmb_graph.Items.Add("Background_No:" + i);
      //    else if (i < bgNo + 1)
      //      cmb_graph.Items.Add("White_Points");
      //    else
      //      cmb_graph.Items.Add("All_Data");
      //  }
      //}
    }

    /// <summary>
    /// when start measure button is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// 
    /// <param name="e"></param>
    private void Btn_StartMeasurment_Click(object sender, RoutedEventArgs e)
    {
      // to enable measurement check
      //btn_CheckMeasurment.IsEnabled = true;
      ColorToShow = Util.ColorSpaceConverter.ToGetLUV(R, G, B);
      ColorToShowXYZ = Util.ColorSpaceConverter.ToGetXYZ(R, G, B);

      //ProjectedColor = PerceptionLib.Color.RGBtoHEX(R, G, B);
      rec_shown.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
      //rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
      rec_BgColor.Fill=new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));

      //StartCapture();
      //DisplayMeasuredValues();
      int i = PerceptionLib.Cs200Connection.ConnectToCS200();
      DisplayMeasuredValuesFromCs200();
      i = PerceptionLib.Cs200Connection.DisconnectCS200();
      DifferenceCalculation();
      pairs.Clear();
      pairs.Add(new MeasurementPair()
      {
        ColorToShow = new PerceptionLib.Color() { L = 0, UP = colorToShow.UP, VP = colorToShow.VP },
        ColorCaptured = new PerceptionLib.Color() { L = 0, UP = colorMeasured.UP, VP = colorMeasured.VP }
      });
      //dtgrid_corrDisplay.ClearValue();
    }



    /// <summary>
    /// to get the propert change name of the property which has changed
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(String name)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(name));
    }

    //private void Btn_CheckMeasurment_Click(object sender, RoutedEventArgs e)
    //{
    //    correctedcolor = new PerceptionLib.Color();
    //    correctedcolor.L = ColorDifference.L + ColorMeasured.L;
    //    correctedcolor.U = ColorDifference.U + ColorMeasured.U;
    //    correctedcolor.V = ColorDifference.V + ColorMeasured.V;
    //    correctedcolor.UP = 0;
    //    correctedcolor.VP = 0;


    //  PerceptionLib.RGBValue rgb = new PerceptionLib.RGBValue();


    //  rgb = PerceptionLib.Color.ToRBG(Correctedcolor);


    //  if (txt_R.Text.ToString() == rgb.R.ToString() && txt_G.Text.ToString() == rgb.G.ToString() && txt_B.Text.ToString() == rgb.B.ToString())
    //      System.Windows.MessageBox.Show("matched R:" + rgb.R + "G:" + rgb.G + "B:" + rgb.B);
    //  else
    //      System.Windows.MessageBox.Show("didn matchR:" + rgb.R + "G:" + rgb.G + "B:" + rgb.B);


    //}

    private void Btn_ExportGrid_Click(object sender, RoutedEventArgs e)
    {

      DataTable dt = new DataTable();


      dt = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      PerceptionLib.CSV.ToCSVFromDataTable(dt);
      DataTable table = CSV.GetDataTableFromCSV(@"C:\see-through-project\gt\STColorCorrection\Src\PerceptionLib\bin\GridData.csv");
      if (table.Columns.Count == 0)
        System.Windows.MessageBox.Show("Error!");
      else
        System.Windows.MessageBox.Show("Success!");
    }

    private void Btn_ImportGrid_Click(object sender, RoutedEventArgs e)
    {
      cmb_graph.IsEnabled = true;
      Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
      dlg.FileName = "Document"; // Default file name 

      dlg.DefaultExt = ".CSV"; // Default file extension 
      dlg.Filter = "Text documents (.csv)|*.CSV"; // Filter files by extension 
      //dlg.DefaultExt = ".xlsx"; // Default file extension 
      //dlg.Filter = "Text documents (.xlsx)|*.XLSX"; // Filter files by extension 

      // Show open file dialog box 
      Nullable<bool> result = dlg.ShowDialog();
      string filename;
      // Process open file dialog box results 
      if (result == true)
      {
        // Open document 
        filename = dlg.FileName;
        PopulateGrid(filename);
        dataTable = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
      }
      else
      {
        System.Windows.MessageBox.Show("File not loaded!");
      }

      pairs.Clear();

      //gamut data has 64 coloms
      if (dataTable.Columns.Count == 64)
      {

        for (int i = 0; i < dataTable.Rows.Count; i++)
        {
          pairs.Add(new MeasurementPair()
          {
            ColorToShow = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][6].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][7].ToString()) },
            ColorCaptured = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][11].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][12].ToString()) }
          });
        }
      }

      if (dataTable.Columns.Count == 183)
      {
        FgNo = 88;

        for (int i = 0; i < dataTable.Rows.Count; i++)
        {
          pairs.Add(new MeasurementPair()
          {
            ColorToShow = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][6].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][7].ToString()) },
            ColorCaptured = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][101].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][102].ToString()) },
            BgColor = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][76].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][77].ToString()) }
          });
        }
      }


    }

    private void ColorCapturedUpdate()
    {
      DateTime Start = DateTime.Now;
      // to measure LUV from Color class
      ColorToShow = Util.ColorSpaceConverter.ToGetLUV(R, G, B);
      ColorToShowXYZ = Util.ColorSpaceConverter.ToGetXYZ(R, G, B);

      //ProjectedColor = PerceptionLib.Color.RGBtoHEX(R, G, B);

      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
       // rec_shown.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
        //rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
        rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));

      }));
      //time2 = DateTime.Now.Subtract(Start).ToString();
    }

    private void StartCapture()
    // private void ColourcaputreWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      // background processor
      //Imagecaputre = new BackgroundWorker();
      //System.Windows.Threading.Dispatcher captureNow = mainW.Dispatcher;
      // temp int cariables to do the calculations for AVG rgb from the cropped pics
      int tempMr = 0, tempMg = 0, tempMb = 0;
      for (int i = 0; i < 5; i++)
      {

        //Imagecaputre.DoWork += delegate(object s, DoWorkEventArgs args)
        //{
        GetImage();
        CropImage();
        //captureNow.BeginInvoke(DisplayMeasuredValues);
        //};

        if (i == 0)
        {
          tempMr = 0;
          tempMg = 0;
          tempMb = 0;

        }

        tempMr = (int)(tempMr + avgRGB.Red);
        tempMg = (int)(tempMg + avgRGB.Green);
        tempMb = (int)(tempMb + avgRGB.Blue);
      }

      // since the first img the cam captures is black for some reason we are ommiting it and calculating for the rest
      tempMr = tempMr / 5;
      tempMg = tempMg / 5;
      tempMb = tempMb / 5;

      // getting the avg values as int for calculation then changing them to bye for passing into system.darawing.color obj's
      MR = (byte)(tempMr);
      MG = (byte)(tempMg);
      MB = (byte)(tempMb);


      //Imagecaputre.RunWorkerAsync();
    }
    private PerceptionLib.Color StartCapture1()
    // private void ColourcaputreWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      // background processor
      //Imagecaputre = new BackgroundWorker();
      //System.Windows.Threading.Dispatcher captureNow = mainW.Dispatcher;
      // temp int cariables to do the calculations for AVG rgb from the cropped pics
      int tempMr = 0, tempMg = 0, tempMb = 0;
      for (int i = 0; i < 1; i++)
      {

        //Imagecaputre.DoWork += delegate(object s, DoWorkEventArgs args)
        //{
        GetImage();
        CropImage();
        //captureNow.BeginInvoke(DisplayMeasuredValues);
        //};

        if (i == 0)
        {
          tempMr = 0;
          tempMg = 0;
          tempMb = 0;

        }

        tempMr = (int)(tempMr + avgRGB.Red);
        tempMg = (int)(tempMg + avgRGB.Green);
        tempMb = (int)(tempMb + avgRGB.Blue);
      }

      // since the first img the cam captures is black for some reason we are ommiting it and calculating for the rest
      tempMr = tempMr / 1;
      tempMg = tempMg / 1;
      tempMb = tempMb / 1;

      // getting the avg values as int for calculation then changing them to bye for passing into system.darawing.color obj's
      MR = (byte)(tempMr);
      MG = (byte)(tempMg);
      MB = (byte)(tempMb);

      return colorMeasured;
      //Imagecaputre.RunWorkerAsync();
    }

    /// <summary>
    /// funtions to capture Image via web cam , Crop it to its center , and get avg RGB value of its center
    /// </summary>

    private void GetImage()
    {
      // make the camera wait 500 milli sec before it caprtures a img
      System.Threading.Thread.Sleep(500);
      //capture device is a global obj
      captureImage = captureDevice.QueryFrame();
      croppedImage = captureImage.Copy();
    }

    /// <summary>
    /// function to crop the img to its center and get its avg RGB value
    /// </summary>
    private void CropImage()
    {
      int Center_x, Center_y;
      Center_x = croppedImage.Width / 2;
      Center_y = croppedImage.Height / 2;
      avgRGB = new Bgr();

      //croppedImage.ROI = new System.Drawing.Rectangle(Center_x, Center_y, 100, 100);
      croppedImage.ROI = new System.Drawing.Rectangle(Center_x, Center_y, 5, 5);
      avgRGB = croppedImage.GetAverage();


      //captureDevice.Dispose();

      //((IDisposable)captureDevice).Dispose();
      //((IDisposable)croppedImage).Dispose();
      ////((IDisposable)captureImage).Dispose();
    }

    /// <summary>
    /// to display all measyre color's values 
    /// </summary>

    private void DisplayMeasuredValues()
    {
      ColorMeasured = Util.ColorSpaceConverter.ToGetLUV(MR, MG, MB);
      ColorMeasuredXYZ = Util.ColorSpaceConverter.ToGetXYZ(MR, MG, MB);
      //to display the color in the rectangle 

      Rectangle_Captured.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(MR, MG, MB));

      if (captureImage != null)
      {

        Image_Camera.Source = Util.ToImageSourceConverter.ToBitmapSource(captureImage);
        // to dispose the query frame instance
        //   captureDevice.QueryFrame().Dispose();
        //  captureDevice.Dispose();

      }


      Image_Croped.Source = Util.ToImageSourceConverter.ToBitmapSource(croppedImage);

      captureImage.Dispose();
      croppedImage.Dispose();

    }

    private void DisplayMeasuredValuesFromCs200() 
    {
      DateTime Start = DateTime.Now;
      //ad this sleep to normal code other than phone
//System.Threading.Thread.Sleep(100);
      DateTime Start1 = DateTime.Now;
      
      //dont ad  normal code other than phone
      //ColorMeasuredXYZ = PerceptionLib.Cs200Connection.StartMeasureXYZForPhone();

      ColorMeasuredXYZ = PerceptionLib.Cs200Connection.StartMeasureXYZ();
      //ColorMeasuredXYZ = PerceptionLib.Cs200Connection.StartAVGMeasureXYZ9();
      //ColorMeasuredXYZ = PerceptionLib.Cs200Connection.StartAVGMeasureXYZ2();
      //ColorMeasuredXYZ = PerceptionLib.Cs200Connection.StartAVGMeasureXYZ3();

      time4 = DateTime.Now.Subtract(Start1).ToString();
      ColorMeasured = PerceptionLib.Color.ToLUV(ColorMeasuredXYZ);

      PerceptionLib.RGBValue RGB = new PerceptionLib.RGBValue();

      RGB = PerceptionLib.Color.ToRBG(ColorMeasured);
      MR = RGB.R;
      MG = RGB.G;
      MB = RGB.B;

      //to display the color in the rectangle 
      //Dispatcher.Invoke(new Action(() => Rectangle_Captured.Fill = new SolidColorBrush((System.Windows.Media.Color.FromRgb(RGB.R, RGB.G, RGB.B)))));

      time3 = DateTime.Now.Subtract(Start).ToString();
    }

   

    private void DifferenceCalculation()
    {
      ColorDifference = new PerceptionLib.Color();
      colorDifference.L = ColorToShow.L - ColorMeasured.L;
      colorDifference.U = ColorToShow.U - ColorMeasured.U;
      colorDifference.V = ColorToShow.V - ColorMeasured.V;
      colorDifference.UP = ColorToShow.UP - ColorMeasured.UP;
      colorDifference.VP = ColorToShow.VP - ColorMeasured.VP;
    }

    private void progress(int i)
    {
      if (progressBar1.Value < BgNo)
        progressBar1.Value = i;
    }

    private void ColorUpdateOnScreenWithBG()
    {
      // to measure LUV from Color class
      ColorToShow = Util.ColorSpaceConverter.ToGetLUV(R, G, B);
      ColorToShowXYZ = Util.ColorSpaceConverter.ToGetXYZ(R, G, B);

      ColorBackGround = Util.ColorSpaceConverter.ToGetLUV(BgR, BgG, BgB);
      ColorBackGroundXYZ = Util.ColorSpaceConverter.ToGetXYZ(BgR, BgG, BgB);

      //rec_shown.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
      rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));

      rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BgR, BgG, BgB));
    }

    private void ColorCaptureJustBG()
    {
      // to measure LUV from Color class
      //ColorToShow = Util.ColorSpaceConverter.ToGetLUV(R, G, B);

      //rec_shown.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
      rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));

      rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BgR, BgG, BgB));
    }

    private void CSColorUpdateOnScreenWithBG()
    {
      // to measure LUV from Color class
      //ColorToShow = Util.ColorSpaceConverter.ToGetLUV(R, G, B);
      //ColorToShowXYZ = Util.ColorSpaceConverter.ToGetXYZ(R, G, B);

      //ColorBackGround = Util.ColorSpaceConverter.ToGetLUV(BgR, BgG, BgB);
      //ColorBackGroundXYZ = Util.ColorSpaceConverter.ToGetXYZ(BgR, BgG, BgB);

      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
        rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BgR, BgG, BgB));
      }));
    }
    

    private void Btn_MixedColour_Click(object sender, RoutedEventArgs e)
    {
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\ColorMixing1.txt");

      txt_R.IsEnabled = false;
      txt_G.IsEnabled = false;
      txt_B.IsEnabled = false;
      DataTable dt = new DataTable();
      // DataTable new_dt = new DataTable();
      DataRow newRow;
      dt = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      // to create a random number
      Random rnd = new Random();
      // to form byte number for bg and fg color
      Byte[] b = new Byte[3];
      Byte[] rgb = new Byte[3];



      // this loop is too changfe the bg color
      //  for (int i = 0; i < 5; i++)
      ThreadPool.QueueUserWorkItem(ignored =>
      {

        for (int i = 0; i < BgNo; i++)
        {
          // for the first set of vaule it tahes balck as the bg colour
          if (i == 0)
          {
            BgR = 0;
            BgG = 0;
            BgB = 0;
          }
          // then it tahes randndom value for BG
          else
          {
            rnd.NextBytes(b);
            BgR = b[0];
            BgG = b[1];
            BgB = b[2];
          }

          for (int j = 0; j < FgNo; j++)
          {
            if (i == 0)
            {
              rnd.NextBytes(rgb);

              R = rgb[0];
              G = rgb[1];
              B = rgb[2];
            }
            else
            {
              mainW.R = Convert.ToByte(dt.Rows[j][0].ToString());
              mainW.G = Convert.ToByte(dt.Rows[j][1].ToString());
              mainW.B = Convert.ToByte(dt.Rows[j][2].ToString());
            }


            //if (i != 0)
            //               System.Threading.Thread.Sleep(500);
            Dispatcher.Invoke(new Action(() => ColorUpdateOnScreenWithBG()));
            //           System.Windows.Forms.Application.DoEvents();


            colorMeasured = StartCapture1();

            Dispatcher.Invoke(new Action(() => DisplayMeasuredValues()));

            DifferenceCalculation();

            newRow = dt.NewRow();

            if (i == 0)
            {
              newRow[0] = R.ToString();
              newRow[1] = G.ToString();
              newRow[2] = B.ToString();
              newRow[3] = colorToShow.L.ToString();
              newRow[4] = colorToShow.U.ToString();
              newRow[5] = colorToShow.V.ToString();
              newRow[6] = colorToShow.UP.ToString();
              newRow[7] = colorToShow.VP.ToString();
              newRow[8] = ColorMeasured.L.ToString();
              newRow[9] = ColorMeasured.U.ToString();
              newRow[10] = ColorMeasured.V.ToString();
              newRow[11] = ColorMeasured.UP.ToString();
              newRow[12] = ColorMeasured.VP.ToString();
              newRow[13] = MR.ToString();
              newRow[14] = MG.ToString();
              newRow[15] = MB.ToString();
              newRow[16] = colorDifference.L.ToString();
              newRow[17] = colorDifference.U.ToString();
              newRow[18] = colorDifference.V.ToString();
              newRow[19] = BgR.ToString();
              newRow[20] = BgG.ToString();
              newRow[21] = BgB.ToString();
              newRow[22] = 0;
              newRow[23] = 0;
              newRow[24] = 0;
              newRow[25] = 0;
              newRow[26] = 0;
              newRow[27] = 0;
              newRow[28] = 0;
              newRow[29] = 0;
              newRow[30] = 0;
              newRow[31] = 0;
              newRow[32] = 0;
              newRow[33] = 0;
              newRow[34] = 0;
              newRow[35] = 0;
              newRow[36] = 0;
              newRow[37] = 0;

              pairs.Clear();
              pairs.Add(new MeasurementPair()
              {
                ColorToShow = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(colorToShow.UP.ToString()), VP = Convert.ToDouble(colorToShow.VP.ToString()) },
                ColorCaptured = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(ColorMeasured.UP.ToString()), VP = Convert.ToDouble(ColorMeasured.VP.ToString()) }
              });

            }
            else
            {

              newRow[0] = R.ToString();
              newRow[1] = G.ToString();
              newRow[2] = B.ToString();
              newRow[3] = colorToShow.L.ToString();
              newRow[4] = colorToShow.U.ToString();
              newRow[5] = colorToShow.V.ToString();
              newRow[6] = colorToShow.UP.ToString();
              newRow[7] = colorToShow.VP.ToString();

              newRow[8] = dt.Rows[j][8].ToString();
              newRow[9] = dt.Rows[j][9].ToString();
              newRow[10] = dt.Rows[j][10].ToString();
              newRow[11] = dt.Rows[j][11].ToString();
              newRow[12] = dt.Rows[j][12].ToString();
              newRow[13] = dt.Rows[j][13].ToString();
              newRow[14] = dt.Rows[j][14].ToString();
              newRow[15] = dt.Rows[j][15].ToString();

              newRow[16] = colorDifference.L.ToString();
              newRow[17] = colorDifference.U.ToString();
              newRow[18] = colorDifference.V.ToString();
              newRow[19] = BgR.ToString();
              newRow[20] = BgG.ToString();
              newRow[21] = BgB.ToString();
              newRow[22] = 0;
              newRow[23] = 0;
              newRow[24] = 0;
              newRow[25] = 0;
              newRow[26] = 0;
              newRow[27] = 0;
              newRow[28] = 0;
              newRow[29] = 0;
              newRow[30] = ColorMeasured.L.ToString();
              newRow[31] = ColorMeasured.U.ToString();
              newRow[32] = ColorMeasured.V.ToString();
              newRow[33] = ColorMeasured.UP.ToString();
              newRow[34] = ColorMeasured.VP.ToString();
              newRow[35] = MR.ToString();
              newRow[36] = MG.ToString();
              newRow[37] = MB.ToString();

              MixedColor = ColorMeasured;
              pairs.Clear();
              pairs.Add(new MeasurementPair()
              {
                // HERE 11 AND 12 ARE THE COLOUR CATPTURED BY THE CAMERA FOR DISPLAY 33 AND 34 ARE MIXED COLOURS
                ColorToShow = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dt.Rows[j][11].ToString()), VP = Convert.ToDouble(dt.Rows[j][12].ToString()) },
                ColorCaptured = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(ColorMeasured.UP.ToString()), VP = Convert.ToDouble(ColorMeasured.VP.ToString()) }
              });

            }

            R = 0; G = 0; B = 0;
            dt.Rows.Add(newRow);

            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt.DefaultView));
            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

          }
        }

        //   dtgrid_corrDisplay.ItemsSource = dt.DefaultView;
        //dtgrid_corrDisplay.Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt.DefaultView));
        // for caluclating just bg colour,.. with  fg & bg white conditions so this for loop should be 3 more than loop for bg color change
        for (int i = 1; i < BgNo + 3; i++)
        {
          if (i < BgNo)
          {
            R = 0;
            G = 0;
            B = 0;

            BgR = Convert.ToByte(dt.Rows[i * FgNo][19].ToString());
            BgG = Convert.ToByte(dt.Rows[i * FgNo][20].ToString());
            BgB = Convert.ToByte(dt.Rows[i * FgNo][21].ToString());

          }
          // both white 
          else if (i == BgNo)
          {
            R = 255;
            G = 255;
            B = 255;

            BgR = 255;
            BgG = 255;
            BgB = 255;
          }
          // fg white
          else if (i == BgNo + 1)
          {
            R = 255;
            G = 255;
            B = 255;

            BgR = 0;
            BgG = 0;
            BgB = 0;
          }
          // bg white
          else
          {
            R = 0;
            G = 0;
            B = 0;

            BgR = 255;
            BgG = 255;
            BgB = 255;

          }


          ////if (i != 0)
          //System.Threading.Thread.Sleep(500);
          //ColorCaptureJustBG();
          //System.Windows.Forms.Application.DoEvents();

          ////does all the caputure and difference calculations
          //System.Threading.Thread.Sleep(500);
          //// StartCapture();

          Dispatcher.Invoke(new Action(() => ColorCaptureJustBG()));
          //           System.Windows.Forms.Application.DoEvents();


          colorMeasured = StartCapture1();

          Dispatcher.Invoke(new Action(() => DisplayMeasuredValues()));

          DifferenceCalculation();

          //just bg capture
          newRow = dt.NewRow();
          if (i < BgNo)
          {

            newRow[0] = R.ToString();
            newRow[1] = G.ToString();
            newRow[2] = B.ToString();
            newRow[3] = 0;
            newRow[4] = 0;
            newRow[5] = 0;
            newRow[6] = 0;
            newRow[7] = 0;
            newRow[8] = 0;
            newRow[9] = 0;
            newRow[10] = 0;
            newRow[11] = 0;
            newRow[12] = 0;
            newRow[13] = 0;
            newRow[14] = 0;
            newRow[15] = 0;
            newRow[16] = 0;
            newRow[17] = 0;
            newRow[18] = 0;
            newRow[19] = BgR.ToString();
            newRow[20] = BgG.ToString();
            newRow[21] = BgB.ToString();
            newRow[22] = MR.ToString();
            newRow[23] = MG.ToString();
            newRow[24] = MB.ToString();
            newRow[25] = ColorMeasured.L.ToString();
            newRow[26] = ColorMeasured.U.ToString();
            newRow[27] = ColorMeasured.V.ToString();
            newRow[28] = ColorMeasured.UP.ToString();
            newRow[29] = ColorMeasured.VP.ToString();
            newRow[30] = 0;
            newRow[31] = 0;
            newRow[32] = 0;
            newRow[33] = 0;
            newRow[34] = 0;
            newRow[35] = 0;
            newRow[36] = 0;
            newRow[37] = 0;

            pairs.Clear();

            BgColor = ColorMeasured;
            pairs.Add(new MeasurementPair()
            {//THE COLOUR DISPLAYED HERE ARE THE BG COLOUR CAPTURED
              BgColor = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(ColorMeasured.UP.ToString()), VP = Convert.ToDouble(ColorMeasured.VP.ToString()) }
            });
          }
          //white point capture
          else
          {
            newRow[0] = R.ToString();
            newRow[1] = G.ToString();
            newRow[2] = B.ToString();
            newRow[3] = 0;
            newRow[4] = 0;
            newRow[5] = 0;
            newRow[6] = 0;
            newRow[7] = 0;
            newRow[8] = 0;
            newRow[9] = 0;
            newRow[10] = 0;
            newRow[11] = 0;
            newRow[12] = 0;
            newRow[13] = 0;
            newRow[14] = 0;
            newRow[15] = 0;
            newRow[16] = 0;
            newRow[17] = 0;
            newRow[18] = 0;
            newRow[19] = BgR.ToString();
            newRow[20] = BgG.ToString();
            newRow[21] = BgB.ToString();
            newRow[22] = 0;
            newRow[23] = 0;
            newRow[24] = 0;
            newRow[25] = 0;
            newRow[26] = 0;
            newRow[27] = 0;
            newRow[28] = 0;
            newRow[29] = 0;
            newRow[30] = ColorMeasured.L.ToString();
            newRow[31] = ColorMeasured.U.ToString();
            newRow[32] = ColorMeasured.V.ToString();
            newRow[33] = ColorMeasured.UP.ToString();
            newRow[34] = ColorMeasured.VP.ToString();
            newRow[35] = MR.ToString();
            newRow[36] = MG.ToString();
            newRow[37] = MB.ToString();

            pairs.Add(new MeasurementPair()
            {
              // HERE 11 AND 12 ARE THE COLOUR CATPTURED BY THE CAMERA FOR DISPLAY 33 AND 34 ARE MIXED COLOURS
              BgColor = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(ColorMeasured.UP.ToString()), VP = Convert.ToDouble(ColorMeasured.VP.ToString()) }
            });


          }

          R = 10;
          dt.Rows.Add(newRow);

          Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt.DefaultView));
          Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
        }



        //taking back the measured value of bg to all the data set
        int totalRow = BgNo * FgNo;
        for (int i = 1; i < BgNo; i++)
        {

          for (int j = 0; j < fgNo; j++)
          {
            int rowNo = (i * FgNo) + j;
            // i am doing -1 cos the data set starts from 0 ,.. so in the first one it will still b e bg*fg but i value will be 0
            dt.Rows[rowNo][22] = dt.Rows[(totalRow) + i - 1][22].ToString();
            dt.Rows[rowNo][23] = dt.Rows[(totalRow) + i - 1][23].ToString();
            dt.Rows[rowNo][24] = dt.Rows[(totalRow) + i - 1][24].ToString();
            dt.Rows[rowNo][25] = dt.Rows[(totalRow) + i - 1][25].ToString();
            dt.Rows[rowNo][26] = dt.Rows[(totalRow) + i - 1][26].ToString();
            dt.Rows[rowNo][27] = dt.Rows[(totalRow) + i - 1][27].ToString();
            dt.Rows[rowNo][28] = dt.Rows[(totalRow) + i - 1][28].ToString();
            dt.Rows[rowNo][29] = dt.Rows[(totalRow) + i - 1][29].ToString();


          }
        }

        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt.DefaultView));
        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
        dataTable = dt;
      });
      cmb_graph.IsEnabled = true;
      btn_ExportGrid.IsEnabled = true;


    }

    private void Btn_StdColorData_Click(object sender, RoutedEventArgs e)
    {
      //  PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\StdColor.txt");
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\StdColortriangle.txt");
      txt_R.IsEnabled = false;
      txt_G.IsEnabled = false;
      txt_B.IsEnabled = false;
      txt_BgNo.IsEnabled = false;
      txt_FgNo.IsEnabled = false;
      btn_MixedColour.IsEnabled = false;
      DataTable dt = new DataTable();
      // DataTable new_dt = new DataTable();
      DataRow newRow;
      dt = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      // to create a random number
      //BgNo = 5;
      BgNo = 20;
      //FgNo = 27;
      FgNo = 61;



      // this loop is too changfe the bg color
      //  for (int i = 0; i < 5; i++)
      ThreadPool.QueueUserWorkItem(ignored =>
      {
        PerceptionLib.RGBValue rgb = new PerceptionLib.RGBValue();
        for (int i = 0; i < BgNo; i++)
        {

          // for the first set of vaule it tahes balck as the bg colour
          switch (i)
          {
            // black bg
            case 0:
              BgR = 0;
              BgG = 0;
              BgB = 0;
              break;
            //Brick
            case 1:
              BgColor = new PerceptionLib.Color();
              bgcolour.L = 65;
              bgcolour.UP = 0.275;
              bgcolour.VP = 0.485;
              bgcolour.U = (13 * bgcolour.L) * (bgcolour.UP - bgcolour.UR);
              bgcolour.V = (13 * bgcolour.L) * (bgcolour.VP - bgcolour.VR);


              rgb = PerceptionLib.Color.ToRBG(bgcolour);
              BgR = rgb.R;
              BgG = rgb.G;
              BgB = rgb.B;
              break;
            //Foliage
            case 2:
              BgColor = new PerceptionLib.Color();
              bgcolour.L = 65;
              bgcolour.UP = 0.123;
              bgcolour.VP = 0.531;
              bgcolour.U = (13 * bgcolour.L) * (bgcolour.UP - bgcolour.UR);
              bgcolour.V = (13 * bgcolour.L) * (bgcolour.VP - bgcolour.VR);

              rgb = PerceptionLib.Color.ToRBG(bgcolour);
              BgR = rgb.R;
              BgG = rgb.G;
              BgB = rgb.B;
              break;
            //Sidewalk
            case 3:
              BgColor = new PerceptionLib.Color();
              bgcolour.L = 65;
              bgcolour.UP = 0.211;
              bgcolour.VP = 0.489;
              bgcolour.U = (13 * bgcolour.L) * (bgcolour.UP - bgcolour.UR);
              bgcolour.V = (13 * bgcolour.L) * (bgcolour.VP - bgcolour.VR);

              rgb = PerceptionLib.Color.ToRBG(bgcolour);
              BgR = rgb.R;
              BgG = rgb.G;
              BgB = rgb.B;

              break;

            //Pavement
            case 4:
              BgColor = new PerceptionLib.Color();
              bgcolour.L = 65;
              bgcolour.UP = 0.203;
              bgcolour.VP = 0.470;
              bgcolour.U = (13 * bgcolour.L) * (bgcolour.UP - bgcolour.UR);
              bgcolour.V = (13 * bgcolour.L) * (bgcolour.VP - bgcolour.VR);

              rgb = PerceptionLib.Color.ToRBG(bgcolour);
              BgR = rgb.R;
              BgG = rgb.G;
              BgB = rgb.B;
              break;


            //vaiants of blue
            case 5:
              BgR = 0;
              BgG = 0;
              BgB = 255;
              break;

            case 6:
              BgR = 0;
              BgG = 0;
              BgB = 225;
              break;

            case 7:
              BgR = 100;
              BgG = 0;
              BgB = 255;
              break;

            case 8:
              BgR = 0;
              BgG = 100;
              BgB = 255;
              break;

            //variants of green

            case 9:
              BgR = 0;
              BgG = 255;
              BgB = 0;
              break;

            case 10:
              BgR = 0;
              BgG = 225;
              BgB = 0;
              break;

            case 11:
              BgR = 100;
              BgG = 255;
              BgB = 0;
              break;

            case 12:
              BgR = 0;
              BgG = 255;
              BgB = 100;
              break;

            //yellow

            case 13:
              BgR = 255;
              BgG = 255;
              BgB = 0;
              break;

            case 14:
              BgR = 200;
              BgG = 200;
              BgB = 0;
              break;

            case 15:
              BgR = 200;
              BgG = 255;
              BgB = 0;
              break;

            //red
            case 16:
              BgR = 255;
              BgG = 0;
              BgB = 0;
              break;

            case 17:
              BgR = 200;
              BgG = 0;
              BgB = 0;
              break;

            case 18:
              BgR = 255;
              BgG = 100;
              BgB = 0;
              break;

            case 19:
              BgR = 255;
              BgG = 0;
              BgB = 100;
              break;


          }


          for (int j = 0; j < FgNo; j++)
          {
            //if (i == 0)
            //{
            //    mainW.R = Convert.ToByte(dt.Rows[j][0].ToString());
            //    mainW.G = Convert.ToByte(dt.Rows[j][1].ToString());
            //    mainW.B = Convert.ToByte(dt.Rows[j][2].ToString());
            //}
            //else
            //{
            mainW.R = Convert.ToByte(dt.Rows[j][0].ToString());
            mainW.G = Convert.ToByte(dt.Rows[j][1].ToString());
            mainW.B = Convert.ToByte(dt.Rows[j][2].ToString());
            //}



            //if (i != 0)
            //               System.Threading.Thread.Sleep(500);
            Dispatcher.Invoke(new Action(() => ColorUpdateOnScreenWithBG()));
            //           System.Windows.Forms.Application.DoEvents();


            colorMeasured = StartCapture1();

            Dispatcher.Invoke(new Action(() => DisplayMeasuredValues()));

            DifferenceCalculation();



            if (i == 0)
            {
              dt.Rows[j][0] = R.ToString();
              dt.Rows[j][1] = G.ToString();
              dt.Rows[j][2] = B.ToString();
              dt.Rows[j][3] = colorToShow.L.ToString();
              dt.Rows[j][4] = colorToShow.U.ToString();
              dt.Rows[j][5] = colorToShow.V.ToString();
              dt.Rows[j][6] = colorToShow.UP.ToString();
              dt.Rows[j][7] = colorToShow.VP.ToString();
              dt.Rows[j][8] = ColorMeasured.L.ToString();
              dt.Rows[j][9] = ColorMeasured.U.ToString();
              dt.Rows[j][10] = ColorMeasured.V.ToString();
              dt.Rows[j][11] = ColorMeasured.UP.ToString();
              dt.Rows[j][12] = ColorMeasured.VP.ToString();
              dt.Rows[j][13] = MR.ToString();
              dt.Rows[j][14] = MG.ToString();
              dt.Rows[j][15] = MB.ToString();
              dt.Rows[j][16] = colorDifference.L.ToString();
              dt.Rows[j][17] = colorDifference.U.ToString();
              dt.Rows[j][18] = colorDifference.V.ToString();
              dt.Rows[j][19] = BgR.ToString();
              dt.Rows[j][20] = BgG.ToString();
              dt.Rows[j][21] = BgB.ToString();
              dt.Rows[j][22] = 0;
              dt.Rows[j][23] = 0;
              dt.Rows[j][24] = 0;
              dt.Rows[j][25] = 0;
              dt.Rows[j][26] = 0;
              dt.Rows[j][27] = 0;
              dt.Rows[j][28] = 0;
              dt.Rows[j][29] = 0;
              dt.Rows[j][30] = 0;
              dt.Rows[j][31] = 0;
              dt.Rows[j][32] = 0;
              dt.Rows[j][33] = 0;
              dt.Rows[j][34] = 0;
              dt.Rows[j][35] = 0;
              dt.Rows[j][36] = 0;
              dt.Rows[j][37] = 0;

              pairs.Clear();
              pairs.Add(new MeasurementPair()
              {
                ColorToShow = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(colorToShow.UP.ToString()), VP = Convert.ToDouble(colorToShow.VP.ToString()) },
                ColorCaptured = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(ColorMeasured.UP.ToString()), VP = Convert.ToDouble(ColorMeasured.VP.ToString()) }
              });

            }
            else
            {

              newRow = dt.NewRow();
              newRow[0] = R.ToString();
              newRow[1] = G.ToString();
              newRow[2] = B.ToString();
              newRow[3] = colorToShow.L.ToString();
              newRow[4] = colorToShow.U.ToString();
              newRow[5] = colorToShow.V.ToString();
              newRow[6] = colorToShow.UP.ToString();
              newRow[7] = colorToShow.VP.ToString();

              newRow[8] = dt.Rows[j][8].ToString();
              newRow[9] = dt.Rows[j][9].ToString();
              newRow[10] = dt.Rows[j][10].ToString();
              newRow[11] = dt.Rows[j][11].ToString();
              newRow[12] = dt.Rows[j][12].ToString();
              newRow[13] = dt.Rows[j][13].ToString();
              newRow[14] = dt.Rows[j][14].ToString();
              newRow[15] = dt.Rows[j][15].ToString();

              newRow[16] = colorDifference.L.ToString();
              newRow[17] = colorDifference.U.ToString();
              newRow[18] = colorDifference.V.ToString();
              newRow[19] = BgR.ToString();
              newRow[20] = BgG.ToString();
              newRow[21] = BgB.ToString();
              newRow[22] = 0;
              newRow[23] = 0;
              newRow[24] = 0;
              newRow[25] = 0;
              newRow[26] = 0;
              newRow[27] = 0;
              newRow[28] = 0;
              newRow[29] = 0;
              newRow[30] = ColorMeasured.L.ToString();
              newRow[31] = ColorMeasured.U.ToString();
              newRow[32] = ColorMeasured.V.ToString();
              newRow[33] = ColorMeasured.UP.ToString();
              newRow[34] = ColorMeasured.VP.ToString();
              newRow[35] = MR.ToString();
              newRow[36] = MG.ToString();
              newRow[37] = MB.ToString();

              MixedColor = ColorMeasured;
              pairs.Clear();
              pairs.Add(new MeasurementPair()
              {
                // HERE 11 AND 12 ARE THE COLOUR CATPTURED BY THE CAMERA FOR DISPLAY 33 AND 34 ARE MIXED COLOURS
                ColorToShow = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dt.Rows[j][11].ToString()), VP = Convert.ToDouble(dt.Rows[j][12].ToString()) },
                ColorCaptured = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(ColorMeasured.UP.ToString()), VP = Convert.ToDouble(ColorMeasured.VP.ToString()) }
              });
              dt.Rows.Add(newRow);

            }

            R = 0; G = 0; B = 0;
            //dt.Rows.Add(newRow);

            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt.DefaultView));
            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

          }
        }

        //   dtgrid_corrDisplay.ItemsSource = dt.DefaultView;
        //dtgrid_corrDisplay.Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt.DefaultView));
        // for caluclating just bg colour,.. with  fg & bg white conditions so this for loop should be 3 more than loop for bg color change
        for (int i = 1; i < BgNo + 3; i++)
        {
          if (i < BgNo)
          {
            R = 0;
            G = 0;
            B = 0;

            BgR = Convert.ToByte(dt.Rows[i * FgNo][19].ToString());
            BgG = Convert.ToByte(dt.Rows[i * FgNo][20].ToString());
            BgB = Convert.ToByte(dt.Rows[i * FgNo][21].ToString());

          }
          // both white 
          else if (i == BgNo)
          {
            R = 255;
            G = 255;
            B = 255;

            BgR = 255;
            BgG = 255;
            BgB = 255;
          }
          // fg white
          else if (i == BgNo + 1)
          {
            R = 255;
            G = 255;
            B = 255;

            BgR = 0;
            BgG = 0;
            BgB = 0;
          }
          // bg white
          else
          {
            R = 0;
            G = 0;
            B = 0;

            BgR = 255;
            BgG = 255;
            BgB = 255;

          }


          ////if (i != 0)
          //System.Threading.Thread.Sleep(500);
          //ColorCaptureJustBG();
          //System.Windows.Forms.Application.DoEvents();

          ////does all the caputure and difference calculations
          //System.Threading.Thread.Sleep(500);
          //// StartCapture();

          Dispatcher.Invoke(new Action(() => ColorCaptureJustBG()));
          //           System.Windows.Forms.Application.DoEvents();


          colorMeasured = StartCapture1();

          Dispatcher.Invoke(new Action(() => DisplayMeasuredValues()));

          DifferenceCalculation();

          //just bg capture
          newRow = dt.NewRow();
          if (i < BgNo)
          {

            newRow[0] = R.ToString();
            newRow[1] = G.ToString();
            newRow[2] = B.ToString();
            newRow[3] = 0;
            newRow[4] = 0;
            newRow[5] = 0;
            newRow[6] = 0;
            newRow[7] = 0;
            newRow[8] = 0;
            newRow[9] = 0;
            newRow[10] = 0;
            newRow[11] = 0;
            newRow[12] = 0;
            newRow[13] = 0;
            newRow[14] = 0;
            newRow[15] = 0;
            newRow[16] = 0;
            newRow[17] = 0;
            newRow[18] = 0;
            newRow[19] = BgR.ToString();
            newRow[20] = BgG.ToString();
            newRow[21] = BgB.ToString();
            newRow[22] = MR.ToString();
            newRow[23] = MG.ToString();
            newRow[24] = MB.ToString();
            newRow[25] = ColorMeasured.L.ToString();
            newRow[26] = ColorMeasured.U.ToString();
            newRow[27] = ColorMeasured.V.ToString();
            newRow[28] = ColorMeasured.UP.ToString();
            newRow[29] = ColorMeasured.VP.ToString();
            newRow[30] = 0;
            newRow[31] = 0;
            newRow[32] = 0;
            newRow[33] = 0;
            newRow[34] = 0;
            newRow[35] = 0;
            newRow[36] = 0;
            newRow[37] = 0;

            pairs.Clear();

            BgColor = ColorMeasured;
            pairs.Add(new MeasurementPair()
            {//THE COLOUR DISPLAYED HERE ARE THE BG COLOUR CAPTURED
              BgColor = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(ColorMeasured.UP.ToString()), VP = Convert.ToDouble(ColorMeasured.VP.ToString()) }
            });
          }
          //white point capture
          else
          {
            newRow[0] = R.ToString();
            newRow[1] = G.ToString();
            newRow[2] = B.ToString();
            newRow[3] = 0;
            newRow[4] = 0;
            newRow[5] = 0;
            newRow[6] = 0;
            newRow[7] = 0;
            newRow[8] = 0;
            newRow[9] = 0;
            newRow[10] = 0;
            newRow[11] = 0;
            newRow[12] = 0;
            newRow[13] = 0;
            newRow[14] = 0;
            newRow[15] = 0;
            newRow[16] = 0;
            newRow[17] = 0;
            newRow[18] = 0;
            newRow[19] = BgR.ToString();
            newRow[20] = BgG.ToString();
            newRow[21] = BgB.ToString();
            newRow[22] = 0;
            newRow[23] = 0;
            newRow[24] = 0;
            newRow[25] = 0;
            newRow[26] = 0;
            newRow[27] = 0;
            newRow[28] = 0;
            newRow[29] = 0;
            newRow[30] = ColorMeasured.L.ToString();
            newRow[31] = ColorMeasured.U.ToString();
            newRow[32] = ColorMeasured.V.ToString();
            newRow[33] = ColorMeasured.UP.ToString();
            newRow[34] = ColorMeasured.VP.ToString();
            newRow[35] = MR.ToString();
            newRow[36] = MG.ToString();
            newRow[37] = MB.ToString();

            pairs.Add(new MeasurementPair()
            {
              // HERE 11 AND 12 ARE THE COLOUR CATPTURED BY THE CAMERA FOR DISPLAY 33 AND 34 ARE MIXED COLOURS
              BgColor = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(ColorMeasured.UP.ToString()), VP = Convert.ToDouble(ColorMeasured.VP.ToString()) }
            });


          }

          R = 10;
          dt.Rows.Add(newRow);

          Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt.DefaultView));
          Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
        }



        //taking back the measured value of bg to all the data set
        int totalRow = BgNo * FgNo;
        for (int i = 1; i < BgNo; i++)
        {

          for (int j = 0; j < fgNo; j++)
          {
            int rowNo = (i * FgNo) + j;
            // i am doing -1 cos the data set starts from 0 ,.. so in the first one it will still b e bg*fg but i value will be 0
            dt.Rows[rowNo][22] = dt.Rows[(totalRow) + i - 1][22].ToString();
            dt.Rows[rowNo][23] = dt.Rows[(totalRow) + i - 1][23].ToString();
            dt.Rows[rowNo][24] = dt.Rows[(totalRow) + i - 1][24].ToString();
            dt.Rows[rowNo][25] = dt.Rows[(totalRow) + i - 1][25].ToString();
            dt.Rows[rowNo][26] = dt.Rows[(totalRow) + i - 1][26].ToString();
            dt.Rows[rowNo][27] = dt.Rows[(totalRow) + i - 1][27].ToString();
            dt.Rows[rowNo][28] = dt.Rows[(totalRow) + i - 1][28].ToString();
            dt.Rows[rowNo][29] = dt.Rows[(totalRow) + i - 1][29].ToString();


          }
        }

        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt.DefaultView));
        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
        dataTable = dt;
      });
      cmb_graph.IsEnabled = true;
      btn_ExportGrid.IsEnabled = true;
      // captureDevice.Dispose();
    }

    private void Btn_UseGridData_Click(object sender, RoutedEventArgs e)
    {
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\color.txt");
      dtgrid_corrDisplay.IsEnabled = false;

      btn_StartMeasurment.IsEnabled = false;
      txt_R.IsEnabled = false;
      txt_G.IsEnabled = false;
      txt_B.IsEnabled = false;
      DataTable dt = new DataTable();
      DataTable new_dt = new DataTable();

      dt = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
      ThreadPool.QueueUserWorkItem(ignored =>
      {

        //for (int i = 1; i < dt.Rows.Count; i++)
        for (int i = 0; i < dt.Rows.Count; i++)
        {
          mainW.R = Convert.ToByte(dt.Rows[i][0].ToString());
          mainW.G = Convert.ToByte(dt.Rows[i][1].ToString());
          mainW.B = Convert.ToByte(dt.Rows[i][2].ToString());

          // converts rendered RGB to luv and displays the colour in to measure

          //NoArgDelegate fetcher = new NoArgDelegate(
          //        this.ColorCapturedUpdate);
          //Dispatcher.BeginInvoke();


          Dispatcher.Invoke(new Action(() => ColorCapturedUpdate()));
          //           System.Windows.Forms.Application.DoEvents();


          colorMeasured = StartCapture1();

          Dispatcher.Invoke(new Action(() => DisplayMeasuredValues()));

          DifferenceCalculation();

          //bradcolor=

          // assignes the data to a the datatable 
          dt.Rows[i][3] = colorToShow.L.ToString();
          dt.Rows[i][4] = colorToShow.U.ToString();
          dt.Rows[i][5] = colorToShow.V.ToString();
          dt.Rows[i][6] = colorToShow.UP.ToString();
          dt.Rows[i][7] = colorToShow.VP.ToString();
          dt.Rows[i][8] = ColorMeasured.L.ToString();
          dt.Rows[i][9] = ColorMeasured.U.ToString();
          dt.Rows[i][10] = ColorMeasured.V.ToString();
          dt.Rows[i][11] = ColorMeasured.UP.ToString();
          dt.Rows[i][12] = ColorMeasured.VP.ToString();
          //dt.Rows[i][13] = ColorDifference.L.ToString();
          //dt.Rows[i][14] = ColorDifference.U.ToString();
          //dt.Rows[i][15] = ColorDifference.V.ToString();

          ////

          dt.Rows[i][13] = colorToShow.LA.ToString();
          dt.Rows[i][14] = colorToShow.A.ToString();
          dt.Rows[i][15] = colorToShow.B.ToString();
          dt.Rows[i][16] = ColorMeasured.LA.ToString();
          dt.Rows[i][17] = ColorMeasured.A.ToString();
          dt.Rows[i][18] = ColorMeasured.B.ToString();

          //

          dt.Rows[i][19] = PerceptionLib.Color.ColorDistanceCal(colorToShow, ColorMeasured).ToString();
          dt.Rows[i][20] = PerceptionLib.Color.ColorDistanceCalAB(colorToShow, ColorMeasured).ToString();
          //
          dt.Rows[i][21] = ColorToShowXYZ.X.ToString();
          dt.Rows[i][22] = ColorToShowXYZ.Y.ToString();
          dt.Rows[i][23] = ColorToShowXYZ.Z.ToString();
          dt.Rows[i][24] = ColorMeasuredXYZ.X.ToString();
          dt.Rows[i][25] = ColorMeasuredXYZ.Y.ToString();
          dt.Rows[i][26] = ColorMeasuredXYZ.Z.ToString();

          dt.Rows[i][27] = MR.ToString();
          dt.Rows[i][28] = MG.ToString();
          dt.Rows[i][29] = MB.ToString();

          BradXYZ = Util.CATCalulation.bradford(ColorToShowXYZ);
          VonXYZ = Util.CATCalulation.VonKries(ColorToShowXYZ);
          ScalingXYZ = Util.CATCalulation.XYZScaling(ColorToShowXYZ);

          Bradcolor = PerceptionLib.Color.ToLUV(BradXYZ);
          Voncolor = PerceptionLib.Color.ToLUV(VonXYZ);
          Scalingcolor = PerceptionLib.Color.ToLUV(ScalingXYZ);


          bradRGB = PerceptionLib.Color.ToRBG(BradXYZ);
          VonRGB = PerceptionLib.Color.ToRBG(VonXYZ);
          scalingRGB = PerceptionLib.Color.ToRBG(ScalingXYZ);

          CalRGB = PerceptionLib.Color.ToRBG(ColorMeasuredXYZ);


          dt.Rows[i][30] = BradXYZ.X.ToString();
          dt.Rows[i][31] = BradXYZ.Y.ToString();
          dt.Rows[i][32] = BradXYZ.Z.ToString();
          dt.Rows[i][33] = PerceptionLib.Color.ColorDistanceCal(ColorMeasured, Bradcolor).ToString();
          dt.Rows[i][34] = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, Bradcolor).ToString();

          dt.Rows[i][35] = VonXYZ.X.ToString();
          dt.Rows[i][36] = VonXYZ.Y.ToString();
          dt.Rows[i][37] = VonXYZ.Z.ToString();
          dt.Rows[i][38] = PerceptionLib.Color.ColorDistanceCal(ColorMeasured, Voncolor).ToString();
          dt.Rows[i][39] = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, Voncolor).ToString();

          dt.Rows[i][40] = ScalingXYZ.X.ToString();
          dt.Rows[i][41] = ScalingXYZ.Y.ToString();
          dt.Rows[i][42] = ScalingXYZ.Z.ToString();
          dt.Rows[i][43] = PerceptionLib.Color.ColorDistanceCal(ColorMeasured, Scalingcolor).ToString();
          dt.Rows[i][44] = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, Scalingcolor).ToString();

          dt.Rows[i][45] = bradRGB.R.ToString();
          dt.Rows[i][46] = bradRGB.G.ToString();
          dt.Rows[i][47] = bradRGB.B.ToString();

          dt.Rows[i][48] = VonRGB.R.ToString();
          dt.Rows[i][49] = VonRGB.G.ToString();
          dt.Rows[i][50] = VonRGB.B.ToString();

          dt.Rows[i][51] = scalingRGB.R.ToString();
          dt.Rows[i][52] = scalingRGB.G.ToString();
          dt.Rows[i][53] = scalingRGB.B.ToString();

          dt.Rows[i][54] = CalRGB.R.ToString();
          dt.Rows[i][55] = CalRGB.G.ToString();
          dt.Rows[i][56] = CalRGB.B.ToString();

          dt.Rows[i][57] = Bradcolor.UP.ToString();
          dt.Rows[i][58] = Bradcolor.VP.ToString();

          dt.Rows[i][59] = Voncolor.UP.ToString();
          dt.Rows[i][60] = Voncolor.VP.ToString();

          dt.Rows[i][61] = Scalingcolor.UP.ToString();
          dt.Rows[i][62] = Scalingcolor.VP.ToString();


          pairs.Clear();
          pairs.Add(new MeasurementPair()
          {
            ColorToShow = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dt.Rows[i][6].ToString()), VP = Convert.ToDouble(dt.Rows[i][7].ToString()) },
            ColorCaptured = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dt.Rows[i][11].ToString()), VP = Convert.ToDouble(dt.Rows[i][12].ToString()) }
          });

          Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt.DefaultView));
          Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));


          // System.Threading.Thread.Sleep(10000);
        }

        // grid is populated with new datatable which has luv values
        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt.DefaultView));
        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));


        // to show all the pairs in cie graph
        pairs.Clear();

        for (int i = 0; i < dt.Rows.Count; i++)
        {
          pairs.Add(new MeasurementPair()
          {
            ColorToShow = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dt.Rows[i][6].ToString()), VP = Convert.ToDouble(dt.Rows[i][7].ToString()) },
            ColorCaptured = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dt.Rows[i][11].ToString()), VP = Convert.ToDouble(dt.Rows[i][12].ToString()) }
          });
        }
      });
      btn_ExportGrid.IsEnabled = true;

      dtgrid_corrDisplay.IsEnabled = true;
      dataTable = dt;



    }

    private void Btn_ColorPredictor_Click(object sender, RoutedEventArgs e)
    {
      //  PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\StdColor.txt");
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\predictor.csv");
      txt_R.IsEnabled = false;
      txt_G.IsEnabled = false;
      txt_B.IsEnabled = false;
      txt_BgNo.IsEnabled = false;
      txt_FgNo.IsEnabled = false;
      btn_MixedColour.IsEnabled = false;
      DataTable dt = new DataTable();
      // DataTable new_dt = new DataTable();
      DataRow newRow;
      dt = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      // to create a random number
      //BgNo = 5;
      BgNo = 24;
      //FgNo = 27;
      FgNo = dt.Rows.Count;


      // this loop is too changfe the bg color
      //  for (int i = 0; i < 5; i++)
      ThreadPool.QueueUserWorkItem(ignored =>
      {
        PerceptionLib.RGBValue rgb = new PerceptionLib.RGBValue();
        for (int i = 0; i < BgNo; i++)
        {

          // for the first set of vaule it tahes balck as the bg colour
          switch (i)
          {
            // black bg
            case 0:
              BgR = 0;
              BgG = 0;
              BgB = 0;
              break;
            //Brick

            case 1:

              BgR = 115;
              BgG = 80;
              BgB = 64;
              break;

            case 2:
              BgR = 195;
              BgG = 151;
              BgB = 130;
              break;



            //Pavement
            case 3:
              BgR = 94;
              BgG = 123;
              BgB = 156;
              break;


            //vaiants of blue
            case 4:
              BgR = 88;
              BgG = 108;
              BgB = 65;
              break;

            case 5:
              BgR = 100;
              BgG = 190;
              BgB = 171;
              break;

            case 6:
              BgR = 217;
              BgG = 122;
              BgB = 37;
              break;

            case 7:
              BgR = 72;
              BgG = 91;
              BgB = 165;
              break;

            //variants of green

            case 8:
              BgR = 194;
              BgG = 84;
              BgB = 98;
              break;

            case 9:
              BgR = 91;
              BgG = 59;
              BgB = 107;
              break;

            case 10:
              BgR = 115;
              BgG = 80;
              BgB = 64;
              break;

            case 11:
              BgR = 160;
              BgG = 188;
              BgB = 60;
              break;

            //yellow

            case 12:
              BgR = 230;
              BgG = 163;
              BgB = 42;
              break;

            case 13:
              BgR = 46;
              BgG = 60;
              BgB = 153;
              break;

            case 14:
              BgR = 71;
              BgG = 150;
              BgB = 69;
              break;

            //red
            case 15:
              BgR = 177;
              BgG = 44;
              BgB = 56;
              break;

            case 16:
              BgR = 238;
              BgG = 200;
              BgB = 27;
              break;

            case 17:
              BgR = 187;
              BgG = 82;
              BgB = 148;
              break;

            case 18:
              BgR = 243;
              BgG = 242;
              BgB = 237;
              break;

            case 19:
              BgR = 201;
              BgG = 201;
              BgB = 201;
              break;


            case 20:
              BgR = 161;
              BgG = 161;
              BgB = 161;
              break;

            case 21:
              BgR = 122;
              BgG = 122;
              BgB = 121;
              break;

            case 22:
              BgR = 83;
              BgG = 83;
              BgB = 83;
              break;

            case 23:
              BgR = 50;
              BgG = 49;
              BgB = 50;
              break;

          }


          for (int j = 0; j < FgNo; j++)
          {
            //if (i == 0)
            //{
            //    mainW.R = Convert.ToByte(dt.Rows[j][0].ToString());
            //    mainW.G = Convert.ToByte(dt.Rows[j][1].ToString());
            //    mainW.B = Convert.ToByte(dt.Rows[j][2].ToString());
            //}
            //else
            //{
            mainW.R = Convert.ToByte(dt.Rows[j][0].ToString());
            mainW.G = Convert.ToByte(dt.Rows[j][1].ToString());
            mainW.B = Convert.ToByte(dt.Rows[j][2].ToString());
            //}



            //if (i != 0)
            //               System.Threading.Thread.Sleep(500);
            Dispatcher.Invoke(new Action(() => ColorUpdateOnScreenWithBG()));
            //           System.Windows.Forms.Application.DoEvents();


            colorMeasured = StartCapture1();

            Dispatcher.Invoke(new Action(() => DisplayMeasuredValues()));
            Dispatcher.Invoke(new Action(() => progress(i)));

            DifferenceCalculation();


            BradXYZ = Util.CATCalulation.bradford(ColorToShowXYZ);
            VonXYZ = Util.CATCalulation.VonKries(ColorToShowXYZ);
            ScalingXYZ = Util.CATCalulation.XYZScaling(ColorToShowXYZ);

            Bradcolor = PerceptionLib.Color.ToLUV(BradXYZ);
            Voncolor = PerceptionLib.Color.ToLUV(VonXYZ);
            Scalingcolor = PerceptionLib.Color.ToLUV(ScalingXYZ);


            bradRGB = PerceptionLib.Color.ToRBG(BradXYZ);
            VonRGB = PerceptionLib.Color.ToRBG(VonXYZ);
            scalingRGB = PerceptionLib.Color.ToRBG(ScalingXYZ);

            CalRGB = PerceptionLib.Color.ToRBG(ColorMeasuredXYZ);



            if (i == 0)
            {
              //dt.Rows[j][0] = R.ToString();
              //dt.Rows[j][1] = G.ToString();
              //dt.Rows[j][2] = B.ToString();
              //dt.Rows[j][3] = colorToShow.L.ToString();
              //dt.Rows[j][4] = colorToShow.U.ToString();
              //dt.Rows[j][5] = colorToShow.V.ToString();
              //dt.Rows[j][6] = colorToShow.UP.ToString();
              //dt.Rows[j][7] = colorToShow.VP.ToString();
              //dt.Rows[j][8] = ColorMeasured.L.ToString();
              //dt.Rows[j][9] = ColorMeasured.U.ToString();
              //dt.Rows[j][10] = ColorMeasured.V.ToString();
              //dt.Rows[j][11] = ColorMeasured.UP.ToString();
              //dt.Rows[j][12] = ColorMeasured.VP.ToString();
              //dt.Rows[j][13] = MR.ToString();
              //dt.Rows[j][14] = MG.ToString();
              //dt.Rows[j][15] = MB.ToString();
              //dt.Rows[j][16] = colorDifference.L.ToString();
              //dt.Rows[j][17] = colorDifference.U.ToString();
              //dt.Rows[j][18] = colorDifference.V.ToString();
              //dt.Rows[j][19] = BgR.ToString();
              //dt.Rows[j][20] = BgG.ToString();
              //dt.Rows[j][21] = BgB.ToString();
              //dt.Rows[j][22] = 0;
              //dt.Rows[j][23] = 0;
              //dt.Rows[j][24] = 0;
              //dt.Rows[j][25] = 0;
              //dt.Rows[j][26] = 0;
              //dt.Rows[j][27] = 0;
              //dt.Rows[j][28] = 0;
              //dt.Rows[j][29] = 0;
              //dt.Rows[j][30] = 0;
              //dt.Rows[j][31] = 0;
              //dt.Rows[j][32] = 0;
              //dt.Rows[j][33] = 0;
              //dt.Rows[j][34] = 0;
              //dt.Rows[j][35] = 0;
              //dt.Rows[j][36] = 0;
              //dt.Rows[j][37] = 0;
              dt.Rows[j][0] = R.ToString();
              dt.Rows[j][1] = G.ToString();
              dt.Rows[j][2] = B.ToString();
              dt.Rows[j][3] = colorToShow.L.ToString();
              dt.Rows[j][4] = colorToShow.U.ToString();
              dt.Rows[j][5] = colorToShow.V.ToString();
              dt.Rows[j][6] = colorToShow.UP.ToString();
              dt.Rows[j][7] = colorToShow.VP.ToString();
              dt.Rows[j][8] = colorToShow.LA.ToString();
              dt.Rows[j][9] = colorToShow.A.ToString();
              dt.Rows[j][10] = colorToShow.B.ToString();
              dt.Rows[j][11] = ColorToShowXYZ.X.ToString();
              dt.Rows[j][12] = ColorToShowXYZ.Y.ToString();
              dt.Rows[j][13] = ColorToShowXYZ.Z.ToString();
              dt.Rows[j][14] = MR.ToString();
              dt.Rows[j][15] = MG.ToString();
              dt.Rows[j][16] = MB.ToString();
              dt.Rows[j][17] = ColorMeasured.L.ToString();
              dt.Rows[j][18] = ColorMeasured.U.ToString();
              dt.Rows[j][19] = ColorMeasured.V.ToString();
              dt.Rows[j][20] = ColorMeasured.UP.ToString();
              dt.Rows[j][21] = ColorMeasured.VP.ToString();
              dt.Rows[j][22] = ColorMeasured.LA.ToString();
              dt.Rows[j][23] = ColorMeasured.A.ToString();
              dt.Rows[j][24] = ColorMeasured.B.ToString();
              dt.Rows[j][25] = ColorMeasuredXYZ.X.ToString();
              dt.Rows[j][26] = ColorMeasuredXYZ.Y.ToString();
              dt.Rows[j][27] = ColorMeasuredXYZ.Z.ToString();

              BradXYZ = Util.CATCalulation.bradford(ColorToShowXYZ);
              VonXYZ = Util.CATCalulation.VonKries(ColorToShowXYZ);
              ScalingXYZ = Util.CATCalulation.XYZScaling(ColorToShowXYZ);

              Bradcolor = PerceptionLib.Color.ToLUV(BradXYZ);
              Voncolor = PerceptionLib.Color.ToLUV(VonXYZ);
              Scalingcolor = PerceptionLib.Color.ToLUV(ScalingXYZ);


              bradRGB = PerceptionLib.Color.ToRBG(BradXYZ);
              VonRGB = PerceptionLib.Color.ToRBG(VonXYZ);
              scalingRGB = PerceptionLib.Color.ToRBG(ScalingXYZ);

              dt.Rows[j][28] = Bradcolor.L.ToString();
              dt.Rows[j][29] = Bradcolor.U.ToString();
              dt.Rows[j][30] = Bradcolor.V.ToString();
              dt.Rows[j][31] = Bradcolor.UP.ToString();
              dt.Rows[j][32] = Bradcolor.VP.ToString();
              dt.Rows[j][33] = Bradcolor.LA.ToString();
              dt.Rows[j][34] = Bradcolor.A.ToString();
              dt.Rows[j][35] = Bradcolor.B.ToString();
              dt.Rows[j][36] = BradXYZ.X.ToString();
              dt.Rows[j][37] = BradXYZ.Y.ToString();
              dt.Rows[j][38] = BradXYZ.Z.ToString();
              dt.Rows[j][39] = bradRGB.R.ToString();
              dt.Rows[j][40] = bradRGB.G.ToString();
              dt.Rows[j][41] = bradRGB.B.ToString();

              dt.Rows[j][42] = Voncolor.L.ToString();
              dt.Rows[j][43] = Voncolor.U.ToString();
              dt.Rows[j][44] = Voncolor.V.ToString();
              dt.Rows[j][45] = Voncolor.UP.ToString();
              dt.Rows[j][46] = Voncolor.VP.ToString();
              dt.Rows[j][47] = Voncolor.LA.ToString();
              dt.Rows[j][48] = Voncolor.A.ToString();
              dt.Rows[j][49] = Voncolor.B.ToString();
              dt.Rows[j][50] = VonXYZ.X.ToString();
              dt.Rows[j][51] = VonXYZ.Y.ToString();
              dt.Rows[j][52] = VonXYZ.Z.ToString();
              dt.Rows[j][53] = VonRGB.R.ToString();
              dt.Rows[j][54] = VonRGB.G.ToString();
              dt.Rows[j][55] = VonRGB.B.ToString();

              dt.Rows[j][57] = Scalingcolor.L.ToString();
              dt.Rows[j][57] = Scalingcolor.U.ToString();
              dt.Rows[j][58] = Scalingcolor.V.ToString();
              dt.Rows[j][59] = Scalingcolor.UP.ToString();
              dt.Rows[j][60] = Scalingcolor.VP.ToString();
              dt.Rows[j][61] = Scalingcolor.LA.ToString();
              dt.Rows[j][62] = Scalingcolor.A.ToString();
              dt.Rows[j][63] = Scalingcolor.B.ToString();
              dt.Rows[j][64] = ScalingXYZ.X.ToString();
              dt.Rows[j][65] = ScalingXYZ.Y.ToString();
              dt.Rows[j][66] = ScalingXYZ.Z.ToString();
              dt.Rows[j][67] = scalingRGB.R.ToString();
              dt.Rows[j][68] = scalingRGB.G.ToString();
              dt.Rows[j][69] = scalingRGB.B.ToString();

              dt.Rows[j][71] = BgR.ToString();
              dt.Rows[j][71] = BgG.ToString();
              dt.Rows[j][72] = BgB.ToString();
              dt.Rows[j][73] = ColorBackGround.L.ToString();
              dt.Rows[j][74] = ColorBackGround.U.ToString();
              dt.Rows[j][75] = ColorBackGround.V.ToString();
              dt.Rows[j][76] = ColorBackGround.UP.ToString();
              dt.Rows[j][77] = ColorBackGround.VP.ToString();
              dt.Rows[j][78] = ColorBackGround.LA.ToString();
              dt.Rows[j][79] = ColorBackGround.A.ToString();
              dt.Rows[j][80] = ColorBackGround.B.ToString();
              dt.Rows[j][81] = ColorBackGroundXYZ.X.ToString();
              dt.Rows[j][82] = ColorBackGroundXYZ.Y.ToString();
              dt.Rows[j][83] = ColorBackGroundXYZ.Z.ToString();
              dt.Rows[j][84] = 0;
              dt.Rows[j][85] = 0;
              dt.Rows[j][86] = 0;
              dt.Rows[j][87] = 0;
              dt.Rows[j][88] = 0;
              dt.Rows[j][89] = 0;
              dt.Rows[j][90] = 0;
              dt.Rows[j][91] = 0;
              dt.Rows[j][92] = 0;
              dt.Rows[j][93] = 0;
              dt.Rows[j][94] = 0;
              dt.Rows[j][95] = 0;
              dt.Rows[j][96] = 0;
              dt.Rows[j][97] = 0;
              dt.Rows[j][98] = 0;
              dt.Rows[j][99] = 0;
              dt.Rows[j][100] = 0;
              dt.Rows[j][101] = 0;
              dt.Rows[j][102] = 0;
              dt.Rows[j][103] = 0;
              dt.Rows[j][104] = 0;
              dt.Rows[j][105] = 0;
              dt.Rows[j][106] = 0;
              dt.Rows[j][107] = 0;
              dt.Rows[j][108] = 0;
              dt.Rows[j][109] = 0;
              dt.Rows[j][110] = 0;
              dt.Rows[j][111] = 0;
              dt.Rows[j][112] = 0;
              dt.Rows[j][113] = 0;
              dt.Rows[j][114] = 0;
              dt.Rows[j][115] = 0;
              dt.Rows[j][116] = 0;
              dt.Rows[j][117] = 0;
              dt.Rows[j][118] = 0;
              dt.Rows[j][119] = 0;
              dt.Rows[j][120] = 0;
              dt.Rows[j][121] = 0;
              dt.Rows[j][122] = 0;
              dt.Rows[j][123] = 0;
              dt.Rows[j][124] = 0;
              dt.Rows[j][125] = 0;
              dt.Rows[j][126] = 0;
              dt.Rows[j][127] = 0;
              dt.Rows[j][128] = 0;
              dt.Rows[j][129] = 0;
              dt.Rows[j][130] = 0;
              dt.Rows[j][131] = 0;
              dt.Rows[j][132] = 0;
              dt.Rows[j][133] = 0;
              dt.Rows[j][134] = 0;
              dt.Rows[j][135] = 0;
              dt.Rows[j][136] = 0;
              dt.Rows[j][137] = 0;
              dt.Rows[j][138] = 0;
              dt.Rows[j][139] = 0;
              dt.Rows[j][140] = 0;
              dt.Rows[j][141] = 0;
              dt.Rows[j][142] = 0;
              dt.Rows[j][143] = 0;
              dt.Rows[j][144] = 0;
              dt.Rows[j][145] = 0;
              dt.Rows[j][146] = 0;
              dt.Rows[j][147] = 0;
              dt.Rows[j][148] = 0;
              dt.Rows[j][149] = 0;
              dt.Rows[j][150] = 0;
              dt.Rows[j][151] = 0;
              dt.Rows[j][152] = 0;
              dt.Rows[j][153] = 0;
              dt.Rows[j][154] = 0;
              dt.Rows[j][155] = 0;
              dt.Rows[j][156] = 0;
              dt.Rows[j][157] = 0;
              dt.Rows[j][158] = 0;
              dt.Rows[j][159] = 0;
              dt.Rows[j][160] = 0;
              dt.Rows[j][161] = 0;
              dt.Rows[j][162] = 0;
              dt.Rows[j][163] = 0;
              dt.Rows[j][164] = 0;
              dt.Rows[j][165] = 0;
              dt.Rows[j][166] = 0;
              dt.Rows[j][167] = 0;
              dt.Rows[j][168] = 0;
              dt.Rows[j][169] = 0;
              dt.Rows[j][170] = 0;
              dt.Rows[j][171] = 0;
              dt.Rows[j][172] = 0;
              dt.Rows[j][173] = 0;
              dt.Rows[j][174] = 0;
              dt.Rows[j][175] = 0;
              dt.Rows[j][176] = 0;
              dt.Rows[j][177] = 0;
              dt.Rows[j][178] = 0;
              dt.Rows[j][179] = 0;
              dt.Rows[j][180] = 0;

              /////////////////




              pairs.Clear();
              pairs.Add(new MeasurementPair()
              {
                ColorToShow = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(colorToShow.UP.ToString()), VP = Convert.ToDouble(colorToShow.VP.ToString()) },
                ColorCaptured = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(ColorMeasured.UP.ToString()), VP = Convert.ToDouble(ColorMeasured.VP.ToString()) }
              });

            }
            else
            {

              newRow = dt.NewRow();
              //newRow[0] = R.ToString();
              //newRow[1] = G.ToString();
              //newRow[2] = B.ToString();
              //newRow[3] = colorToShow.L.ToString();
              //newRow[4] = colorToShow.U.ToString();
              //newRow[5] = colorToShow.V.ToString();
              //newRow[6] = colorToShow.UP.ToString();
              //newRow[7] = colorToShow.VP.ToString();

              //newRow[8]  = dt.Rows[j][8].ToString();
              //newRow[9]  = dt.Rows[j][9].ToString();
              //newRow[10] = dt.Rows[j][10].ToString();
              //newRow[11] = dt.Rows[j][11].ToString();
              //newRow[12] = dt.Rows[j][12].ToString();
              //newRow[13] = dt.Rows[j][13].ToString();
              //newRow[14] = dt.Rows[j][14].ToString();
              //newRow[15] = dt.Rows[j][15].ToString();

              //newRow[16] = colorDifference.L.ToString();
              //newRow[17] = colorDifference.U.ToString();
              //newRow[18] = colorDifference.V.ToString();
              //newRow[19] = BgR.ToString();
              //newRow[20] = BgG.ToString();
              //newRow[21] = BgB.ToString();
              //newRow[22] = 0;
              //newRow[23] = 0;
              //newRow[24] = 0;
              //newRow[25] = 0;
              //newRow[26] = 0;
              //newRow[27] = 0;
              //newRow[28] = 0;
              //newRow[29] = 0;
              //newRow[30] = ColorMeasured.L.ToString();
              //newRow[31] = ColorMeasured.U.ToString();
              //newRow[32] = ColorMeasured.V.ToString();
              //newRow[33] = ColorMeasured.UP.ToString();
              //newRow[34] = ColorMeasured.VP.ToString();
              //newRow[35] = MR.ToString();
              //newRow[36] = MG.ToString();
              //newRow[37] = MB.ToString();


              newRow[0] = R.ToString();
              newRow[1] = G.ToString();
              newRow[2] = B.ToString();
              newRow[3] = colorToShow.L.ToString();
              newRow[4] = colorToShow.U.ToString();
              newRow[5] = colorToShow.V.ToString();
              newRow[6] = colorToShow.UP.ToString();
              newRow[7] = colorToShow.VP.ToString();
              newRow[8] = colorToShow.LA.ToString();
              newRow[9] = colorToShow.A.ToString();
              newRow[10] = colorToShow.B.ToString();
              newRow[11] = ColorToShowXYZ.X.ToString();
              newRow[12] = ColorToShowXYZ.Y.ToString();
              newRow[13] = ColorToShowXYZ.Z.ToString();

              newRow[14] = dt.Rows[j][14].ToString();
              newRow[15] = dt.Rows[j][15].ToString();
              newRow[16] = dt.Rows[j][16].ToString();
              newRow[17] = dt.Rows[j][17].ToString();
              newRow[18] = dt.Rows[j][18].ToString();
              newRow[19] = dt.Rows[j][19].ToString();
              newRow[20] = dt.Rows[j][20].ToString();
              newRow[21] = dt.Rows[j][21].ToString();
              newRow[22] = dt.Rows[j][22].ToString();
              newRow[23] = dt.Rows[j][23].ToString();
              newRow[24] = dt.Rows[j][24].ToString();
              newRow[25] = dt.Rows[j][25].ToString();
              newRow[26] = dt.Rows[j][26].ToString();
              newRow[27] = dt.Rows[j][27].ToString();

              newRow[28] = dt.Rows[j][28].ToString();
              newRow[29] = dt.Rows[j][29].ToString();
              newRow[30] = dt.Rows[j][30].ToString();
              newRow[31] = dt.Rows[j][31].ToString();
              newRow[32] = dt.Rows[j][32].ToString();
              newRow[33] = dt.Rows[j][33].ToString();
              newRow[34] = dt.Rows[j][34].ToString();
              newRow[35] = dt.Rows[j][35].ToString();
              newRow[36] = dt.Rows[j][36].ToString();
              newRow[37] = dt.Rows[j][37].ToString();
              newRow[38] = dt.Rows[j][38].ToString();
              newRow[39] = dt.Rows[j][39].ToString();
              newRow[40] = dt.Rows[j][40].ToString();
              newRow[41] = dt.Rows[j][41].ToString();

              newRow[42] = dt.Rows[j][42].ToString();
              newRow[43] = dt.Rows[j][43].ToString();
              newRow[44] = dt.Rows[j][44].ToString();
              newRow[45] = dt.Rows[j][45].ToString();
              newRow[46] = dt.Rows[j][46].ToString();
              newRow[47] = dt.Rows[j][47].ToString();
              newRow[48] = dt.Rows[j][48].ToString();
              newRow[49] = dt.Rows[j][49].ToString();
              newRow[50] = dt.Rows[j][50].ToString();
              newRow[51] = dt.Rows[j][51].ToString();
              newRow[52] = dt.Rows[j][52].ToString();
              newRow[53] = dt.Rows[j][53].ToString();
              newRow[54] = dt.Rows[j][54].ToString();
              newRow[55] = dt.Rows[j][55].ToString();

              newRow[56] = dt.Rows[j][56].ToString();
              newRow[57] = dt.Rows[j][57].ToString();
              newRow[58] = dt.Rows[j][58].ToString();
              newRow[59] = dt.Rows[j][59].ToString();
              newRow[60] = dt.Rows[j][60].ToString();
              newRow[61] = dt.Rows[j][61].ToString();
              newRow[62] = dt.Rows[j][62].ToString();
              newRow[63] = dt.Rows[j][63].ToString();
              newRow[64] = dt.Rows[j][64].ToString();
              newRow[65] = dt.Rows[j][65].ToString();
              newRow[66] = dt.Rows[j][66].ToString();
              newRow[67] = dt.Rows[j][67].ToString();
              newRow[68] = dt.Rows[j][68].ToString();
              newRow[69] = dt.Rows[j][69].ToString();

              newRow[70] = BgR.ToString();
              newRow[71] = BgG.ToString();
              newRow[72] = BgB.ToString();
              newRow[73] = ColorBackGround.L.ToString();
              newRow[74] = ColorBackGround.U.ToString();
              newRow[75] = ColorBackGround.V.ToString();
              newRow[76] = ColorBackGround.UP.ToString();
              newRow[77] = ColorBackGround.VP.ToString();
              newRow[78] = ColorBackGround.LA.ToString();
              newRow[79] = ColorBackGround.A.ToString();
              newRow[80] = ColorBackGround.B.ToString();
              newRow[81] = ColorBackGroundXYZ.X.ToString();
              newRow[82] = ColorBackGroundXYZ.Y.ToString();
              newRow[83] = ColorBackGroundXYZ.Z.ToString();
              newRow[84] = 0;
              newRow[85] = 0;
              newRow[86] = 0;
              newRow[87] = 0;
              newRow[88] = 0;
              newRow[89] = 0;
              newRow[90] = 0;
              newRow[91] = 0;
              newRow[92] = 0;
              newRow[93] = 0;
              newRow[94] = 0;
              newRow[95] = 0;
              newRow[96] = 0;
              newRow[97] = 0;
              newRow[98] = ColorMeasured.L.ToString();
              newRow[99] = ColorMeasured.U.ToString();
              newRow[100] = ColorMeasured.V.ToString();
              newRow[101] = ColorMeasured.UP.ToString();
              newRow[102] = ColorMeasured.VP.ToString();
              newRow[103] = ColorMeasured.LA.ToString();
              newRow[104] = ColorMeasured.A.ToString();
              newRow[105] = ColorMeasured.B.ToString();
              newRow[106] = ColorMeasuredXYZ.X.ToString();
              newRow[107] = ColorMeasuredXYZ.Y.ToString();
              newRow[108] = ColorMeasuredXYZ.Z.ToString();
              newRow[109] = MR.ToString();
              newRow[110] = MG.ToString();
              newRow[111] = MB.ToString();
              newRow[112] = 0;
              newRow[113] = 0;
              newRow[114] = 0;
              newRow[115] = 0;
              newRow[116] = 0;
              newRow[117] = 0;
              newRow[118] = 0;
              newRow[119] = 0;
              newRow[120] = 0;
              newRow[121] = 0;
              newRow[122] = 0;
              newRow[123] = 0;
              newRow[124] = 0;
              newRow[125] = 0;
              newRow[126] = 0;
              newRow[127] = 0;
              newRow[128] = 0;
              newRow[129] = 0;
              newRow[130] = 0;
              newRow[131] = 0;
              newRow[132] = 0;
              newRow[133] = 0;
              newRow[134] = 0;
              newRow[135] = 0;
              newRow[136] = 0;
              newRow[137] = 0;
              newRow[138] = 0;
              newRow[139] = 0;
              newRow[140] = 0;
              newRow[141] = 0;
              newRow[142] = 0;
              newRow[143] = 0;
              newRow[144] = 0;
              newRow[145] = 0;
              newRow[146] = 0;
              newRow[147] = 0;
              newRow[148] = 0;
              newRow[149] = 0;
              newRow[150] = 0;
              newRow[151] = 0;
              newRow[152] = 0;
              newRow[153] = 0;
              newRow[154] = 0;
              newRow[155] = 0;
              newRow[156] = 0;
              newRow[157] = 0;
              newRow[158] = 0;
              newRow[159] = 0;
              newRow[160] = 0;
              newRow[161] = 0;
              newRow[162] = 0;
              newRow[163] = 0;
              newRow[164] = 0;
              newRow[165] = 0;
              newRow[166] = 0;
              newRow[167] = 0;
              newRow[168] = 0;
              newRow[169] = 0;
              newRow[170] = 0;
              newRow[171] = 0;
              newRow[172] = 0;
              newRow[173] = 0;
              newRow[174] = 0;
              newRow[175] = 0;
              newRow[176] = 0;
              newRow[177] = 0;
              newRow[178] = 0;
              newRow[179] = 0;
              newRow[180] = 0;




              MixedColor = ColorMeasured;
              pairs.Clear();
              pairs.Add(new MeasurementPair()
              {
                // HERE 11 AND 12 ARE THE COLOUR CATPTURED BY THE CAMERA FOR DISPLAY 33 AND 34 ARE MIXED COLOURS
                ColorToShow = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dt.Rows[j][20].ToString()), VP = Convert.ToDouble(dt.Rows[j][21].ToString()) },
                ColorCaptured = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(ColorMeasured.UP.ToString()), VP = Convert.ToDouble(ColorMeasured.VP.ToString()) }
              });
              dt.Rows.Add(newRow);

            }

            R = 0; G = 0; B = 0;
            //dt.Rows.Add(newRow);

            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt.DefaultView));
            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

          }
        }

        //   dtgrid_corrDisplay.ItemsSource = dt.DefaultView;
        //dtgrid_corrDisplay.Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt.DefaultView));
        // for caluclating just bg colour,.. with  fg & bg white conditions so this for loop should be 3 more than loop for bg color change
        for (int i = 1; i < BgNo + 3; i++)
        {
          if (i < BgNo)
          {
            R = 0;
            G = 0;
            B = 0;

            BgR = Convert.ToByte(dt.Rows[i * FgNo][70].ToString());
            BgG = Convert.ToByte(dt.Rows[i * FgNo][71].ToString());
            BgB = Convert.ToByte(dt.Rows[i * FgNo][72].ToString());

          }
          // both white 
          else if (i == BgNo)
          {
            R = 255;
            G = 255;
            B = 255;

            BgR = 255;
            BgG = 255;
            BgB = 255;
          }
          // fg white
          else if (i == BgNo + 1)
          {
            R = 255;
            G = 255;
            B = 255;

            BgR = 0;
            BgG = 0;
            BgB = 0;
          }
          // bg white
          else
          {
            R = 0;
            G = 0;
            B = 0;

            BgR = 255;
            BgG = 255;
            BgB = 255;

          }


          ////if (i != 0)
          //System.Threading.Thread.Sleep(500);
          //ColorCaptureJustBG();
          //System.Windows.Forms.Application.DoEvents();

          ////does all the caputure and difference calculations
          //System.Threading.Thread.Sleep(500);
          //// StartCapture();

          Dispatcher.Invoke(new Action(() => ColorCaptureJustBG()));
          //           System.Windows.Forms.Application.DoEvents();


          colorMeasured = StartCapture1();

          Dispatcher.Invoke(new Action(() => DisplayMeasuredValues()));

          DifferenceCalculation();


          //just bg capture
          newRow = dt.NewRow();
          if (i < BgNo)
          {

            //newRow[0] = R.ToString();
            //newRow[1] = G.ToString();
            //newRow[2] = B.ToString();
            //newRow[3] = 0;
            //newRow[4] = 0;
            //newRow[5] = 0;
            //newRow[6] = 0;
            //newRow[7] = 0;
            //newRow[8] = 0;
            //newRow[9] = 0;
            //newRow[10] = 0;
            //newRow[11] = 0;
            //newRow[12] = 0;
            //newRow[13] = 0;
            //newRow[14] = 0;
            //newRow[15] = 0;
            //newRow[16] = 0;
            //newRow[17] = 0;
            //newRow[18] = 0;
            //newRow[19] = BgR.ToString();
            //newRow[20] = BgG.ToString();
            //newRow[21] = BgB.ToString();
            //newRow[22] = MR.ToString();
            //newRow[23] = MG.ToString();
            //newRow[24] = MB.ToString();
            //newRow[25] = ColorMeasured.L.ToString();
            //newRow[26] = ColorMeasured.U.ToString();
            //newRow[27] = ColorMeasured.V.ToString();
            //newRow[28] = ColorMeasured.UP.ToString();
            //newRow[29] = ColorMeasured.VP.ToString();
            //newRow[30] = 0;
            //newRow[31] = 0;
            //newRow[32] = 0;
            //newRow[33] = 0;
            //newRow[34] = 0;
            //newRow[35] = 0;
            //newRow[36] = 0;
            //newRow[37] = 0;

            newRow[0] = R.ToString();
            newRow[1] = G.ToString();
            newRow[2] = B.ToString();
            newRow[3] = 0;
            newRow[4] = 0;
            newRow[5] = 0;
            newRow[6] = 0;
            newRow[7] = 0;
            newRow[8] = 0;
            newRow[9] = 0;
            newRow[10] = 0;
            newRow[11] = 0;
            newRow[12] = 0;
            newRow[13] = 0;
            newRow[14] = 0;
            newRow[15] = 0;
            newRow[16] = 0;
            newRow[17] = 0;
            newRow[18] = 0;
            newRow[19] = 0;
            newRow[20] = 0;
            newRow[21] = 0;
            newRow[22] = 0;
            newRow[23] = 0;
            newRow[24] = 0;
            newRow[25] = 0;
            newRow[26] = 0;
            newRow[27] = 0;
            newRow[28] = 0;
            newRow[29] = 0;
            newRow[30] = 0;
            newRow[31] = 0;
            newRow[32] = 0;
            newRow[33] = 0;
            newRow[34] = 0;
            newRow[35] = 0;
            newRow[36] = 0;
            newRow[37] = 0;
            newRow[38] = 0;
            newRow[39] = 0;
            newRow[40] = 0;
            newRow[41] = 0;
            newRow[42] = 0;
            newRow[43] = 0;
            newRow[44] = 0;
            newRow[45] = 0;
            newRow[46] = 0;
            newRow[47] = 0;
            newRow[48] = 0;
            newRow[49] = 0;
            newRow[50] = 0;
            newRow[51] = 0;
            newRow[52] = 0;
            newRow[53] = 0;
            newRow[54] = 0;
            newRow[55] = 0;
            newRow[56] = 0;
            newRow[57] = 0;
            newRow[58] = 0;
            newRow[59] = 0;
            newRow[60] = 0;
            newRow[61] = 0;
            newRow[62] = 0;
            newRow[63] = 0;
            newRow[64] = 0;
            newRow[65] = 0;
            newRow[66] = 0;
            newRow[67] = 0;
            newRow[68] = 0;
            newRow[69] = 0;
            newRow[70] = BgR.ToString();
            newRow[71] = BgG.ToString();
            newRow[72] = BgB.ToString();
            newRow[73] = ColorBackGround.L.ToString();
            newRow[74] = ColorBackGround.U.ToString();
            newRow[75] = ColorBackGround.V.ToString();
            newRow[76] = ColorBackGround.UP.ToString();
            newRow[77] = ColorBackGround.VP.ToString();
            newRow[78] = ColorBackGround.LA.ToString();
            newRow[79] = ColorBackGround.A.ToString();
            newRow[80] = ColorBackGround.B.ToString();
            newRow[81] = ColorBackGroundXYZ.X.ToString();
            newRow[82] = ColorBackGroundXYZ.Y.ToString();
            newRow[83] = ColorBackGroundXYZ.Z.ToString();
            newRow[84] = MR.ToString();
            newRow[85] = MG.ToString();
            newRow[86] = MB.ToString();
            newRow[87] = ColorMeasured.L.ToString();
            newRow[88] = ColorMeasured.U.ToString();
            newRow[89] = ColorMeasured.V.ToString();
            newRow[90] = ColorMeasured.UP.ToString();
            newRow[91] = ColorMeasured.VP.ToString();
            newRow[92] = ColorMeasured.LA.ToString();
            newRow[93] = ColorMeasured.A.ToString();
            newRow[94] = ColorMeasured.B.ToString();
            newRow[95] = ColorMeasuredXYZ.X.ToString();
            newRow[96] = ColorMeasuredXYZ.Y.ToString();
            newRow[97] = ColorMeasuredXYZ.Z.ToString();
            newRow[98] = 0;
            newRow[99] = 0;
            newRow[100] = 0;
            newRow[101] = 0;
            newRow[102] = 0;
            newRow[103] = 0;
            newRow[104] = 0;
            newRow[105] = 0;
            newRow[106] = 0;
            newRow[107] = 0;
            newRow[108] = 0;
            newRow[109] = 0;
            newRow[110] = 0;
            newRow[111] = 0;
            newRow[112] = 0;
            newRow[113] = 0;
            newRow[114] = 0;
            newRow[115] = 0;
            newRow[116] = 0;
            newRow[117] = 0;
            newRow[118] = 0;
            newRow[119] = 0;
            newRow[120] = 0;
            newRow[121] = 0;
            newRow[122] = 0;
            newRow[123] = 0;
            newRow[124] = 0;
            newRow[125] = 0;
            newRow[126] = 0;
            newRow[127] = 0;
            newRow[128] = 0;
            newRow[129] = 0;
            newRow[130] = 0;
            newRow[131] = 0;
            newRow[132] = 0;
            newRow[133] = 0;
            newRow[134] = 0;
            newRow[135] = 0;
            newRow[136] = 0;
            newRow[137] = 0;
            newRow[138] = 0;
            newRow[139] = 0;
            newRow[140] = 0;
            newRow[141] = 0;
            newRow[142] = 0;
            newRow[143] = 0;
            newRow[144] = 0;
            newRow[145] = 0;
            newRow[146] = 0;
            newRow[147] = 0;
            newRow[148] = 0;
            newRow[149] = 0;
            newRow[150] = 0;
            newRow[151] = 0;
            newRow[152] = 0;
            newRow[153] = 0;
            newRow[154] = 0;
            newRow[155] = 0;
            newRow[156] = 0;
            newRow[157] = 0;
            newRow[158] = 0;
            newRow[159] = 0;
            newRow[160] = 0;
            newRow[161] = 0;
            newRow[162] = 0;
            newRow[163] = 0;
            newRow[164] = 0;
            newRow[165] = 0;
            newRow[166] = 0;
            newRow[167] = 0;
            newRow[168] = 0;
            newRow[169] = 0;
            newRow[170] = 0;
            newRow[171] = 0;
            newRow[172] = 0;
            newRow[173] = 0;
            newRow[174] = 0;
            newRow[175] = 0;
            newRow[176] = 0;
            newRow[177] = 0;
            newRow[178] = 0;
            newRow[179] = 0;
            newRow[180] = 0;


            pairs.Clear();

            BgColor = ColorMeasured;
            pairs.Add(new MeasurementPair()
            {//THE COLOUR DISPLAYED HERE ARE THE BG COLOUR CAPTURED
              BgColor = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(ColorMeasured.UP.ToString()), VP = Convert.ToDouble(ColorMeasured.VP.ToString()) }
            });
          }
          //white point capture
          else
          {
            //newRow[0] = R.ToString();
            //newRow[1] = G.ToString();
            //newRow[2] = B.ToString();
            //newRow[3] = 0;
            //newRow[4] = 0;
            //newRow[5] = 0;
            //newRow[6] = 0;
            //newRow[7] = 0;
            //newRow[8] = 0;
            //newRow[9] = 0;
            //newRow[10] = 0;
            //newRow[11] = 0;
            //newRow[12] = 0;
            //newRow[13] = 0;
            //newRow[14] = 0;
            //newRow[15] = 0;
            //newRow[16] = 0;
            //newRow[17] = 0;
            //newRow[18] = 0;
            //newRow[19] = BgR.ToString();
            //newRow[20] = BgG.ToString();
            //newRow[21] = BgB.ToString();
            //newRow[22] = 0;
            //newRow[23] = 0;
            //newRow[24] = 0;
            //newRow[25] = 0;
            //newRow[26] = 0;
            //newRow[27] = 0;
            //newRow[28] = 0;
            //newRow[29] = 0;
            //newRow[30] = ColorMeasured.L.ToString();
            //newRow[31] = ColorMeasured.U.ToString();
            //newRow[32] = ColorMeasured.V.ToString();
            //newRow[33] = ColorMeasured.UP.ToString();
            //newRow[34] = ColorMeasured.VP.ToString();
            //newRow[35] = MR.ToString();
            //newRow[36] = MG.ToString();
            //newRow[37] = MB.ToString();

            newRow[0] = R.ToString();
            newRow[1] = G.ToString();
            newRow[2] = B.ToString();
            newRow[3] = 0;
            newRow[4] = 0;
            newRow[5] = 0;
            newRow[6] = 0;
            newRow[7] = 0;
            newRow[8] = 0;
            newRow[9] = 0;
            newRow[10] = 0;
            newRow[11] = 0;
            newRow[12] = 0;
            newRow[13] = 0;
            newRow[14] = 0;
            newRow[15] = 0;
            newRow[16] = 0;
            newRow[17] = 0;
            newRow[18] = 0;
            newRow[19] = 0;
            newRow[20] = 0;
            newRow[21] = 0;
            newRow[22] = 0;
            newRow[23] = 0;
            newRow[24] = 0;
            newRow[25] = 0;
            newRow[26] = 0;
            newRow[27] = 0;
            newRow[28] = 0;
            newRow[29] = 0;
            newRow[30] = 0;
            newRow[31] = 0;
            newRow[32] = 0;
            newRow[33] = 0;
            newRow[34] = 0;
            newRow[35] = 0;
            newRow[36] = 0;
            newRow[37] = 0;
            newRow[38] = 0;
            newRow[39] = 0;
            newRow[40] = 0;
            newRow[41] = 0;
            newRow[42] = 0;
            newRow[43] = 0;
            newRow[44] = 0;
            newRow[45] = 0;
            newRow[46] = 0;
            newRow[47] = 0;
            newRow[48] = 0;
            newRow[49] = 0;
            newRow[50] = 0;
            newRow[51] = 0;
            newRow[52] = 0;
            newRow[53] = 0;
            newRow[54] = 0;
            newRow[55] = 0;
            newRow[56] = 0;
            newRow[57] = 0;
            newRow[58] = 0;
            newRow[59] = 0;
            newRow[60] = 0;
            newRow[61] = 0;
            newRow[62] = 0;
            newRow[63] = 0;
            newRow[64] = 0;
            newRow[65] = 0;
            newRow[66] = 0;
            newRow[67] = 0;
            newRow[68] = 0;
            newRow[69] = 0;
            newRow[70] = BgR.ToString();
            newRow[71] = BgG.ToString();
            newRow[72] = BgB.ToString();
            newRow[73] = 0;
            newRow[74] = 0;
            newRow[75] = 0;
            newRow[76] = 0;
            newRow[77] = 0;
            newRow[78] = 0;
            newRow[79] = 0;
            newRow[80] = 0;
            newRow[81] = 0;
            newRow[82] = 0;
            newRow[83] = 0;
            newRow[84] = 0;
            newRow[85] = 0;
            newRow[86] = 0;
            newRow[87] = 0;
            newRow[88] = 0;
            newRow[89] = 0;
            newRow[90] = 0;
            newRow[91] = 0;
            newRow[92] = 0;
            newRow[93] = 0;
            newRow[94] = 0;
            newRow[95] = 0;
            newRow[96] = 0;
            newRow[97] = 0;
            newRow[98] = ColorMeasured.L.ToString();
            newRow[99] = ColorMeasured.U.ToString();
            newRow[100] = ColorMeasured.V.ToString();
            newRow[101] = ColorMeasured.UP.ToString();
            newRow[102] = ColorMeasured.VP.ToString();
            newRow[103] = ColorMeasured.LA.ToString();
            newRow[104] = ColorMeasured.A.ToString();
            newRow[105] = ColorMeasured.B.ToString();
            newRow[106] = ColorMeasuredXYZ.X.ToString();
            newRow[107] = ColorMeasuredXYZ.Y.ToString();
            newRow[108] = ColorMeasuredXYZ.Z.ToString();
            newRow[109] = MR.ToString();
            newRow[110] = MG.ToString();
            newRow[111] = MB.ToString();
            newRow[112] = 0;
            newRow[113] = 0;
            newRow[114] = 0;
            newRow[115] = 0;
            newRow[116] = 0;
            newRow[117] = 0;
            newRow[118] = 0;
            newRow[119] = 0;
            newRow[120] = 0;
            newRow[121] = 0;
            newRow[122] = 0;
            newRow[123] = 0;
            newRow[124] = 0;
            newRow[125] = 0;
            newRow[126] = 0;
            newRow[127] = 0;
            newRow[128] = 0;
            newRow[129] = 0;
            newRow[130] = 0;
            newRow[131] = 0;
            newRow[132] = 0;
            newRow[133] = 0;
            newRow[134] = 0;
            newRow[135] = 0;
            newRow[136] = 0;
            newRow[137] = 0;
            newRow[138] = 0;
            newRow[139] = 0;
            newRow[140] = 0;
            newRow[141] = 0;
            newRow[142] = 0;
            newRow[143] = 0;
            newRow[144] = 0;
            newRow[145] = 0;
            newRow[146] = 0;
            newRow[147] = 0;
            newRow[148] = 0;
            newRow[149] = 0;
            newRow[150] = 0;
            newRow[151] = 0;
            newRow[152] = 0;
            newRow[153] = 0;
            newRow[154] = 0;
            newRow[155] = 0;
            newRow[156] = 0;
            newRow[157] = 0;
            newRow[158] = 0;
            newRow[159] = 0;
            newRow[160] = 0;
            newRow[161] = 0;
            newRow[162] = 0;
            newRow[163] = 0;
            newRow[164] = 0;
            newRow[165] = 0;
            newRow[166] = 0;
            newRow[167] = 0;
            newRow[168] = 0;
            newRow[169] = 0;
            newRow[170] = 0;
            newRow[171] = 0;
            newRow[172] = 0;
            newRow[173] = 0;
            newRow[174] = 0;
            newRow[175] = 0;
            newRow[176] = 0;
            newRow[177] = 0;
            newRow[178] = 0;
            newRow[179] = 0;
            newRow[180] = 0;

            pairs.Add(new MeasurementPair()
            {
              // HERE 11 AND 12 ARE THE COLOUR CATPTURED BY THE CAMERA FOR DISPLAY 33 AND 34 ARE MIXED COLOURS
              BgColor = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(ColorMeasured.UP.ToString()), VP = Convert.ToDouble(ColorMeasured.VP.ToString()) }
            });


          }

          R = 10;
          dt.Rows.Add(newRow);

          Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt.DefaultView));
          Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
        }



        //taking back the measured value of bg to all the data set
        // adding mixed valued for cat and absolute back to all 
        int totalRow = BgNo * FgNo;
        for (int i = 1; i < BgNo; i++)
        {

          for (int j = 0; j < fgNo; j++)
          {
            int rowNo = (i * FgNo) + j;
            // i am doing -1 cos the data set starts from 0 ,.. so in the first one it will still b e bg*fg but i value will be 0
            dt.Rows[rowNo][84] = dt.Rows[(totalRow) + i - 1][84].ToString();
            dt.Rows[rowNo][85] = dt.Rows[(totalRow) + i - 1][85].ToString();
            dt.Rows[rowNo][86] = dt.Rows[(totalRow) + i - 1][86].ToString();

            dt.Rows[rowNo][87] = dt.Rows[(totalRow) + i - 1][87].ToString();
            dt.Rows[rowNo][88] = dt.Rows[(totalRow) + i - 1][88].ToString();
            dt.Rows[rowNo][89] = dt.Rows[(totalRow) + i - 1][89].ToString();
            dt.Rows[rowNo][90] = dt.Rows[(totalRow) + i - 1][90].ToString();
            dt.Rows[rowNo][91] = dt.Rows[(totalRow) + i - 1][91].ToString();
            dt.Rows[rowNo][92] = dt.Rows[(totalRow) + i - 1][92].ToString();
            dt.Rows[rowNo][93] = dt.Rows[(totalRow) + i - 1][93].ToString();
            dt.Rows[rowNo][94] = dt.Rows[(totalRow) + i - 1][94].ToString();
            dt.Rows[rowNo][95] = dt.Rows[(totalRow) + i - 1][95].ToString();
            dt.Rows[rowNo][96] = dt.Rows[(totalRow) + i - 1][96].ToString();
            dt.Rows[rowNo][97] = dt.Rows[(totalRow) + i - 1][97].ToString();


            ColorToShowXYZ.X = Convert.ToDouble(dt.Rows[rowNo][11].ToString());
            ColorToShowXYZ.Y = Convert.ToDouble(dt.Rows[rowNo][12].ToString());
            ColorToShowXYZ.Z = Convert.ToDouble(dt.Rows[rowNo][13].ToString());

            ColorMeasuredXYZ.X = Convert.ToDouble(dt.Rows[rowNo][25].ToString());
            ColorMeasuredXYZ.Y = Convert.ToDouble(dt.Rows[rowNo][26].ToString());
            ColorMeasuredXYZ.Z = Convert.ToDouble(dt.Rows[rowNo][27].ToString());

            ColorBackGroundXYZ.X = Convert.ToDouble(dt.Rows[rowNo][81].ToString());
            ColorBackGroundXYZ.Y = Convert.ToDouble(dt.Rows[rowNo][82].ToString());
            ColorBackGroundXYZ.Z = Convert.ToDouble(dt.Rows[rowNo][83].ToString());

            //
            //McolorBackGroundXYZ.X = Convert.ToDouble(dt.Rows[rowNo][95].ToString());
            //McolorBackGroundXYZ.Y = Convert.ToDouble(dt.Rows[rowNo][96].ToString());
            //McolorBackGroundXYZ.Z = Convert.ToDouble(dt.Rows[rowNo][97].ToString());

            ColorToShow.L = Convert.ToDouble(dt.Rows[rowNo][3].ToString());
            ColorToShow.U = Convert.ToDouble(dt.Rows[rowNo][4].ToString());
            ColorToShow.V = Convert.ToDouble(dt.Rows[rowNo][5].ToString());
            ColorToShow.LA = Convert.ToDouble(dt.Rows[rowNo][8].ToString());
            ColorToShow.A = Convert.ToDouble(dt.Rows[rowNo][9].ToString());
            ColorToShow.B = Convert.ToDouble(dt.Rows[rowNo][10].ToString());

            ColorMeasured.L = Convert.ToDouble(dt.Rows[rowNo][17].ToString());
            ColorMeasured.U = Convert.ToDouble(dt.Rows[rowNo][18].ToString());
            ColorMeasured.V = Convert.ToDouble(dt.Rows[rowNo][19].ToString());
            ColorMeasured.LA = Convert.ToDouble(dt.Rows[rowNo][22].ToString());
            ColorMeasured.A = Convert.ToDouble(dt.Rows[rowNo][23].ToString());
            ColorMeasured.B = Convert.ToDouble(dt.Rows[rowNo][24].ToString());

            //  HERE I CHOSE TO USE  BG COLOR IN PLACES OF MEASURE COLOR !!
            BgColor.L = Convert.ToDouble(dt.Rows[rowNo][98].ToString());
            BgColor.U = Convert.ToDouble(dt.Rows[rowNo][99].ToString());
            BgColor.V = Convert.ToDouble(dt.Rows[rowNo][100].ToString());
            BgColor.LA = Convert.ToDouble(dt.Rows[rowNo][103].ToString());
            BgColor.A = Convert.ToDouble(dt.Rows[rowNo][104].ToString());
            BgColor.B = Convert.ToDouble(dt.Rows[rowNo][105].ToString());


            BradXYZ.X = Convert.ToDouble(dt.Rows[rowNo][36].ToString());
            BradXYZ.Y = Convert.ToDouble(dt.Rows[rowNo][37].ToString());
            BradXYZ.Z = Convert.ToDouble(dt.Rows[rowNo][38].ToString());

            VonXYZ.X = Convert.ToDouble(dt.Rows[rowNo][50].ToString());
            VonXYZ.Y = Convert.ToDouble(dt.Rows[rowNo][51].ToString());
            VonXYZ.Z = Convert.ToDouble(dt.Rows[rowNo][52].ToString());

            ScalingXYZ.X = Convert.ToDouble(dt.Rows[rowNo][64].ToString());
            ScalingXYZ.Y = Convert.ToDouble(dt.Rows[rowNo][65].ToString());
            ScalingXYZ.Z = Convert.ToDouble(dt.Rows[rowNo][66].ToString());

            AcolorXYZ = Util.ColorSpaceConverter.AddXYZ(ColorToShowXYZ, ColorBackGroundXYZ);
            Acolor = Util.ColorSpaceConverter.XYZToLABLUV(ColorToShowXYZ, ColorBackGroundXYZ);
            acolorRGB = Util.ColorSpaceConverter.XYZToRGB(ColorToShowXYZ, ColorBackGroundXYZ);

            // i use original bg color not captured bg color

            MBradXYZ = Util.ColorSpaceConverter.AddXYZ(BradXYZ, ColorBackGroundXYZ);
            MBradcolor = Util.ColorSpaceConverter.XYZToLABLUV(BradXYZ, ColorBackGroundXYZ);
            MbradRGB = Util.ColorSpaceConverter.XYZToRGB(BradXYZ, ColorBackGroundXYZ);

            MVonXYZ = Util.ColorSpaceConverter.AddXYZ(VonXYZ, ColorBackGroundXYZ);
            MVoncolor = Util.ColorSpaceConverter.XYZToLABLUV(VonXYZ, ColorBackGroundXYZ);
            MVonRGB = Util.ColorSpaceConverter.XYZToRGB(VonXYZ, ColorBackGroundXYZ);

            MScalingXYZ = Util.ColorSpaceConverter.AddXYZ(ScalingXYZ, ColorBackGroundXYZ);
            MScalingcolor = Util.ColorSpaceConverter.XYZToLABLUV(ScalingXYZ, ColorBackGroundXYZ);
            MscalingRGB = Util.ColorSpaceConverter.XYZToRGB(ScalingXYZ, ColorBackGroundXYZ);


            dt.Rows[rowNo][112] = Acolor.L.ToString();
            dt.Rows[rowNo][113] = Acolor.U.ToString();
            dt.Rows[rowNo][114] = Acolor.V.ToString();
            dt.Rows[rowNo][115] = Acolor.UP.ToString();
            dt.Rows[rowNo][116] = Acolor.VP.ToString();
            dt.Rows[rowNo][117] = Acolor.LA.ToString();
            dt.Rows[rowNo][118] = Acolor.A.ToString();
            dt.Rows[rowNo][119] = Acolor.B.ToString();
            dt.Rows[rowNo][120] = AcolorXYZ.X.ToString();
            dt.Rows[rowNo][121] = AcolorXYZ.Y.ToString();
            dt.Rows[rowNo][122] = AcolorXYZ.Z.ToString();
            dt.Rows[rowNo][123] = acolorRGB.R.ToString();
            dt.Rows[rowNo][124] = acolorRGB.G.ToString();
            dt.Rows[rowNo][125] = acolorRGB.B.ToString();


            dt.Rows[rowNo][126] = MBradcolor.L.ToString();
            dt.Rows[rowNo][127] = MBradcolor.U.ToString();
            dt.Rows[rowNo][128] = MBradcolor.V.ToString();
            dt.Rows[rowNo][129] = MBradcolor.UP.ToString();
            dt.Rows[rowNo][130] = MBradcolor.VP.ToString();
            dt.Rows[rowNo][131] = MBradcolor.LA.ToString();
            dt.Rows[rowNo][132] = MBradcolor.A.ToString();
            dt.Rows[rowNo][133] = MBradcolor.B.ToString();
            dt.Rows[rowNo][134] = MBradXYZ.X.ToString();
            dt.Rows[rowNo][135] = MBradXYZ.Y.ToString();
            dt.Rows[rowNo][136] = MBradXYZ.Z.ToString();
            dt.Rows[rowNo][137] = MbradRGB.R.ToString();
            dt.Rows[rowNo][138] = MbradRGB.G.ToString();
            dt.Rows[rowNo][139] = MbradRGB.B.ToString();

            dt.Rows[rowNo][140] = MVoncolor.L.ToString();
            dt.Rows[rowNo][141] = MVoncolor.U.ToString();
            dt.Rows[rowNo][142] = MVoncolor.V.ToString();
            dt.Rows[rowNo][143] = MVoncolor.UP.ToString();
            dt.Rows[rowNo][144] = MVoncolor.VP.ToString();
            dt.Rows[rowNo][145] = MVoncolor.LA.ToString();
            dt.Rows[rowNo][146] = MVoncolor.A.ToString();
            dt.Rows[rowNo][147] = MVoncolor.B.ToString();
            dt.Rows[rowNo][148] = MVonXYZ.X.ToString();
            dt.Rows[rowNo][149] = MVonXYZ.Y.ToString();
            dt.Rows[rowNo][150] = MVonXYZ.Z.ToString();
            dt.Rows[rowNo][151] = MVonRGB.R.ToString();
            dt.Rows[rowNo][152] = MVonRGB.G.ToString();
            dt.Rows[rowNo][153] = MVonRGB.B.ToString();

            dt.Rows[rowNo][154] = MScalingcolor.L.ToString();
            dt.Rows[rowNo][155] = MScalingcolor.U.ToString();
            dt.Rows[rowNo][156] = MScalingcolor.V.ToString();
            dt.Rows[rowNo][157] = MScalingcolor.UP.ToString();
            dt.Rows[rowNo][158] = MScalingcolor.VP.ToString();
            dt.Rows[rowNo][159] = MScalingcolor.LA.ToString();
            dt.Rows[rowNo][160] = MScalingcolor.A.ToString();
            dt.Rows[rowNo][161] = MScalingcolor.B.ToString();
            dt.Rows[rowNo][162] = MScalingXYZ.X.ToString();
            dt.Rows[rowNo][163] = MScalingXYZ.Y.ToString();
            dt.Rows[rowNo][164] = MScalingXYZ.Z.ToString();
            dt.Rows[rowNo][165] = MscalingRGB.R.ToString();
            dt.Rows[rowNo][166] = MscalingRGB.G.ToString();
            dt.Rows[rowNo][167] = MscalingRGB.B.ToString();
            dt.Rows[rowNo][168] = 0;
            dt.Rows[rowNo][169] = acolorRGB.gmt;
            dt.Rows[rowNo][170] = MbradRGB.gmt;
            dt.Rows[rowNo][171] = MVonRGB.gmt;
            dt.Rows[rowNo][172] = MscalingRGB.gmt;
            dt.Rows[rowNo][173] = PerceptionLib.Color.ColorDistanceCal(BgColor, Acolor).ToString();
            dt.Rows[rowNo][174] = PerceptionLib.Color.ColorDistanceCalAB(BgColor, Acolor).ToString();
            dt.Rows[rowNo][175] = PerceptionLib.Color.ColorDistanceCal(BgColor, Bradcolor).ToString();
            dt.Rows[rowNo][176] = PerceptionLib.Color.ColorDistanceCalAB(BgColor, Bradcolor).ToString();
            dt.Rows[rowNo][177] = PerceptionLib.Color.ColorDistanceCal(BgColor, Voncolor).ToString();
            dt.Rows[rowNo][178] = PerceptionLib.Color.ColorDistanceCalAB(BgColor, Voncolor).ToString();
            dt.Rows[rowNo][179] = PerceptionLib.Color.ColorDistanceCal(BgColor, Scalingcolor).ToString();
            dt.Rows[rowNo][180] = PerceptionLib.Color.ColorDistanceCalAB(BgColor, Scalingcolor).ToString();
            dt.Rows[rowNo][181] = PerceptionLib.Color.ColorDistanceCalAB(ColorToShow, ColorMeasured).ToString();
            dt.Rows[rowNo][182] = PerceptionLib.Color.ColorDistanceCalAB(BgColor, ColorMeasured).ToString();




          }
        }

        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt.DefaultView));
        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
        dataTable = dt;
      });
      cmb_graph.IsEnabled = true;
      btn_ExportGrid.IsEnabled = true;
      // captureDevice.Dispose();
    }

    private void Btn_BinColor_Click(object sender, RoutedEventArgs e)
    {
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\binLoad.csv");
      dtgridClick = 1;
      txt_R.IsEnabled = false;
      txt_G.IsEnabled = false;
      txt_B.IsEnabled = false;
      txt_BgNo.IsEnabled = false;
      txt_FgNo.IsEnabled = false;
      btn_MixedColour.IsEnabled = false;
      DataTable dt = new DataTable();
      // DataTable new_dt = new DataTable();
      DataRow newRow;
      dt = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      List<RGBbin> binRGB = PerceptionLib.Color.RGBbinedData();

      int count = binRGB.Count();
      int rowcount = dt.Rows.Count;

      //         int temp = -1;

      for (int i = 0; i < count; i++)
      {

        newRow = dt.NewRow();
        newRow[0] = binRGB[i].R.ToString();
        newRow[1] = binRGB[i].G.ToString();
        newRow[2] = binRGB[i].B.ToString();


        dt.Rows.Add(newRow);
      }
      DataView dv = new DataView(dt);
      // set the output columns array of the destination dt 
      string[] strColumns = { "R", "G", "B" };
      // true = yes, i need distinct values. 

      dt = dv.ToTable(true, strColumns);
      dv = new DataView(dt);


      DataTable dt2 = dt.Clone();
      dt2.Columns["R"].DataType = Type.GetType("System.Int32");
      dt2.Columns["G"].DataType = Type.GetType("System.Int32");
      dt2.Columns["B"].DataType = Type.GetType("System.Int32");

      foreach (DataRow dr in dt.Rows)
      {
        dt2.ImportRow(dr);
      }
      dt2.AcceptChanges();
      dv = dt2.DefaultView;
      dv.Sort = "R,G,B";

      dt = dv.ToTable();
      dataTable = dt;

      Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt.DefaultView));
      Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
      // to check if the RGB value is previous aquried 
      //for (int j = temp; j <= rowcount; j++)
      //{

      //    if (rowcount > 0)
      //        if (dt.Rows[j][0].ToString() == binRGB[j].R.ToString() && dt.Rows[j][1].ToString() == binRGB[j].G.ToString() && dt.Rows[j][2].ToString() == binRGB[j].B.ToString())
      //            continue;
      //        else
      //        {
      //            newRow = dt.NewRow();
      //            newRow[0] = binRGB[i].R.ToString();
      //            newRow[1] = binRGB[i].G.ToString();
      //            newRow[2] = binRGB[i].B.ToString();
      //            dt.Rows.Add(newRow);
      //            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt.DefaultView));
      //            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
      //            rowcount++;
      //        }
      //    else
      //    {
      //        newRow = dt.NewRow();
      //        newRow[0] = binRGB[i].R.ToString();
      //        newRow[1] = binRGB[i].G.ToString();
      //        newRow[2] = binRGB[i].B.ToString();
      //        dt.Rows.Add(newRow);
      //        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt.DefaultView));
      //        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
      //        rowcount++;
      //        temp=0;
      //    }

      //}

      btn_ExportGrid.IsEnabled = true;



    }

    private void Btn_CSGammut_Click(object sender, RoutedEventArgs e)
    {
      ///add this sleep for phone

      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\color88ForPhone.txt");
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\colorForPhone.txt");
     // PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\bin8000.csv");
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\bgcolor.csv");
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\binforL.csv");
      dtgrid_corrDisplay.IsEnabled = false;
      
      //Thread.Sleep(16500);
      btn_StartMeasurment.IsEnabled = false;
      txt_R.IsEnabled = false;
      txt_G.IsEnabled = false;
      txt_B.IsEnabled = false;
      DataTable dt = new DataTable();
      DataTable new_dt = new DataTable();


      dt = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
      int connect = PerceptionLib.Cs200Connection.ConnectToCS200();
      //if not connected then connect will be 1

      BgNo = dt.Rows.Count;
      ThreadPool.QueueUserWorkItem(ignored =>
      {
        DateTime Start = DateTime.Now;
        if (connect == 0)
        {

          //for (int i = 1; i < dt.Rows.Count; i++)
          for (int i = 0; i < dt.Rows.Count; i++)
          {

            mainW.R = Convert.ToByte(dt.Rows[i][0].ToString());
            mainW.G = Convert.ToByte(dt.Rows[i][1].ToString());
            mainW.B = Convert.ToByte(dt.Rows[i][2].ToString());

            // converts rendered RGB to luv and displays the colour in to measure

            //NoArgDelegate fetcher = new NoArgDelegate(
            //        this.ColorCapturedUpdate);
            //Dispatcher.BeginInvoke();


            // Dispatcher.Invoke(new Action(() => ColorCapturedUpdate()));



            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //take it out for any thing other than phone
            ColorCapturedUpdate();
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            
            
            //colorMeasured = StartCapture1();
            //Dispatcher.Invoke(DispatcherPriority.Send, new Action(() => DisplayMeasuredValuesFromCs200()));
            
            
            //Thread.Sleep(500);
            DisplayMeasuredValuesFromCs200(); 
            DifferenceCalculation();

            //bradcolor=
            // if (i > 0)

            // assignes the data to a the datatable 
            dt.Rows[i][3] = colorToShow.L.ToString();
            dt.Rows[i][4] = colorToShow.U.ToString();
            dt.Rows[i][5] = colorToShow.V.ToString();
            dt.Rows[i][6] = colorToShow.UP.ToString();
            dt.Rows[i][7] = colorToShow.VP.ToString();
            dt.Rows[i][8] = ColorMeasured.L.ToString();
            dt.Rows[i][9] = ColorMeasured.U.ToString();
            dt.Rows[i][10] = ColorMeasured.V.ToString();
            dt.Rows[i][11] = ColorMeasured.UP.ToString();
            dt.Rows[i][12] = ColorMeasured.VP.ToString();
            //dt.Rows][13] = ColorDifference.L.ToString();
            //dt.Rows][14] = ColorDifference.U.ToString();
            //dt.Rows][15] = ColorDifference.V.ToString();

            ////

            dt.Rows[i][13] = colorToShow.LA.ToString();
            dt.Rows[i][14] = colorToShow.A.ToString();
            dt.Rows[i][15] = colorToShow.B.ToString();
            dt.Rows[i][16] = ColorMeasured.LA.ToString();
            dt.Rows[i][17] = ColorMeasured.A.ToString();
            dt.Rows[i][18] = ColorMeasured.B.ToString();

            //

            dt.Rows[i][19] = PerceptionLib.Color.ColorDistanceCal(colorToShow, ColorMeasured).ToString();
            dt.Rows[i][20] = PerceptionLib.Color.ColorDistanceCalAB(colorToShow, ColorMeasured).ToString();
            //
            dt.Rows[i][21] = ColorToShowXYZ.X.ToString();
            dt.Rows[i][22] = ColorToShowXYZ.Y.ToString();
            dt.Rows[i][23] = ColorToShowXYZ.Z.ToString();
            dt.Rows[i][24] = ColorMeasuredXYZ.X.ToString();
            dt.Rows[i][25] = ColorMeasuredXYZ.Y.ToString();
            dt.Rows[i][26] = ColorMeasuredXYZ.Z.ToString();

            dt.Rows[i][27] = MR.ToString();
            dt.Rows[i][28] = MG.ToString();
            dt.Rows[i][29] = MB.ToString();

            BradXYZ = Util.CATCalulation.bradford(ColorToShowXYZ);
            VonXYZ = Util.CATCalulation.VonKries(ColorToShowXYZ);
            ScalingXYZ = Util.CATCalulation.XYZScaling(ColorToShowXYZ);

            Bradcolor = PerceptionLib.Color.ToLUV(BradXYZ);
            Voncolor = PerceptionLib.Color.ToLUV(VonXYZ);
            Scalingcolor = PerceptionLib.Color.ToLUV(ScalingXYZ);


            bradRGB = PerceptionLib.Color.ToRBG(Bradcolor);
            VonRGB = PerceptionLib.Color.ToRBG(Voncolor);
            scalingRGB = PerceptionLib.Color.ToRBG(Scalingcolor);

            CalRGB = PerceptionLib.Color.ToRBG(ColorMeasuredXYZ);


            dt.Rows[i][30] = BradXYZ.X.ToString();
            dt.Rows[i][31] = BradXYZ.Y.ToString();
            dt.Rows[i][32] = BradXYZ.Z.ToString();
            dt.Rows[i][33] = PerceptionLib.Color.ColorDistanceCal(ColorMeasured, Bradcolor).ToString();
            dt.Rows[i][34] = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, Bradcolor).ToString();

            dt.Rows[i][35] = VonXYZ.X.ToString();
            dt.Rows[i][36] = VonXYZ.Y.ToString();
            dt.Rows[i][37] = VonXYZ.Z.ToString();
            dt.Rows[i][38] = PerceptionLib.Color.ColorDistanceCal(ColorMeasured, Voncolor).ToString();
            dt.Rows[i][39] = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, Voncolor).ToString();

            dt.Rows[i][40] = ScalingXYZ.X.ToString();
            dt.Rows[i][41] = ScalingXYZ.Y.ToString();
            dt.Rows[i][42] = ScalingXYZ.Z.ToString();
            dt.Rows[i][43] = PerceptionLib.Color.ColorDistanceCal(ColorMeasured, Scalingcolor).ToString();
            dt.Rows[i][44] = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, Scalingcolor).ToString();

            dt.Rows[i][45] = bradRGB.R.ToString();
            dt.Rows[i][46] = bradRGB.G.ToString();
            dt.Rows[i][47] = bradRGB.B.ToString();

            dt.Rows[i][48] = VonRGB.R.ToString();
            dt.Rows[i][49] = VonRGB.G.ToString();
            dt.Rows[i][50] = VonRGB.B.ToString();

            dt.Rows[i][51] = scalingRGB.R.ToString();
            dt.Rows[i][52] = scalingRGB.G.ToString();
            dt.Rows[i][53] = scalingRGB.B.ToString();

            dt.Rows[i][54] = CalRGB.R.ToString();
            dt.Rows[i][55] = CalRGB.G.ToString();
            dt.Rows[i][56] = CalRGB.B.ToString();

            dt.Rows[i][57] = Bradcolor.UP.ToString();
            dt.Rows[i][58] = Bradcolor.VP.ToString();

            dt.Rows[i][59] = Voncolor.UP.ToString();
            dt.Rows[i][60] = Voncolor.VP.ToString();

            dt.Rows[i][61] = Scalingcolor.UP.ToString();
            dt.Rows[i][62] = Scalingcolor.VP.ToString();

            dt.Rows[i][63] = ColorMeasuredXYZ.stdx.ToString();
            dt.Rows[i][64] = ColorMeasuredXYZ.stdy.ToString();
            dt.Rows[i][65] = ColorMeasuredXYZ.stdz.ToString();



            //pairs.Clear();
            //pairs.Add(new MeasurementPair()
            //{
            //  ColorToShow = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dt.Rows[i][6].ToString()), VP = Convert.ToDouble(dt.Rows[i][7].ToString()) },
            //  ColorCaptured = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dt.Rows[i][11].ToString()), VP = Convert.ToDouble(dt.Rows[i][12].ToString()) },
            //  BradColor = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][57].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][58].ToString()) },
            //  VonColor = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][59].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][60].ToString()) },
            //  Scalingcolor = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][61].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][62].ToString()) }

            //});
            //if (i == 9)
            //{
            //Dispatcher.Invoke(new Action(() =>
            //{
            //  dtgrid_corrDisplay.ItemsSource = dt.DefaultView;
            //  dtgrid_corrDisplay.Items.Refresh();

            //}));
            //}


            //if (i == (dt.Rows.Count - 1))
            //{
            //  int disconnect = PerceptionLib.Cs200Connection.DisconnectCS200();
            //}
          }

          // System.Threading.Thread.Sleep(10000);
          // grid is populated with new datatable which has luv values
          Dispatcher.Invoke(new Action(() =>
          {
            dtgrid_corrDisplay.ItemsSource = dt.DefaultView;
            dtgrid_corrDisplay.Items.Refresh();
          }));


          //  to show all the pairs in cie graph
          pairs.Clear();

          //for (int i = 0; i < dt.Rows.Count; i++)
          //{
          //  pairs.Add(new MeasurementPair()
          //  {
          //    ColorToShow = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dt.Rows[i][6].ToString()), VP = Convert.ToDouble(dt.Rows[i][7].ToString()) },
          //    ColorCaptured = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dt.Rows[i][11].ToString()), VP = Convert.ToDouble(dt.Rows[i][12].ToString()) }
          //  });
          //}

        }
        time = DateTime.Now.Subtract(Start).ToString();
      });


      btn_ExportGrid.IsEnabled = true;

      dtgrid_corrDisplay.IsEnabled = true;
      dataTable = dt;


    }
    
    private void Btn_CSPredictor_Click(object sender, RoutedEventArgs e)
    {
      dtgrid_corrDisplay.IsEnabled = true;
      btn_StartMeasurment.IsEnabled = false;
      txt_R.IsEnabled = false;
      txt_G.IsEnabled = false;
      txt_B.IsEnabled = false;

      double X, Y, Z, DeltaA_Luv, DeltaA_LAb, DeltaBin_Luv, DeltaBin_Lab, DeltaB_luv, DeltaB_Lab, DeltaS_Luv, DeltaS_Lab, DeltaV_Luv, DeltaV_Lab,DeltaC,BinDeltaC;


      DataRow newRow;

      int connect = PerceptionLib.Cs200Connection.ConnectToCS200();

      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\BgValue.csv");
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\BgValueWtPt.csv");
      DataTable dt_Bg = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        dt_Bg = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));

      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\RGBgammut.csv");
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\bigProjecor\RGB8000.csv");
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\bigProjecor\RGB88.csv");
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\bigProjecor\RGB88.csv");
      DataTable dt_RGBGammut = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        dt_RGBGammut = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
      }));

      BgNo = dt_Bg.Rows.Count;
      FgNo = dt_RGBGammut.Rows.Count;

      //BgNo = 6;
      //FgNo = 4;
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\bigProjecor\Cspredictor.csv");
      DataTable dt_DataCollection = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        dt_DataCollection = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
      }));

      ThreadPool.QueueUserWorkItem(ignored =>
      {
        PerceptionLib.RGBValue rgb = new PerceptionLib.RGBValue();
        for (int i = 0; i < BgNo; i++)
        {

          for (int j = 0; j < FgNo; j=j+20)
          {
            BgR = Convert.ToByte(dt_Bg.Rows[i][0].ToString());
            BgG = Convert.ToByte(dt_Bg.Rows[i][1].ToString());
            BgB = Convert.ToByte(dt_Bg.Rows[i][2].ToString());

            X = Convert.ToDouble(dt_Bg.Rows[i][3].ToString());
            Y = Convert.ToDouble(dt_Bg.Rows[i][4].ToString());
            Z = Convert.ToDouble(dt_Bg.Rows[i][5].ToString());

            ColorBackGroundXYZ = new CIEXYZ(X, Y, Z);


            R = Convert.ToByte(dt_RGBGammut.Rows[j][0].ToString());
            G = Convert.ToByte(dt_RGBGammut.Rows[j][1].ToString());
            B = Convert.ToByte(dt_RGBGammut.Rows[j][2].ToString());


            //cat model
            X = Convert.ToDouble(dt_RGBGammut.Rows[j][25].ToString());
            Y = Convert.ToDouble(dt_RGBGammut.Rows[j][26].ToString());
            Z = Convert.ToDouble(dt_RGBGammut.Rows[j][27].ToString());
            BradXYZ = new CIEXYZ(X, Y, Z);



            X = Convert.ToDouble(dt_RGBGammut.Rows[j][28].ToString());
            Y = Convert.ToDouble(dt_RGBGammut.Rows[j][29].ToString());
            Z = Convert.ToDouble(dt_RGBGammut.Rows[j][30].ToString());
            VonXYZ = new CIEXYZ(X, Y, Z);

            X = Convert.ToDouble(dt_RGBGammut.Rows[j][31].ToString());
            Y = Convert.ToDouble(dt_RGBGammut.Rows[j][32].ToString());
            Z = Convert.ToDouble(dt_RGBGammut.Rows[j][33].ToString());
            ScalingXYZ = new CIEXYZ(X, Y, Z);

            //direct model
            X = Convert.ToDouble(dt_RGBGammut.Rows[j][11].ToString());
            Y = Convert.ToDouble(dt_RGBGammut.Rows[j][12].ToString());
            Z = Convert.ToDouble(dt_RGBGammut.Rows[j][13].ToString());
            AcolorXYZ = new CIEXYZ(X, Y, Z);

            Acolor = PerceptionLib.Color.ToLUV(AcolorXYZ);

            //bin model
            X = Convert.ToDouble(dt_RGBGammut.Rows[j][22].ToString());
            Y = Convert.ToDouble(dt_RGBGammut.Rows[j][23].ToString());
            Z = Convert.ToDouble(dt_RGBGammut.Rows[j][24].ToString());
            BINcolorXYZ = new CIEXYZ(X, Y, Z);

            BINColor = PerceptionLib.Color.ToLUV(BINcolorXYZ);

            //adding XYZ values
            MBradXYZ = Util.ColorSpaceConverter.AddXYZ(BradXYZ, ColorBackGroundXYZ);
            MScalingXYZ = Util.ColorSpaceConverter.AddXYZ(ScalingXYZ, ColorBackGroundXYZ);
            MVonXYZ = Util.ColorSpaceConverter.AddXYZ(VonXYZ, ColorBackGroundXYZ);
            MAcolorXYZ = Util.ColorSpaceConverter.AddXYZ(AcolorXYZ, ColorBackGroundXYZ);
            MBINcolorXYZ = Util.ColorSpaceConverter.AddXYZ(BINcolorXYZ, ColorBackGroundXYZ);

            //converting back to various spaces fpr color difference calculation
            MBradcolor = PerceptionLib.Color.ToLUV(MBradXYZ);
            MVoncolor = PerceptionLib.Color.ToLUV(MVonXYZ);
            MScalingcolor = PerceptionLib.Color.ToLUV(MScalingXYZ);
            MAcolor = PerceptionLib.Color.ToLUV(MAcolorXYZ);
            MBINColor = PerceptionLib.Color.ToLUV(MBINcolorXYZ);

            bradRGB = PerceptionLib.Color.ToRBG(MBradXYZ);
            VonRGB = PerceptionLib.Color.ToRBG(MVonXYZ);
            scalingRGB = PerceptionLib.Color.ToRBG(MScalingXYZ);
            acolorRGB = PerceptionLib.Color.ToRBG(MAcolorXYZ);
            BincolorRGB = PerceptionLib.Color.ToRBG(MBINcolorXYZ);

            CSColorUpdateOnScreenWithBG();

            DisplayMeasuredValuesFromCs200();

            //difference calculation
            DeltaB_luv = PerceptionLib.Color.ColorDistanceCal(Mbradcolor, ColorMeasured);
            DeltaB_Lab = PerceptionLib.Color.ColorDistanceCalAB(Mbradcolor, ColorMeasured);

            DeltaV_Luv = PerceptionLib.Color.ColorDistanceCal(ColorMeasured, MVoncolor);
            DeltaV_Lab = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, MVoncolor);

            DeltaS_Luv = PerceptionLib.Color.ColorDistanceCal(ColorMeasured, MScalingcolor);
            DeltaS_Lab = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, MScalingcolor);

            DeltaA_Luv = PerceptionLib.Color.ColorDistanceCal(ColorMeasured, MAcolor);
            DeltaA_LAb = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, MAcolor);

            DeltaBin_Luv = PerceptionLib.Color.ColorDistanceCal(ColorMeasured, MBINColor);
            DeltaBin_Lab = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, MBINColor);

            BinDeltaC = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, BINColor);
            
            DeltaC = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, Acolor);


            newRow = dt_DataCollection.NewRow();
            newRow[0] = R;
            newRow[1] = G;
            newRow[2] = B;
            newRow[3] = dt_RGBGammut.Rows[j][3].ToString();
            newRow[4] = dt_RGBGammut.Rows[j][4].ToString();
            newRow[5] = dt_RGBGammut.Rows[j][5].ToString();
            newRow[6] = dt_RGBGammut.Rows[j][6].ToString();
            newRow[7] = dt_RGBGammut.Rows[j][7].ToString();
            newRow[8] = dt_RGBGammut.Rows[j][8].ToString();
            newRow[9] = dt_RGBGammut.Rows[j][9].ToString();
            newRow[10] = dt_RGBGammut.Rows[j][10].ToString();
            newRow[11] = dt_RGBGammut.Rows[j][11].ToString();
            newRow[12] = dt_RGBGammut.Rows[j][12].ToString();
            newRow[13] = dt_RGBGammut.Rows[j][13].ToString();
            newRow[14] = dt_RGBGammut.Rows[j][14].ToString();
            newRow[15] = dt_RGBGammut.Rows[j][15].ToString();
            newRow[16] = dt_RGBGammut.Rows[j][16].ToString();
            newRow[17] = dt_RGBGammut.Rows[j][17].ToString();
            newRow[18] = dt_RGBGammut.Rows[j][18].ToString();
            newRow[19] = dt_RGBGammut.Rows[j][19].ToString();
            newRow[20] = dt_RGBGammut.Rows[j][20].ToString();
            newRow[21] = dt_RGBGammut.Rows[j][21].ToString();
            newRow[22] = dt_RGBGammut.Rows[j][22].ToString();
            newRow[23] = dt_RGBGammut.Rows[j][23].ToString();
            newRow[24] = dt_RGBGammut.Rows[j][24].ToString();
            newRow[25] = dt_RGBGammut.Rows[j][25].ToString();
            newRow[26] = dt_RGBGammut.Rows[j][26].ToString();
            newRow[27] = dt_RGBGammut.Rows[j][27].ToString();
            newRow[28] = dt_RGBGammut.Rows[j][28].ToString();
            newRow[29] = dt_RGBGammut.Rows[j][29].ToString();
            newRow[30] = dt_RGBGammut.Rows[j][30].ToString();
            newRow[31] = dt_RGBGammut.Rows[j][31].ToString();
            newRow[32] = dt_RGBGammut.Rows[j][32].ToString();
            newRow[33] = dt_RGBGammut.Rows[j][33].ToString();
            newRow[34] = BgR;
            newRow[35] = BgG;
            newRow[36] = BgB;
            newRow[37] = dt_Bg.Rows[i][3].ToString();
            newRow[38] = dt_Bg.Rows[i][4].ToString();
            newRow[39] = dt_Bg.Rows[i][5].ToString();
            newRow[40] = ColorMeasuredXYZ.X.ToString();
            newRow[41] = ColorMeasuredXYZ.Y.ToString();
            newRow[42] = ColorMeasuredXYZ.Z.ToString();
            newRow[43] = ColorMeasured.L.ToString();
            newRow[44] = ColorMeasured.U.ToString();
            newRow[45] = ColorMeasured.V.ToString();
            newRow[46] = ColorMeasured.UP.ToString();
            newRow[47] = ColorMeasured.VP.ToString();
            newRow[48] = ColorMeasured.LA.ToString();
            newRow[49] = ColorMeasured.A.ToString();
            newRow[50] = ColorMeasured.B.ToString();
            newRow[51] = MR.ToString();
            newRow[52] = MG.ToString();
            newRow[53] = MB.ToString();
            newRow[54] = MAcolorXYZ.X.ToString();
            newRow[55] = MAcolorXYZ.Y.ToString();
            newRow[56] = MAcolorXYZ.Z.ToString();
            newRow[57] = MAcolor.L.ToString();
            newRow[58] = MAcolor.U.ToString();
            newRow[59] = MAcolor.V.ToString();
            newRow[60] = MAcolor.UP.ToString();
            newRow[61] = MAcolor.VP.ToString();
            newRow[62] = MAcolor.LA.ToString();
            newRow[63] = MAcolor.A.ToString();
            newRow[64] = MAcolor.B.ToString();
            newRow[65] = acolorRGB.R.ToString();
            newRow[66] = acolorRGB.G.ToString();
            newRow[67] = acolorRGB.B.ToString();
            newRow[68] = MBradXYZ.X.ToString();
            newRow[69] = MBradXYZ.Y.ToString();
            newRow[70] = MBradXYZ.Z.ToString();
            newRow[71] = MBradcolor.L.ToString();
            newRow[72] = MBradcolor.U.ToString();
            newRow[73] = MBradcolor.V.ToString();
            newRow[74] = MBradcolor.UP.ToString();
            newRow[75] = MBradcolor.VP.ToString();
            newRow[76] = MBradcolor.LA.ToString();
            newRow[77] = MBradcolor.A.ToString();
            newRow[78] = MBradcolor.B.ToString();
            newRow[79] = MVonXYZ.X.ToString();
            newRow[80] = MVonXYZ.Y.ToString();
            newRow[81] = MVonXYZ.Z.ToString();
            newRow[82] = MVoncolor.L.ToString();
            newRow[83] = MVoncolor.U.ToString();
            newRow[84] = MVoncolor.V.ToString();
            newRow[85] = MVoncolor.UP.ToString();
            newRow[86] = MVoncolor.VP.ToString();
            newRow[87] = MVoncolor.LA.ToString();
            newRow[88] = MVoncolor.A.ToString();
            newRow[89] = MVoncolor.B.ToString();
            newRow[90] = MScalingXYZ.X.ToString();
            newRow[91] = MScalingXYZ.Y.ToString();
            newRow[92] = MScalingXYZ.Z.ToString();
            newRow[93] = MScalingcolor.L.ToString();
            newRow[94] = MScalingcolor.U.ToString();
            newRow[95] = MScalingcolor.V.ToString();
            newRow[96] = MScalingcolor.UP.ToString();
            newRow[97] = MScalingcolor.VP.ToString();
            newRow[98] = MScalingcolor.LA.ToString();
            newRow[99] = MScalingcolor.A.ToString();
            newRow[100] = MScalingcolor.B.ToString();
            newRow[101] = MBINcolorXYZ.X.ToString();
            newRow[102] = MBINcolorXYZ.Y.ToString();
            newRow[103] = MBINcolorXYZ.Z.ToString();
            newRow[104] = MBINColor.L.ToString();
            newRow[105] = MBINColor.U.ToString();
            newRow[106] = MBINColor.V.ToString();
            newRow[107] = MBINColor.UP.ToString();
            newRow[108] = MBINColor.VP.ToString();
            newRow[109] = MBINColor.LA.ToString();
            newRow[110] = MBINColor.A.ToString();
            newRow[111] = MBINColor.B.ToString();
            newRow[112] = DeltaA_Luv.ToString();
            newRow[113] = DeltaA_LAb.ToString();
            newRow[114] = DeltaBin_Luv.ToString();
            newRow[115] = DeltaBin_Lab.ToString();
            newRow[116] = DeltaB_luv.ToString();
            newRow[117] = DeltaB_Lab.ToString();
            newRow[118] = DeltaV_Luv.ToString();
            newRow[119] = DeltaV_Lab.ToString();
            newRow[120] = DeltaS_Luv.ToString();
            newRow[121] = DeltaS_Lab.ToString();
            newRow[122] = BinDeltaC.ToString();
            newRow[123] = DeltaC.ToString();

            //calculation for Bg through the screen.
            X = Convert.ToDouble(dt_Bg.Rows[i][6].ToString());
            Y = Convert.ToDouble(dt_Bg.Rows[i][7].ToString());
            Z = Convert.ToDouble(dt_Bg.Rows[i][8].ToString());

            ColorBackGroundXYZ = new CIEXYZ(X, Y, Z);

            MBradXYZ = Util.ColorSpaceConverter.AddXYZ(BradXYZ, ColorBackGroundXYZ);
            MScalingXYZ = Util.ColorSpaceConverter.AddXYZ(ScalingXYZ, ColorBackGroundXYZ);
            MVonXYZ = Util.ColorSpaceConverter.AddXYZ(VonXYZ, ColorBackGroundXYZ);
            MAcolorXYZ = Util.ColorSpaceConverter.AddXYZ(AcolorXYZ, ColorBackGroundXYZ);
            MBINcolorXYZ = Util.ColorSpaceConverter.AddXYZ(BINcolorXYZ, ColorBackGroundXYZ);

            //converting back to various spaces fpr color difference calculation
            MBradcolor = PerceptionLib.Color.ToLUV(MBradXYZ);
            MVoncolor = PerceptionLib.Color.ToLUV(MVonXYZ);
            MScalingcolor = PerceptionLib.Color.ToLUV(MScalingXYZ);
            MAcolor = PerceptionLib.Color.ToLUV(MAcolorXYZ);
            MBINColor = PerceptionLib.Color.ToLUV(MBINcolorXYZ);

            bradRGB = PerceptionLib.Color.ToRBG(MBradXYZ);
            VonRGB = PerceptionLib.Color.ToRBG(MVonXYZ);
            scalingRGB = PerceptionLib.Color.ToRBG(MScalingXYZ);
            acolorRGB = PerceptionLib.Color.ToRBG(MAcolorXYZ);
            BincolorRGB = PerceptionLib.Color.ToRBG(MBINcolorXYZ);

            DeltaB_luv = PerceptionLib.Color.ColorDistanceCal(Mbradcolor, ColorMeasured);
            DeltaB_Lab = PerceptionLib.Color.ColorDistanceCalAB(Mbradcolor, ColorMeasured);

            DeltaV_Luv = PerceptionLib.Color.ColorDistanceCal(ColorMeasured, MVoncolor);
            DeltaV_Lab = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, MVoncolor);

            DeltaS_Luv = PerceptionLib.Color.ColorDistanceCal(ColorMeasured, MScalingcolor);
            DeltaS_Lab = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, MScalingcolor);

            DeltaA_Luv = PerceptionLib.Color.ColorDistanceCal(ColorMeasured, MAcolor);
            DeltaA_LAb = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, MAcolor);

            DeltaBin_Luv = PerceptionLib.Color.ColorDistanceCal(ColorMeasured, MBINColor);
            DeltaBin_Lab = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, MBINColor);

            newRow[124] = MAcolorXYZ.X.ToString();
            newRow[125] = MAcolorXYZ.Y.ToString();
            newRow[126] = MAcolorXYZ.Z.ToString();
            newRow[127] = MAcolor.L.ToString();
            newRow[128] = MAcolor.U.ToString();
            newRow[129] = MAcolor.V.ToString();
            newRow[130] = MAcolor.UP.ToString();
            newRow[131] = MAcolor.VP.ToString();
            newRow[132] = MAcolor.LA.ToString();
            newRow[133] = MAcolor.A.ToString();
            newRow[134] = MAcolor.B.ToString();
            newRow[135] = acolorRGB.R.ToString();
            newRow[136] = acolorRGB.G.ToString();
            newRow[137] = acolorRGB.B.ToString();
            newRow[138] = MBradXYZ.X.ToString();
            newRow[139] = MBradXYZ.Y.ToString();
            newRow[140] = MBradXYZ.Z.ToString();
            newRow[141] = MBradcolor.L.ToString();
            newRow[142] = MBradcolor.U.ToString();
            newRow[143] = MBradcolor.V.ToString();
            newRow[144] = MBradcolor.UP.ToString();
            newRow[145] = MBradcolor.VP.ToString();
            newRow[146] = MBradcolor.LA.ToString();
            newRow[147] = MBradcolor.A.ToString();
            newRow[148] = MBradcolor.B.ToString();
            newRow[149] = MVonXYZ.X.ToString();
            newRow[150] = MVonXYZ.Y.ToString();
            newRow[151] = MVonXYZ.Z.ToString();
            newRow[152] = MVoncolor.L.ToString();
            newRow[153] = MVoncolor.U.ToString();
            newRow[154] = MVoncolor.V.ToString();
            newRow[155] = MVoncolor.UP.ToString();
            newRow[156] = MVoncolor.VP.ToString();
            newRow[157] = MVoncolor.LA.ToString();
            newRow[158] = MVoncolor.A.ToString();
            newRow[159] = MVoncolor.B.ToString();
            newRow[160] = MScalingXYZ.X.ToString();
            newRow[161] = MScalingXYZ.Y.ToString();
            newRow[162] = MScalingXYZ.Z.ToString();
            newRow[163] = MScalingcolor.L.ToString();
            newRow[164] = MScalingcolor.U.ToString();
            newRow[165] = MScalingcolor.V.ToString();
            newRow[166] = MScalingcolor.UP.ToString();
            newRow[167] = MScalingcolor.VP.ToString();
            newRow[168] = MScalingcolor.LA.ToString();
            newRow[169] = MScalingcolor.A.ToString();
            newRow[170] = MScalingcolor.B.ToString();
            newRow[171] = MBINcolorXYZ.X.ToString();
            newRow[172] = MBINcolorXYZ.Y.ToString();
            newRow[173] = MBINcolorXYZ.Z.ToString();
            newRow[174] = MBINColor.L.ToString();
            newRow[175] = MBINColor.U.ToString();
            newRow[176] = MBINColor.V.ToString();
            newRow[177] = MBINColor.UP.ToString();
            newRow[178] = MBINColor.VP.ToString();
            newRow[179] = MBINColor.LA.ToString();
            newRow[180] = MBINColor.A.ToString();
            newRow[181] = MBINColor.B.ToString();
            newRow[182] = DeltaA_Luv.ToString();
            newRow[183] = DeltaA_LAb.ToString();
            newRow[184] = DeltaBin_Luv.ToString();
            newRow[185] = DeltaBin_Lab.ToString();
            newRow[186] = DeltaB_luv.ToString();
            newRow[187] = DeltaB_Lab.ToString();
            newRow[188] = DeltaV_Luv.ToString();
            newRow[189] = DeltaV_Lab.ToString();
            newRow[190] = DeltaS_Luv.ToString();
            newRow[191] = DeltaS_Lab.ToString();
            newRow[192] = Util.CATCalulation.HueAngle(ColorMeasured).ToString();
            newRow[193] = Util.CATCalulation.HueAngle(Acolor).ToString();
            newRow[194] = Util.CATCalulation.HueAngle(MBINColor).ToString();
            
            dt_DataCollection.Rows.Add(newRow);


            R = 0; G = 0; B = 0;
            //dt.Rows.Add(newRow);

            //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt_DataCollection.DefaultView));
            //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
          }


        }
        
        //double AvgBin,AvgA,Avgb,AvgV,Avgs,dbc,dc;
        //double sAvgBin, sAvgA, sAvgb, sAvgV, sAvgs;
        //AvgBin =0;
        //AvgA = 0;
        //Avgb = 0;
        //AvgV = 0;
        //Avgs = 0;
        //dbc = 0;
        //dc = 0;

        //sAvgBin = 0;
        //sAvgA = 0;
        //sAvgb = 0;
        //sAvgV = 0;
        //sAvgs = 0;
        //dbc = 0;
        //dc = 0;
        
        //for (int i = 0; i < BgNo * FgNo; i = i + FgNo)
        //{
        //  newRow = dt_DataCollection.NewRow();
        //  for (int j = i; j < i+FgNo; j++)
        //  {

        //    if (j == i)
        //    {
        //      AvgBin = Convert.ToDouble(dt_DataCollection.Rows[j][115].ToString());
        //      AvgA = Convert.ToDouble(dt_DataCollection.Rows[j][113].ToString());
        //      Avgb = Convert.ToDouble(dt_DataCollection.Rows[j][117].ToString());
        //      AvgV = Convert.ToDouble(dt_DataCollection.Rows[j][119].ToString());
        //      Avgs = Convert.ToDouble(dt_DataCollection.Rows[j][121].ToString());
        //      sAvgBin = Convert.ToDouble(dt_DataCollection.Rows[j][184].ToString());
        //      sAvgA = Convert.ToDouble(dt_DataCollection.Rows[j][182].ToString());
        //      sAvgb = Convert.ToDouble(dt_DataCollection.Rows[j][186].ToString());
        //      sAvgV = Convert.ToDouble(dt_DataCollection.Rows[j][188].ToString());
        //      sAvgs = Convert.ToDouble(dt_DataCollection.Rows[j][190].ToString());
        //      dbc = Convert.ToDouble(dt_DataCollection.Rows[j][122].ToString());
        //      dc=Convert.ToDouble(dt_DataCollection.Rows[j][123].ToString());
        //    }
        //    else
        //    {
        //      //AvgBin = AvgBin + Convert.ToDouble(dt_DataCollection.Rows[j][115].ToString());
        //      //AvgA = AvgA+ Convert.ToDouble(dt_DataCollection.Rows[j][113].ToString());
        //      //Avgb = Avgb+Convert.ToDouble(dt_DataCollection.Rows[j][117].ToString());
        //      //AvgV = AvgV+Convert.ToDouble(dt_DataCollection.Rows[j][119].ToString());
        //      //Avgs = Avgs+Convert.ToDouble(dt_DataCollection.Rows[j][121].ToString());
        //      //sAvgBin = sAvgBin + Convert.ToDouble(dt_DataCollection.Rows[j][184].ToString());
        //      //sAvgA   = sAvgA + Convert.ToDouble(dt_DataCollection.Rows[j][182].ToString());
        //      //sAvgb   = sAvgb + Convert.ToDouble(dt_DataCollection.Rows[j][186].ToString());
        //      //sAvgV   = sAvgV + Convert.ToDouble(dt_DataCollection.Rows[j][188].ToString());
        //      //sAvgs   = sAvgs + Convert.ToDouble(dt_DataCollection.Rows[j][190].ToString());
        //      //dbc = dbc+Convert.ToDouble(dt_DataCollection.Rows[j][122].ToString());
        //      //dc =dc+Convert.ToDouble(dt_DataCollection.Rows[j][123].ToString());
        //    }
             
            
        //  }
          
        //  newRow[1] = (AvgBin / FgNo).ToString();
        //  newRow[2] = (AvgA/ FgNo).ToString();
        //  newRow[3] = (Avgb/ FgNo).ToString();
        //  newRow[4] = (AvgV/ FgNo).ToString();
        //  newRow[5] = (Avgs/ FgNo).ToString();
        //  newRow[6] = (dbc / FgNo).ToString();
        //  newRow[7] = (dc / FgNo).ToString();
        //  newRow[8] = (sAvgBin/ FgNo).ToString();
        //  newRow[9] = (sAvgA/ FgNo).ToString();
        //  newRow[10] = (sAvgb/ FgNo).ToString();
        //  newRow[11] = (sAvgV/ FgNo).ToString();
        //  newRow[11] = (sAvgs/ FgNo).ToString();
        //  dt_DataCollection.Rows.Add(newRow);
        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt_DataCollection.DefaultView));
        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
        //}
        
       
      });

      btn_ExportGrid.IsEnabled = true;

    }
    
    private void Btn_CSCompensation_Click(object sender, RoutedEventArgs e)
    {
      dtgrid_corrDisplay.IsEnabled = true;
      btn_StartMeasurment.IsEnabled = false;
      txt_R.IsEnabled = false;
      txt_G.IsEnabled = false;
      txt_B.IsEnabled = false;

      double X, Y, Z, DeltaA_Luv, DeltaA_LAb, DeltaBin_Luv, DeltaBin_Lab, DeltaB_luv, DeltaB_Lab, DeltaS_Luv, DeltaS_Lab, DeltaV_Luv, DeltaV_Lab, DeltaC, BinDeltaC;
      double CDeltaA_Luv, CDeltaA_LAb, CDeltaBin_Luv, CDeltaBin_Lab, CDeltaB_luv, CDeltaB_Lab, CDeltaS_Luv, CDeltaS_Lab, CDeltaV_Luv, CDeltaV_Lab;

      int binnumber = 0;
      DataRow newRow;

      int connect = PerceptionLib.Cs200Connection.ConnectToCS200();

      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\BgValue.csv");
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\BgValueSet1.csv");
      DataTable dt_Bg = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        dt_Bg = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));

      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\BaseBinFile.csv");
      DataTable bin = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));


      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\RGBgammut.csv");
      DataTable dt_RGBGammut = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        dt_RGBGammut = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
      }));

      BgNo = dt_Bg.Rows.Count;
      FgNo = dt_RGBGammut.Rows.Count;

      BgNo = 4;
      //FgNo = 2;
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\CscOMPENSATOR.csv");
      DataTable dt_DataCollection = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        dt_DataCollection = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
      }));

      ThreadPool.QueueUserWorkItem(ignored =>
      {
        PerceptionLib.RGBValue rgb = new PerceptionLib.RGBValue();
        for (int i = 0; i < BgNo; i++)
        {

          for (int j = 1; j < FgNo; j=j+20)
          {
            BgR = Convert.ToByte(dt_Bg.Rows[i][0].ToString());
            BgG = Convert.ToByte(dt_Bg.Rows[i][1].ToString());
            BgB = Convert.ToByte(dt_Bg.Rows[i][2].ToString());

            X = Convert.ToDouble(dt_Bg.Rows[i][3].ToString());
            Y = Convert.ToDouble(dt_Bg.Rows[i][4].ToString());
            Z = Convert.ToDouble(dt_Bg.Rows[i][5].ToString());

            ColorBackGroundXYZ = new CIEXYZ(X, Y, Z);


            R = Convert.ToByte(dt_RGBGammut.Rows[j][0].ToString());
            G = Convert.ToByte(dt_RGBGammut.Rows[j][1].ToString());
            B = Convert.ToByte(dt_RGBGammut.Rows[j][2].ToString());


            //cat model
            X = Convert.ToDouble(dt_RGBGammut.Rows[j][25].ToString());
            Y = Convert.ToDouble(dt_RGBGammut.Rows[j][26].ToString());
            Z = Convert.ToDouble(dt_RGBGammut.Rows[j][27].ToString());
            BradXYZ = new CIEXYZ(X, Y, Z);


            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
              rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
              rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
            }));

            DisplayMeasuredValuesFromCs200();

            ColorDifference = ColorMeasured;


            X = Convert.ToDouble(dt_RGBGammut.Rows[j][28].ToString());
            Y = Convert.ToDouble(dt_RGBGammut.Rows[j][29].ToString());
            Z = Convert.ToDouble(dt_RGBGammut.Rows[j][30].ToString());
            VonXYZ = new CIEXYZ(X, Y, Z);

            X = Convert.ToDouble(dt_RGBGammut.Rows[j][31].ToString());
            Y = Convert.ToDouble(dt_RGBGammut.Rows[j][32].ToString());
            Z = Convert.ToDouble(dt_RGBGammut.Rows[j][33].ToString());
            ScalingXYZ = new CIEXYZ(X, Y, Z);

            //direct model
            X = Convert.ToDouble(dt_RGBGammut.Rows[j][11].ToString());
            Y = Convert.ToDouble(dt_RGBGammut.Rows[j][12].ToString());
            Z = Convert.ToDouble(dt_RGBGammut.Rows[j][13].ToString());
            AcolorXYZ = new CIEXYZ(X, Y, Z);

            Acolor = PerceptionLib.Color.ToLUV(AcolorXYZ);

            //bin model
            X = Convert.ToDouble(dt_RGBGammut.Rows[j][22].ToString());
            Y = Convert.ToDouble(dt_RGBGammut.Rows[j][23].ToString());
            Z = Convert.ToDouble(dt_RGBGammut.Rows[j][24].ToString());
            BINcolorXYZ = new CIEXYZ(X, Y, Z);

            BINColor = PerceptionLib.Color.ToLUV(BINcolorXYZ);

            //adding XYZ values
            MBradXYZ = Util.ColorSpaceConverter.SubtractXYZ(BradXYZ, ColorBackGroundXYZ);
            MScalingXYZ = Util.ColorSpaceConverter.SubtractXYZ(ScalingXYZ, ColorBackGroundXYZ);
            MVonXYZ = Util.ColorSpaceConverter.SubtractXYZ(VonXYZ, ColorBackGroundXYZ);
            MAcolorXYZ = Util.ColorSpaceConverter.SubtractXYZ(AcolorXYZ, ColorBackGroundXYZ);
            MBINcolorXYZ = Util.ColorSpaceConverter.SubtractXYZ(BINcolorXYZ, ColorBackGroundXYZ);

            //converting back to various spaces fpr color difference calculation
            MBradcolor = PerceptionLib.Color.ToLUV(MBradXYZ);
            MVoncolor = PerceptionLib.Color.ToLUV(MVonXYZ);
            MScalingcolor = PerceptionLib.Color.ToLUV(MScalingXYZ);
            MAcolor = PerceptionLib.Color.ToLUV(MAcolorXYZ);
            MBINColor = PerceptionLib.Color.ToLUV(MBINcolorXYZ);



            bradRGB = PerceptionLib.Color.ToRBGFromLAB(MBradcolor);
            VonRGB = PerceptionLib.Color.ToRBGFromLAB(MVoncolor);
            scalingRGB = PerceptionLib.Color.ToRBGFromLAB(MScalingcolor);
            acolorRGB = PerceptionLib.Color.ToRBGFromLAB(MAcolor);
            BincolorRGB = PerceptionLib.Color.ToRBGFromLAB(MBINColor);


            

            newRow = dt_DataCollection.NewRow();
            newRow[0] = R;
            newRow[1] = G;
            newRow[2] = B;
            newRow[3] = dt_RGBGammut.Rows[j][3].ToString();
            newRow[4] = dt_RGBGammut.Rows[j][4].ToString();
            newRow[5] = dt_RGBGammut.Rows[j][5].ToString();
            newRow[6] = dt_RGBGammut.Rows[j][6].ToString();
            newRow[7] = dt_RGBGammut.Rows[j][7].ToString();
            newRow[8] = dt_RGBGammut.Rows[j][8].ToString();
            newRow[9] = dt_RGBGammut.Rows[j][9].ToString();
            newRow[10] = dt_RGBGammut.Rows[j][10].ToString();
            newRow[11] = dt_RGBGammut.Rows[j][11].ToString();
            newRow[12] = dt_RGBGammut.Rows[j][12].ToString();
            newRow[13] = dt_RGBGammut.Rows[j][13].ToString();
            newRow[14] = dt_RGBGammut.Rows[j][14].ToString();
            newRow[15] = dt_RGBGammut.Rows[j][15].ToString();
            newRow[16] = dt_RGBGammut.Rows[j][16].ToString();
            newRow[17] = dt_RGBGammut.Rows[j][17].ToString();
            newRow[18] = dt_RGBGammut.Rows[j][18].ToString();
            newRow[19] = dt_RGBGammut.Rows[j][19].ToString();
            newRow[20] = dt_RGBGammut.Rows[j][20].ToString();
            newRow[21] = dt_RGBGammut.Rows[j][21].ToString();
            newRow[22] = dt_RGBGammut.Rows[j][22].ToString();
            newRow[23] = dt_RGBGammut.Rows[j][23].ToString();
            newRow[24] = dt_RGBGammut.Rows[j][24].ToString();
            newRow[25] = dt_RGBGammut.Rows[j][25].ToString();
            newRow[26] = dt_RGBGammut.Rows[j][26].ToString();
            newRow[27] = dt_RGBGammut.Rows[j][27].ToString();
            newRow[28] = dt_RGBGammut.Rows[j][28].ToString();
            newRow[29] = dt_RGBGammut.Rows[j][29].ToString();
            newRow[30] = dt_RGBGammut.Rows[j][30].ToString();
            newRow[31] = dt_RGBGammut.Rows[j][31].ToString();
            newRow[32] = dt_RGBGammut.Rows[j][32].ToString();
            newRow[33] = dt_RGBGammut.Rows[j][33].ToString();
            newRow[34] = BgR;
            newRow[35] = BgG;
            newRow[36] = BgB;
            newRow[37] = dt_Bg.Rows[i][3].ToString();
            newRow[38] = dt_Bg.Rows[i][4].ToString();
            newRow[39] = dt_Bg.Rows[i][5].ToString();
            newRow[40] = dt_Bg.Rows[i][6].ToString();
            newRow[41] = dt_Bg.Rows[i][7].ToString();
            newRow[42] = dt_Bg.Rows[i][8].ToString();
            
            newRow[43] = MAcolorXYZ.X.ToString();
            newRow[44] = MAcolorXYZ.Y.ToString();
            newRow[45] = MAcolorXYZ.Z.ToString();
            newRow[46] = MAcolor.LA.ToString();
            newRow[47] = MAcolor.A.ToString();
            newRow[48] = MAcolor.B.ToString();
            newRow[49] = acolorRGB.R.ToString();
            newRow[50] = acolorRGB.G.ToString();
            newRow[51] = acolorRGB.B.ToString();
                       
            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
              rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BgR, BgG, BgB));
              rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(acolorRGB.R, acolorRGB.G, acolorRGB.B));
            }));

            DisplayMeasuredValuesFromCs200();

            acolorRGB = PerceptionLib.Color.ToRBG(MAcolorXYZ);

            newRow[52] = ColorMeasuredXYZ.X.ToString();
            newRow[53] = ColorMeasuredXYZ.Y.ToString();
            newRow[54] = ColorMeasuredXYZ.Z.ToString();
            newRow[55] = ColorMeasured.LA.ToString();
            newRow[56] = ColorMeasured.A.ToString();
            newRow[57] = ColorMeasured.B.ToString();
            newRow[58] = MR.ToString();
            newRow[59] = MG.ToString();
            newRow[60] = MB.ToString();

            ColorToShow.LA =Convert.ToDouble( dt_RGBGammut.Rows[j][19].ToString());
            ColorToShow.A = Convert.ToDouble( dt_RGBGammut.Rows[j][20].ToString());
            ColorToShow.B = Convert.ToDouble( dt_RGBGammut.Rows[j][21].ToString());


           // DeltaA_Luv = PerceptionLib.Color.ColorDistanceCalAB(MAcolor, ColorMeasured);
            DeltaA_Luv = PerceptionLib.Color.ColorDistanceCalAB(ColorToShow, ColorMeasured);
            CDeltaA_Luv = PerceptionLib.Color.ColorDistanceCalAB(ColorDifference, ColorMeasured);

            X = Convert.ToDouble(dt_Bg.Rows[i][6].ToString());
            Y = Convert.ToDouble(dt_Bg.Rows[i][7].ToString());
            Z = Convert.ToDouble(dt_Bg.Rows[i][8].ToString());

            ColorBackGroundXYZ = new CIEXYZ(X, Y, Z);
            
            MAcolorXYZ = Util.ColorSpaceConverter.SubtractXYZ(AcolorXYZ, ColorBackGroundXYZ);
            MAcolor = PerceptionLib.Color.ToLUV(MAcolorXYZ);
            acolorRGB = PerceptionLib.Color.ToRBGFromLAB(MAcolor);
            
            
            newRow[61] = MAcolorXYZ.X.ToString();
            newRow[62] = MAcolorXYZ.Y.ToString();
            newRow[63] = MAcolorXYZ.Z.ToString();
            newRow[64] = MAcolor.LA.ToString();
            newRow[65] = MAcolor.A.ToString();
            newRow[66] = MAcolor.B.ToString();
            newRow[67] = acolorRGB.R.ToString();
            newRow[68] = acolorRGB.G.ToString();
            newRow[69] = acolorRGB.B.ToString();

            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
              rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(acolorRGB.R, acolorRGB.G, acolorRGB.B));
             }));

            DisplayMeasuredValuesFromCs200();

            MacolorRGB = PerceptionLib.Color.ToRBG(MAcolorXYZ);
            
            newRow[70] = ColorMeasuredXYZ.X.ToString();
            newRow[71] = ColorMeasuredXYZ.Y.ToString();
            newRow[72] = ColorMeasuredXYZ.Z.ToString();
            newRow[73] = ColorMeasured.LA.ToString();
            newRow[74] = ColorMeasured.A.ToString();
            newRow[75] = ColorMeasured.B.ToString();
            newRow[76] = MR.ToString();
            newRow[77] = MG.ToString();
            newRow[78] = MB.ToString();

           // DeltaA_LAb = PerceptionLib.Color.ColorDistanceCalAB(MAcolor, ColorMeasured);
            DeltaA_LAb = PerceptionLib.Color.ColorDistanceCalAB(ColorToShow, ColorMeasured);
            CDeltaA_LAb = PerceptionLib.Color.ColorDistanceCalAB(ColorDifference, ColorMeasured);

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////brad color
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

           // newRow[79] = MBradXYZ.X.ToString();
           // newRow[80] = MBradXYZ.Y.ToString();
           // newRow[81] = MBradXYZ.Z.ToString();
           // newRow[82] = MBradcolor.LA.ToString();
           // newRow[83] = MBradcolor.A.ToString();
           // newRow[84] = MBradcolor.B.ToString();
           // newRow[85] = bradRGB.R.ToString();
           // newRow[86] = bradRGB.G.ToString();
           // newRow[87] = bradRGB.B.ToString();

           // Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
           // {
              
           //   rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(bradRGB.R, bradRGB.G, bradRGB.B));
           // }));

           // DisplayMeasuredValuesFromCs200();
           // bradRGB = PerceptionLib.Color.ToRBG(MBradXYZ);


           // newRow[88] = ColorMeasuredXYZ.X.ToString();
           // newRow[89] = ColorMeasuredXYZ.Y.ToString();
           // newRow[90] = ColorMeasuredXYZ.Z.ToString();
           // newRow[91] = ColorMeasured.LA.ToString();
           // newRow[92] = ColorMeasured.A.ToString();
           // newRow[93] = ColorMeasured.B.ToString();
           // newRow[94] = MR.ToString();
           // newRow[95] = MG.ToString();
           // newRow[96] = MB.ToString();

           // //DeltaB_luv = PerceptionLib.Color.ColorDistanceCalAB(MBradcolor, ColorMeasured);
           // DeltaB_luv = PerceptionLib.Color.ColorDistanceCalAB(ColorToShow, ColorMeasured);
           // CDeltaB_luv = PerceptionLib.Color.ColorDistanceCalAB(ColorDifference, ColorMeasured);

           // X = Convert.ToDouble(dt_Bg.Rows[i][6].ToString());
           // Y = Convert.ToDouble(dt_Bg.Rows[i][7].ToString());
           // Z = Convert.ToDouble(dt_Bg.Rows[i][8].ToString());

           // ColorBackGroundXYZ = new CIEXYZ(X, Y, Z);

           // MBradXYZ = Util.ColorSpaceConverter.SubtractXYZ(BradXYZ, ColorBackGroundXYZ);
           // MBradcolor = PerceptionLib.Color.ToLUV(MBradXYZ);
           // bradRGB = PerceptionLib.Color.ToRBGFromLAB(MBradcolor);


           // newRow[97] = MBradXYZ.X.ToString();
           // newRow[98] = MBradXYZ.Y.ToString();
           // newRow[99] = MBradXYZ.Z.ToString();
           // newRow[100] = MBradcolor.LA.ToString();
           // newRow[101] = MBradcolor.A.ToString();
           // newRow[102] = MBradcolor.B.ToString();
           // newRow[103] = bradRGB.R.ToString();
           // newRow[104] = bradRGB.G.ToString();
           // newRow[105] = bradRGB.B.ToString();

           // Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
           // {
           //   rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(bradRGB.R, bradRGB.G, bradRGB.B));
           // }));

           // DisplayMeasuredValuesFromCs200();

           // MbradRGB = PerceptionLib.Color.ToRBG(MBradXYZ);

           // newRow[106] = ColorMeasuredXYZ.X.ToString();
           // newRow[107] = ColorMeasuredXYZ.Y.ToString();
           // newRow[108] = ColorMeasuredXYZ.Z.ToString();
           // newRow[109] = ColorMeasured.LA.ToString();
           // newRow[110] = ColorMeasured.A.ToString();
           // newRow[111] = ColorMeasured.B.ToString();
           // newRow[112] = MR.ToString();
           // newRow[113] = MG.ToString();
           // newRow[114] = MB.ToString();

           // //DeltaB_Lab = PerceptionLib.Color.ColorDistanceCalAB(MBradcolor, ColorMeasured);
           // DeltaB_Lab = PerceptionLib.Color.ColorDistanceCalAB(ColorToShow, ColorMeasured);
           // CDeltaB_Lab = PerceptionLib.Color.ColorDistanceCalAB(ColorDifference, ColorMeasured);

           // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
           // ////Von color
           // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

           // newRow[115] = MVonXYZ.X.ToString();
           // newRow[116] = MVonXYZ.Y.ToString();
           // newRow[117] = MVonXYZ.Z.ToString();
           // newRow[118] = MVoncolor.LA.ToString();
           // newRow[119] = MVoncolor.A.ToString();
           // newRow[120] = MVoncolor.B.ToString();
           // newRow[121] = VonRGB.R.ToString();
           // newRow[122] = VonRGB.G.ToString();
           // newRow[123] = VonRGB.B.ToString();

           // Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
           // {
           //   rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(VonRGB.R, VonRGB.G, VonRGB.B));
           // }));

           // DisplayMeasuredValuesFromCs200();

           // VonRGB = PerceptionLib.Color.ToRBG(MVonXYZ);

           // newRow[124] = ColorMeasuredXYZ.X.ToString();
           // newRow[125] = ColorMeasuredXYZ.Y.ToString();
           // newRow[126] = ColorMeasuredXYZ.Z.ToString();
           // newRow[127] = ColorMeasured.LA.ToString();
           // newRow[128] = ColorMeasured.A.ToString();
           // newRow[129] = ColorMeasured.B.ToString();
           // newRow[130] = MR.ToString();
           // newRow[131] = MG.ToString();
           // newRow[132] = MB.ToString();

           //// DeltaV_Luv = PerceptionLib.Color.ColorDistanceCalAB(MVoncolor, ColorMeasured);
           // DeltaV_Luv = PerceptionLib.Color.ColorDistanceCalAB(ColorToShow, ColorMeasured);
           // CDeltaV_Luv = PerceptionLib.Color.ColorDistanceCalAB(ColorDifference, ColorMeasured);

           // X = Convert.ToDouble(dt_Bg.Rows[i][6].ToString());
           // Y = Convert.ToDouble(dt_Bg.Rows[i][7].ToString());
           // Z = Convert.ToDouble(dt_Bg.Rows[i][8].ToString());

           // ColorBackGroundXYZ = new CIEXYZ(X, Y, Z);

           // MVonXYZ = Util.ColorSpaceConverter.SubtractXYZ(VonXYZ, ColorBackGroundXYZ);
           // MVoncolor = PerceptionLib.Color.ToLUV(MVonXYZ);
           // VonRGB = PerceptionLib.Color.ToRBGFromLAB(MVoncolor);
                   


           // newRow[133] = MVonXYZ.X.ToString();
           // newRow[134] = MVonXYZ.Y.ToString();
           // newRow[135] = MVonXYZ.Z.ToString();
           // newRow[136] = MVoncolor.LA.ToString();
           // newRow[137] = MVoncolor.A.ToString();
           // newRow[138] = MVoncolor.B.ToString();
           // newRow[139] = VonRGB.R.ToString();
           // newRow[140] = VonRGB.G.ToString();
           // newRow[141] = VonRGB.B.ToString();

           // Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
           // {
           //   rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(VonRGB.R, VonRGB.G, VonRGB.B));
           // }));

           // DisplayMeasuredValuesFromCs200();

           // MVonRGB = PerceptionLib.Color.ToRBG(MVonXYZ);

           // newRow[142] = ColorMeasuredXYZ.X.ToString();
           // newRow[143] = ColorMeasuredXYZ.Y.ToString();
           // newRow[144] = ColorMeasuredXYZ.Z.ToString();
           // newRow[145] = ColorMeasured.LA.ToString();
           // newRow[146] = ColorMeasured.A.ToString();
           // newRow[147] = ColorMeasured.B.ToString();
           // newRow[148] = MR.ToString();
           // newRow[149] = MG.ToString();
           // newRow[150] = MB.ToString();

           // //DeltaV_Lab = PerceptionLib.Color.ColorDistanceCalAB(MVoncolor, ColorMeasured);
           // DeltaV_Lab = PerceptionLib.Color.ColorDistanceCalAB(ColorToShow, ColorMeasured);
           // CDeltaV_Lab = PerceptionLib.Color.ColorDistanceCalAB(ColorDifference, ColorMeasured);
            
           // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
           // ////S color
           // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

           // newRow[151] = MScalingXYZ.X.ToString();
           // newRow[152] = MScalingXYZ.Y.ToString();
           // newRow[153] = MScalingXYZ.Z.ToString();
           // newRow[154] = MScalingcolor.LA.ToString();
           // newRow[155] = MScalingcolor.A.ToString();
           // newRow[156] = MScalingcolor.B.ToString();
           // newRow[157] = scalingRGB.R.ToString();
           // newRow[158] = scalingRGB.G.ToString();
           // newRow[159] = scalingRGB.B.ToString();
            
           // Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
           // {
           //   rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(scalingRGB.R, scalingRGB.G, scalingRGB.B));
           // }));

           // DisplayMeasuredValuesFromCs200();

           // scalingRGB = PerceptionLib.Color.ToRBG(MScalingXYZ);

           // newRow[160] = ColorMeasuredXYZ.X.ToString();
           // newRow[161] = ColorMeasuredXYZ.Y.ToString();
           // newRow[162] = ColorMeasuredXYZ.Z.ToString();
           // newRow[163] = ColorMeasured.LA.ToString();
           // newRow[164] = ColorMeasured.A.ToString();
           // newRow[165] = ColorMeasured.B.ToString();
           // newRow[166] = MR.ToString();
           // newRow[167] = MG.ToString();
           // newRow[168] = MB.ToString();

           // //DeltaS_Luv = PerceptionLib.Color.ColorDistanceCalAB(MScalingcolor, ColorMeasured);
           // DeltaS_Luv = PerceptionLib.Color.ColorDistanceCalAB(ColorToShow, ColorMeasured);
           // CDeltaS_Luv = PerceptionLib.Color.ColorDistanceCalAB(ColorDifference, ColorMeasured);

           // X = Convert.ToDouble(dt_Bg.Rows[i][6].ToString());
           // Y = Convert.ToDouble(dt_Bg.Rows[i][7].ToString());
           // Z = Convert.ToDouble(dt_Bg.Rows[i][8].ToString());

           // ColorBackGroundXYZ = new CIEXYZ(X, Y, Z);

           // MScalingXYZ = Util.ColorSpaceConverter.SubtractXYZ(ScalingXYZ, ColorBackGroundXYZ);
           // MScalingcolor = PerceptionLib.Color.ToLUV(MVonXYZ);
           // scalingRGB = PerceptionLib.Color.ToRBGFromLAB(MScalingcolor);


           // newRow[169] = MScalingXYZ.X.ToString();
           // newRow[170] = MScalingXYZ.Y.ToString();
           // newRow[171] = MScalingXYZ.Z.ToString();
           // newRow[172] = MScalingcolor.LA.ToString();
           // newRow[173] = MScalingcolor.A.ToString();
           // newRow[174] = MScalingcolor.B.ToString();
           // newRow[175] = scalingRGB.R.ToString();
           // newRow[176] = scalingRGB.G.ToString();
           // newRow[177] = scalingRGB.B.ToString();

           // Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
           // {
           //   rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(scalingRGB.R, scalingRGB.G, scalingRGB.B));
           // }));

           // DisplayMeasuredValuesFromCs200();

           // MscalingRGB = PerceptionLib.Color.ToRBG(MScalingXYZ);

           // newRow[178] = ColorMeasuredXYZ.X.ToString();
           // newRow[179] = ColorMeasuredXYZ.Y.ToString();
           // newRow[180] = ColorMeasuredXYZ.Z.ToString();
           // newRow[181] = ColorMeasured.LA.ToString();
           // newRow[182] = ColorMeasured.A.ToString();
           // newRow[183] = ColorMeasured.B.ToString();
           // newRow[184] = MR.ToString();
           // newRow[185] = MG.ToString();
           // newRow[186] = MB.ToString();

           // //DeltaS_Lab = PerceptionLib.Color.ColorDistanceCalAB(MScalingcolor, ColorMeasured);
           // DeltaS_Lab = PerceptionLib.Color.ColorDistanceCalAB(ColorToShow, ColorMeasured);
           // CDeltaS_Lab = PerceptionLib.Color.ColorDistanceCalAB(ColorDifference, ColorMeasured);

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////bin color
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            binnumber = Util.CATCalulation.MatchWithBinnedColors(MBINColor, bin,ColorBackGroundXYZ,BINColor);
            int binnum = Util.CATCalulation.MatchWithBinnedColorsBasedOn3D(MBINColor, bin, ColorBackGroundXYZ, BINColor);

            MBINcolorXYZ.X = Convert.ToDouble(bin.Rows[binnumber][6].ToString());
            MBINcolorXYZ.Y = Convert.ToDouble(bin.Rows[binnumber][7].ToString());
            MBINcolorXYZ.Z = Convert.ToDouble(bin.Rows[binnumber][8].ToString());
            MBINColor.LA = Convert.ToDouble(bin.Rows[binnumber][3].ToString());
            MBINColor.A = Convert.ToDouble(bin.Rows[binnumber][4].ToString());
            MBINColor.B = Convert.ToDouble(bin.Rows[binnumber][5].ToString());
            BincolorRGB.R = Convert.ToByte(bin.Rows[binnumber][0].ToString());
            BincolorRGB.G = Convert.ToByte(bin.Rows[binnumber][1].ToString());
            BincolorRGB.B = Convert.ToByte(bin.Rows[binnumber][2].ToString());
            
            newRow[187] = MBINcolorXYZ.X.ToString();
            newRow[188] = MBINcolorXYZ.Y.ToString();
            newRow[189] = MBINcolorXYZ.Z.ToString();
            newRow[190] = MBINColor.LA.ToString();
            newRow[191] = MBINColor.A.ToString();
            newRow[192] = MBINColor.B.ToString();
            newRow[193] = BincolorRGB.R.ToString();
            newRow[194] = BincolorRGB.G.ToString();
            newRow[195] = BincolorRGB.B.ToString();

            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
              rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BincolorRGB.R, BincolorRGB.G, BincolorRGB.B));
            }));

            DisplayMeasuredValuesFromCs200();
            BincolorRGB = PerceptionLib.Color.ToRBG(MBINcolorXYZ);

            newRow[196] = ColorMeasuredXYZ.X.ToString();
            newRow[197] = ColorMeasuredXYZ.Y.ToString();
            newRow[198] = ColorMeasuredXYZ.Z.ToString();
            newRow[199] = ColorMeasured.LA.ToString();
            newRow[200] = ColorMeasured.A.ToString();
            newRow[201] = ColorMeasured.B.ToString();
            newRow[202] = MR.ToString();
            newRow[203] = MG.ToString();
            newRow[204] = MB.ToString();

            //DeltaBin_Luv = PerceptionLib.Color.ColorDistanceCalAB(MBINColor, ColorMeasured);
            DeltaBin_Luv = PerceptionLib.Color.ColorDistanceCalAB(ColorToShow, ColorMeasured);
            CDeltaBin_Luv = PerceptionLib.Color.ColorDistanceCalAB(ColorDifference, ColorMeasured);

            X = Convert.ToDouble(dt_Bg.Rows[i][6].ToString());
            Y = Convert.ToDouble(dt_Bg.Rows[i][7].ToString());
            Z = Convert.ToDouble(dt_Bg.Rows[i][8].ToString());

            ColorBackGroundXYZ = new CIEXYZ(X, Y, Z);

            MBINcolorXYZ = Util.ColorSpaceConverter.SubtractXYZ(BinXYZ, ColorBackGroundXYZ);
            MBINColor = PerceptionLib.Color.ToLUV(MBINcolorXYZ);
            //BincolorRGB = PerceptionLib.Color.ToRBGFromLAB(MBINColor);

            binnumber = Util.CATCalulation.MatchWithBinnedColors(MBINColor, bin, ColorBackGroundXYZ,BINColor);

            MBINcolorXYZ.X = Convert.ToDouble(bin.Rows[binnumber][6].ToString());
            MBINcolorXYZ.Y = Convert.ToDouble(bin.Rows[binnumber][7].ToString());
            MBINcolorXYZ.Z = Convert.ToDouble(bin.Rows[binnumber][8].ToString());
            MBINColor.LA = Convert.ToDouble(bin.Rows[binnumber][3].ToString());
            MBINColor.A = Convert.ToDouble(bin.Rows[binnumber][4].ToString());
            MBINColor.B = Convert.ToDouble(bin.Rows[binnumber][5].ToString());
            BincolorRGB.R = Convert.ToByte(bin.Rows[binnumber][0].ToString());
            BincolorRGB.G = Convert.ToByte(bin.Rows[binnumber][1].ToString());
            BincolorRGB.B = Convert.ToByte(bin.Rows[binnumber][2].ToString());

            newRow[205] = MBINcolorXYZ.X.ToString();
            newRow[206] = MBINcolorXYZ.Y.ToString();
            newRow[207] = MBINcolorXYZ.Z.ToString();
            newRow[208] = MBINColor.LA.ToString();
            newRow[209] = MBINColor.A.ToString();
            newRow[210] = MBINColor.B.ToString();
            newRow[211] = BincolorRGB.R.ToString();
            newRow[212] = BincolorRGB.G.ToString();
            newRow[213] = BincolorRGB.B.ToString();

            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
              rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BincolorRGB.R, BincolorRGB.G, BincolorRGB.B));
            }));

            DisplayMeasuredValuesFromCs200();

            MBincolorRGB = PerceptionLib.Color.ToRBG(MBINcolorXYZ);

            newRow[214] = ColorMeasuredXYZ.X.ToString();
            newRow[215] = ColorMeasuredXYZ.Y.ToString();
            newRow[216] = ColorMeasuredXYZ.Z.ToString();
            newRow[217] = ColorMeasured.LA.ToString();
            newRow[218] = ColorMeasured.A.ToString();
            newRow[219] = ColorMeasured.B.ToString();
            newRow[220] = MR.ToString();
            newRow[221] = MG.ToString();
            newRow[222] = MB.ToString();

            //DeltaBin_Lab = PerceptionLib.Color.ColorDistanceCalAB(MBINColor, ColorMeasured);
            DeltaBin_Lab = PerceptionLib.Color.ColorDistanceCalAB(ColorToShow, ColorMeasured);
            CDeltaBin_Lab = PerceptionLib.Color.ColorDistanceCalAB(ColorDifference, ColorMeasured);

            newRow[223] = DeltaA_Luv.ToString();
            newRow[224] = DeltaA_LAb.ToString();
            //newRow[225] = DeltaB_luv.ToString();
            //newRow[226] = DeltaB_Lab.ToString();
            //newRow[227] = DeltaV_Luv.ToString();
            //newRow[228] = DeltaV_Lab.ToString();
            //newRow[229] = DeltaS_Luv.ToString();
            //newRow[230] = DeltaS_Lab.ToString();
            newRow[231] = DeltaBin_Luv.ToString();
            newRow[232] = DeltaBin_Lab.ToString();

            newRow[233] = acolorRGB.gmt.ToString();
            //newRow[234] = bradRGB.gmt.ToString();
            //newRow[235] = VonRGB.gmt.ToString();
            //newRow[236] = scalingRGB.gmt.ToString();
            newRow[237] = BincolorRGB.gmt.ToString();
            newRow[238] = MacolorRGB.gmt.ToString(); 
            //newRow[239] = MbradRGB.gmt.ToString();
            //newRow[240] = MVonRGB.gmt.ToString();
            //newRow[241] = MscalingRGB.gmt.ToString();
            newRow[242] = MBincolorRGB.gmt.ToString();
           
            newRow[243] = CDeltaA_Luv.ToString();
            newRow[244] = CDeltaA_LAb.ToString();
            //newRow[245] = CDeltaB_luv.ToString();
            //newRow[246] = CDeltaB_Lab.ToString();
            //newRow[247] = CDeltaV_Luv.ToString();
            //newRow[248] = CDeltaV_Lab.ToString();
            //newRow[249] = CDeltaS_Luv.ToString();
            //newRow[250] = CDeltaS_Lab.ToString();
            newRow[251] = CDeltaBin_Luv.ToString();
            newRow[252] = CDeltaBin_Lab.ToString();
            newRow[253] = ColorDifference.LA.ToString();
            newRow[254] = ColorDifference.A.ToString() + "," + scalingRGB.gmt.ToString() ;
           // newRow[255] = scalingRGB.gmt.ToString();;


            newRow[249] = binnumber.ToString();
            newRow[250] = binnum.ToString();
            
            dt_DataCollection.Rows.Add(newRow);



            R = 0; G = 0; B = 0;


            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt_DataCollection.DefaultView));
            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
          }


        }
        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt_DataCollection.DefaultView));
        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

      //  double AvgBin, AvgA, Avgb, AvgV, Avgs, dbc, dc;
      //  double sAvgBin, sAvgA, sAvgb, sAvgV, sAvgs;
      //  AvgBin = 0;
      //  AvgA = 0;
      //  Avgb = 0;
      //  AvgV = 0;
      //  Avgs = 0;
      //  dbc = 0;
      //  dc = 0;

      //  sAvgBin = 0;
      //  sAvgA = 0;
      //  sAvgb = 0;
      //  sAvgV = 0;
      //  sAvgs = 0;
      //  dbc = 0;
      //  dc = 0;

      //  for (int i = 0; i < BgNo * FgNo; i = i + FgNo)
      //  {
      //    newRow = dt_DataCollection.NewRow();
      //    for (int j = i; j < i + FgNo; j++)
      //    {

      //      if (j == i)
      //      {
      //        AvgBin = Convert.ToDouble(dt_DataCollection.Rows[j][115].ToString());
      //        AvgA = Convert.ToDouble(dt_DataCollection.Rows[j][113].ToString());
      //        Avgb = Convert.ToDouble(dt_DataCollection.Rows[j][117].ToString());
      //        AvgV = Convert.ToDouble(dt_DataCollection.Rows[j][119].ToString());
      //        Avgs = Convert.ToDouble(dt_DataCollection.Rows[j][121].ToString());
      //        sAvgBin = Convert.ToDouble(dt_DataCollection.Rows[j][184].ToString());
      //        sAvgA = Convert.ToDouble(dt_DataCollection.Rows[j][182].ToString());
      //        sAvgb = Convert.ToDouble(dt_DataCollection.Rows[j][186].ToString());
      //        sAvgV = Convert.ToDouble(dt_DataCollection.Rows[j][188].ToString());
      //        sAvgs = Convert.ToDouble(dt_DataCollection.Rows[j][190].ToString());
      //        dbc = Convert.ToDouble(dt_DataCollection.Rows[j][122].ToString());
      //        dc = Convert.ToDouble(dt_DataCollection.Rows[j][123].ToString());
      //      }
      //      else
      //      {
      //        AvgBin = AvgBin + Convert.ToDouble(dt_DataCollection.Rows[j][115].ToString());
      //        AvgA = AvgA + Convert.ToDouble(dt_DataCollection.Rows[j][113].ToString());
      //        Avgb = Avgb + Convert.ToDouble(dt_DataCollection.Rows[j][117].ToString());
      //        AvgV = AvgV + Convert.ToDouble(dt_DataCollection.Rows[j][119].ToString());
      //        Avgs = Avgs + Convert.ToDouble(dt_DataCollection.Rows[j][121].ToString());
      //        sAvgBin = sAvgBin + Convert.ToDouble(dt_DataCollection.Rows[j][184].ToString());
      //        sAvgA = sAvgA + Convert.ToDouble(dt_DataCollection.Rows[j][182].ToString());
      //        sAvgb = sAvgb + Convert.ToDouble(dt_DataCollection.Rows[j][186].ToString());
      //        sAvgV = sAvgV + Convert.ToDouble(dt_DataCollection.Rows[j][188].ToString());
      //        sAvgs = sAvgs + Convert.ToDouble(dt_DataCollection.Rows[j][190].ToString());
      //        dbc = dbc + Convert.ToDouble(dt_DataCollection.Rows[j][122].ToString());
      //        dc = dc + Convert.ToDouble(dt_DataCollection.Rows[j][123].ToString());
      //      }


      //    }

      //    newRow[1] = (AvgBin / FgNo).ToString();
      //    newRow[2] = (AvgA / FgNo).ToString();
      //    newRow[3] = (Avgb / FgNo).ToString();
      //    newRow[4] = (AvgV / FgNo).ToString();
      //    newRow[5] = (Avgs / FgNo).ToString();
      //    newRow[6] = (dbc / FgNo).ToString();
      //    newRow[7] = (dc / FgNo).ToString();
      //    newRow[8] = (sAvgBin / FgNo).ToString();
      //    newRow[9] = (sAvgA / FgNo).ToString();
      //    newRow[10] = (sAvgb / FgNo).ToString();
      //    newRow[11] = (sAvgV / FgNo).ToString();
      //    newRow[11] = (sAvgs / FgNo).ToString();
      //    dt_DataCollection.Rows.Add(newRow);
      //    Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt_DataCollection.DefaultView));
      //    Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
      //  }


      });

      btn_ExportGrid.IsEnabled = true;
    }

    private void Btn_CSBasicCompensation_Click(object sender, RoutedEventArgs e)
    {
      dtgrid_corrDisplay.IsEnabled = true;
      btn_StartMeasurment.IsEnabled = false;
      txt_R.IsEnabled = false;
      txt_G.IsEnabled = false;
      txt_B.IsEnabled = false;

      double X, Y, Z, DeltaA_Luv, DeltaA_LAb, DeltaBin_Luv, DeltaBin_Lab;
      double CDeltaA_Luv, CDeltaA_LAb, CDeltaBin_Luv, CDeltaBin_Lab;

      int binnumber = 0;
      int GammutRangeCheck1, GammutRangeCheck2, GammutRangeCheck3, GammutRangeCheck4;

      DataRow newRow;

      int connect = PerceptionLib.Cs200Connection.ConnectToCS200();
      
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\BgValueWtPt.csv");
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\BgValueSet1.csv");
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\BgValue.csv");
      DataTable dt_Bg = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        dt_Bg = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));

      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\BaseBinFile.csv");
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\bigProjecor\BaseBinFileLSort.csv");
      DataTable bin = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));
      
      double minLLevel = double.MaxValue;
      double maxLLevel = double.MinValue;
      double minALevel = double.MaxValue;
      double maxALevel = double.MinValue;
      double minBLevel = double.MaxValue;
      double maxBLevel = double.MinValue;

      double minL = double.MaxValue;
      double maxL = double.MinValue;
      double minA = double.MaxValue;
      double maxA = double.MinValue;
      double minB = double.MaxValue;
      double maxB = double.MinValue;

      foreach (DataRow dr in bin.Rows)
      {
        double accountLevel = dr.Field<double>("MLA");
        minLLevel = Math.Min(minLLevel, accountLevel);
        maxLLevel = Math.Max(maxLLevel, accountLevel);

        double ALevel = dr.Field<double>("MA");
        minALevel = Math.Min(minALevel, ALevel);
        maxALevel = Math.Max(maxALevel, ALevel);
        
        double BLevel = dr.Field<double>("MlaB");
        minBLevel = Math.Min(minBLevel, BLevel);
        maxBLevel = Math.Max(maxBLevel, BLevel);

        double account = dr.Field<double>("LA");
        minL = Math.Min(minL, account);
        maxL = Math.Max(maxL, account);

        double ALvl = dr.Field<double>("A");
        minA = Math.Min(minA, ALvl);
        maxA = Math.Max(maxA, ALvl);

        double BLvl = dr.Field<double>("LB");
        minB = Math.Min(minBLevel, BLvl);
        maxB = Math.Max(maxBLevel, BLvl);
      }

      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\RGBgammut.csv");
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\bigProjecor\RGB88.csv");
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\bigProjecor\RGB8000Sorted.csv");
      DataTable dt_RGBGammut = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        dt_RGBGammut = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
      }));

      BgNo = dt_Bg.Rows.Count;
      FgNo = dt_RGBGammut.Rows.Count;

      BgNo = 24;
      //FgNo = 400;
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\CsBasicCOMPENSATOR.csv");
      
      DataTable dt_DataCollection = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        dt_DataCollection = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
      }));

      ThreadPool.QueueUserWorkItem(ignored =>
      {
        PerceptionLib.RGBValue rgb = new PerceptionLib.RGBValue();
        for (int i = 0; i < BgNo; i++)
        {

          for (int j = 0; j < FgNo; j=j+20)
          {
            newRow = dt_DataCollection.NewRow();
            BgR = Convert.ToByte(dt_Bg.Rows[i][0].ToString());
            BgG = Convert.ToByte(dt_Bg.Rows[i][1].ToString());
            BgB = Convert.ToByte(dt_Bg.Rows[i][2].ToString());

            X = Convert.ToDouble(dt_Bg.Rows[i][3].ToString());
            Y = Convert.ToDouble(dt_Bg.Rows[i][4].ToString());
            Z = Convert.ToDouble(dt_Bg.Rows[i][5].ToString());

            ColorBackGroundXYZ = new CIEXYZ(X, Y, Z);
            
            R = Convert.ToByte(dt_RGBGammut.Rows[j][0].ToString());
            G = Convert.ToByte(dt_RGBGammut.Rows[j][1].ToString());
            B = Convert.ToByte(dt_RGBGammut.Rows[j][2].ToString());
            
            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
              rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
              rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
            }));

            //DisplayMeasuredValuesFromCs200();

            ColorDifference = ColorMeasured;

            //to measure the color mixture as well
            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
              //rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
              rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BgR, BgG, BgB));
            }));
            
            //DisplayMeasuredValuesFromCs200();
            //aclor is use tem for mixed color capture
            acolorRGB = PerceptionLib.Color.ToRBGFromLAB(ColorMeasured);
            newRow[132] = ColorMeasured.LA.ToString();
            newRow[133] = ColorMeasured.A.ToString();
            newRow[134] = ColorMeasured.B.ToString();
            newRow[135] = acolorRGB.R.ToString();
            newRow[136] = acolorRGB.G.ToString();
            newRow[137] = acolorRGB.B.ToString();
            newRow[138] = Util.CATCalulation.HueAngle(ColorMeasured).ToString();
            newRow[139] = Util.CATCalulation.hueDifference(ColorMeasured, ColorDifference).ToString();


            //direct model
            X = Convert.ToDouble(dt_RGBGammut.Rows[j][11].ToString());
            Y = Convert.ToDouble(dt_RGBGammut.Rows[j][12].ToString());
            Z = Convert.ToDouble(dt_RGBGammut.Rows[j][13].ToString());
            AcolorXYZ = new CIEXYZ(X, Y, Z);

            Acolor = PerceptionLib.Color.ToLUV(AcolorXYZ);
            
            //adding XYZ values
          
            MAcolorXYZ = Util.ColorSpaceConverter.SubtractXYZ(AcolorXYZ, ColorBackGroundXYZ);
            
            //converting back to various spaces fpr color difference calculation
           
            MAcolor = PerceptionLib.Color.ToLUV(MAcolorXYZ);
            
            acolorRGB = PerceptionLib.Color.ToRBGFromLAB(MAcolor);
            
            
            newRow[0] = R;
            newRow[1] = G;
            newRow[2] = B;
            newRow[3] = dt_RGBGammut.Rows[j][3].ToString();
            newRow[4] = dt_RGBGammut.Rows[j][4].ToString();
            newRow[5] = dt_RGBGammut.Rows[j][5].ToString();
            newRow[6] = dt_RGBGammut.Rows[j][6].ToString();
            newRow[7] = dt_RGBGammut.Rows[j][7].ToString();
            newRow[8] = dt_RGBGammut.Rows[j][8].ToString();
            newRow[9] = dt_RGBGammut.Rows[j][9].ToString();
            newRow[10] = dt_RGBGammut.Rows[j][10].ToString();
            newRow[11] = dt_RGBGammut.Rows[j][11].ToString();
            newRow[12] = dt_RGBGammut.Rows[j][12].ToString();
            newRow[13] = dt_RGBGammut.Rows[j][13].ToString();
            newRow[14] = dt_RGBGammut.Rows[j][14].ToString();
            newRow[15] = dt_RGBGammut.Rows[j][15].ToString();
            newRow[16] = dt_RGBGammut.Rows[j][16].ToString();
            newRow[17] = dt_RGBGammut.Rows[j][17].ToString();
            newRow[18] = dt_RGBGammut.Rows[j][18].ToString();
            newRow[19] = dt_RGBGammut.Rows[j][19].ToString();
            newRow[20] = dt_RGBGammut.Rows[j][20].ToString();
            newRow[21] = dt_RGBGammut.Rows[j][21].ToString();
            newRow[22] = dt_RGBGammut.Rows[j][22].ToString();
            newRow[23] = dt_RGBGammut.Rows[j][23].ToString();
            newRow[24] = dt_RGBGammut.Rows[j][24].ToString();
            newRow[25] = BgR;
            newRow[26] = BgG;
            newRow[27] = BgB;
            newRow[28] = dt_Bg.Rows[i][3].ToString();
            newRow[29] = dt_Bg.Rows[i][4].ToString();
            newRow[30] = dt_Bg.Rows[i][5].ToString();
            newRow[31] = dt_Bg.Rows[i][6].ToString();
            newRow[32] = dt_Bg.Rows[i][7].ToString();
            newRow[33] = dt_Bg.Rows[i][8].ToString();
            newRow[34] = MAcolorXYZ.X.ToString();
            newRow[35] = MAcolorXYZ.Y.ToString();
            newRow[36] = MAcolorXYZ.Z.ToString();
            newRow[37] = MAcolor.LA.ToString();
            newRow[38] = MAcolor.A.ToString();
            newRow[39] = MAcolor.B.ToString();
            GammutRangeCheck1 = 1;
           
            if ((MAcolor.LA >= minL) & (MAcolor.LA <= maxL) & (MAcolor.A >= minA) & (MAcolor.A <= maxA) & (MAcolor.B >= minB) & (MAcolor.B <= maxB))
            {
              for (int index = 0; index < bin.Rows.Count; index++)
              {
                double tempL, tempA, tempB;
                tempL = Convert.ToDouble(bin.Rows[index][3].ToString());
                tempA = Convert.ToDouble(bin.Rows[index][4].ToString());
                tempB = Convert.ToDouble(bin.Rows[index][5].ToString());

                if ((MAcolor.LA >= tempL - 5) & (MAcolor.LA <= tempL + 5) & (MAcolor.A >= tempA - 5) & (MAcolor.A <= tempA + 5) & (MAcolor.B >= tempB - 5) & (MAcolor.B <= tempB + 5))
                {
                  GammutRangeCheck1 = 0;
                  break;
                }
                GammutRangeCheck1 = 1;
              }
            }
            else
              GammutRangeCheck1 = 1;

            newRow[40] = acolorRGB.R.ToString();
            newRow[41] = acolorRGB.G.ToString();
            newRow[42] = acolorRGB.B.ToString();

            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
              rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BgR, BgG, BgB));
              rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(acolorRGB.R, acolorRGB.G, acolorRGB.B));
            }));

            DisplayMeasuredValuesFromCs200();

            acolorRGB = PerceptionLib.Color.ToRBG(MAcolorXYZ);

            newRow[43] = ColorMeasuredXYZ.X.ToString();
            newRow[44] = ColorMeasuredXYZ.Y.ToString();
            newRow[45] = ColorMeasuredXYZ.Z.ToString();
            newRow[46] = ColorMeasured.LA.ToString();
            newRow[47] = ColorMeasured.A.ToString();
            newRow[48] = ColorMeasured.B.ToString();
            newRow[49] = MR.ToString();
            newRow[50] = MG.ToString();
            newRow[51] = MB.ToString();

            ColorToShow.LA = Convert.ToDouble(dt_RGBGammut.Rows[j][19].ToString());
            ColorToShow.A = Convert.ToDouble(dt_RGBGammut.Rows[j][20].ToString());
            ColorToShow.B = Convert.ToDouble(dt_RGBGammut.Rows[j][21].ToString());


            // DeltaA_Luv = PerceptionLib.Color.ColorDistanceCalAB(MAcolor, ColorMeasured);
            DeltaA_Luv = PerceptionLib.Color.ColorDistanceCalAB(ColorToShow, ColorMeasured);
            CDeltaA_Luv = PerceptionLib.Color.ColorDistanceCalAB(ColorDifference, ColorMeasured);

            X = Convert.ToDouble(dt_Bg.Rows[i][6].ToString());
            Y = Convert.ToDouble(dt_Bg.Rows[i][7].ToString());
            Z = Convert.ToDouble(dt_Bg.Rows[i][8].ToString());

            ColorBackGroundXYZ = new CIEXYZ(X, Y, Z);

            MAcolorXYZ = Util.ColorSpaceConverter.SubtractXYZ(AcolorXYZ, ColorBackGroundXYZ);
            MAcolor = PerceptionLib.Color.ToLUV(MAcolorXYZ);
            acolorRGB = PerceptionLib.Color.ToRBGFromLAB(MAcolor);


             newRow[52]= MAcolorXYZ.X.ToString();
             newRow[53]= MAcolorXYZ.Y.ToString();
             newRow[54]= MAcolorXYZ.Z.ToString();
             newRow[55]= MAcolor.LA.ToString();
             newRow[56]= MAcolor.A.ToString();
             newRow[57]= MAcolor.B.ToString();
             newRow[58]= acolorRGB.R.ToString();
             newRow[59]= acolorRGB.G.ToString();
             newRow[60]= acolorRGB.B.ToString();
             GammutRangeCheck2 = 1;
             if ((MAcolor.LA >= minL) & (MAcolor.LA <= maxL) & (MAcolor.A >= minA) & (MAcolor.A <= maxA) & (MAcolor.B >= minB) & (MAcolor.B <= maxB))
             {
               for (int index = 0; index < bin.Rows.Count; index++)
               {
                 double tempL, tempA, tempB;
                 tempL = Convert.ToDouble(bin.Rows[index][3].ToString());
                 tempA = Convert.ToDouble(bin.Rows[index][4].ToString());
                 tempB = Convert.ToDouble(bin.Rows[index][5].ToString());

                 if ((MAcolor.LA >= tempL - 5) & (MAcolor.LA <= tempL + 5) & (MAcolor.A >= tempA - 5) & (MAcolor.A <= tempA + 5) & (MAcolor.B >= tempB - 5) & (MAcolor.B <= tempB + 5))
                 {
                   GammutRangeCheck2 = 0;
                   break;
                 }
                 GammutRangeCheck2 = 1;
               }
             }
             else
               GammutRangeCheck2 = 1;

            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
              rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(acolorRGB.R, acolorRGB.G, acolorRGB.B));
            }));

            DisplayMeasuredValuesFromCs200();

            MacolorRGB = PerceptionLib.Color.ToRBG(MAcolorXYZ);

             newRow[61]= ColorMeasuredXYZ.X.ToString();
             newRow[62]= ColorMeasuredXYZ.Y.ToString();
             newRow[63]= ColorMeasuredXYZ.Z.ToString();
             newRow[64]= ColorMeasured.LA.ToString();
             newRow[65]= ColorMeasured.A.ToString();
             newRow[66]= ColorMeasured.B.ToString();
             newRow[67]= MR.ToString();
             newRow[68]= MG.ToString();
             newRow[69]= MB.ToString();
             
            newRow[131] = Util.CATCalulation.HueAngle(MAcolor);
            // DeltaA_LAb = PerceptionLib.Color.ColorDistanceCalAB(MAcolor, ColorMeasured);
            DeltaA_LAb = PerceptionLib.Color.ColorDistanceCalAB(ColorToShow, ColorMeasured);
            CDeltaA_LAb = PerceptionLib.Color.ColorDistanceCalAB(ColorDifference, ColorMeasured);
            
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //bin model
            X = Convert.ToDouble(dt_RGBGammut.Rows[j][22].ToString());
            Y = Convert.ToDouble(dt_RGBGammut.Rows[j][23].ToString());
            Z = Convert.ToDouble(dt_RGBGammut.Rows[j][24].ToString());
            BINcolorXYZ = new CIEXYZ(X, Y, Z);
            BincolorRGB = new RGBValue();
            BINColor = PerceptionLib.Color.ToLUV(BINcolorXYZ);
            
            MBINcolorXYZ = Util.ColorSpaceConverter.SubtractXYZ(BINcolorXYZ, ColorBackGroundXYZ);

            MBINColor = PerceptionLib.Color.ToLUV(MBINcolorXYZ);
            GammutRangeCheck3 = 1;
            if ((MBINColor.LA >= minLLevel) & (MBINColor.LA <= maxLLevel) & (MBINColor.A >= minALevel) & (MBINColor.A <= maxALevel) & (MBINColor.B >= minBLevel) & (MBINColor.B <= maxBLevel))
            {
              for (int index = 0; index < bin.Rows.Count; index++)
              {
                double tempL, tempA, tempB;
                tempL = Convert.ToDouble(bin.Rows[index][9].ToString());
                tempA = Convert.ToDouble(bin.Rows[index][10].ToString());
                tempB = Convert.ToDouble(bin.Rows[index][11].ToString());

                if ((MBINColor.LA >= tempL - 5) & (MBINColor.LA <= tempL + 5) & (MBINColor.A >= tempA - 5) & (MBINColor.A <= tempA + 5) & (MBINColor.B >= tempB - 5) & (MBINColor.B <= tempB + 5))
                {
                  GammutRangeCheck3 = 0;
                  break;
                }
                GammutRangeCheck3 = 1;
              }
            }
            else
              GammutRangeCheck3 = 1;

            binnumber = Util.CATCalulation.MatchWithBinnedColors(MBINColor, bin, ColorBackGroundXYZ, BINColor);
            newRow[124] = Util.CATCalulation.closestColorInsideTheBin.ToString();
            newRow[125] = Util.CATCalulation.closestColorOnAddition.ToString();
            //int binnum = Util.CATCalulation.MatchWithBinnedColorsBasedOn3D(MBINColor, bin);

            //binnumber = Util.CATCalulation.MatchWithBinnedColorsBasedOn3D(MBINColor, bin, ColorBackGroundXYZ, BINColor);

            MBINcolorXYZ.X = Convert.ToDouble(bin.Rows[binnumber][6].ToString());
            MBINcolorXYZ.Y = Convert.ToDouble(bin.Rows[binnumber][7].ToString());
            MBINcolorXYZ.Z = Convert.ToDouble(bin.Rows[binnumber][8].ToString());
            MBINColor.LA =   Convert.ToDouble(bin.Rows[binnumber][3].ToString());
            MBINColor.A =    Convert.ToDouble(bin.Rows[binnumber][4].ToString());
            MBINColor.B =    Convert.ToDouble(bin.Rows[binnumber][5].ToString());
            

            BincolorRGB = PerceptionLib.Color.ToRBGFromLAB(MBINColor);

            newRow[70] = MBINcolorXYZ.X.ToString();
            newRow[71] = MBINcolorXYZ.Y.ToString();
            newRow[72] = MBINcolorXYZ.Z.ToString();
            newRow[73] = MBINColor.LA.ToString();
            newRow[74] = MBINColor.A.ToString();
            newRow[75] = MBINColor.B.ToString();
            newRow[76] = BincolorRGB.R.ToString();
            newRow[77] = BincolorRGB.G.ToString();
            newRow[78] = BincolorRGB.B.ToString();

            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
              rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BincolorRGB.R, BincolorRGB.G, BincolorRGB.B));
            }));

            DisplayMeasuredValuesFromCs200();
            BincolorRGB = PerceptionLib.Color.ToRBG(MBINcolorXYZ);

            newRow[79] = ColorMeasuredXYZ.X.ToString();
            newRow[80] = ColorMeasuredXYZ.Y.ToString();
            newRow[81] = ColorMeasuredXYZ.Z.ToString();
            newRow[82] = ColorMeasured.LA.ToString();
            newRow[83] = ColorMeasured.A.ToString();
            newRow[84] = ColorMeasured.B.ToString();
            newRow[85] = MR.ToString();
            newRow[86] = MG.ToString();
            newRow[87] = MB.ToString();

            //DeltaBin_Luv = PerceptionLib.Color.ColorDistanceCalAB(MBINColor, ColorMeasured);
            DeltaBin_Luv = PerceptionLib.Color.ColorDistanceCalAB(ColorToShow, ColorMeasured);
            CDeltaBin_Luv = PerceptionLib.Color.ColorDistanceCalAB(ColorDifference, ColorMeasured);

            X = Convert.ToDouble(dt_Bg.Rows[i][6].ToString());
            Y = Convert.ToDouble(dt_Bg.Rows[i][7].ToString());
            Z = Convert.ToDouble(dt_Bg.Rows[i][8].ToString());

            ColorBackGroundXYZ = new CIEXYZ(X, Y, Z);

            MBINcolorXYZ = Util.ColorSpaceConverter.SubtractXYZ(BinXYZ, ColorBackGroundXYZ);
            MBINColor = PerceptionLib.Color.ToLUV(MBINcolorXYZ);
            BincolorRGB = PerceptionLib.Color.ToRBGFromLAB(MBINColor);

            newRow[140] = MBINColor.LA.ToString(); 
            newRow[141] = MBINColor.A.ToString();
            newRow[142]=MBINColor.B.ToString();

            binnumber = Util.CATCalulation.MatchWithBinnedColors(MBINColor, bin, ColorBackGroundXYZ, BINColor);
            newRow[126] = Util.CATCalulation.closestColorInsideTheBin.ToString();
            newRow[127] = Util.CATCalulation.closestColorOnAddition.ToString();
            GammutRangeCheck4 = 1;

            newRow[143] = Util.CATCalulation.ClosestL.ToString();
            newRow[144] = Util.CATCalulation.ClosestA.ToString();
            newRow[145] = Util.CATCalulation.ClosestB.ToString();
            

            if ((MBINColor.LA >= minLLevel) & (MBINColor.LA <= maxLLevel) & (MBINColor.A >= minALevel) & (MBINColor.A <= maxALevel) & (MBINColor.B >= minBLevel) & (MBINColor.B <= maxBLevel))
            {
              for (int index = 0; index < bin.Rows.Count; index++)
              {
                double tempL, tempA, tempB;
                tempL = Convert.ToDouble(bin.Rows[index][9].ToString());
                tempA = Convert.ToDouble(bin.Rows[index][10].ToString());
                tempB = Convert.ToDouble(bin.Rows[index][11].ToString());

                if ((MBINColor.LA >= tempL - 5) & (MBINColor.LA <= tempL + 5) & (MBINColor.A >= tempA - 5) & (MBINColor.A <= tempA + 5) & (MBINColor.B >= tempB - 5) & (MBINColor.B <= tempB + 5))
                {
                  GammutRangeCheck4 = 0;
                  break;
                }
                GammutRangeCheck4 = 1;
              }
              
            }
            else
              GammutRangeCheck4 = 1;
            //
            //binnumber = Util.CATCalulation.MatchWithBinnedColorsBasedOn3D(MBINColor, bin, ColorBackGroundXYZ, BINColor);

            MBINcolorXYZ.X = Convert.ToDouble(bin.Rows[binnumber][6].ToString());
            MBINcolorXYZ.Y = Convert.ToDouble(bin.Rows[binnumber][7].ToString());
            MBINcolorXYZ.Z = Convert.ToDouble(bin.Rows[binnumber][8].ToString());
            MBINColor.LA = Convert.ToDouble(bin.Rows[binnumber][3].ToString());
            MBINColor.A = Convert.ToDouble(bin.Rows[binnumber][4].ToString());
            MBINColor.B = Convert.ToDouble(bin.Rows[binnumber][5].ToString());
            BincolorRGB = PerceptionLib.Color.ToRBGFromLAB(MBINColor);

            newRow[88] = MBINcolorXYZ.X.ToString();
            newRow[89] = MBINcolorXYZ.Y.ToString();
            newRow[90] = MBINcolorXYZ.Z.ToString();
            newRow[91] = MBINColor.LA.ToString();
            newRow[92] = MBINColor.A.ToString();
            newRow[93] = MBINColor.B.ToString();
            newRow[94] = BincolorRGB.R.ToString();
            newRow[95] = BincolorRGB.G.ToString();
            newRow[96] = BincolorRGB.B.ToString();

            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
              rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BincolorRGB.R, BincolorRGB.G, BincolorRGB.B));
            }));

            DisplayMeasuredValuesFromCs200();

            MBincolorRGB = PerceptionLib.Color.ToRBG(MBINcolorXYZ);

            newRow[97] = ColorMeasuredXYZ.X.ToString();
            newRow[98] = ColorMeasuredXYZ.Y.ToString();
            newRow[99] = ColorMeasuredXYZ.Z.ToString();
            newRow[100] = ColorMeasured.LA.ToString();
            newRow[101] = ColorMeasured.A.ToString();
            newRow[102] = ColorMeasured.B.ToString();
            newRow[103] = MR.ToString();
            newRow[104] = MG.ToString();
            newRow[105] = MB.ToString();
            newRow[160] = Util.CATCalulation.HueAngle(ColorMeasured);

            newRow[122] = binnumber.ToString();

            //DeltaBin_Lab = PerceptionLib.Color.ColorDistanceCalAB(MBINColor, ColorMeasured);
            DeltaBin_Lab = PerceptionLib.Color.ColorDistanceCalAB(ColorToShow, ColorMeasured);
            CDeltaBin_Lab = PerceptionLib.Color.ColorDistanceCalAB(ColorDifference, ColorMeasured);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //color with closest hue

            MBINcolorXYZ = Util.ColorSpaceConverter.SubtractXYZ(BinXYZ, ColorBackGroundXYZ);
            MBINColor = PerceptionLib.Color.ToLUV(MBINcolorXYZ);
            BincolorRGB = PerceptionLib.Color.ToRBGFromLAB(MBINColor);

            binnumber = Util.CATCalulation.MatchWithBinnedColorsWithHue(MBINColor, bin, ColorBackGroundXYZ, BINColor);
        
            if ((MBINColor.LA >= minLLevel) & (MBINColor.LA <= maxLLevel) & (MBINColor.A >= minALevel) & (MBINColor.A <= maxALevel) & (MBINColor.B >= minBLevel) & (MBINColor.B <= maxBLevel))
            {
              for (int index = 0; index < bin.Rows.Count; index++)
              {
                double tempL, tempA, tempB;
                tempL = Convert.ToDouble(bin.Rows[index][9].ToString());
                tempA = Convert.ToDouble(bin.Rows[index][10].ToString());
                tempB = Convert.ToDouble(bin.Rows[index][11].ToString());

                if ((MBINColor.LA >= tempL - 5) & (MBINColor.LA <= tempL + 5) & (MBINColor.A >= tempA - 5) & (MBINColor.A <= tempA + 5) & (MBINColor.B >= tempB - 5) & (MBINColor.B <= tempB + 5))
                {
                  GammutRangeCheck4 = 0;
                  break;
                }
                GammutRangeCheck4 = 1;
              }

            }
            else
              GammutRangeCheck4 = 1;
            //
            //binnumber = Util.CATCalulation.MatchWithBinnedColorsBasedOn3D(MBINColor, bin, ColorBackGroundXYZ, BINColor);

            MBINcolorXYZ.X = Convert.ToDouble(bin.Rows[binnumber][6].ToString());
            MBINcolorXYZ.Y = Convert.ToDouble(bin.Rows[binnumber][7].ToString());
            MBINcolorXYZ.Z = Convert.ToDouble(bin.Rows[binnumber][8].ToString());
            MBINColor.LA = Convert.ToDouble(bin.Rows[binnumber][3].ToString());
            MBINColor.A = Convert.ToDouble(bin.Rows[binnumber][4].ToString());
            MBINColor.B = Convert.ToDouble(bin.Rows[binnumber][5].ToString());
            BincolorRGB = PerceptionLib.Color.ToRBGFromLAB(MBINColor);

            newRow[146] = MBINColor.LA.ToString();
            newRow[147] = MBINColor.A.ToString();
            newRow[148] = MBINColor.B.ToString();
            newRow[149] = BincolorRGB.R.ToString();
            newRow[150] = BincolorRGB.G.ToString();
            newRow[151] = BincolorRGB.B.ToString();

            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
              rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BincolorRGB.R, BincolorRGB.G, BincolorRGB.B));
            }));

            DisplayMeasuredValuesFromCs200();

            MBincolorRGB = PerceptionLib.Color.ToRBG(MBINcolorXYZ);


            newRow[123] = binnumber.ToString();

            newRow[152] = ColorMeasured.LA.ToString();
            newRow[153] = ColorMeasured.A.ToString();
            newRow[154] = ColorMeasured.B.ToString();
            newRow[155] = MR.ToString();
            newRow[156] = MG.ToString();
            newRow[157] = MB.ToString();
            newRow[158] = Util.CATCalulation.HueAngle(ColorMeasured).ToString();


            //DeltaBin_Lab = PerceptionLib.Color.ColorDistanceCalAB(MBINColor, ColorMeasured);

            newRow[159] = PerceptionLib.Color.ColorDistanceCalAB(ColorDifference, ColorMeasured).ToString();
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            newRow[106] = DeltaA_Luv.ToString();
            newRow[107] = DeltaA_LAb.ToString();
            newRow[108] = DeltaBin_Luv.ToString();
            newRow[109] = DeltaBin_Lab.ToString();
            newRow[110] = GammutRangeCheck1.ToString();
            newRow[111] = GammutRangeCheck2.ToString();
            newRow[112] = GammutRangeCheck3.ToString();
            newRow[113] = GammutRangeCheck4.ToString();
            newRow[114] = CDeltaA_Luv.ToString();
            newRow[115] = CDeltaA_LAb.ToString();
            newRow[116] = CDeltaBin_Luv.ToString();
            newRow[117] = CDeltaBin_Lab.ToString();
            newRow[118] = ColorDifference.LA.ToString();
            newRow[119] = ColorDifference.A.ToString();
            newRow[120] = ColorDifference.B.ToString();
            //newRow[122] = binnumber.ToString();
            //newRow[123] = binnum.ToString();
            newRow[121] = (PerceptionLib.Color.ColorDistanceCalAB(ColorDifference, ColorToShow)).ToString();
            newRow[128] = Util.CATCalulation.HueAngeDifference.ToString();
            newRow[129] = Util.CATCalulation.HueAngle(ColorToShow).ToString();
            newRow[130] = Util.CATCalulation.HueAngle(ColorMeasured).ToString();
            dt_DataCollection.Rows.Add(newRow);
            
            R = 0; G = 0; B = 0;

            //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt_DataCollection.DefaultView));
            //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
          }


        }
        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt_DataCollection.DefaultView));
        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

        
      });

      btn_ExportGrid.IsEnabled = true;

    }
    
    private void Btn_CSBasicCompensation_Encd_Click(object sender, RoutedEventArgs e)
    {
      dtgrid_corrDisplay.IsEnabled = true;
      btn_StartMeasurment.IsEnabled = false;
      txt_R.IsEnabled = false;
      txt_G.IsEnabled = false;
      txt_B.IsEnabled = false;

      double X, Y, Z, DeltaA_Luv, DeltaA_LAb, DeltaBin_Luv, DeltaBin_Lab;
      double CDeltaA_Luv, CDeltaA_LAb, CDeltaBin_Luv, CDeltaBin_Lab;

      int binnumber = 0;
      int GammutRangeCheck1, GammutRangeCheck2, GammutRangeCheck3, GammutRangeCheck4;

      DataRow newRow;

      int connect = PerceptionLib.Cs200Connection.ConnectToCS200();

      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\BgValueWtPt.csv");
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\BgValueSet1.csv");
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\BgValue.csv");
      DataTable dt_Bg = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        dt_Bg = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));

      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\BaseBinFile.csv");
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\bigProjecor\BaseBinFile.csv");
      DataTable bin = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));

      double minLLevel = double.MaxValue;
      double maxLLevel = double.MinValue;
      double minALevel = double.MaxValue;
      double maxALevel = double.MinValue;
      double minBLevel = double.MaxValue;
      double maxBLevel = double.MinValue;

      double minL = double.MaxValue;
      double maxL = double.MinValue;
      double minA = double.MaxValue;
      double maxA = double.MinValue;
      double minB = double.MaxValue;
      double maxB = double.MinValue;

      foreach (DataRow dr in bin.Rows)
      {
        double accountLevel = dr.Field<double>("MLA");
        minLLevel = Math.Min(minLLevel, accountLevel);
        maxLLevel = Math.Max(maxLLevel, accountLevel);

        double ALevel = dr.Field<double>("MA");
        minALevel = Math.Min(minALevel, ALevel);
        maxALevel = Math.Max(maxALevel, ALevel);

        double BLevel = dr.Field<double>("MlaB");
        minBLevel = Math.Min(minBLevel, BLevel);
        maxBLevel = Math.Max(maxBLevel, BLevel);

        double account = dr.Field<double>("LA");
        minL = Math.Min(minL, account);
        maxL = Math.Max(maxL, account);

        double ALvl = dr.Field<double>("A");
        minA = Math.Min(minA, ALvl);
        maxA = Math.Max(maxA, ALvl);

        double BLvl = dr.Field<double>("LB");
        minB = Math.Min(minBLevel, BLvl);
        maxB = Math.Max(maxBLevel, BLvl);
      }

      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\RGBgammut.csv");
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\bigProjecor\RGB88.csv");
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\bigProjecor\RGB8000Sorted.csv");
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\bigProjecor\RGB8000NotSorted.csv");
      DataTable dt_RGBGammut = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        dt_RGBGammut = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
      }));

      BgNo = dt_Bg.Rows.Count;
      FgNo = dt_RGBGammut.Rows.Count;

      //BgNo = 12;
     // FgNo = 100;
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\CsBasicCOMPENSATOR.csv");

      DataTable dt_DataCollection = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        dt_DataCollection = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
      }));

      ThreadPool.QueueUserWorkItem(ignored =>
      {
        PerceptionLib.RGBValue rgb = new PerceptionLib.RGBValue();
        for (int i = 12; i < BgNo; i++)
        {

          for (int j = 11; j < FgNo; j = j + 20)
          {
            newRow = dt_DataCollection.NewRow();
            BgR = Convert.ToByte(dt_Bg.Rows[i][0].ToString());
            BgG = Convert.ToByte(dt_Bg.Rows[i][1].ToString());
            BgB = Convert.ToByte(dt_Bg.Rows[i][2].ToString());

            X = Convert.ToDouble(dt_Bg.Rows[i][3].ToString());
            Y = Convert.ToDouble(dt_Bg.Rows[i][4].ToString());
            Z = Convert.ToDouble(dt_Bg.Rows[i][5].ToString());

            ColorBackGroundXYZ = new CIEXYZ(X, Y, Z);

            R = Convert.ToByte(dt_RGBGammut.Rows[j][0].ToString());
            G = Convert.ToByte(dt_RGBGammut.Rows[j][1].ToString());
            B = Convert.ToByte(dt_RGBGammut.Rows[j][2].ToString());

            //Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            //{
            //  rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
            //  rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
            //}));

            //DisplayMeasuredValuesFromCs200();

            //ColorDifference = ColorMeasured;

            ////to measure the color mixture as well
            //Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            //{
            //  //rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
            //  rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BgR, BgG, BgB));
            //}));

            //DisplayMeasuredValuesFromCs200();
            ////aclor is use tem for mixed color capture
            //acolorRGB = PerceptionLib.Color.ToRBGFromLAB(ColorMeasured);
            //newRow[132] = ColorMeasured.LA.ToString();
            //newRow[133] = ColorMeasured.A.ToString();
            //newRow[134] = ColorMeasured.B.ToString();
            //newRow[135] = acolorRGB.R.ToString();
            //newRow[136] = acolorRGB.G.ToString();
            //newRow[137] = acolorRGB.B.ToString();
            //newRow[138] = Util.CATCalulation.HueAngle(ColorMeasured).ToString();
            //newRow[139] = Util.CATCalulation.hueDifference(ColorMeasured, ColorDifference).ToString();


            //direct model
            X = Convert.ToDouble(dt_RGBGammut.Rows[j][11].ToString());
            Y = Convert.ToDouble(dt_RGBGammut.Rows[j][12].ToString());
            Z = Convert.ToDouble(dt_RGBGammut.Rows[j][13].ToString());
            AcolorXYZ = new CIEXYZ(X, Y, Z);

            Acolor = PerceptionLib.Color.ToLUV(AcolorXYZ);

            //adding XYZ values

            MAcolorXYZ = Util.ColorSpaceConverter.SubtractXYZ(AcolorXYZ, ColorBackGroundXYZ);

            //converting back to various spaces fpr color difference calculation

            MAcolor = PerceptionLib.Color.ToLUV(MAcolorXYZ);

            acolorRGB = PerceptionLib.Color.ToRBGFromLAB(MAcolor);


            newRow[0] = R;
            newRow[1] = G;
            newRow[2] = B;
            newRow[3] = dt_RGBGammut.Rows[j][3].ToString();
            newRow[4] = dt_RGBGammut.Rows[j][4].ToString();
            newRow[5] = dt_RGBGammut.Rows[j][5].ToString();
            newRow[6] = dt_RGBGammut.Rows[j][6].ToString();
            newRow[7] = dt_RGBGammut.Rows[j][7].ToString();
            newRow[8] = dt_RGBGammut.Rows[j][8].ToString();
            newRow[9] = dt_RGBGammut.Rows[j][9].ToString();
            newRow[10] = dt_RGBGammut.Rows[j][10].ToString();
            newRow[11] = dt_RGBGammut.Rows[j][11].ToString();
            newRow[12] = dt_RGBGammut.Rows[j][12].ToString();
            newRow[13] = dt_RGBGammut.Rows[j][13].ToString();
            newRow[14] = dt_RGBGammut.Rows[j][14].ToString();
            newRow[15] = dt_RGBGammut.Rows[j][15].ToString();
            newRow[16] = dt_RGBGammut.Rows[j][16].ToString();
            newRow[17] = dt_RGBGammut.Rows[j][17].ToString();
            newRow[18] = dt_RGBGammut.Rows[j][18].ToString();
            newRow[19] = dt_RGBGammut.Rows[j][19].ToString();
            newRow[20] = dt_RGBGammut.Rows[j][20].ToString();
            newRow[21] = dt_RGBGammut.Rows[j][21].ToString();
            newRow[22] = dt_RGBGammut.Rows[j][22].ToString();
            newRow[23] = dt_RGBGammut.Rows[j][23].ToString();
            newRow[24] = dt_RGBGammut.Rows[j][24].ToString();
            newRow[25] = BgR;
            newRow[26] = BgG;
            newRow[27] = BgB;
            newRow[28] = dt_Bg.Rows[i][3].ToString();
            newRow[29] = dt_Bg.Rows[i][4].ToString();
            newRow[30] = dt_Bg.Rows[i][5].ToString();
            newRow[31] = dt_Bg.Rows[i][6].ToString();
            newRow[32] = dt_Bg.Rows[i][7].ToString();
            newRow[33] = dt_Bg.Rows[i][8].ToString();
            newRow[34] = MAcolorXYZ.X.ToString();
            newRow[35] = MAcolorXYZ.Y.ToString();
            newRow[36] = MAcolorXYZ.Z.ToString();
            newRow[37] = MAcolor.LA.ToString();
            newRow[38] = MAcolor.A.ToString();
            newRow[39] = MAcolor.B.ToString();
            GammutRangeCheck1 = 1;

            if ((MAcolor.LA >= minL) & (MAcolor.LA <= maxL) & (MAcolor.A >= minA) & (MAcolor.A <= maxA) & (MAcolor.B >= minB) & (MAcolor.B <= maxB))
            {
              for (int index = 0; index < bin.Rows.Count; index++)
              {
                double tempL, tempA, tempB;
                tempL = Convert.ToDouble(bin.Rows[index][3].ToString());
                tempA = Convert.ToDouble(bin.Rows[index][4].ToString());
                tempB = Convert.ToDouble(bin.Rows[index][5].ToString());

                if ((MAcolor.LA >= tempL - 5) & (MAcolor.LA <= tempL + 5) & (MAcolor.A >= tempA - 5) & (MAcolor.A <= tempA + 5) & (MAcolor.B >= tempB - 5) & (MAcolor.B <= tempB + 5))
                {
                  GammutRangeCheck1 = 0;
                  break;
                }
                GammutRangeCheck1 = 1;
              }
            }
            else
              GammutRangeCheck1 = 1;

            newRow[40] = acolorRGB.R.ToString();
            newRow[41] = acolorRGB.G.ToString();
            newRow[42] = acolorRGB.B.ToString();

            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
              rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BgR, BgG, BgB));
              rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(acolorRGB.R, acolorRGB.G, acolorRGB.B));
            }));

            DisplayMeasuredValuesFromCs200();

            acolorRGB = PerceptionLib.Color.ToRBG(MAcolorXYZ);

            newRow[43] = ColorMeasuredXYZ.X.ToString();
            newRow[44] = ColorMeasuredXYZ.Y.ToString();
            newRow[45] = ColorMeasuredXYZ.Z.ToString();
            newRow[46] = ColorMeasured.LA.ToString();
            newRow[47] = ColorMeasured.A.ToString();
            newRow[48] = ColorMeasured.B.ToString();
            newRow[49] = MR.ToString();
            newRow[50] = MG.ToString();
            newRow[51] = MB.ToString();

            ColorToShow.LA = Convert.ToDouble(dt_RGBGammut.Rows[j][19].ToString());
            ColorToShow.A = Convert.ToDouble(dt_RGBGammut.Rows[j][20].ToString());
            ColorToShow.B = Convert.ToDouble(dt_RGBGammut.Rows[j][21].ToString());


            // DeltaA_Luv = PerceptionLib.Color.ColorDistanceCalAB(MAcolor, ColorMeasured);
            DeltaA_Luv = PerceptionLib.Color.ColorDistanceCalAB(ColorToShow, ColorMeasured);
            //CDeltaA_Luv = PerceptionLib.Color.ColorDistanceCalAB(ColorDifference, ColorMeasured);

            //X = Convert.ToDouble(dt_Bg.Rows[i][6].ToString());
            //Y = Convert.ToDouble(dt_Bg.Rows[i][7].ToString());
            //Z = Convert.ToDouble(dt_Bg.Rows[i][8].ToString());

            //ColorBackGroundXYZ = new CIEXYZ(X, Y, Z);

            //MAcolorXYZ = Util.ColorSpaceConverter.SubtractXYZ(AcolorXYZ, ColorBackGroundXYZ);
            //MAcolor = PerceptionLib.Color.ToLUV(MAcolorXYZ);
            //acolorRGB = PerceptionLib.Color.ToRBGFromLAB(MAcolor);


            //newRow[52] = MAcolorXYZ.X.ToString();
            //newRow[53] = MAcolorXYZ.Y.ToString();
            //newRow[54] = MAcolorXYZ.Z.ToString();
            //newRow[55] = MAcolor.LA.ToString();
            //newRow[56] = MAcolor.A.ToString();
            //newRow[57] = MAcolor.B.ToString();
            //newRow[58] = acolorRGB.R.ToString();
            //newRow[59] = acolorRGB.G.ToString();
            //newRow[60] = acolorRGB.B.ToString();
            //GammutRangeCheck2 = 1;
            //if ((MAcolor.LA >= minL) & (MAcolor.LA <= maxL) & (MAcolor.A >= minA) & (MAcolor.A <= maxA) & (MAcolor.B >= minB) & (MAcolor.B <= maxB))
            //{
            //  for (int index = 0; index < bin.Rows.Count; index++)
            //  {
            //    double tempL, tempA, tempB;
            //    tempL = Convert.ToDouble(bin.Rows[index][3].ToString());
            //    tempA = Convert.ToDouble(bin.Rows[index][4].ToString());
            //    tempB = Convert.ToDouble(bin.Rows[index][5].ToString());

            //    if ((MAcolor.LA >= tempL - 5) & (MAcolor.LA <= tempL + 5) & (MAcolor.A >= tempA - 5) & (MAcolor.A <= tempA + 5) & (MAcolor.B >= tempB - 5) & (MAcolor.B <= tempB + 5))
            //    {
            //      GammutRangeCheck2 = 0;
            //      break;
            //    }
            //    GammutRangeCheck2 = 1;
            //  }
            //}
            //else
            //  GammutRangeCheck2 = 1;

            //Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            //{
            //  rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(acolorRGB.R, acolorRGB.G, acolorRGB.B));
            //}));

            //DisplayMeasuredValuesFromCs200();

            //MacolorRGB = PerceptionLib.Color.ToRBG(MAcolorXYZ);

            //newRow[61] = ColorMeasuredXYZ.X.ToString();
            //newRow[62] = ColorMeasuredXYZ.Y.ToString();
            //newRow[63] = ColorMeasuredXYZ.Z.ToString();
            //newRow[64] = ColorMeasured.LA.ToString();
            //newRow[65] = ColorMeasured.A.ToString();
            //newRow[66] = ColorMeasured.B.ToString();
            //newRow[67] = MR.ToString();
            //newRow[68] = MG.ToString();
            //newRow[69] = MB.ToString();

            //newRow[131] = Util.CATCalulation.HueAngle(MAcolor);
            //// DeltaA_LAb = PerceptionLib.Color.ColorDistanceCalAB(MAcolor, ColorMeasured);
            //DeltaA_LAb = PerceptionLib.Color.ColorDistanceCalAB(ColorToShow, ColorMeasured);
            //CDeltaA_LAb = PerceptionLib.Color.ColorDistanceCalAB(ColorDifference, ColorMeasured);

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //bin model
            X = Convert.ToDouble(dt_RGBGammut.Rows[j][22].ToString());
            Y = Convert.ToDouble(dt_RGBGammut.Rows[j][23].ToString());
            Z = Convert.ToDouble(dt_RGBGammut.Rows[j][24].ToString());
            BINcolorXYZ = new CIEXYZ(X, Y, Z);
            BincolorRGB = new RGBValue();
            BINColor = PerceptionLib.Color.ToLUV(BINcolorXYZ);

            //MBINcolorXYZ = Util.ColorSpaceConverter.SubtractXYZ(BINcolorXYZ, ColorBackGroundXYZ);

            //MBINColor = PerceptionLib.Color.ToLUV(MBINcolorXYZ);
            //GammutRangeCheck3 = 1;
            //if ((MBINColor.LA >= minLLevel) & (MBINColor.LA <= maxLLevel) & (MBINColor.A >= minALevel) & (MBINColor.A <= maxALevel) & (MBINColor.B >= minBLevel) & (MBINColor.B <= maxBLevel))
            //{
            //  for (int index = 0; index < bin.Rows.Count; index++)
            //  {
            //    double tempL, tempA, tempB;
            //    tempL = Convert.ToDouble(bin.Rows[index][9].ToString());
            //    tempA = Convert.ToDouble(bin.Rows[index][10].ToString());
            //    tempB = Convert.ToDouble(bin.Rows[index][11].ToString());

            //    if ((MBINColor.LA >= tempL - 5) & (MBINColor.LA <= tempL + 5) & (MBINColor.A >= tempA - 5) & (MBINColor.A <= tempA + 5) & (MBINColor.B >= tempB - 5) & (MBINColor.B <= tempB + 5))
            //    {
            //      GammutRangeCheck3 = 0;
            //      break;
            //    }
            //    GammutRangeCheck3 = 1;
            //  }
            //}
            //else
            //  GammutRangeCheck3 = 1;

            //binnumber = Util.CATCalulation.MatchWithBinnedColors(MBINColor, bin, ColorBackGroundXYZ, BINColor);
            //newRow[124] = Util.CATCalulation.closestColorInsideTheBin.ToString();
            //newRow[125] = Util.CATCalulation.closestColorOnAddition.ToString();
            ////int binnum = Util.CATCalulation.MatchWithBinnedColorsBasedOn3D(MBINColor, bin);

            ////binnumber = Util.CATCalulation.MatchWithBinnedColorsBasedOn3D(MBINColor, bin, ColorBackGroundXYZ, BINColor);

            //MBINcolorXYZ.X = Convert.ToDouble(bin.Rows[binnumber][6].ToString());
            //MBINcolorXYZ.Y = Convert.ToDouble(bin.Rows[binnumber][7].ToString());
            //MBINcolorXYZ.Z = Convert.ToDouble(bin.Rows[binnumber][8].ToString());
            //MBINColor.LA = Convert.ToDouble(bin.Rows[binnumber][3].ToString());
            //MBINColor.A = Convert.ToDouble(bin.Rows[binnumber][4].ToString());
            //MBINColor.B = Convert.ToDouble(bin.Rows[binnumber][5].ToString());
            //BincolorRGB.R = Convert.ToByte(bin.Rows[binnumber][0].ToString());
            //BincolorRGB.G = Convert.ToByte(bin.Rows[binnumber][1].ToString());
            //BincolorRGB.B = Convert.ToByte(bin.Rows[binnumber][2].ToString());

            //newRow[70] = MBINcolorXYZ.X.ToString();
            //newRow[71] = MBINcolorXYZ.Y.ToString();
            //newRow[72] = MBINcolorXYZ.Z.ToString();
            //newRow[73] = MBINColor.LA.ToString();
            //newRow[74] = MBINColor.A.ToString();
            //newRow[75] = MBINColor.B.ToString();
            //newRow[76] = BincolorRGB.R.ToString();
            //newRow[77] = BincolorRGB.G.ToString();
            //newRow[78] = BincolorRGB.B.ToString();

            //Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            //{
            //  rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BincolorRGB.R, BincolorRGB.G, BincolorRGB.B));
            //}));

            //DisplayMeasuredValuesFromCs200();
            //BincolorRGB = PerceptionLib.Color.ToRBG(MBINcolorXYZ);

            //newRow[79] = ColorMeasuredXYZ.X.ToString();
            //newRow[80] = ColorMeasuredXYZ.Y.ToString();
            //newRow[81] = ColorMeasuredXYZ.Z.ToString();
            //newRow[82] = ColorMeasured.LA.ToString();
            //newRow[83] = ColorMeasured.A.ToString();
            //newRow[84] = ColorMeasured.B.ToString();
            //newRow[85] = MR.ToString();
            //newRow[86] = MG.ToString();
            //newRow[87] = MB.ToString();

            ////DeltaBin_Luv = PerceptionLib.Color.ColorDistanceCalAB(MBINColor, ColorMeasured);
            //DeltaBin_Luv = PerceptionLib.Color.ColorDistanceCalAB(ColorToShow, ColorMeasured);
            //CDeltaBin_Luv = PerceptionLib.Color.ColorDistanceCalAB(ColorDifference, ColorMeasured);

            X = Convert.ToDouble(dt_Bg.Rows[i][6].ToString());
            Y = Convert.ToDouble(dt_Bg.Rows[i][7].ToString());
            Z = Convert.ToDouble(dt_Bg.Rows[i][8].ToString());

            ColorBackGroundXYZ = new CIEXYZ(X, Y, Z);

            MBINcolorXYZ = Util.ColorSpaceConverter.SubtractXYZ(BINcolorXYZ, ColorBackGroundXYZ);
            MBINColor = PerceptionLib.Color.ToLUV(MBINcolorXYZ);
            BincolorRGB = PerceptionLib.Color.ToRBGFromLAB(MBINColor);

            newRow[140] = MBINColor.LA.ToString();
            newRow[141] = MBINColor.A.ToString();
            newRow[142] = MBINColor.B.ToString();

            binnumber = Util.CATCalulation.MatchWithBinnedColors(MBINColor, bin, ColorBackGroundXYZ, BINColor);
            newRow[126] = Util.CATCalulation.closestColorInsideTheBin.ToString();
            newRow[127] = Util.CATCalulation.closestColorOnAddition.ToString();
            GammutRangeCheck4 = 1;

            newRow[143] = Util.CATCalulation.ClosestL.ToString();
            newRow[144] = Util.CATCalulation.ClosestA.ToString();
            newRow[145] = Util.CATCalulation.ClosestB.ToString();


            if ((MBINColor.LA >= minLLevel) & (MBINColor.LA <= maxLLevel) & (MBINColor.A >= minALevel) & (MBINColor.A <= maxALevel) & (MBINColor.B >= minBLevel) & (MBINColor.B <= maxBLevel))
            {
              for (int index = 0; index < bin.Rows.Count; index++)
              {
                double tempL, tempA, tempB;
                tempL = Convert.ToDouble(bin.Rows[index][9].ToString());
                tempA = Convert.ToDouble(bin.Rows[index][10].ToString());
                tempB = Convert.ToDouble(bin.Rows[index][11].ToString());

                if ((MBINColor.LA >= tempL - 5) & (MBINColor.LA <= tempL + 5) & (MBINColor.A >= tempA - 5) & (MBINColor.A <= tempA + 5) & (MBINColor.B >= tempB - 5) & (MBINColor.B <= tempB + 5))
                {
                  GammutRangeCheck4 = 0;
                  break;
                }
                GammutRangeCheck4 = 1;
              }

            }
            else
              GammutRangeCheck4 = 1;
            //
            //binnumber = Util.CATCalulation.MatchWithBinnedColorsBasedOn3D(MBINColor, bin, ColorBackGroundXYZ, BINColor);

            MBINcolorXYZ.X = Convert.ToDouble(bin.Rows[binnumber][6].ToString());
            MBINcolorXYZ.Y = Convert.ToDouble(bin.Rows[binnumber][7].ToString());
            MBINcolorXYZ.Z = Convert.ToDouble(bin.Rows[binnumber][8].ToString());
            MBINColor.LA = Convert.ToDouble(bin.Rows[binnumber][3].ToString());
            MBINColor.A = Convert.ToDouble(bin.Rows[binnumber][4].ToString());
            MBINColor.B = Convert.ToDouble(bin.Rows[binnumber][5].ToString());
            BincolorRGB.R = Convert.ToByte(bin.Rows[binnumber][0].ToString());
            BincolorRGB.G = Convert.ToByte(bin.Rows[binnumber][1].ToString());
            BincolorRGB.B = Convert.ToByte(bin.Rows[binnumber][2].ToString());

            newRow[88] = MBINcolorXYZ.X.ToString();
            newRow[89] = MBINcolorXYZ.Y.ToString();
            newRow[90] = MBINcolorXYZ.Z.ToString();
            newRow[91] = MBINColor.LA.ToString();
            newRow[92] = MBINColor.A.ToString();
            newRow[93] = MBINColor.B.ToString();
            newRow[94] = BincolorRGB.R.ToString();
            newRow[95] = BincolorRGB.G.ToString();
            newRow[96] = BincolorRGB.B.ToString();

            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
              rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BincolorRGB.R, BincolorRGB.G, BincolorRGB.B));
            }));

            DisplayMeasuredValuesFromCs200();

            MBincolorRGB = PerceptionLib.Color.ToRBG(MBINcolorXYZ);

            newRow[97] = ColorMeasuredXYZ.X.ToString();
            newRow[98] = ColorMeasuredXYZ.Y.ToString();
            newRow[99] = ColorMeasuredXYZ.Z.ToString();
            newRow[100] = ColorMeasured.LA.ToString();
            newRow[101] = ColorMeasured.A.ToString();
            newRow[102] = ColorMeasured.B.ToString();
            newRow[103] = MR.ToString();
            newRow[104] = MG.ToString();
            newRow[105] = MB.ToString();
            newRow[160] = Util.CATCalulation.HueAngle(ColorMeasured);

            newRow[122] = binnumber.ToString();

            //DeltaBin_Lab = PerceptionLib.Color.ColorDistanceCalAB(MBINColor, ColorMeasured);
            DeltaBin_Lab = PerceptionLib.Color.ColorDistanceCalAB(ColorToShow, ColorMeasured);
            //CDeltaBin_Lab = PerceptionLib.Color.ColorDistanceCalAB(ColorDifference, ColorMeasured);

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////color with closest hue

            //MBINcolorXYZ = Util.ColorSpaceConverter.SubtractXYZ(BinXYZ, ColorBackGroundXYZ);
            //MBINColor = PerceptionLib.Color.ToLUV(MBINcolorXYZ);
            //BincolorRGB = PerceptionLib.Color.ToRBGFromLAB(MBINColor);

            //binnumber = Util.CATCalulation.MatchWithBinnedColorsWithHue(MBINColor, bin, ColorBackGroundXYZ, BINColor);

            //if ((MBINColor.LA >= minLLevel) & (MBINColor.LA <= maxLLevel) & (MBINColor.A >= minALevel) & (MBINColor.A <= maxALevel) & (MBINColor.B >= minBLevel) & (MBINColor.B <= maxBLevel))
            //{
            //  for (int index = 0; index < bin.Rows.Count; index++)
            //  {
            //    double tempL, tempA, tempB;
            //    tempL = Convert.ToDouble(bin.Rows[index][9].ToString());
            //    tempA = Convert.ToDouble(bin.Rows[index][10].ToString());
            //    tempB = Convert.ToDouble(bin.Rows[index][11].ToString());

            //    if ((MBINColor.LA >= tempL - 5) & (MBINColor.LA <= tempL + 5) & (MBINColor.A >= tempA - 5) & (MBINColor.A <= tempA + 5) & (MBINColor.B >= tempB - 5) & (MBINColor.B <= tempB + 5))
            //    {
            //      GammutRangeCheck4 = 0;
            //      break;
            //    }
            //    GammutRangeCheck4 = 1;
            //  }

            //}
            //else
            //  GammutRangeCheck4 = 1;
            ////
            ////binnumber = Util.CATCalulation.MatchWithBinnedColorsBasedOn3D(MBINColor, bin, ColorBackGroundXYZ, BINColor);

            //MBINcolorXYZ.X = Convert.ToDouble(bin.Rows[binnumber][6].ToString());
            //MBINcolorXYZ.Y = Convert.ToDouble(bin.Rows[binnumber][7].ToString());
            //MBINcolorXYZ.Z = Convert.ToDouble(bin.Rows[binnumber][8].ToString());
            //MBINColor.LA = Convert.ToDouble(bin.Rows[binnumber][3].ToString());
            //MBINColor.A = Convert.ToDouble(bin.Rows[binnumber][4].ToString());
            //MBINColor.B = Convert.ToDouble(bin.Rows[binnumber][5].ToString());
            //BincolorRGB.R = Convert.ToByte(bin.Rows[binnumber][0].ToString());
            //BincolorRGB.G = Convert.ToByte(bin.Rows[binnumber][1].ToString());
            //BincolorRGB.B = Convert.ToByte(bin.Rows[binnumber][2].ToString());

            //newRow[146] = MBINColor.LA.ToString();
            //newRow[147] = MBINColor.A.ToString();
            //newRow[148] = MBINColor.B.ToString();
            //newRow[149] = BincolorRGB.R.ToString();
            //newRow[150] = BincolorRGB.G.ToString();
            //newRow[151] = BincolorRGB.B.ToString();

            //Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            //{
            //  rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BincolorRGB.R, BincolorRGB.G, BincolorRGB.B));
            //}));

            //DisplayMeasuredValuesFromCs200();

            //MBincolorRGB = PerceptionLib.Color.ToRBG(MBINcolorXYZ);


            //newRow[123] = binnumber.ToString();

            //newRow[152] = ColorMeasured.LA.ToString();
            //newRow[153] = ColorMeasured.A.ToString();
            //newRow[154] = ColorMeasured.B.ToString();
            //newRow[155] = MR.ToString();
            //newRow[156] = MG.ToString();
            //newRow[157] = MB.ToString();
            //newRow[158] = Util.CATCalulation.HueAngle(ColorMeasured).ToString();


            ////DeltaBin_Lab = PerceptionLib.Color.ColorDistanceCalAB(MBINColor, ColorMeasured);

            //newRow[159] = PerceptionLib.Color.ColorDistanceCalAB(ColorDifference, ColorMeasured).ToString();
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            newRow[106] = DeltaA_Luv.ToString();
            //newRow[107] = DeltaA_LAb.ToString();
            //newRow[108] = DeltaBin_Luv.ToString();
            newRow[109] = DeltaBin_Lab.ToString();
            newRow[110] = GammutRangeCheck1.ToString();
            //newRow[111] = GammutRangeCheck2.ToString();
            //newRow[112] = GammutRangeCheck3.ToString();
            newRow[113] = GammutRangeCheck4.ToString();
            //newRow[114] = CDeltaA_Luv.ToString();
            //newRow[115] = CDeltaA_LAb.ToString();
            //newRow[116] = CDeltaBin_Luv.ToString();
            //newRow[117] = CDeltaBin_Lab.ToString();
            //newRow[118] = ColorDifference.LA.ToString();
            //newRow[119] = ColorDifference.A.ToString();
            //newRow[120] = ColorDifference.B.ToString();
            ////newRow[122] = binnumber.ToString();
            ////newRow[123] = binnum.ToString();
            //newRow[121] = (PerceptionLib.Color.ColorDistanceCalAB(ColorDifference, ColorToShow)).ToString();
            newRow[128] = Util.CATCalulation.HueAngeDifference.ToString();
            newRow[129] = Util.CATCalulation.HueAngle(ColorToShow).ToString();
            newRow[130] = Util.CATCalulation.HueAngle(ColorMeasured).ToString();
            dt_DataCollection.Rows.Add(newRow);

            R = 0; G = 0; B = 0;


            //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt_DataCollection.DefaultView));
            //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
          }


        }
        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt_DataCollection.DefaultView));
        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));


      });

      btn_ExportGrid.IsEnabled = true;

    }


    private void Btn_Recheck_Click(object sender, RoutedEventArgs e)
    {
      dtgrid_corrDisplay.IsEnabled = true;
      btn_StartMeasurment.IsEnabled = false;
      txt_R.IsEnabled = false;
      txt_G.IsEnabled = false;
      txt_B.IsEnabled = false;

      double X, Y, Z;

      int binnumber = 0;
      int GammutRangeCheck1, GammutRangeCheck4;



      CIEXYZ BINcolorXYZ = new CIEXYZ(0, 0, 0);
      CIEXYZ MBINcolorXYZ = new CIEXYZ(0, 0, 0);

      CIEXYZ NoBINcolorXYZ = new CIEXYZ(0, 0, 0);
      CIEXYZ NoMBINcolorXYZ = new CIEXYZ(0, 0, 0);
      int connect = PerceptionLib.Cs200Connection.ConnectToCS200();


      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\bigProjecor\BaseBinFile.csv");
      DataTable bin = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));

      double minLLevel = double.MaxValue;
      double maxLLevel = double.MinValue;
      double minALevel = double.MaxValue;
      double maxALevel = double.MinValue;
      double minBLevel = double.MaxValue;
      double maxBLevel = double.MinValue;

      double minL = double.MaxValue;
      double maxL = double.MinValue;
      double minA = double.MaxValue;
      double maxA = double.MinValue;
      double minB = double.MaxValue;
      double maxB = double.MinValue;

      foreach (DataRow dr in bin.Rows)
      {
        double accountLevel = dr.Field<double>("MLA");
        minLLevel = Math.Min(minLLevel, accountLevel);
        maxLLevel = Math.Max(maxLLevel, accountLevel);

        double ALevel = dr.Field<double>("MA");
        minALevel = Math.Min(minALevel, ALevel);
        maxALevel = Math.Max(maxALevel, ALevel);

        double BLevel = dr.Field<double>("MlaB");
        minBLevel = Math.Min(minBLevel, BLevel);
        maxBLevel = Math.Max(maxBLevel, BLevel);

        double account = dr.Field<double>("LA");
        minL = Math.Min(minL, account);
        maxL = Math.Max(maxL, account);

        double ALvl = dr.Field<double>("A");
        minA = Math.Min(minA, ALvl);
        maxA = Math.Max(maxA, ALvl);

        double BLvl = dr.Field<double>("LB");
        minB = Math.Min(minBLevel, BLvl);
        maxB = Math.Max(maxBLevel, BLvl);
      }

      
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\big_pro_crosscheck.csv");
      DataTable dt_Bg = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        dt_Bg = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));
      ThreadPool.QueueUserWorkItem(ignored =>
    {
      PerceptionLib.RGBValue rgb = new PerceptionLib.RGBValue();
      for (int i = 0; i < dt_Bg.Rows.Count; i++)
      {
        if (stop == 1)
        {
          stop = 0;
          break;
        }

        BgR = Convert.ToByte(dt_Bg.Rows[i][12].ToString());
        BgG = Convert.ToByte(dt_Bg.Rows[i][13].ToString());
        BgB = Convert.ToByte(dt_Bg.Rows[i][14].ToString());

        X = Convert.ToDouble(dt_Bg.Rows[i][15].ToString());
        Y = Convert.ToDouble(dt_Bg.Rows[i][16].ToString());
        Z = Convert.ToDouble(dt_Bg.Rows[i][17].ToString());

        ColorBackGroundXYZ = new CIEXYZ(X, Y, Z);
        
        X = Convert.ToDouble(dt_Bg.Rows[i][9].ToString());
        Y = Convert.ToDouble(dt_Bg.Rows[i][10].ToString());
        Z = Convert.ToDouble(dt_Bg.Rows[i][11].ToString());
        BINcolorXYZ = new CIEXYZ(X, Y, Z);
        BINColor = PerceptionLib.Color.ToLAB(MBINcolorXYZ);
       
        MBINcolorXYZ = Util.ColorSpaceConverter.SubtractXYZ(BINcolorXYZ, ColorBackGroundXYZ);
        MBINColor = PerceptionLib.Color.ToLAB(MBINcolorXYZ);
       
        dt_Bg.Rows[i][27] = MBINColor.LA.ToString();
        dt_Bg.Rows[i][28] = MBINColor.A.ToString();
        dt_Bg.Rows[i][29] = MBINColor.B.ToString();

        ColorToShow.LA = Convert.ToDouble(dt_Bg.Rows[i][6].ToString());
        ColorToShow.A = Convert.ToDouble(dt_Bg.Rows[i][7].ToString());
        ColorToShow.B = Convert.ToDouble(dt_Bg.Rows[i][8].ToString());


        binnumber = Util.CATCalulation.MatchWithBinned(MBINColor, bin, ColorBackGroundXYZ, BINColor);
       
        GammutRangeCheck4 = 1;
              


        if ((MBINColor.LA >= minLLevel) & (MBINColor.LA <= maxLLevel) & (MBINColor.A >= minALevel) & (MBINColor.A <= maxALevel) & (MBINColor.B >= minBLevel) & (MBINColor.B <= maxBLevel))
        {
          for (int index = 0; index < bin.Rows.Count; index++)
          {
            double tempL, tempA, tempB;
            tempL = Convert.ToDouble(bin.Rows[index][9].ToString());
            tempA = Convert.ToDouble(bin.Rows[index][10].ToString());
            tempB = Convert.ToDouble(bin.Rows[index][11].ToString());

            if ((MBINColor.LA >= tempL - 5) & (MBINColor.LA <= tempL + 5) & (MBINColor.A >= tempA - 5) & (MBINColor.A <= tempA + 5) & (MBINColor.B >= tempB - 5) & (MBINColor.B <= tempB + 5))
            {
              GammutRangeCheck4 = 0;
              break;
            }
            GammutRangeCheck4 = 1;
          }

        }
        else
          GammutRangeCheck4 = 1;
        //
        //binnumber = Util.CATCalulation.MatchWithBinnedColorsBasedOn3D(MBINColor, bin, ColorBackGroundXYZ, BINColor);

        MBINcolorXYZ.X = Convert.ToDouble(bin.Rows[binnumber][6].ToString());
        MBINcolorXYZ.Y = Convert.ToDouble(bin.Rows[binnumber][7].ToString());
        MBINcolorXYZ.Z = Convert.ToDouble(bin.Rows[binnumber][8].ToString());
        double LA = Convert.ToDouble(bin.Rows[binnumber][3].ToString());
        double A = Convert.ToDouble(bin.Rows[binnumber][4].ToString());
        double B = Convert.ToDouble(bin.Rows[binnumber][5].ToString());
        byte R = Convert.ToByte(bin.Rows[binnumber][0].ToString());
        byte G = Convert.ToByte(bin.Rows[binnumber][1].ToString());
        byte Bi = Convert.ToByte(bin.Rows[binnumber][2].ToString());

        dt_Bg.Rows[i][45] = LA.ToString();
        dt_Bg.Rows[i][46] = A.ToString();
        dt_Bg.Rows[i][47] = B.ToString();

        Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
        {
          rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BgR, BgG, BgB));
          rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, Bi));
         
        }));
        DisplayMeasuredValuesFromCs200();

        MBincolorRGB = PerceptionLib.Color.ToRBG(MBINcolorXYZ);

        
 
        dt_Bg.Rows[i][48] = ColorMeasured.LA.ToString();
        dt_Bg.Rows[i][49] = ColorMeasured.A.ToString();
        dt_Bg.Rows[i][50] = ColorMeasured.B.ToString();


        ColorToShow.L =Convert.ToDouble(dt_Bg.Rows[i][6].ToString());
        ColorToShow.L = Convert.ToDouble(dt_Bg.Rows[i][7].ToString());
        ColorToShow.L = Convert.ToDouble(dt_Bg.Rows[i][8].ToString());


        double DeltaBin_Lab = PerceptionLib.Color.ColorDistanceCalAB(ColorToShow, ColorMeasured);

        dt_Bg.Rows[i][51] = DeltaBin_Lab.ToString();
        dt_Bg.Rows[i][52] = GammutRangeCheck4.ToString();


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    



        //binnumber = Util.CATCalulation.MatchWithBinned(MBINColor, bin, ColorBackGroundXYZ, BINColor);

        //GammutRangeCheck1 = 1;



        //if ((MBINColor.LA >= minLLevel) & (MBINColor.LA <= maxLLevel) & (MBINColor.A >= minALevel) & (MBINColor.A <= maxALevel) & (MBINColor.B >= minBLevel) & (MBINColor.B <= maxBLevel))
        //{
        //  for (int index = 0; index < bin.Rows.Count; index++)
        //  {
        //    double tempL, tempA, tempB;
        //    tempL = Convert.ToDouble(bin.Rows[index][9].ToString());
        //    tempA = Convert.ToDouble(bin.Rows[index][10].ToString());
        //    tempB = Convert.ToDouble(bin.Rows[index][11].ToString());

        //    if ((MBINColor.LA >= tempL - 5) & (MBINColor.LA <= tempL + 5) & (MBINColor.A >= tempA - 5) & (MBINColor.A <= tempA + 5) & (MBINColor.B >= tempB - 5) & (MBINColor.B <= tempB + 5))
        //    {
        //      GammutRangeCheck1 = 0;
        //      break;
        //    }
        //    GammutRangeCheck1 = 1;
        //  }

        //}
        //else
        //  GammutRangeCheck1 = 1;
        ////
       

        //NoMBINcolorXYZ.X = Convert.ToDouble(bin.Rows[binnumber][6].ToString());
        //NoMBINcolorXYZ.Y = Convert.ToDouble(bin.Rows[binnumber][7].ToString());
        //NoMBINcolorXYZ.Z = Convert.ToDouble(bin.Rows[binnumber][8].ToString());
        //double NoLA = Convert.ToDouble(bin.Rows[binnumber][3].ToString());
        //double NoA = Convert.ToDouble(bin.Rows[binnumber][4].ToString());
        //double NoB = Convert.ToDouble(bin.Rows[binnumber][5].ToString());
        //byte NoR = Convert.ToByte(bin.Rows[binnumber][0].ToString());
        //byte NoG = Convert.ToByte(bin.Rows[binnumber][1].ToString());
        //byte NoBi = Convert.ToByte(bin.Rows[binnumber][2].ToString());

        //dt_Bg.Rows[i][45] = LA.ToString();
        //dt_Bg.Rows[i][46] = A.ToString();
        //dt_Bg.Rows[i][47] = B.ToString();

        //Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
        //{
        //  rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BgR, BgG, BgB));
        //  rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(NoR, NOG, NoBi));

        //}));
        //DisplayMeasuredValuesFromCs200();

        //MBincolorRGB = PerceptionLib.Color.ToRBG(MBINcolorXYZ);



        //dt_Bg.Rows[i][48] = ColorMeasured.LA.ToString();
        //dt_Bg.Rows[i][49] = ColorMeasured.A.ToString();
        //dt_Bg.Rows[i][50] = ColorMeasured.B.ToString();


       


        //double NODeltaBin_Lab = PerceptionLib.Color.ColorDistanceCalAB(ColorToShow, ColorMeasured);

        //dt_Bg.Rows[i][51] = DeltaBin_Lab.ToString();
        //dt_Bg.Rows[i][52] = GammutRangeCheck4.ToString();
        
        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt_Bg.DefaultView));
        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

      }
      Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt_Bg.DefaultView));
      Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
    });

      btn_ExportGrid.IsEnabled = true;


    }
   
    
    
    
    //phone caluclator
    private void Btn_CATCalulator_Click(object sender, RoutedEventArgs e)
    {
      dtgrid_corrDisplay.IsEnabled = true;
      btn_StartMeasurment.IsEnabled = false;
      txt_R.IsEnabled = false;
      txt_G.IsEnabled = false;
      txt_B.IsEnabled = false;

      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\PerceptionLib\bin\previous data\cs-200 data\color mixing\phone\mixtureGroundtruth\bg6_HEX88.csv");
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\phone\bg\nobg_88Phone.csv");
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\phone\bincal\0pts\4000-6000.csv");
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\phone\bincal\4000-6k0.csv");
      DataTable bin = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));

      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\Phone_Bin_wt0.csv");
      DataTable binTable = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        binTable = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));

      DataRow newRow;

      for (int i = 0; i < bin.Rows.Count; i++)
      {
        byte Rt = 0;
        byte tR = 0;
        byte Gt = 0;
        byte tG = 0;
        byte Bt = 0;
        byte tB = 0;

        if (i > 0)
        {
          Rt = Convert.ToByte(bin.Rows[i - 1][0].ToString());
          Gt = Convert.ToByte(bin.Rows[i - 1][1].ToString());
          Bt = Convert.ToByte(bin.Rows[i - 1][2].ToString());

        }
        byte R = Convert.ToByte(bin.Rows[i][0].ToString());
        byte G = Convert.ToByte(bin.Rows[i][1].ToString());
        byte B = Convert.ToByte(bin.Rows[i][2].ToString());

        newRow = binTable.NewRow();


        //if (R >= 174 & G >=164  & B >= 149)
        if (R < 29 & G <= 60 & B <= 89)
        {
          newRow[0] = 0;
          newRow[1] = 0;
          newRow[2] = 0;
          newRow[3] = 0;
          newRow[4] = 0;
          newRow[5] = 0;
          newRow[6] = 0;
          newRow[7] = 0;
          newRow[8] = 0;

        }
        else
        {
          //if (Rt >= 174 & Gt >= 164 & Bt >= 149)
          if (Rt < 29& Gt <= 60 & Bt <= 89)
          {
            newRow[0] = 0;
            newRow[1] = 0;
            newRow[2] = 0;
            newRow[3] = 0;
            newRow[4] = 0;
            newRow[5] = 0;
            newRow[6] = 0;
            newRow[7] = 0;
            newRow[8] = 0;

          }

          else
          {
            newRow[0] = bin.Rows[i][0].ToString();
            newRow[1] = bin.Rows[i][1].ToString();
            newRow[2] = bin.Rows[i][2].ToString();
            newRow[3] = bin.Rows[i][3].ToString();
            newRow[4] = bin.Rows[i][4].ToString();
            newRow[5] = bin.Rows[i][5].ToString();
            newRow[6] = bin.Rows[i][6].ToString();
            newRow[7] = bin.Rows[i][7].ToString();
            newRow[8] = bin.Rows[i][8].ToString();

          }
        }
        binTable.Rows.Add(newRow);
      }

      Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = binTable.DefaultView));
      Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////  // zero elimination
      bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\Phone_Bin_wt0.csv");
      binTable = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        binTable = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));

      for (int i = 1; i < bin.Rows.Count; i++)
      {

        byte R = Convert.ToByte(bin.Rows[i - 1][0].ToString());
        byte G = Convert.ToByte(bin.Rows[i - 1][1].ToString());
        byte B = Convert.ToByte(bin.Rows[i - 1][2].ToString());

        byte r = Convert.ToByte(bin.Rows[i][0].ToString());
        byte g = Convert.ToByte(bin.Rows[i][1].ToString());
        byte b = Convert.ToByte(bin.Rows[i][2].ToString());

        if (R == 0 & G == 0 & B == 0)
        {
          if (r > 0 || g > 0 || b > 0)
          {
            byte r1 = Convert.ToByte(bin.Rows[i][0].ToString());
            byte g1 = Convert.ToByte(bin.Rows[i][1].ToString());
            byte b1 = Convert.ToByte(bin.Rows[i][2].ToString());
            double l1 = Convert.ToDouble(bin.Rows[i][3].ToString());
            double a1 = Convert.ToDouble(bin.Rows[i][4].ToString());
            double lb1 = Convert.ToDouble(bin.Rows[i][5].ToString());
            double x1 = Convert.ToDouble(bin.Rows[i][6].ToString());
            double y1 = Convert.ToDouble(bin.Rows[i][7].ToString());
            double z1 = Convert.ToDouble(bin.Rows[i][8].ToString());

            byte r2 = Convert.ToByte(bin.Rows[i + 1][0].ToString());
            byte g2 = Convert.ToByte(bin.Rows[i + 1][1].ToString());
            byte b2 = Convert.ToByte(bin.Rows[i + 1][2].ToString());
            double l2 = Convert.ToDouble(bin.Rows[i + 1][3].ToString());
            double a2 = Convert.ToDouble(bin.Rows[i + 1][4].ToString());
            double lb2 = Convert.ToDouble(bin.Rows[i + 1][5].ToString());
            double x2 = Convert.ToDouble(bin.Rows[i + 1][6].ToString());
            double y2 = Convert.ToDouble(bin.Rows[i + 1][7].ToString());
            double z2 = Convert.ToDouble(bin.Rows[i + 1][8].ToString());

            byte r3 = Convert.ToByte(bin.Rows[i + 2][0].ToString());
            byte g3 = Convert.ToByte(bin.Rows[i + 2][1].ToString());
            byte b3 = Convert.ToByte(bin.Rows[i + 2][2].ToString());
            double l3 = Convert.ToDouble(bin.Rows[i + 2][3].ToString());
            double a3 = Convert.ToDouble(bin.Rows[i + 2][4].ToString());
            double lb3 = Convert.ToDouble(bin.Rows[i + 2][5].ToString());
            double x3 = Convert.ToDouble(bin.Rows[i + 2][6].ToString());
            double y3 = Convert.ToDouble(bin.Rows[i + 2][7].ToString());
            double z3 = Convert.ToDouble(bin.Rows[i + 2][8].ToString());

            byte rFinal = Convert.ToByte((r1 + r2 + r3) / 3);
            byte gFinal = Convert.ToByte((g1 + g2 + g3) / 3);
            byte bFinal = Convert.ToByte((b1 + b2 + b3) / 3);
            double lFinal = (l1 + l2 + l3) / 3.0;
            double aFinal = (a1 + a2 + a3) / 3.0;
            double lbFinal = (lb1 + lb2 + lb3) / 3.0;
            double xFinal = (x1 + x2 + x3) / 3.0;
            double yFinal = (y1 + y2 + y3) / 3.0;
            double zFinal = (z1 + z2 + z3) / 3.0;

            newRow = binTable.NewRow();
            newRow[0] = r1.ToString();
            newRow[1] = g1.ToString();
            newRow[2] = b1.ToString();
            newRow[3] = l1.ToString();
            newRow[4] = a1.ToString();
            newRow[5] = lb1.ToString();
            newRow[6] = x1.ToString();
            newRow[7] = y1.ToString();
            newRow[8] = z1.ToString();



            //newRow = binTable.NewRow();
            //newRow[0] = bin.Rows[i][0].ToString();
            //newRow[1] = bin.Rows[i][1].ToString();
            //newRow[2] = bin.Rows[i][2].ToString();
            //newRow[3] = bin.Rows[i][3].ToString();
            //newRow[4] = bin.Rows[i][4].ToString();
            //newRow[5] = bin.Rows[i][5].ToString();
            //newRow[6] = bin.Rows[i][6].ToString();
            //newRow[7] = bin.Rows[i][7].ToString();
            //newRow[8] = bin.Rows[i][8].ToString();


            binTable.Rows.Add(newRow);
          }

        }
      }

      Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = binTable.DefaultView));
      Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

      btn_ExportGrid.IsEnabled = true;
    }

    public DataTable dt = new DataTable();
    private void Btn_ManualBinCalculator_Click(object sender, RoutedEventArgs e)
    {
      int connect = 0;
      ///add this sleep for phone
      int i = BinNo;
      if (i == 0)
      {
        PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\color88ForPhone.txt");
        //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\colorForPhone.txt");
        //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\bin8000.csv");
        dtgrid_corrDisplay.IsEnabled = false;


        //Thread.Sleep(16800);
        btn_StartMeasurment.IsEnabled = false;
        txt_R.IsEnabled = false;
        txt_G.IsEnabled = false;
        txt_B.IsEnabled = false;

        DataTable new_dt = new DataTable();


        dt = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
        connect = PerceptionLib.Cs200Connection.ConnectToCS200();
        //if not connected then connect will be 1


        BgNo = dt.Rows.Count;
      }
     
        DateTime Start = DateTime.Now;
        if (connect == 0)
        {

            mainW.R = Convert.ToByte(dt.Rows[i][0].ToString());
            mainW.G = Convert.ToByte(dt.Rows[i][1].ToString());
            mainW.B = Convert.ToByte(dt.Rows[i][2].ToString());

          
            DisplayMeasuredValuesFromCs200();

            DifferenceCalculation();

          

            // assignes the data to a the datatable 
            dt.Rows[i][3] = colorToShow.L.ToString();
            dt.Rows[i][4] = colorToShow.U.ToString();
            dt.Rows[i][5] = colorToShow.V.ToString();
            dt.Rows[i][6] = colorToShow.UP.ToString();
            dt.Rows[i][7] = colorToShow.VP.ToString();
            dt.Rows[i][8] = ColorMeasured.L.ToString();
            dt.Rows[i][9] = ColorMeasured.U.ToString();
            dt.Rows[i][10] = ColorMeasured.V.ToString();
            dt.Rows[i][11] = ColorMeasured.UP.ToString();
            dt.Rows[i][12] = ColorMeasured.VP.ToString();
            //dt.Rows][13] = ColorDifference.L.ToString();
            //dt.Rows][14] = ColorDifference.U.ToString();
            //dt.Rows][15] = ColorDifference.V.ToString();

            ////

            dt.Rows[i][13] = colorToShow.LA.ToString();
            dt.Rows[i][14] = colorToShow.A.ToString();
            dt.Rows[i][15] = colorToShow.B.ToString();
            dt.Rows[i][16] = ColorMeasured.LA.ToString();
            dt.Rows[i][17] = ColorMeasured.A.ToString();
            dt.Rows[i][18] = ColorMeasured.B.ToString();

            //

            dt.Rows[i][19] = PerceptionLib.Color.ColorDistanceCal(colorToShow, ColorMeasured).ToString();
            dt.Rows[i][20] = PerceptionLib.Color.ColorDistanceCalAB(colorToShow, ColorMeasured).ToString();
            //
            dt.Rows[i][21] = ColorToShowXYZ.X.ToString();
            dt.Rows[i][22] = ColorToShowXYZ.Y.ToString();
            dt.Rows[i][23] = ColorToShowXYZ.Z.ToString();
            dt.Rows[i][24] = ColorMeasuredXYZ.X.ToString();
            dt.Rows[i][25] = ColorMeasuredXYZ.Y.ToString();
            dt.Rows[i][26] = ColorMeasuredXYZ.Z.ToString();

            dt.Rows[i][27] = MR.ToString();
            dt.Rows[i][28] = MG.ToString();
            dt.Rows[i][29] = MB.ToString();

            BradXYZ = Util.CATCalulation.bradford(ColorToShowXYZ);
            VonXYZ = Util.CATCalulation.VonKries(ColorToShowXYZ);
            ScalingXYZ = Util.CATCalulation.XYZScaling(ColorToShowXYZ);

            Bradcolor = PerceptionLib.Color.ToLUV(BradXYZ);
            Voncolor = PerceptionLib.Color.ToLUV(VonXYZ);
            Scalingcolor = PerceptionLib.Color.ToLUV(ScalingXYZ);


            bradRGB = PerceptionLib.Color.ToRBG(Bradcolor);
            VonRGB = PerceptionLib.Color.ToRBG(Voncolor);
            scalingRGB = PerceptionLib.Color.ToRBG(Scalingcolor);

            CalRGB = PerceptionLib.Color.ToRBG(ColorMeasuredXYZ);


            dt.Rows[i][30] = BradXYZ.X.ToString();
            dt.Rows[i][31] = BradXYZ.Y.ToString();
            dt.Rows[i][32] = BradXYZ.Z.ToString();
            dt.Rows[i][33] = PerceptionLib.Color.ColorDistanceCal(ColorMeasured, Bradcolor).ToString();
            dt.Rows[i][34] = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, Bradcolor).ToString();

            dt.Rows[i][35] = VonXYZ.X.ToString();
            dt.Rows[i][36] = VonXYZ.Y.ToString();
            dt.Rows[i][37] = VonXYZ.Z.ToString();
            dt.Rows[i][38] = PerceptionLib.Color.ColorDistanceCal(ColorMeasured, Voncolor).ToString();
            dt.Rows[i][39] = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, Voncolor).ToString();

            dt.Rows[i][40] = ScalingXYZ.X.ToString();
            dt.Rows[i][41] = ScalingXYZ.Y.ToString();
            dt.Rows[i][42] = ScalingXYZ.Z.ToString();
            dt.Rows[i][43] = PerceptionLib.Color.ColorDistanceCal(ColorMeasured, Scalingcolor).ToString();
            dt.Rows[i][44] = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, Scalingcolor).ToString();

            dt.Rows[i][45] = bradRGB.R.ToString();
            dt.Rows[i][46] = bradRGB.G.ToString();
            dt.Rows[i][47] = bradRGB.B.ToString();

            dt.Rows[i][48] = VonRGB.R.ToString();
            dt.Rows[i][49] = VonRGB.G.ToString();
            dt.Rows[i][50] = VonRGB.B.ToString();

            dt.Rows[i][51] = scalingRGB.R.ToString();
            dt.Rows[i][52] = scalingRGB.G.ToString();
            dt.Rows[i][53] = scalingRGB.B.ToString();

            dt.Rows[i][54] = CalRGB.R.ToString();
            dt.Rows[i][55] = CalRGB.G.ToString();
            dt.Rows[i][56] = CalRGB.B.ToString();

            dt.Rows[i][57] = Bradcolor.UP.ToString();
            dt.Rows[i][58] = Bradcolor.VP.ToString();

            dt.Rows[i][59] = Voncolor.UP.ToString();
            dt.Rows[i][60] = Voncolor.VP.ToString();

            dt.Rows[i][61] = Scalingcolor.UP.ToString();
            dt.Rows[i][62] = Scalingcolor.VP.ToString();

            dt.Rows[i][63] = ColorMeasuredXYZ.stdx.ToString();
            dt.Rows[i][64] = ColorMeasuredXYZ.stdy.ToString();
            dt.Rows[i][65] = ColorMeasuredXYZ.stdz.ToString();



            //if (i == 9)
            //{
            //  Dispatcher.Invoke(new Action(() =>
            //  {
            //    dtgrid_corrDisplay.ItemsSource = dt.DefaultView;
            //    dtgrid_corrDisplay.Items.Refresh();

            //  }));
            //}


            //if (i == (dt.Rows.Count - 1))
            //{
            //  int disconnect = PerceptionLib.Cs200Connection.DisconnectCS200();
            //}
          }

          // System.Threading.Thread.Sleep(10000);
          // grid is populated with new datatable which has luv values
         //Dispatcher.Invoke(new Action(() =>
          //{
            dtgrid_corrDisplay.ItemsSource = dt.DefaultView;
            dtgrid_corrDisplay.Items.Refresh();
          //}));


          //  to show all the pairs in cie graph
          //pairs.Clear();

          //for (int i = 0; i < dt.Rows.Count; i++)
          //{
          //  pairs.Add(new MeasurementPair()
          //  {
          //    ColorToShow = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dt.Rows[i][6].ToString()), VP = Convert.ToDouble(dt.Rows[i][7].ToString()) },
          //    ColorCaptured = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dt.Rows[i][11].ToString()), VP = Convert.ToDouble(dt.Rows[i][12].ToString()) }
          //  });
          //}


          BinNo++;


      btn_ExportGrid.IsEnabled = true;

      dtgrid_corrDisplay.IsEnabled = true;
      dataTable = dt;
    }

    public static int stop = 0;
    private void Btn_Stop_Click(object sender, RoutedEventArgs e)
    {
      stop = 1;
    }
   
    
    private void Btn_PhoneBinCalculator_Click(object sender, RoutedEventArgs e)
    {
      BgNo = 1;
      int connect = PerceptionLib.Cs200Connection.ConnectToCS200();
      btn_StartMeasurment.IsEnabled = false;
      txt_R.IsEnabled = false;
      txt_G.IsEnabled = false;
      txt_B.IsEnabled = false;

      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\PhoneBin.csv");
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\BgValueSet1.csv");
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\BgValue.csv");
      DataTable dt_Bg = new DataTable();
      DataRow newRow;
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        dt_Bg = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));
     
     
      double loopvalue=double.MaxValue;
      
      ThreadPool.QueueUserWorkItem(ignored =>
      {
       
        
        // bg1
        //loopvalue = 15;

      //  Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      //  {
      //    rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(187, 82, 148));

      //  }));
      //for (int i = 0; i < loopvalue; i++)
      //{
        
      //  if (stop == 1)
      //  {
      //    stop = 0;
      //    break;
      //  }
      //  DisplayMeasuredValuesFromCs200();
      //  newRow = dt_Bg.NewRow();
    
      //  newRow[0] = MR.ToString();
      //  newRow[1] = MG.ToString();
      //  newRow[2] = MB.ToString();
      //  newRow[3] = ColorMeasuredXYZ.X.ToString();
      //  newRow[4] = ColorMeasuredXYZ.Y.ToString();
      //  newRow[5] = ColorMeasuredXYZ.Z.ToString();
      //  newRow[6] = colorMeasured.LA.ToString();
      //  newRow[7] = colorMeasured.A.ToString();
      //  newRow[8] = colorMeasured.B.ToString();
      //  dt_Bg.Rows.Add(newRow);
      //  //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt_Bg.DefaultView));
      //  //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
        
      //  //78500 3000 phone

      //  int temp = 0;
      //  if (i > 3050)
      //  {
      //    for (int j = dt_Bg.Rows.Count - 20; j < dt_Bg.Rows.Count; j++)
      //    {
      //      FgNo = j;
      //      double r = Convert.ToDouble(dt_Bg.Rows[j][0].ToString());
      //      double g = Convert.ToDouble(dt_Bg.Rows[j][1].ToString());
      //      double b = Convert.ToDouble(dt_Bg.Rows[j][2].ToString());

      //      if (r>202)//(r< 7)//(r == 0)
      //      {
      //       if (g>169)//(g < 58)//if (g < 180 & 173 < g)
      //        {
      //          if (b>170)//(b < 89)//if (b < 169 & 150 < b)
      //        {
      //            temp++;
      //        }
      //        }

      //      }
      //    }
      //  }

      //  if (temp > 15)
      //  {
      //    newRow[9] = BgNo.ToString(); 
      //    BgNo++;
      //    FgNo = 0;
      //    break;
      //  }
            
      //}

      //// bg2

      //Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      //{
      //  rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(243, 242, 237));

      //}));
      //for (int i = 0; i < loopvalue; i++)
      //{
       
      //  if (stop == 1)
      //  {
      //    stop = 0;
      //    break;
      //  }
      //  DisplayMeasuredValuesFromCs200();
      //  newRow = dt_Bg.NewRow();

      //  newRow[0] = MR.ToString();
      //  newRow[1] = MG.ToString();
      //  newRow[2] = MB.ToString();
      //  newRow[3] = ColorMeasuredXYZ.X.ToString();
      //  newRow[4] = ColorMeasuredXYZ.Y.ToString();
      //  newRow[5] = ColorMeasuredXYZ.Z.ToString();
      //  newRow[6] = colorMeasured.LA.ToString();
      //  newRow[7] = colorMeasured.A.ToString();
      //  newRow[8] = colorMeasured.B.ToString();
      //  dt_Bg.Rows.Add(newRow);
      //  //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt_Bg.DefaultView));
      //  //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

       
      //  //78500 3000 phone
      //  int temp = 0;
      //  if (i >3050)
      //  {
         
      //    temp = 0;
      //    for (int j = dt_Bg.Rows.Count -20; j < dt_Bg.Rows.Count; j++)
      //    {
      //      FgNo = j;
      //      double r = Convert.ToDouble(dt_Bg.Rows[j][0].ToString());
      //      double g = Convert.ToDouble(dt_Bg.Rows[j][1].ToString());
      //      double b = Convert.ToDouble(dt_Bg.Rows[j][2].ToString());

      //      if (r > 224)//(r< 7)//(r == 0)
      //      {
      //        if (g > 216)//(g < 58)//if (g < 180 & 173 < g)
      //        {
      //          if (b > 200)//(b < 89)//if (b < 169 & 150 < b)
      //          {
      //            temp++;
      //          }
      //        }

      //      }
      //    }
      //  }

      //  if (temp > 15)
      //  {
      //    newRow[9] = BgNo.ToString();
      //    BgNo++;
      //    FgNo = 0;
      //    break;
      //  }

      //}


      //// bg3
      //Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      //{
      //  rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(201, 201, 201));

      //}));
      //for (int i = 0; i < loopvalue; i++)
      //{
        
      //  if (stop == 1)
      //  {
      //    stop = 0;
      //    break;
      //  }
      //  DisplayMeasuredValuesFromCs200();
      //  newRow = dt_Bg.NewRow();

      //  newRow[0] = MR.ToString();
      //  newRow[1] = MG.ToString();
      //  newRow[2] = MB.ToString();
      //  newRow[3] = ColorMeasuredXYZ.X.ToString();
      //  newRow[4] = ColorMeasuredXYZ.Y.ToString();
      //  newRow[5] = ColorMeasuredXYZ.Z.ToString();
      //  newRow[6] = colorMeasured.LA.ToString();
      //  newRow[7] = colorMeasured.A.ToString();
      //  newRow[8] = colorMeasured.B.ToString();
      //  dt_Bg.Rows.Add(newRow);
      //  //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt_Bg.DefaultView));
      //  //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

      //  //78500 3000 phone
      //  int temp = 0;
      //  if (i >3050)
      //  {
          
      //    temp = 0;
      //    for (int j = dt_Bg.Rows.Count - 20; j < dt_Bg.Rows.Count; j++)
      //    {
      //      FgNo = j;
      //      double r = Convert.ToDouble(dt_Bg.Rows[j][0].ToString());
      //      double g = Convert.ToDouble(dt_Bg.Rows[j][1].ToString());
      //      double b = Convert.ToDouble(dt_Bg.Rows[j][2].ToString());

      //      if (r > 206)//(r< 7)//(r == 0)
      //      {
      //        if (g > 198)//(g < 58)//if (g < 180 & 173 < g)
      //        {
      //          if (b > 187)//(b < 89)//if (b < 169 & 150 < b)
      //          {
      //            temp++;
      //          }
      //        }

      //      }
      //    }
      //  }

      //  if (temp > 15)
      //  {
      //    newRow[9] = BgNo.ToString();
      //    BgNo++;
      //    FgNo = 0;
      //    break;
      //  }

      //}


      ////// bg4
      //  Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      //  {
      //    rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(100, 190, 171));

      //  }));
      //  for (int i = 0; i < loopvalue; i++)
      //  {

      //    if (stop == 1)
      //    {
      //      stop = 0;
      //      break;
      //    }
      //    DisplayMeasuredValuesFromCs200();
      //    newRow = dt_Bg.NewRow();

      //    newRow[0] = MR.ToString();
      //    newRow[1] = MG.ToString();
      //    newRow[2] = MB.ToString();
      //    newRow[3] = ColorMeasuredXYZ.X.ToString();
      //    newRow[4] = ColorMeasuredXYZ.Y.ToString();
      //    newRow[5] = ColorMeasuredXYZ.Z.ToString();
      //    newRow[6] = colorMeasured.LA.ToString();
      //    newRow[7] = colorMeasured.A.ToString();
      //    newRow[8] = colorMeasured.B.ToString();
      //    dt_Bg.Rows.Add(newRow);
      //    //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt_Bg.DefaultView));
      //    //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

      //    //78500 3000 phone
      //    int temp = 0;
      //    if (i > 9630)
      //    {

      //      temp = 0;
      //      for (int j = dt_Bg.Rows.Count - 20; j < dt_Bg.Rows.Count; j++)
      //      {
      //        FgNo = j;
      //        double r = Convert.ToDouble(dt_Bg.Rows[j][0].ToString());
      //        double g = Convert.ToDouble(dt_Bg.Rows[j][1].ToString());
      //        double b = Convert.ToDouble(dt_Bg.Rows[j][2].ToString());

      //        if (r < 58)//(r == 0)
      //        {
      //          if (g < 128)//(g < 58)//if (g < 180 & 173 < g)
      //          {
      //            if (b < 130)//(b < 89)//if (b < 169 & 150 < b)
      //            {
      //              temp++;
      //            }
      //          }

      //        }
      //      }
      //    }

      //    if (temp > 15)
      //    {
      //      newRow[9] = BgNo.ToString();
      //      BgNo++;
      //      FgNo = 0;
      //      break;
      //    }

      //  }


        //// bg5
        //Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
        //{
        //  rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(122, 122, 121));
         
        //}));
        
        //for (int i = 0; i < loopvalue; i++)
        //{

        //  if (stop == 1)
        //  {
        //    stop = 0;
        //    break;
        //  }
        //  DisplayMeasuredValuesFromCs200();
        //  newRow = dt_Bg.NewRow();

        //  newRow[0] = MR.ToString();
        //  newRow[1] = MG.ToString();
        //  newRow[2] = MB.ToString();
        //  newRow[3] = ColorMeasuredXYZ.X.ToString();
        //  newRow[4] = ColorMeasuredXYZ.Y.ToString();
        //  newRow[5] = ColorMeasuredXYZ.Z.ToString();
        //  newRow[6] = colorMeasured.LA.ToString();
        //  newRow[7] = colorMeasured.A.ToString();
        //  newRow[8] = colorMeasured.B.ToString();
        //  dt_Bg.Rows.Add(newRow);
        //  //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt_Bg.DefaultView));
        //  //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

        //  //78500 3000 phone
        //  int temp = 0;
        //  if (i > 9630)
        //  {
        //    temp = 0;
        //    for (int j = dt_Bg.Rows.Count - 20; j < dt_Bg.Rows.Count; j++)
        //    {
        //      FgNo = j;
        //      double r = Convert.ToDouble(dt_Bg.Rows[j][0].ToString());
        //      double g = Convert.ToDouble(dt_Bg.Rows[j][1].ToString());
        //      double b = Convert.ToDouble(dt_Bg.Rows[j][2].ToString());

        //      if (r < 77)//(r == 0)
        //      {
        //        if (g < 90)//if (g < 180 & 173 < g)
        //        {
        //          if (b < 110)//if (b < 169 & 150 < b)
        //          {
        //            temp++;
        //          }
        //        }

        //      }
        //    }
        //  }

        //  if (temp > 15)
        //  {
        //    newRow[9] = BgNo.ToString();
        //    BgNo++;
        //    FgNo = 0;
        //    break;
        //  }

        //}

        // //bg6
        //Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
        //{
        //  rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(83, 83, 83));

        //}));
        //for (int i = 0; i < loopvalue; i++)
        //{


        //  if (stop == 1)
        //  {
        //    stop = 0;
        //    break;
        //  }
        //  DisplayMeasuredValuesFromCs200();
        //  newRow = dt_Bg.NewRow();

        //  newRow[0] = MR.ToString();
        //  newRow[1] = MG.ToString();
        //  newRow[2] = MB.ToString();
        //  newRow[3] = ColorMeasuredXYZ.X.ToString();
        //  newRow[4] = ColorMeasuredXYZ.Y.ToString();
        //  newRow[5] = ColorMeasuredXYZ.Z.ToString();
        //  newRow[6] = colorMeasured.LA.ToString();
        //  newRow[7] = colorMeasured.A.ToString();
        //  newRow[8] = colorMeasured.B.ToString();
        //  dt_Bg.Rows.Add(newRow);
        //  //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt_Bg.DefaultView));
        //  //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

        //  //78500 3000 phone
        //  int temp = 0;
        //  if (i > 9600)
        //  {
        //    temp = 0;
        //    for (int j = dt_Bg.Rows.Count - 20; j < dt_Bg.Rows.Count; j++)
        //    {
        //      FgNo = j;
        //      double r = Convert.ToDouble(dt_Bg.Rows[j][0].ToString());
        //      double g = Convert.ToDouble(dt_Bg.Rows[j][1].ToString());
        //      double b = Convert.ToDouble(dt_Bg.Rows[j][2].ToString());

        //      if (r < 50)//(r == 0)
        //      {
        //        if (g < 74)//if (g < 180 & 173 < g)
        //        {
        //          if (b < 99)//if (b < 169 & 150 < b)
        //          {
        //            temp++;
        //          }
        //        }

        //      }
        //    }
        //  }

        //  if (temp > 15)
        //  {
        //    newRow[9] = BgNo.ToString();
        //    BgNo++;
        //    FgNo = 0;
        //    break;
        //  }

        //}

        //bg7
        Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
        {
          rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(72, 91, 165));
        }));

        for (int i = 0; i < loopvalue; i++)
        {

          if (stop == 1)
          {
            stop = 0;
            break;
          }
          DisplayMeasuredValuesFromCs200();
          newRow = dt_Bg.NewRow();

          newRow[0] = MR.ToString();
          newRow[1] = MG.ToString();
          newRow[2] = MB.ToString();
          newRow[3] = ColorMeasuredXYZ.X.ToString();
          newRow[4] = ColorMeasuredXYZ.Y.ToString();
          newRow[5] = ColorMeasuredXYZ.Z.ToString();
          newRow[6] = colorMeasured.LA.ToString();
          newRow[7] = colorMeasured.A.ToString();
          newRow[8] = colorMeasured.B.ToString();
          dt_Bg.Rows.Add(newRow);
          //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt_Bg.DefaultView));
          //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

          //78500 3000 phone
          int temp = 0;
          if (i > 9630)
          //if (i > 3060)
          {
            temp = 0;
            for (int j = dt_Bg.Rows.Count - 20; j < dt_Bg.Rows.Count; j++)
            {
              FgNo = j;
              double r = Convert.ToDouble(dt_Bg.Rows[j][0].ToString());
              double g = Convert.ToDouble(dt_Bg.Rows[j][1].ToString());
              double b = Convert.ToDouble(dt_Bg.Rows[j][2].ToString());

              if (r < 40)//(r == 0)
              {
                if (g < 80)//if (g < 180 & 173 < g)
                {
                  if (b < 128)//if (b < 169 & 150 < b)
                  {
                    temp++;
                  }
                }

              }
            }
          }

          if (temp > 15)
          {
            newRow[9] = BgNo.ToString();
            BgNo++;
            FgNo = 0;
            break;
          }

        }

      Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt_Bg.DefaultView));
      Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
      });

      btn_ExportGrid.IsEnabled = true;
    }
    
    private void Btn_CAT_Click(object sender, RoutedEventArgs e)
    {
      
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\BinForCATlab.csv");
     //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\bigProjecor\BinRGB.csv");
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\Reduscer.csv");
      DataTable bin = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));


      foreach (DataRow dr in bin.Rows)
      {

        //CAT LAB caluclation

        //brad
        //ColorToShowXYZ.X = Convert.ToDouble(dr["bradX"].ToString());
        //ColorToShowXYZ.Y = Convert.ToDouble(dr["bradY"].ToString());
        //ColorToShowXYZ.Z = Convert.ToDouble(dr["bradZ"].ToString());

        //ColorToShow = PerceptionLib.Color.ToLAB(ColorToShowXYZ);

        //dr["bradL"] = ColorToShow.LA.ToString();
        //dr["bradA"] = ColorToShow.A.ToString();
        //dr["bradB"] = ColorToShow.B.ToString();

        ////Von
        //ColorToShowXYZ.X = Convert.ToDouble(dr["VonX"].ToString());
        //ColorToShowXYZ.Y = Convert.ToDouble(dr["VonY"].ToString());
        //ColorToShowXYZ.Z = Convert.ToDouble(dr["VonZ"].ToString());

        //ColorToShow = PerceptionLib.Color.ToLAB(ColorToShowXYZ);

        ////scaling
        //dr["vonL"] = ColorToShow.LA.ToString();
        //dr["vonA"] = ColorToShow.A.ToString();
        //dr["vonB"] = ColorToShow.B.ToString();

        //ColorToShowXYZ.X = Convert.ToDouble(dr["SX"].ToString());
        //ColorToShowXYZ.Y = Convert.ToDouble(dr["SY"].ToString());
        //ColorToShowXYZ.Z = Convert.ToDouble(dr["SZ"].ToString());

        //ColorToShow = PerceptionLib.Color.ToLAB(ColorToShowXYZ);

        //dr["sL"] = ColorToShow.LA.ToString();
        //dr["sA"] = ColorToShow.A.ToString();
        //dr["sB"] = ColorToShow.B.ToString();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //xyz caluclation
        
        //ColorToShowXYZ.X = Convert.ToDouble(dr["X"].ToString());
        //ColorToShowXYZ.Y = Convert.ToDouble(dr["Y"].ToString());
        //ColorToShowXYZ.Z = Convert.ToDouble(dr["Z"].ToString());

        //BradXYZ = Util.CATCalulation.bradford(ColorToShowXYZ);
        //VonXYZ = Util.CATCalulation.VonKries(ColorToShowXYZ);
        //ScalingXYZ = Util.CATCalulation.XYZScaling(ColorToShowXYZ);

        //dr["bradX"] = BradXYZ.X.ToString();
        //dr["bradY"] = BradXYZ.Y.ToString();
        //dr["bradZ"] = BradXYZ.Z.ToString();

        //dr["bradX"] = VonXYZ.X.ToString();
        //dr["bradY"] = VonXYZ.Y.ToString();
        //dr["bradZ"] = VonXYZ.Z.ToString();

        //dr["bradX"] = BradXYZ.X.ToString();
        //dr["bradY"] = BradXYZ.Y.ToString();
        //dr["bradZ"] = BradXYZ.Z.ToString();

        //byte R = Convert.ToByte(dr["r"].ToString());
        //byte G = Convert.ToByte(dr["g"].ToString());
        //byte B = Convert.ToByte(dr["b"].ToString());

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Hex color
        //String Hex = PerceptionLib.Color.RGBtoHEX(R, G, B);
        //dr["HEX"] = "0x" + Hex;
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        ColorToShowXYZ = new CIEXYZ(0, 0, 0);
        ColorToShowXYZ.X = Convert.ToDouble(dr["X"].ToString());
        ColorToShowXYZ.Y = Convert.ToDouble(dr["Y"].ToString());
        ColorToShowXYZ.Z = Convert.ToDouble(dr["Z"].ToString());
        
        ColorToShow = PerceptionLib.Color.ToLUV(ColorToShowXYZ);
        PerceptionLib.RGBValue ab = PerceptionLib.Color.ToRBGFromLAB(ColorToShow);

        dr["L"] = ColorToShow.LA.ToString();
        dr["A"] = ColorToShow.A.ToString();
        dr["LB"] = ColorToShow.B.ToString();
        dr["R"] = ab.R.ToString();
        dr["G"] = ab.G.ToString();
        dr["B"] = ab.B.ToString();

        String Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
        dr["HEX"] = "0x" + Hex;
        
        //////////////////////////////////////////////////////////////////////////////////////////////////////////




        
        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = bin.DefaultView));
        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

      }
      btn_ExportGrid.IsEnabled = true;
    }
        
    
    private void Btn_PhoneDataCalculator_Click(object sender, RoutedEventArgs e)
    {
      dtgrid_corrDisplay.IsEnabled = true;
      btn_StartMeasurment.IsEnabled = false;
      txt_R.IsEnabled = false;
      txt_G.IsEnabled = false;
      txt_B.IsEnabled = false;


      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\phone\cal\consolidated_Phone_NewBG.csv");

      DataTable MixedXYZ = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        MixedXYZ = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));



      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\phone\cal\FinalValue.csv");

      DataTable FinalValue = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        FinalValue = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));

      CIEXYZ BgWithScreenXYZ;
      CIEXYZ colorToShowBINXYZ;
      PerceptionLib.Color colorToShowBIN;

      Double CDiff, ADiff, AsDiff, BDiff, BsDiff;

      DataRow newRow;

      for (int i = 0; i < MixedXYZ.Rows.Count; i++)
      {
        newRow = FinalValue.NewRow();

        double xbG = Convert.ToDouble(MixedXYZ.Rows[i][1].ToString());
        double ybG = Convert.ToDouble(MixedXYZ.Rows[i][2].ToString());
        double zbG = Convert.ToDouble(MixedXYZ.Rows[i][3].ToString());
        ColorBackGroundXYZ = new CIEXYZ(xbG, ybG, zbG);


        double xbGS = Convert.ToDouble(MixedXYZ.Rows[i][4].ToString());
        double ybGS = Convert.ToDouble(MixedXYZ.Rows[i][5].ToString());
        double zbGS = Convert.ToDouble(MixedXYZ.Rows[i][6].ToString());
        BgWithScreenXYZ = new CIEXYZ(xbGS, ybGS, zbGS);
    

        double xfG = Convert.ToDouble(MixedXYZ.Rows[i][8].ToString());
        double yfG = Convert.ToDouble(MixedXYZ.Rows[i][9].ToString());
        double zfG = Convert.ToDouble(MixedXYZ.Rows[i][10].ToString());
        ColorToShowXYZ = new CIEXYZ(xfG, yfG, zfG);

        double xfGB = Convert.ToDouble(MixedXYZ.Rows[i][11].ToString());
        double yfGB = Convert.ToDouble(MixedXYZ.Rows[i][12].ToString());
        double zfGB = Convert.ToDouble(MixedXYZ.Rows[i][13].ToString());
        colorToShowBINXYZ = new CIEXYZ(xfGB, yfGB, zfGB);
        colorToShowBIN = PerceptionLib.Color.ToLAB(colorToShowBINXYZ);

        double xcfG = Convert.ToDouble(MixedXYZ.Rows[i][14].ToString());
        double ycfG = Convert.ToDouble(MixedXYZ.Rows[i][15].ToString());
        double zcfG = Convert.ToDouble(MixedXYZ.Rows[i][16].ToString());

        ColorMeasuredXYZ = new CIEXYZ(xcfG, ycfG, zcfG);
        ColorMeasured = PerceptionLib.Color.ToLAB(ColorMeasuredXYZ);
        
        AcolorXYZ = new CIEXYZ(0, 0, 0);
        AcolorXYZ.X = xbG + xfG;
        AcolorXYZ.Y = ybG + yfG;
        AcolorXYZ.Z = zbG + zfG;

        Acolor = PerceptionLib.Color.ToLAB(AcolorXYZ);
        MAcolorXYZ = new CIEXYZ(0, 0, 0);
        MAcolorXYZ.X = xbGS + xfG;
        MAcolorXYZ.Y = ybGS + yfG;
        MAcolorXYZ.Z = zbGS + zfG;

        MAcolor = PerceptionLib.Color.ToLAB(MAcolorXYZ);
        BINcolorXYZ = new CIEXYZ(0, 0, 0);
        BINcolorXYZ.X = xbG + xcfG;
        BINcolorXYZ.Y = ybG + ycfG;
        BINcolorXYZ.Z = zbG + zcfG;

        BINColor = PerceptionLib.Color.ToLAB(BINcolorXYZ);
        MBINcolorXYZ = new CIEXYZ(0, 0, 0);
        MBINcolorXYZ.X = xbGS + xcfG;
        MBINcolorXYZ.Y = ybGS + ycfG;
        MBINcolorXYZ.Z = zbGS + zcfG;
        
        MBINColor = PerceptionLib.Color.ToLAB(MBINcolorXYZ);

        CDiff = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, colorToShowBIN);
        ADiff = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, Acolor);
        AsDiff = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, MAcolor);
        BDiff = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, BINColor);
        BsDiff = PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, MBINColor);


        newRow[0] =MixedXYZ.Rows[i][0].ToString();
        newRow[1] =MixedXYZ.Rows[i][7].ToString();
        newRow[2] =colorToShowBIN.LA.ToString();
        newRow[3] =colorToShowBIN.A.ToString();
        newRow[4] =colorToShowBIN.B.ToString();
        newRow[5] =ColorMeasured.LA.ToString();
        newRow[6] =ColorMeasured.A.ToString();
        newRow[7] =ColorMeasured.B.ToString();
        newRow[8] =Acolor.LA.ToString();
        newRow[9] =Acolor.A.ToString();
        newRow[10]=Acolor.B.ToString();
        newRow[11] = MAcolor.LA.ToString();
        newRow[12] = MAcolor.A.ToString();
        newRow[13] = MAcolor.B.ToString();

        newRow[14] = BINColor.LA.ToString();
        newRow[15] = BINColor.A.ToString();
        newRow[16] = BINColor.B.ToString();
        newRow[17] = MBINColor.LA.ToString();
        newRow[18] = MBINColor.A.ToString();
        newRow[19] = MBINColor.B.ToString();

        newRow[20] = ADiff.ToString();
        newRow[21] = AsDiff.ToString();
        newRow[22] = BDiff.ToString();
        newRow[23] = BsDiff.ToString();
        newRow[24] = CDiff.ToString();
        FinalValue.Rows.Add(newRow);
      }

      Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = FinalValue.DefaultView));
      Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

      btn_ExportGrid.IsEnabled = true;


    }

    //excel reducer
    private void Btn_Reduce_Click(object sender, RoutedEventArgs e)
    {
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\phone\phoneBin_full.csv");
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\Reduscer.csv");
      DataTable bin = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));


      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\phone\phoneBin_template.csv");
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\Reduscer_outPut.csv");
      //DataTable binTable = new DataTable();
      //Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      //{
      //  dtgrid_corrDisplay.Items.Refresh();
      //  binTable = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      //}));

      DataTable binTable = new DataTable();
      binTable = bin.Clone();
    
      ThreadPool.QueueUserWorkItem(ignored =>
      {
        for (int i = 0; i < bin.Rows.Count; i = i + 10)
        {
          binTable.ImportRow(bin.Rows[i]);
    
        }

        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = binTable.DefaultView));
        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
      });

      btn_ExportGrid.IsEnabled = true;
    }



    void cmb_graph_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      DataTable dt = new DataTable();
      //dt=dataTable;
      DataRow newRow;
      newRow = dt.NewRow();
      int selectedItem = cmb_graph.SelectedIndex;
      int totalRow = BgNo * FgNo;
      if (dataTable.Columns.Count == 64)
      {

        if (selectedItem == 0)
        {
          LBL_GraphText.Content = "Red is the Colour to be shown by the display, Blue is what the camera caputerd";
          pairs.Clear();
          for (int i = 0; i < FgNo; i++)
          {
            pairs.Add(new MeasurementPair()
            {
              // HERE 11 AND 12 ARE THE COLOUR CATPTURED BY THE CAMERA FOR DISPLAY 
              ColorToShow = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][6].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][7].ToString()) },
              ColorCaptured = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][11].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][12].ToString()) }
            });

            //newRow[i] = dataTable.Rows[i];
          }
          //dt.Rows.Add(newRow);
          //dtgrid_corrDisplay.ItemsSource = dt.DefaultView; 

        }

        else if (selectedItem == BgNo)
        {

          pairs.Clear();
          for (int i = totalRow + BgNo - 1; i < (totalRow + BgNo + 2); i++)
          {
            pairs.Add(new MeasurementPair()
            {
              BgColor = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][33].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][34].ToString()) }
            });

            //newRow[i] = dataTable.Rows[i];
          }
          //dt.Rows.Add(newRow);
          //dtgrid_corrDisplay.ItemsSource = dt.DefaultView; 

        }
        else
        {
          LBL_GraphText.Content = "Red is displayed colour captured with noBG condition, Blue is what the camera caputerd with the BG and Black is the BG:" + selectedItem;
          int startPoint = selectedItem * FgNo;
          int endPoint = (selectedItem * FgNo) + (FgNo);
          pairs.Clear();

          for (int i = selectedItem * FgNo; i < endPoint; i++)
          {

            pairs.Add(new MeasurementPair()
            {
              ColorToShow = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][11].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][12].ToString()) },
              ColorCaptured = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][33].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][34].ToString()) },
              BgColor = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][28].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][29].ToString()) }
            });

            //newRow[i] = dataTable.Rows[i];
          }

        }


      }

      if (dataTable.Columns.Count == 182 || dataTable.Columns.Count == 183)
      {

        if (selectedItem == 0)
        {
          LBL_GraphText.Content = "Red is the Colour to be shown by the display, Blue is what the camera caputerd";
          pairs.Clear();
          for (int i = 0; i < FgNo; i++)
          {
            pairs.Add(new MeasurementPair()
            {
              // HERE 11 AND 12 ARE THE COLOUR CATPTURED BY THE CAMERA FOR DISPLAY 
              ColorToShow = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][6].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][7].ToString()) },
              ColorCaptured = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][20].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][21].ToString()) }
            });

            //newRow[i] = dataTable.Rows[i];
          }
          //dt.Rows.Add(newRow);
          //dtgrid_corrDisplay.ItemsSource = dt.DefaultView; 

        }

        else if (selectedItem == BgNo)
        {

          pairs.Clear();
          for (int i = totalRow + BgNo - 1; i < (totalRow + BgNo + 2); i++)
          {
            pairs.Add(new MeasurementPair()
            {
              BgColor = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][76].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][77].ToString()) }
            });

            //newRow[i] = dataTable.Rows[i];
          }
          //dt.Rows.Add(newRow);
          //dtgrid_corrDisplay.ItemsSource = dt.DefaultView; 

        }
        else
        {
          LBL_GraphText.Content = "Red is displayed colour captured with noBG condition, Blue is what the camera caputerd with the BG and Black is the BG:" + selectedItem;
          int startPoint = selectedItem * FgNo;
          int endPoint = (selectedItem * FgNo) + (FgNo);
          pairs.Clear();

          for (int i = selectedItem * FgNo; i < endPoint; i++)
          {

            pairs.Add(new MeasurementPair()
            {
              ColorToShow = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][6].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][7].ToString()) },
              ColorCaptured = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][101].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][102].ToString()) },
              BgColor = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][76].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][77].ToString()) }
            });

            //newRow[i] = dataTable.Rows[i];
          }

        }

      }



    }

    private void dtgrid_corrDisplay_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (dtgridClick == 0)
      {

        if (dtgrid_corrDisplay.SelectedItem != null)
        {
          //gamut button data has 63 coloms in it 
          if (dataTable.Columns.Count == 64)
          {
            int i = dtgrid_corrDisplay.SelectedIndex;
            if (i != dataTable.Rows.Count)
            {



              rect_shown.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(Convert.ToByte(dataTable.Rows[i][0].ToString()), Convert.ToByte(dataTable.Rows[i][1].ToString()), Convert.ToByte((dataTable.Rows[i][2]).ToString())));

              rect_Seen.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(Convert.ToByte(dataTable.Rows[i][27].ToString()), Convert.ToByte(dataTable.Rows[i][28].ToString()), Convert.ToByte((dataTable.Rows[i][29]).ToString())));

              rect_Brad.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(Convert.ToByte(dataTable.Rows[i][45].ToString()), Convert.ToByte(dataTable.Rows[i][46].ToString()), Convert.ToByte((dataTable.Rows[i][47]).ToString())));

              rect_Von.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(Convert.ToByte(dataTable.Rows[i][48].ToString()), Convert.ToByte(dataTable.Rows[i][49].ToString()), Convert.ToByte((dataTable.Rows[i][50]).ToString())));

              rect_Scaling.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(Convert.ToByte(dataTable.Rows[i][51].ToString()), Convert.ToByte(dataTable.Rows[i][52].ToString()), Convert.ToByte((dataTable.Rows[i][53]).ToString())));



              pairs.Clear();
              pairs.Add(new MeasurementPair()
              {
                ColorToShow = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][6].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][7].ToString()) },
                ColorCaptured = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][11].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][12].ToString()) },
                BradColor = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][57].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][58].ToString()) },
                VonColor = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][59].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][60].ToString()) },
                Scalingcolor = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][61].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][62].ToString()) },
              });
            }
          }

          if (dataTable.Columns.Count == 182 || dataTable.Columns.Count == 183)
          {
            int i = dtgrid_corrDisplay.SelectedIndex;
            if (i != dataTable.Rows.Count)
            {

              rect_shown.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(Convert.ToByte(dataTable.Rows[i][0].ToString()), Convert.ToByte(dataTable.Rows[i][1].ToString()), Convert.ToByte((dataTable.Rows[i][2]).ToString())));

              rect_Seen.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(Convert.ToByte(dataTable.Rows[i][84].ToString()), Convert.ToByte(dataTable.Rows[i][85].ToString()), Convert.ToByte((dataTable.Rows[i][86]).ToString())));

              if (i > FgNo + 1)
              {
                rect_Seen.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(Convert.ToByte(dataTable.Rows[i][84].ToString()), Convert.ToByte(dataTable.Rows[i][85].ToString()), Convert.ToByte((dataTable.Rows[i][86]).ToString())));

                rect_Brad.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(Convert.ToByte(dataTable.Rows[i][137].ToString()), Convert.ToByte(dataTable.Rows[i][138].ToString()), Convert.ToByte((dataTable.Rows[i][139]).ToString())));

                rect_Von.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(Convert.ToByte(dataTable.Rows[i][151].ToString()), Convert.ToByte(dataTable.Rows[i][152].ToString()), Convert.ToByte((dataTable.Rows[i][153]).ToString())));

                rect_Scaling.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(Convert.ToByte(dataTable.Rows[i][165].ToString()), Convert.ToByte(dataTable.Rows[i][166].ToString()), Convert.ToByte((dataTable.Rows[i][167]).ToString())));

                rect_Acolor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(Convert.ToByte(dataTable.Rows[i][123].ToString()), Convert.ToByte(dataTable.Rows[i][124].ToString()), Convert.ToByte((dataTable.Rows[i][125]).ToString())));

                rect_bg.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(Convert.ToByte(dataTable.Rows[i][70].ToString()), Convert.ToByte(dataTable.Rows[i][71].ToString()), Convert.ToByte((dataTable.Rows[i][72]).ToString())));
              }

              double U, V;

              if (i < FgNo + 1)
              {
                U = Convert.ToDouble(dataTable.Rows[i][20].ToString());
                V = Convert.ToDouble(dataTable.Rows[i][21].ToString());
              }
              else
              {
                U = Convert.ToDouble(dataTable.Rows[i][101].ToString());
                V = Convert.ToDouble(dataTable.Rows[i][102].ToString());
              }

              pairs.Clear();
              pairs.Add(new MeasurementPair()
              {
                ColorToShow = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][6].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][7].ToString()) },
                ColorCaptured = new PerceptionLib.Color() { L = 0, UP = U, VP = V },

                Acolor = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][115].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][116].ToString()) },
                BgColor = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][76].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][77].ToString()) },
                BradColor = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][129].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][130].ToString()) },
                VonColor = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][143].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][144].ToString()) },
                Scalingcolor = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dataTable.Rows[i][157].ToString()), VP = Convert.ToDouble(dataTable.Rows[i][158].ToString()) }

              });

            }

          }

        }

      }

      else
      {
        int i = dtgrid_corrDisplay.SelectedIndex;
        rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(Convert.ToByte(dataTable.Rows[i][0].ToString()), Convert.ToByte(dataTable.Rows[i][1].ToString()), Convert.ToByte((dataTable.Rows[i][2]).ToString())));
      }
    }


   

   
 

   

  }


}


