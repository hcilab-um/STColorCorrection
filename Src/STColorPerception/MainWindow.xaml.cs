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
      
    // color class object
    private PerceptionLib.Color colorToShow;
    private PerceptionLib.Color colorMeasured;
    private PerceptionLib.Color colorDifference;
  
    private PerceptionLib.Color bgcolour;
    private PerceptionLib.Color mixedcolor;
    private MTObservableCollection<MeasurementPair> pairs;

   
    
    
      //input values
      //  rbg is input rgb which is displayed
      //  mr mg mb measured displyed colour
      //  bgr bgg bgb measured background color 
      //  mcr mcb mcg measured nacground colour
  
      private byte r, g, b, mr, mg, mb,bgr,bgg,bgb,mcr,mcg,mcb;
    int bgNo, fgNo;

    //EMGU CV objects
    private Image<Bgr, Byte> captureImage;
    private Image<Bgr, Byte> croppedImage;

    //Data Loader class obj
    DataTable dataTable = new DataTable();
     
    // this is a color object of BGR value ,.. uit  gives avg blue green and red value from cropped image.
    Bgr avgRGB;

    // obj for web cam capture
    private Capture captureDevice = new Capture();

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
       
     // captureDevice = new Capture();
        
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\color.txt");

      ColorToShow = new PerceptionLib.Color();
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
     if ("BgNo".Equals(e.PropertyName))
     {
         cmb_graph.Items.Clear();

         for (int i = 0; i <= bgNo+1; i++)
         {
             if(i==0)
             cmb_graph.Items.Add("No_background");
             else if(i< bgNo)
             cmb_graph.Items.Add("Background_No:"+i);
             else if (i < bgNo + 1)
             cmb_graph.Items.Add("White_Points");
             else
             cmb_graph.Items.Add("All_Data");
         }
     }
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

        rec_shown.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
        rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
        StartCapture();
        DisplayMeasuredValues();
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

  

    private void ColorCapturedUpdate()
    {
        // to measure LUV from Color class
        ColorToShow = Util.ColorSpaceConverter.ToGetLUV(R, G, B);

        rec_shown.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
        rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
       // Rectangle_Bg.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BgR, BgG, BgB));
    }
   

    private void StartCapture()
   // private void ColourcaputreWorker_DoWork(object sender, DoWorkEventArgs e)
    {
        // background processor
        //Imagecaputre = new BackgroundWorker();
        //System.Windows.Threading.Dispatcher captureNow = mainW.Dispatcher;
        // temp int cariables to do the calculations for AVG rgb from the cropped pics
        int tempMr = 0, tempMg = 0, tempMb = 0;
        for (int i = 0; i < 6; i++)
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
        for (int i = 0; i < 6; i++)
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
        croppedImage.ROI = new System.Drawing.Rectangle(Center_x, Center_y, 50, 50);
        avgRGB = croppedImage.GetAverage();

       
        //captureDevice.Dispose();

        //((IDisposable)captureDevice).Dispose();
        //((IDisposable)croppedImage).Dispose();
        ////((IDisposable)captureImage).Dispose();
    }


    ///// <summary>
    ///// funtions to capture Image via web cam , Crop it to its center , and get avg RGB value of its center
    ///// </summary>

    //private void GetImage()
    //{
    //    // make the camera wait 500 milli sec before it caprtures a img
    //    System.Threading.Thread.Sleep(500);

    //    // caputure device obj
    //    captureDevice = new Capture();
    //    // query frame catures web cam image in EMGU CV
    //    captureImage = captureDevice.QueryFrame();
    //    croppedImage = captureImage.Copy();

    //    if (captureImage != null)
    //    {

    //        Image_Camera.Source = Util.ToImageSourceConverter.ToBitmapSource(captureImage);
    //        // to dispose the query frame instance
    //        captureDevice.QueryFrame().Dispose();

    //    }
    //}

    ///// <summary>
    ///// function to crop the img to its center and get its avg RGB value
    ///// </summary>
    //private void CropImage()
    //{
    //    int Center_x, Center_y;
    //    Center_x = croppedImage.Width / 2;
    //    Center_y = croppedImage.Height / 2;
    //    avgRGB = new Bgr();

    //    croppedImage.ROI = new System.Drawing.Rectangle(Center_x, Center_y, 100, 100);
    //    Image_Croped.Source = Util.ToImageSourceConverter.ToBitmapSource(croppedImage);
    //    avgRGB = croppedImage.GetAverage();

    //    captureImage.Dispose();
    //    croppedImage.Dispose();
    //    //captureDevice.Dispose();

    //    //((IDisposable)captureDevice).Dispose();
    //    //((IDisposable)croppedImage).Dispose();
    //    ////((IDisposable)captureImage).Dispose();
    //}


    /// <summary>
    /// to display all measyre color's values 
    /// </summary>

    private void DisplayMeasuredValues()
    {
        ColorMeasured = Util.ColorSpaceConverter.ToGetLUV(MR, MG, MB);
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
    private void DifferenceCalculation()
    {
        ColorDifference = new PerceptionLib.Color();
        colorDifference.L = ColorToShow.L - ColorMeasured.L;
        colorDifference.U = ColorToShow.U - ColorMeasured.U;
        colorDifference.V = ColorToShow.V - ColorMeasured.V;
        colorDifference.UP = ColorToShow.UP - ColorMeasured.UP;
        colorDifference.VP = ColorToShow.VP - ColorMeasured.VP;
    }
  
    private void ColorUpdateOnScreenWithBG()
    {
        // to measure LUV from Color class
        ColorToShow = Util.ColorSpaceConverter.ToGetLUV(R, G, B);

        //rec_shown.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
        rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));

        rec_BgColor.Fill= new SolidColorBrush(System.Windows.Media.Color.FromRgb(BgR, BgG, BgB));
    }
    private void ColorCaptureJustBG()
    {
        // to measure LUV from Color class
        //ColorToShow = Util.ColorSpaceConverter.ToGetLUV(R, G, B);

        //rec_shown.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
        rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));

        rec_BgColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BgR, BgG, BgB));
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
         txt_BgNo.IsEnabled=false;
         txt_FgNo.IsEnabled=false;
         btn_MixedColour.IsEnabled=false;
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
                 switch(i)
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
         
         // btn_StartMeasurment.IsEnabled = false;
         txt_R.IsEnabled = false;
         txt_G.IsEnabled = false;
         txt_B.IsEnabled = false;
         DataTable dt = new DataTable();
         DataTable new_dt = new DataTable();

         dt = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
         ThreadPool.QueueUserWorkItem(ignored =>
         {

             //for (int i = 1; i < dt.Rows.Count; i++)
             for (int i = 0; i < dt.Rows.Count - 1; i++)
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
                 dt.Rows[i][13] = ColorDifference.L.ToString();
                 dt.Rows[i][14] = ColorDifference.U.ToString();
                 dt.Rows[i][15] = ColorDifference.V.ToString();

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

             for (int i = 0; i < dt.Rows.Count - 1; i++)
             {
                 pairs.Add(new MeasurementPair()
                 {
                     ColorToShow = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dt.Rows[i][6].ToString()), VP = Convert.ToDouble(dt.Rows[i][7].ToString()) },
                     ColorCaptured = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dt.Rows[i][11].ToString()), VP = Convert.ToDouble(dt.Rows[i][12].ToString()) }
                 });
             }
         });


        

     }

     private void cmb_graph_SelectionChanged(object sender, SelectionChangedEventArgs e)
     {
         DataTable dt = new DataTable();
         //dt=dataTable;
         DataRow newRow;
         newRow = dt.NewRow();
         int selectedItem = cmb_graph.SelectedIndex;
         int totalRow = BgNo * FgNo;

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

    
  }


 }
  

 