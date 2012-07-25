using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PerceptionLib.Hacks;
using PerceptionLib;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;

namespace STColorPerception
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    // color class object
    PerceptionLib.Color colorObject;
    PerceptionLib.Color colorObject_M;

    //EMGU CV objects
    Image<Bgr, Byte> capture;
    Image<Bgr, Byte> Image_My;
    
    // obj for web cam capture
    private Capture _capture;

   
    
    public MainWindow()
    {
      InitializeComponent();
      _capture = new Capture();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {


      //Btn_Load_File.IsEnabled = false;

      Btn_StartMeasurment.IsEnabled = false;


      //MTObservableCollection<MeasurementPair> pairs = new MTObservableCollection<MeasurementPair>();
      //pairs.Add(new MeasurementPair()
      //{
      //  ColorToShow = new PerceptionLib.Color() { L = 0, UP = 0, VP = 0 },
      //  ColorCaptured = new PerceptionLib.Color() { L = 0, UP = 0.1, VP = 0.2 }
      //});


      //  pairs.Add(new MeasurementPair()
      //  {
      //    ColorToShow = new PerceptionLib.Color() { L = 0, UP = 0.6, VP = 0 },
      //    ColorCaptured = new PerceptionLib.Color() { L = 0, UP = 0.5, VP = 0.1 }
      //  });
      //  pairs.Add(new MeasurementPair()
      //  {
      //    ColorToShow = new PerceptionLib.Color() { L = 0, UP = 0, VP = 0.6 },
      //    ColorCaptured = new PerceptionLib.Color() { L = 0, UP = 0.1, VP = 0.5 }
      //  });
      //  pairs.Add(new MeasurementPair()
      //  {
      //    ColorToShow = new PerceptionLib.Color() { L = 0, UP = 0.6, VP = 0.6 },
      //    ColorCaptured = new PerceptionLib.Color() { L = 0, UP = 0.5, VP = 0.5 }
      //  });
      //pairs.Add(new MeasurementPair());
      //pairs.Add(new MeasurementPair() { ColorToShow = new PerceptionLib.Color() { L = 0, UP = 0.3, VP = 0.3 } });
      //cie1976C.DataContext = pairs;
      }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      Btn_StartMeasurment.IsEnabled = true;

    
      colorObject = new PerceptionLib.Color();
      System.Drawing.Color RGB = new System.Drawing.Color();

      int R, G, B;

      R = Convert.ToInt32(txt_R.Text);
      G = Convert.ToInt32(txt_G.Text);
      B = Convert.ToInt32(txt_B.Text);

      RGB = Util.ColorSpaceConverter.ToGetRGB(R, G, B);

      // to measure LUV from Color class
      colorObject = Util.ColorSpaceConverter.ToGetLUV(R, G, B);
      
     

      lbl_L.Content = colorObject.L.ToString();
      lbl_U.Content = colorObject.U.ToString();
      lbl_V.Content = colorObject.V.ToString();
      lbl_UP.Content = colorObject.UP.ToString();
      lbl_VP.Content = colorObject.VP.ToString();
      
      Rectangle_shown.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(RGB.R,RGB.G,RGB.B));
      Rectangle_DisplayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(RGB.R, RGB.G, RGB.B));

    }

    
    
    private void Btn_StartMeasurment_Click(object sender, RoutedEventArgs e)
    {
      GetImage();
      CropImage();
      MeasureRGB();

    }


    /// <summary>
    /// funtions to capture Image via web cam , Crop it to its center , and get avg RGB value of its center
    /// </summary>

    private void GetImage()
    {
      capture = _capture.QueryFrame();
      Image_My = capture.Copy();
      if (capture != null)
      {
        //My_image_copy = My_Image.ToBitmap();
        Image_Camera.Source = Util.ToImageSourceConverter.ToBitmapSource(capture);
        _capture.QueryFrame().Dispose();
      }
    }
    
    private void CropImage()
    {
      int Center_x, Center_y;
      Center_x = Image_My.Width / 2;
      Center_y = Image_My.Height / 2;

      Image_My.ROI = new System.Drawing.Rectangle(Center_x, Center_y, 100, 100);
      Image_Croped.Source = Util.ToImageSourceConverter.ToBitmapSource(Image_My);
    }
       
    private void MeasureRGB()
    {
      colorObject_M = new PerceptionLib.Color();
      System.Drawing.Color RGB = new System.Drawing.Color();

      int Avg_R, Avg_G, Avg_B;
      // this is a color object of BGR value ,.. uit gives blue green and red value.
      Bgr a = new Bgr();
      // fget avg give the avg color value of the image in its present ROI
      a = Image_My.GetAverage();

      Avg_B = Convert.ToInt32(a.Blue);
      Avg_G = Convert.ToInt32(a.Green);
      Avg_R = Convert.ToInt32(a.Red);

      RGB = Util.ColorSpaceConverter.ToGetRGB(Avg_R, Avg_G, Avg_B);
      colorObject_M = Util.ColorSpaceConverter.ToGetLUV(Avg_R, Avg_G, Avg_B);
    
      // to display the shift
      MTObservableCollection<MeasurementPair> pairs = new MTObservableCollection<MeasurementPair>();
      pairs.Add(new MeasurementPair()
      {
        ColorToShow = new PerceptionLib.Color() { L = 0, UP = colorObject.UP, VP = colorObject.VP },
        ColorCaptured = new PerceptionLib.Color() { L = 0, UP = colorObject_M.UP, VP = colorObject_M.VP }
      });
      cie1976C.DataContext = pairs;

      //to display the color in the rectangle 
      Rectangle_Captured.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(RGB.R, RGB.G, RGB.B));

      lbl_MR.Content = RGB.R.ToString();
      lbl_MG.Content = RGB.G.ToString();
      lbl_MB.Content = RGB.B.ToString();

      lbl_ML.Content = colorObject_M.L.ToString();
      lbl_MU.Content = colorObject_M.U.ToString();
      lbl_MV.Content = colorObject_M.V.ToString();
      lbl_MUP.Content = colorObject_M.UP.ToString();
      lbl_MVP.Content = colorObject_M.VP.ToString();

      
     
    }

    
  }
}
