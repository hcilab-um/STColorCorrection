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
    private PerceptionLib.Color correctedcolor;
    private MTObservableCollection<MeasurementPair> pairs;

    //input values
    private byte r, g, b, mr, mg, mb;

    //EMGU CV objects
    private Image<Bgr, Byte> captureImage;
    private Image<Bgr, Byte> croppedImage;

    //Data Loader class obj
    //DataLoader dataTable = new DataLoader();
     
    // this is a color object of BGR value ,.. uit  gives avg blue green and red value from cropped image.
    Bgr avgRGB;

    // obj for web cam capture
    private Capture captureDevice;

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

    public PerceptionLib.Color Correctedcolor
    {
      get { return correctedcolor; }
      set
      {
        colorToShow = value;
        OnPropertyChanged("Correctedcolor");
      }
    }

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

    public MainWindow()
    {
      InitializeComponent();
      captureDevice = new Capture();

      PopulateGrid();

      ColorToShow = new PerceptionLib.Color();
      pairs = new MTObservableCollection<MeasurementPair>();
      cie1976C.DataContext = pairs;

      PropertyChanged += new PropertyChangedEventHandler(MainWindow_PropertyChanged);
    }

    private void PopulateGrid()
    {
        DataTable table = CSV.GetDataTableFromCSV(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\color.txt");
      if (table.Columns.Count == 0)
       System.Windows.MessageBox.Show("Error!");
      else
        dtgrid_corrDisplay.ItemsSource = table.DefaultView;
     
      dtgrid_corrDisplay.AutoGenerateColumns = true; 
      
    }


    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      //Btn_Load_File.IsEnabled = false;
      btn_StartMeasurment.IsEnabled = false;
     // btn_CheckMeasurment.IsEnabled = false;


   //   // initial xy plots to cross verify the graph's accuracy 
   ////   MTObservableCollection<MeasurementPair> pairs = new MTObservableCollection<MeasurementPair>();
   //   pairs.Add(new MeasurementPair()
   //   {
   //     ColorToShow = new PerceptionLib.Color() { L = 0, UP = 0, VP = 0 },
   //     ColorCaptured = new PerceptionLib.Color() { L = 0, UP = 0.1, VP = 0.2 }
   //   });


   //   pairs.Add(new MeasurementPair()
   //   {
   //     ColorToShow = new PerceptionLib.Color() { L = 0, UP = 0.6, VP = 0 },
   //     ColorCaptured = new PerceptionLib.Color() { L = 0, UP = 0.5, VP = 0.1 }
   //   });
   //   pairs.Add(new MeasurementPair()
   //   {
   //     ColorToShow = new PerceptionLib.Color() { L = 0, UP = 0, VP = 0.6 },
   //     ColorCaptured = new PerceptionLib.Color() { L = 0, UP = 0.1, VP = 0.5 }
   //   });
   //   pairs.Add(new MeasurementPair()
   //   {
   //     ColorToShow = new PerceptionLib.Color() { L = 0, UP = 0.6, VP = 0.6 },
   //     ColorCaptured = new PerceptionLib.Color() { L = 0, UP = 0.5, VP = 0.5 }
   //   });
   //   pairs.Add(new MeasurementPair());
   //   pairs.Add(new MeasurementPair() { ColorToShow = new PerceptionLib.Color() { L = 0, UP = 0.3, VP = 0.3 } });
   //   cie1976C.DataContext = pairs;
    }


    /// <summary>
    ///   // to change all values of shown color when each time RGB text box value chabges
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void MainWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
     if ("R".Equals(e.PropertyName) || "G".Equals(e.PropertyName) || "B".Equals(e.PropertyName))
      {
        btn_StartMeasurment.IsEnabled = true;

        // to measure LUV from Color class
        ColorToShow = Util.ColorSpaceConverter.ToGetLUV(R, G, B);

        rec_shown.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
        rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
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
      btn_CheckMeasurment.IsEnabled = true;
      startCapture();
    

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

  
    private void Btn_CheckMeasurment_Click(object sender, RoutedEventArgs e)
    {
        correctedcolor = new PerceptionLib.Color();
        correctedcolor.L = ColorDifference.L + ColorMeasured.L;
        correctedcolor.U = ColorDifference.U + ColorMeasured.U;
        correctedcolor.V = ColorDifference.V + ColorMeasured.V;
        correctedcolor.UP = 0;
        correctedcolor.VP = 0;


      PerceptionLib.RGBValue rgb = new PerceptionLib.RGBValue();


      rgb = PerceptionLib.Color.ToRBG(Correctedcolor);


      if (txt_R.Text.ToString() == rgb.R.ToString() && txt_G.Text.ToString() == rgb.G.ToString() && txt_B.Text.ToString() == rgb.B.ToString())
          System.Windows.MessageBox.Show("matched R:" + rgb.R + "G:" + rgb.G + "B:" + rgb.B);
      else
          System.Windows.MessageBox.Show("didn matchR:" + rgb.R + "G:" + rgb.G + "B:" + rgb.B);
            

    }

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

    private void Btn_UseGridData_Click(object sender, RoutedEventArgs e)
    {
        btn_StartMeasurment.IsEnabled = false;
        txt_R.IsEnabled = false;
        txt_G.IsEnabled = false;
        txt_B.IsEnabled = false;
        DataTable dt = new DataTable();
        DataTable new_dt = new DataTable();
                  
        dt = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
        //mainW.R = Convert.ToByte(dt.Rows[0][0].ToString());
        //mainW.G = Convert.ToByte(dt.Rows[0][1].ToString());
        //mainW.B = Convert.ToByte(dt.Rows[0][2].ToString());
        //ColorCapturedUpdate();
        //System.Windows.Forms.Application.DoEvents();

        
        //for (int i = 1; i < dt.Rows.Count; i++)
        for (int i = 0; i < dt.Rows.Count-1; i++)
        {
            mainW.R = Convert.ToByte(dt.Rows[i][0].ToString());
            mainW.G = Convert.ToByte(dt.Rows[i][1].ToString());
            mainW.B = Convert.ToByte(dt.Rows[i][2].ToString());

            // converts rendered RGB to luv and displays the colour in to measure

            //NoArgDelegate fetcher = new NoArgDelegate(
            //        this.ColorCapturedUpdate);
            

            //Dispatcher.BeginInvoke();

            
            if(i!=0)
            System.Threading.Thread.Sleep(500);
            ColorCapturedUpdate();
            System.Windows.Forms.Application.DoEvents();

            //does all the caputure and difference calculations
            //System.Threading.Thread.Sleep(500);
            startCapture();
            //System.Windows.Forms.Application.DoEvents();

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
            
           // System.Threading.Thread.Sleep(10000);
        }
        
        // grid is populated with new datatable which has luv values
        dtgrid_corrDisplay.ItemsSource = dt.DefaultView;
        
        // to show all the pairs in cie graph
        pairs.Clear();

        for (int i = 0; i < dt.Rows.Count-1; i++)
        {
            pairs.Add(new MeasurementPair()
            {
                ColorToShow = new PerceptionLib.Color() { L = 0, UP = Convert.ToDouble(dt.Rows[i][6].ToString()), VP = Convert.ToDouble(dt.Rows[i][7].ToString()) },
                ColorCaptured = new PerceptionLib.Color() { L = 0, UP =  Convert.ToDouble(dt.Rows[i][11].ToString()), VP =  Convert.ToDouble(dt.Rows[i][12].ToString()) }
            });
        }


        //dtgrid_corrDisplay.ItemsSource=
    }

    private void ColorCapturedUpdate()
    {
        // to measure LUV from Color class
        ColorToShow = Util.ColorSpaceConverter.ToGetLUV(R, G, B);

        rec_shown.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
        rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
    }



    private void startCapture()
    {
        // temp int cariables to do the calculations for AVG rgb from the cropped pics
        int tempMr = 0, tempMg = 0, tempMb = 0;
        for (int i = 0; i < 6; i++)
        {

            GetImage();
            CropImage();
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

        DisplayMeasuredValues();
        DifferenceCalculation();
    }

    /// <summary>
    /// funtions to capture Image via web cam , Crop it to its center , and get avg RGB value of its center
    /// </summary>

    private void GetImage()
    {
        // make the camera wait 500 milli sec before it caprtures a img
        System.Threading.Thread.Sleep(500);

        // caputure device obj
        captureDevice = new Capture();
        // query frame catures web cam image in EMGU CV
        captureImage = captureDevice.QueryFrame();
        croppedImage = captureImage.Copy();

        if (captureImage != null)
        {

            Image_Camera.Source = Util.ToImageSourceConverter.ToBitmapSource(captureImage);
            // to dispose the query frame instance
            captureDevice.QueryFrame().Dispose();

        }
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

        croppedImage.ROI = new System.Drawing.Rectangle(Center_x, Center_y, 100, 100);
        Image_Croped.Source = Util.ToImageSourceConverter.ToBitmapSource(croppedImage);
        avgRGB = croppedImage.GetAverage();

        captureImage.Dispose();
        croppedImage.Dispose();
        //// captureDevice.Dispose();

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
        //to display the color in the rectangle 

        Rectangle_Captured.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(MR, MG, MB));


        // to display the shift in the CIE 1976 graph
        //pairs.Clear();
        //pairs.Add(new MeasurementPair()
        //{
        //    ColorToShow = new PerceptionLib.Color() { L = 0, UP = colorToShow.UP, VP = colorToShow.VP },
        //    ColorCaptured = new PerceptionLib.Color() { L = 0, UP = colorMeasured.UP, VP = colorMeasured.VP }
        //});


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

   



  }
}
