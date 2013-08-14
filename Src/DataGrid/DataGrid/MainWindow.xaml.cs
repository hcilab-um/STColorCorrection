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
using System.Windows.Data;
using System.Data;
using System.Windows.Threading;
using PerceptionLib;
using System.Drawing;
using System.Diagnostics;

namespace DataGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PerceptionLib.CIEXYZ ColorToShowXYZ = new CIEXYZ(0, 0, 0);
            PerceptionLib.CIEXYZ BradXYZ = new CIEXYZ(0, 0, 0);

            ColorToShowXYZ.X = 0.003889613;
            ColorToShowXYZ.Y = 0.001555845;
            ColorToShowXYZ.Z = 0.020485292;


            ColorToShowXYZ = new PerceptionLib.CIEXYZ(0.041177778, 0.036633333, 0.020966667);
            PerceptionLib.CIEXYZ ColorToShowXYZ2 = new PerceptionLib.CIEXYZ(0.806205144, 0.775540661, 0.843191729);
            PerceptionLib.CIEXYZ ColorToShowXYZ3 = new PerceptionLib.CIEXYZ(0, 0, 0);

            ColorToShowXYZ3.X = ColorToShowXYZ.X + ColorToShowXYZ2.X;
            ColorToShowXYZ3.Y = ColorToShowXYZ.Y + ColorToShowXYZ2.Y;
            ColorToShowXYZ3.Z = ColorToShowXYZ.Z + ColorToShowXYZ2.Z;

            CIEXYZ screenBgWt = new CIEXYZ(0.724775, 0.759896, 0.727336);
            PerceptionLib.Color ColorToShow = PerceptionLib.Color.ToLAB(ColorToShowXYZ3, screenBgWt);

            BradXYZ = CATCalulation.bradford(ColorToShowXYZ);

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

        private void MathCal_Click(object sender, RoutedEventArgs e)
        {
            PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\Reduscer_for_phoneCAT.csv");
            DataTable bin = new DataTable();
            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
                dtgrid_corrDisplay.Items.Refresh();
                bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

            }));
            PerceptionLib.CIEXYZ ColorToShowXYZ = new CIEXYZ(0, 0, 0);
            PerceptionLib.CIEXYZ BradXYZ = new CIEXYZ(0, 0, 0);
            PerceptionLib.CIEXYZ VonXYZ = new CIEXYZ(0, 0, 0);
            PerceptionLib.CIEXYZ ScalingXYZ = new CIEXYZ(0, 0, 0);
            PerceptionLib.Color ColorToShow = new PerceptionLib.Color();

            foreach (DataRow dr in bin.Rows)
            {
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

                ColorToShowXYZ.X = Convert.ToDouble(dr["X"].ToString());
                ColorToShowXYZ.Y = Convert.ToDouble(dr["Y"].ToString());
                ColorToShowXYZ.Z = Convert.ToDouble(dr["Z"].ToString());

                BradXYZ = CATCalulation.bradford(ColorToShowXYZ);
                VonXYZ = CATCalulation.VonKries(ColorToShowXYZ);
                ScalingXYZ = CATCalulation.XYZScaling(ColorToShowXYZ);

                dr["bradX"] = BradXYZ.X.ToString();
                dr["bradY"] = BradXYZ.Y.ToString();
                dr["bradZ"] = BradXYZ.Z.ToString();

                dr["VonX"] = VonXYZ.X.ToString();
                dr["VonY"] = VonXYZ.Y.ToString();
                dr["VonZ"] = VonXYZ.Z.ToString();

                dr["ScalX"] = ScalingXYZ.X.ToString();
                dr["ScalY"] = ScalingXYZ.Y.ToString();
                dr["ScalZ"] = ScalingXYZ.Z.ToString();

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //ColorToShow.LA = Convert.ToDouble(dr["L"].ToString());
                //ColorToShow.A = Convert.ToDouble(dr["A"].ToString());
                //ColorToShow.B = Convert.ToDouble(dr["B"].ToString());
                //PerceptionLib.RGBValue ab = PerceptionLib.Color.ToRBGFromLAB(ColorToShow);

                ////Hex color
                //String Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
                //dr["HEX"] = "0x" + Hex;
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////

                //ColorToShowXYZ = new CIEXYZ(0, 0, 0);
                //ColorToShowXYZ.X = Convert.ToDouble(dr["X"].ToString());
                //ColorToShowXYZ.Y = Convert.ToDouble(dr["Y"].ToString());
                //ColorToShowXYZ.Z = Convert.ToDouble(dr["Z"].ToString());

                //ColorToShow = PerceptionLib.Color.ToLUV(ColorToShowXYZ);
                //PerceptionLib.RGBValue ab = PerceptionLib.Color.ToRBGFromLAB(ColorToShow);

                //dr["L"] = ColorToShow.LA.ToString();
                //dr["A"] = ColorToShow.A.ToString();
                //dr["LB"] = ColorToShow.B.ToString();
                //dr["R"] = ab.R.ToString();
                //dr["G"] = ab.G.ToString();
                //dr["B"] = ab.B.ToString();

                //String Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
                //dr["HEX"] = "0x" + Hex;

                //////////////////////////////////////////////////////////////////////////////////////////////////////////
                // hue calculation
                //ColorToShow.LA = Convert.ToDouble(dr["L"].ToString());
                //ColorToShow.A = Convert.ToDouble(dr["A"].ToString());
                //ColorToShow.B = Convert.ToDouble(dr["B"].ToString());
                //double hue= CATCalulation.HueAngle(ColorToShow);
                //dr["Hue"] = hue.ToString();



                Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = bin.DefaultView));
                Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

            }
            btn_ExportGrid.IsEnabled = true;
        }

        private void btn_ExportGrid_Click(object sender, RoutedEventArgs e)
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

        private void Phonedata_Click(object sender, RoutedEventArgs e)
        {
            dtgrid_corrDisplay.IsEnabled = true;


            //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\PerceptionLib\bin\previous data\cs-200 data\color mixing\phone\mixtureGroundtruth\bg6_HEX88.csv");
            //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\phone\bg\nobg_88Phone.csv");
            //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\phone\bincal\0pts\800_2.csv");
            //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\PerceptionLib\bin\previous data\cs-200 data\color mixing\1phone\4bg_600fg.csv");
            //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\PerceptionLib\bin\previous data\cs-200 data\color compensation\phone input\data to go in phone\New folder\23bg_td0.csv");
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

                byte Gt = 0;

                byte Bt = 0;


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


                //if (R >= 150 & G >= 150 & B >= 150)

                if (R <= 97 & G <= 98 & B <= 138)
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
                    //if (Rt >= 150 & Gt >= 150 & Bt >= 150)
                    // if (Rt < 10 & Gt <= 60 & Bt <= 89)
                    if (Rt <= 97 & Gt <= 98 & Bt <= 138)
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

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

            //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\Phone_Bin_wt0.csv");
            //binTable = new DataTable();
            //Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            //{
            //  dtgrid_corrDisplay.Items.Refresh();
            //  binTable = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

            //}));

            //for (int i = 1; i < bin.Rows.Count; i++)
            //{

            //  byte R = Convert.ToByte(bin.Rows[i - 1][0].ToString());
            //  byte G = Convert.ToByte(bin.Rows[i - 1][1].ToString());
            //  byte B = Convert.ToByte(bin.Rows[i - 1][2].ToString());

            //  byte r = Convert.ToByte(bin.Rows[i][0].ToString());
            //  byte g = Convert.ToByte(bin.Rows[i][1].ToString());
            //  byte b = Convert.ToByte(bin.Rows[i][2].ToString());

            //  if (R == 0 & G == 0 & B == 0)
            //  {
            //    if (r > 0 || g > 0 || b > 0)
            //    {
            //      byte r1 = Convert.ToByte(bin.Rows[i][0].ToString());
            //      byte g1 = Convert.ToByte(bin.Rows[i][1].ToString());
            //      byte b1 = Convert.ToByte(bin.Rows[i][2].ToString());
            //      double l1 = Convert.ToDouble(bin.Rows[i][3].ToString());
            //      double a1 = Convert.ToDouble(bin.Rows[i][4].ToString());
            //      double lb1 = Convert.ToDouble(bin.Rows[i][5].ToString());
            //      double x1 = Convert.ToDouble(bin.Rows[i][6].ToString());
            //      double y1 = Convert.ToDouble(bin.Rows[i][7].ToString());
            //      double z1 = Convert.ToDouble(bin.Rows[i][8].ToString());

            //      byte r2 = Convert.ToByte(bin.Rows[i + 1][0].ToString());
            //      byte g2 = Convert.ToByte(bin.Rows[i + 1][1].ToString());
            //      byte b2 = Convert.ToByte(bin.Rows[i + 1][2].ToString());
            //      double l2 = Convert.ToDouble(bin.Rows[i + 1][3].ToString());
            //      double a2 = Convert.ToDouble(bin.Rows[i + 1][4].ToString());
            //      double lb2 = Convert.ToDouble(bin.Rows[i + 1][5].ToString());
            //      double x2 = Convert.ToDouble(bin.Rows[i + 1][6].ToString());
            //      double y2 = Convert.ToDouble(bin.Rows[i + 1][7].ToString());
            //      double z2 = Convert.ToDouble(bin.Rows[i + 1][8].ToString());

            //      byte r3 = Convert.ToByte(bin.Rows[i + 2][0].ToString());
            //      byte g3 = Convert.ToByte(bin.Rows[i + 2][1].ToString());
            //      byte b3 = Convert.ToByte(bin.Rows[i + 2][2].ToString());
            //      double l3 = Convert.ToDouble(bin.Rows[i + 2][3].ToString());
            //      double a3 = Convert.ToDouble(bin.Rows[i + 2][4].ToString());
            //      double lb3 = Convert.ToDouble(bin.Rows[i + 2][5].ToString());
            //      double x3 = Convert.ToDouble(bin.Rows[i + 2][6].ToString());
            //      double y3 = Convert.ToDouble(bin.Rows[i + 2][7].ToString());
            //      double z3 = Convert.ToDouble(bin.Rows[i + 2][8].ToString());

            //      byte rFinal = Convert.ToByte((r1 + r2 + r3) / 3);
            //      byte gFinal = Convert.ToByte((g1 + g2 + g3) / 3);
            //      byte bFinal = Convert.ToByte((b1 + b2 + b3) / 3);
            //      double lFinal = (l1 + l2 + l3) / 3.0;
            //      double aFinal = (a1 + a2 + a3) / 3.0;
            //      double lbFinal = (lb1 + lb2 + lb3) / 3.0;
            //      double xFinal = (x1 + x2 + x3) / 3.0;
            //      double yFinal = (y1 + y2 + y3) / 3.0;
            //      double zFinal = (z1 + z2 + z3) / 3.0;

            //      newRow = binTable.NewRow();
            //      newRow[0] = r1.ToString();
            //      newRow[1] = g1.ToString();
            //      newRow[2] = b1.ToString();
            //      newRow[3] = l1.ToString();
            //      newRow[4] = a1.ToString();
            //      newRow[5] = lb1.ToString();
            //      newRow[6] = x1.ToString();
            //      newRow[7] = y1.ToString();
            //      newRow[8] = z1.ToString();



            //      //newRow = binTable.NewRow();
            //      //newRow[0] = bin.Rows[i][0].ToString();
            //      //newRow[1] = bin.Rows[i][1].ToString();
            //      //newRow[2] = bin.Rows[i][2].ToString();
            //      //newRow[3] = bin.Rows[i][3].ToString();
            //      //newRow[4] = bin.Rows[i][4].ToString();
            //      //newRow[5] = bin.Rows[i][5].ToString();
            //      //newRow[6] = bin.Rows[i][6].ToString();
            //      //newRow[7] = bin.Rows[i][7].ToString();
            //      //newRow[8] = bin.Rows[i][8].ToString();


            //      binTable.Rows.Add(newRow);
            //    }

            //  }
            //}
            //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = binTable.DefaultView));
            //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

            btn_ExportGrid.IsEnabled = true;
        }
        //cat
        private void CatCal_Click(object sender, RoutedEventArgs e)
        {
            // PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\small pro cat mixture\SmallPro800.csv");
            // PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\phone\PhonePredcition800_diff200.csv");
            //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\Reduscer_HEX.csv");
            PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\PerceptionLib\bin\previous data\cs-200 data\color mixing\phone data pewdiction data 800\Bg4.csv");
            DataTable bin = new DataTable();
            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
                dtgrid_corrDisplay.Items.Refresh();
                bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

            }));
            PerceptionLib.CIEXYZ ColorToShowXYZ = new CIEXYZ(0, 0, 0);
            PerceptionLib.CIEXYZ ColorMeasuredXYZ = new CIEXYZ(0, 0, 0);

            PerceptionLib.Color ColorMeasured_fordiff = new PerceptionLib.Color();



            PerceptionLib.CIEXYZ DBg = new CIEXYZ(0, 0, 0);
            PerceptionLib.CIEXYZ TDBg = new CIEXYZ(0, 0, 0);

            PerceptionLib.CIEXYZ AXYZ = new CIEXYZ(0, 0, 0);
            PerceptionLib.CIEXYZ DAXYZ = new CIEXYZ(0, 0, 0);
            PerceptionLib.CIEXYZ TDAXYZ = new CIEXYZ(0, 0, 0);

            PerceptionLib.CIEXYZ BinXYZ = new CIEXYZ(0, 0, 0);
            PerceptionLib.CIEXYZ TDBinXYZ = new CIEXYZ(0, 0, 0);
            PerceptionLib.CIEXYZ DBinXYZ = new CIEXYZ(0, 0, 0);


            PerceptionLib.CIEXYZ BradXYZ = new CIEXYZ(0, 0, 0);
            PerceptionLib.CIEXYZ VonXYZ = new CIEXYZ(0, 0, 0);
            PerceptionLib.CIEXYZ ScalXYZ = new CIEXYZ(0, 0, 0);

            PerceptionLib.CIEXYZ TDBradXYZ = new CIEXYZ(0, 0, 0);
            PerceptionLib.CIEXYZ TDVonXYZ = new CIEXYZ(0, 0, 0);
            PerceptionLib.CIEXYZ TDScalXYZ = new CIEXYZ(0, 0, 0);


            PerceptionLib.CIEXYZ DBradXYZ = new CIEXYZ(0, 0, 0);
            PerceptionLib.CIEXYZ DVonXYZ = new CIEXYZ(0, 0, 0);
            PerceptionLib.CIEXYZ DScalXYZ = new CIEXYZ(0, 0, 0);


            PerceptionLib.Color Acolor = new PerceptionLib.Color();
            PerceptionLib.Color TDAcolor = new PerceptionLib.Color();
            PerceptionLib.Color DAcolor = new PerceptionLib.Color();

            PerceptionLib.Color Bin = new PerceptionLib.Color();
            PerceptionLib.Color TDBin = new PerceptionLib.Color();
            PerceptionLib.Color DBin = new PerceptionLib.Color();

            PerceptionLib.Color Brad = new PerceptionLib.Color();
            PerceptionLib.Color Von = new PerceptionLib.Color();
            PerceptionLib.Color Scal = new PerceptionLib.Color();
            PerceptionLib.Color TDBrad = new PerceptionLib.Color();
            PerceptionLib.Color TDVon = new PerceptionLib.Color();
            PerceptionLib.Color TDScal = new PerceptionLib.Color();
            PerceptionLib.Color DBrad = new PerceptionLib.Color();
            PerceptionLib.Color DVon = new PerceptionLib.Color();
            PerceptionLib.Color DScal = new PerceptionLib.Color();
            PerceptionLib.RGBValue ab;

            /// small pro
            //CIEXYZ screenWt = new CIEXYZ(0.265572, 0.282182, 0.481033);

            //CIEXYZ screenBgWt = new CIEXYZ(0.9504, 0.990041, 1.0888);

            //phone
            CIEXYZ screenWt = new CIEXYZ(0.383264, 0.395001, 0.369982);

            CIEXYZ screenBgWt = new CIEXYZ(0.724775, 0.759896, 0.727336);

            //     ;
            //string Von      ;
            //string Scaling  ;
            //string TDBrad   ;
            //string TDVon    ;
            //string TDScaling;
            //string DBrad    ;
            //string DVon     ;
            //string DScaling ;

            PerceptionLib.Color ColorToShow = new PerceptionLib.Color();
            PerceptionLib.Color ColorMeasured = new PerceptionLib.Color();
            int i = 0;
            foreach (DataRow dr in bin.Rows)
            {
                //i++;

                DBg.X = Convert.ToDouble(dr["BX"].ToString());
                DBg.Y = Convert.ToDouble(dr["BY"].ToString());
                DBg.Z = Convert.ToDouble(dr["BZ"].ToString());

                TDBg.X = Convert.ToDouble(dr["BMX"].ToString());
                TDBg.Y = Convert.ToDouble(dr["BMY"].ToString());
                TDBg.Z = Convert.ToDouble(dr["BMZ"].ToString());

                //bin fg
                BinXYZ.X = Convert.ToDouble(dr["BinX"].ToString());
                BinXYZ.Y = Convert.ToDouble(dr["BinY"].ToString());
                BinXYZ.Z = Convert.ToDouble(dr["BinZ"].ToString());

                Bin = PerceptionLib.Color.ToLAB(BinXYZ, screenWt);
                dr["BP_Fg_L"] = Bin.LA.ToString();
                dr["BP_Fg_a"] = Bin.A.ToString();
                dr["BP_Fg_b"] = Bin.B.ToString();

                ab = PerceptionLib.Color.ToRBGFromLAB(Bin);
                String Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
                dr["BP_Fg_RRGGBB"] = "0x" + Hex;

                //pure


                AXYZ.X = Convert.ToDouble(dr["X"].ToString());
                AXYZ.Y = Convert.ToDouble(dr["Y"].ToString());
                AXYZ.Z = Convert.ToDouble(dr["Z"].ToString());

                Acolor.LA = Convert.ToDouble(dr["Fg_L"].ToString());
                Acolor.A = Convert.ToDouble(dr["Fg_A"].ToString());
                Acolor.B = Convert.ToDouble(dr["Fg_B"].ToString());

                ab = PerceptionLib.Color.ToRBGFromLAB(Acolor);
                Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
                dr["Fg_RRGGBB"] = "0x" + Hex;


                BradXYZ.X = Convert.ToDouble(dr["BradX"].ToString());
                BradXYZ.Y = Convert.ToDouble(dr["BradY"].ToString());
                BradXYZ.Z = Convert.ToDouble(dr["BradZ"].ToString());

                Brad = PerceptionLib.Color.ToLAB(BradXYZ, screenWt);
                dr["CAT1_Fg_L"] = Brad.LA.ToString();
                dr["CAT1_Fg_a"] = Brad.A.ToString();
                dr["CAT1_Fg_b"] = Brad.B.ToString();

                ab = PerceptionLib.Color.ToRBGFromLAB(Brad);
                Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
                dr["CAT1_Fg_RRGGBB"] = "0x" + Hex;

                VonXYZ.X = Convert.ToDouble(dr["VonX"].ToString());
                VonXYZ.Y = Convert.ToDouble(dr["VonY"].ToString());
                VonXYZ.Z = Convert.ToDouble(dr["VonZ"].ToString());

                Von = PerceptionLib.Color.ToLAB(VonXYZ, screenWt);
                dr["CAT2_Fg_L"] = Von.LA.ToString();
                dr["CAT2_Fg_a"] = Von.A.ToString();
                dr["CAT2_Fg_b"] = Von.B.ToString();
                ab = PerceptionLib.Color.ToRBGFromLAB(Von);
                Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
                dr["CAT2_Fg_RRGGBB"] = "0x" + Hex;

                ScalXYZ.X = Convert.ToDouble(dr["ScalX"].ToString());
                ScalXYZ.Y = Convert.ToDouble(dr["ScalY"].ToString());
                ScalXYZ.Z = Convert.ToDouble(dr["ScalZ"].ToString());

                Scal = PerceptionLib.Color.ToLAB(ScalXYZ, screenWt);
                dr["CAT3_Fg_L"] = Scal.LA.ToString();
                dr["CAT3_Fg_a"] = Scal.A.ToString();
                dr["CAT3_Fg_b"] = Scal.B.ToString();
                ab = PerceptionLib.Color.ToRBGFromLAB(Scal);
                Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
                dr["CAT3_Fg_RRGGBB"] = "0x" + Hex;

                // color measured
                ColorMeasuredXYZ.X = Convert.ToDouble(dr["MX"].ToString());
                ColorMeasuredXYZ.Y = Convert.ToDouble(dr["MY"].ToString());
                ColorMeasuredXYZ.Z = Convert.ToDouble(dr["MZ"].ToString());
                ColorMeasured = PerceptionLib.Color.ToLAB(ColorMeasuredXYZ, screenBgWt);
                ColorMeasured_fordiff = PerceptionLib.Color.ToLAB(ColorMeasuredXYZ, screenWt);

                dr["Bc_L"] = ColorMeasured.LA.ToString();
                dr["Bc_a"] = ColorMeasured.A.ToString();
                dr["Bc_b"] = ColorMeasured.B.ToString();

                ab = PerceptionLib.Color.ToRBGFromLAB(ColorMeasured);
                Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
                dr["Bc_RRGGBB"] = "0x" + Hex;


                //aCOLOR

                TDAXYZ.X = AXYZ.X + TDBg.X;
                TDAXYZ.Y = AXYZ.Y + TDBg.Y;
                TDAXYZ.Z = AXYZ.Z + TDBg.Z;

                TDAcolor = PerceptionLib.Color.ToLAB(TDAXYZ, screenBgWt);
                dr["P_Bc_TD_L"] = TDAcolor.LA.ToString();
                dr["P_Bc_TD_a"] = TDAcolor.A.ToString();
                dr["P_Bc_TD_b"] = TDAcolor.B.ToString();

                ab = PerceptionLib.Color.ToRBGFromLAB(TDAcolor);
                Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
                dr["P_Bc_TD_RRGGBB"] = "0x" + Hex;

                DAXYZ.X = AXYZ.X + DBg.X;
                DAXYZ.Y = AXYZ.Y + DBg.Y;
                DAXYZ.Z = AXYZ.Z + DBg.Z;

                DAcolor = PerceptionLib.Color.ToLAB(DAXYZ, screenBgWt);
                dr["P_Bc_ND_L"] = DAcolor.LA.ToString();
                dr["P_Bc_ND_a"] = DAcolor.A.ToString();
                dr["P_Bc_ND_b"] = DAcolor.B.ToString();

                ab = PerceptionLib.Color.ToRBGFromLAB(DAcolor);
                Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
                dr["P_Bc_ND_RRGGBB"] = "0x" + Hex;



                //brad

                TDBradXYZ.X = BradXYZ.X + TDBg.X;
                TDBradXYZ.Y = BradXYZ.Y + TDBg.Y;
                TDBradXYZ.Z = BradXYZ.Z + TDBg.Z;

                TDBrad = PerceptionLib.Color.ToLAB(TDBradXYZ, screenBgWt);
                dr["CAT1_P_Bc_TD_L"] = TDBrad.LA.ToString();
                dr["CAT1_P_Bc_TD_a"] = TDBrad.A.ToString();
                dr["CAT1_P_Bc_TD_b"] = TDBrad.B.ToString();
                ab = PerceptionLib.Color.ToRBGFromLAB(TDBrad);
                Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
                dr["CAT1_P_Bc_TD_RRGGBB"] = "0x" + Hex;


                DBradXYZ.X = BradXYZ.X + DBg.X;
                DBradXYZ.Y = BradXYZ.Y + DBg.Y;
                DBradXYZ.Z = BradXYZ.Z + DBg.Z;

                DBrad = PerceptionLib.Color.ToLAB(DBradXYZ, screenBgWt);
                dr["CAT1_P_Bc_ND_L"] = DBrad.LA.ToString();
                dr["CAT1_P_Bc_ND_a"] = DBrad.A.ToString();
                dr["CAT1_P_Bc_ND_b"] = DBrad.B.ToString();
                ab = PerceptionLib.Color.ToRBGFromLAB(DBrad);
                Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
                dr["CAT1_P_Bc_ND_RRGGBB"] = "0x" + Hex;


                //von
                TDVonXYZ.X = VonXYZ.X + TDBg.X;
                TDVonXYZ.Y = VonXYZ.Y + TDBg.Y;
                TDVonXYZ.Z = VonXYZ.Z + TDBg.Z;

                TDVon = PerceptionLib.Color.ToLAB(TDVonXYZ, screenBgWt);
                dr["CAT2_P_Bc_TD_L"] = TDVon.LA.ToString();
                dr["CAT2_P_Bc_TD_a"] = TDVon.A.ToString();
                dr["CAT2_P_Bc_TD_b"] = TDVon.B.ToString();
                ab = PerceptionLib.Color.ToRBGFromLAB(TDVon);
                Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
                dr["CAT2_P_Bc_TD_RRGGBB"] = "0x" + Hex;

                DVonXYZ.X = VonXYZ.X + DBg.X;
                DVonXYZ.Y = VonXYZ.Y + DBg.Y;
                DVonXYZ.Z = VonXYZ.Z + DBg.Z;

                DVon = PerceptionLib.Color.ToLAB(DVonXYZ, screenBgWt);
                dr["CAT2_P_Bc_ND_L"] = DVon.LA.ToString();
                dr["CAT2_P_Bc_ND_a"] = DVon.A.ToString();
                dr["CAT2_P_Bc_ND_b"] = DVon.B.ToString();
                ab = PerceptionLib.Color.ToRBGFromLAB(DVon);
                Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
                dr["CAT2_P_Bc_ND_RRGGBB"] = "0x" + Hex;

                // scal

                TDScalXYZ.X = ScalXYZ.X + TDBg.X;
                TDScalXYZ.Y = ScalXYZ.Y + TDBg.Y;
                TDScalXYZ.Z = ScalXYZ.Z + TDBg.Z;

                TDScal = PerceptionLib.Color.ToLAB(TDScalXYZ, screenBgWt);
                dr["CAT3_P_Bc_TD_L"] = TDScal.LA.ToString();
                dr["CAT3_P_Bc_TD_a"] = TDScal.A.ToString();
                dr["CAT3_P_Bc_TD_b"] = TDScal.B.ToString();
                ab = PerceptionLib.Color.ToRBGFromLAB(TDScal);
                Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
                dr["CAT3_P_Bc_TD_RRGGBB"] = "0x" + Hex;

                DScalXYZ.X = ScalXYZ.X + DBg.X;
                DScalXYZ.Y = ScalXYZ.Y + DBg.Y;
                DScalXYZ.Z = ScalXYZ.Z + DBg.Z;

                DScal = PerceptionLib.Color.ToLAB(DScalXYZ, screenBgWt);
                dr["CAT3_P_Bc_ND_L"] = DScal.LA.ToString();
                dr["CAT3_P_Bc_ND_a"] = DScal.A.ToString();
                dr["CAT3_P_Bc_ND_b"] = DScal.B.ToString();
                ab = PerceptionLib.Color.ToRBGFromLAB(DScal);
                Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
                dr["CAT3_P_Bc_ND_RRGGBB"] = "0x" + Hex;


                //bin
                TDBinXYZ.X = BinXYZ.X + TDBg.X;
                TDBinXYZ.Y = BinXYZ.Y + TDBg.Y;
                TDBinXYZ.Z = BinXYZ.Z + TDBg.Z;

                TDBin = PerceptionLib.Color.ToLAB(TDBinXYZ, screenBgWt);
                dr["BP_P_Bc_TD_L"] = TDBin.LA.ToString();
                dr["BP_P_Bc_TD_a"] = TDBin.A.ToString();
                dr["BP_P_Bc_TD_b"] = TDBin.B.ToString();
                ab = PerceptionLib.Color.ToRBGFromLAB(TDBin);
                Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
                dr["BP_P_Bc_TD_RRGGBB"] = "0x" + Hex;

                DBinXYZ.X = BinXYZ.X + DBg.X;
                DBinXYZ.Y = BinXYZ.Y + DBg.Y;
                DBinXYZ.Z = BinXYZ.Z + DBg.Z;

                DBin = PerceptionLib.Color.ToLAB(DBinXYZ, screenBgWt);
                dr["BP_P_Bc_ND_L"] = DBin.LA.ToString();
                dr["BP_P_Bc_ND_a"] = DBin.A.ToString();
                dr["BP_P_Bc_ND_b"] = DBin.B.ToString();
                ab = PerceptionLib.Color.ToRBGFromLAB(DBin);
                Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
                dr["BP_P_Bc_ND_RRGGBB"] = "0x" + Hex;


                dr["Dist_DM_P_TD"] = (PerceptionLib.Color.ColorDistanceCalAB(TDAcolor, ColorMeasured)).ToString();
                dr["Dist_DM_P_ND"] = (PerceptionLib.Color.ColorDistanceCalAB(DAcolor, ColorMeasured)).ToString();

                dr["Dist_CAT1_P_TD"] = (PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, TDBrad)).ToString();
                dr["Dist_CAT1_P_ND"] = (PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, DBrad)).ToString();

                dr["Dist_CAT2_P_TD"] = (PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, TDVon)).ToString();
                dr["Dist_CAT2_P_ND"] = (PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, DVon)).ToString();

                dr["Dist_CAT3_P_TD"] = (PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, TDScal)).ToString();
                dr["Dist_CAT3_P_ND"] = (PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, DScal)).ToString();


                dr["Dist_BP_P_TD"] = (PerceptionLib.Color.ColorDistanceCalAB(TDBin, ColorMeasured)).ToString();
                dr["Dist_BP_P_ND"] = (PerceptionLib.Color.ColorDistanceCalAB(DBin, ColorMeasured)).ToString();

                dr["Dist_Fg"] = (PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured_fordiff, Acolor)).ToString();
                dr["Dist_CAT1_Fg"] = (PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured_fordiff, Brad)).ToString();
                dr["Dist_CAT2_Fg"] = (PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured_fordiff, Von)).ToString();
                dr["Dist_CAT3_Fg"] = (PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured_fordiff, Scal)).ToString();
                dr["Dist_BP_Fg"] = (PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured_fordiff, Bin)).ToString();


            }


            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = bin.DefaultView));
            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
            btn_ExportGrid.IsEnabled = true;
        }

        private void Reduce_Click(object sender, RoutedEventArgs e)
        {
            //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\phone\phoneBin_full.csv");
            //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\Reduscer_outPut.csv");

            PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\phone\phoneBin_to_reduce.csv");
            DataTable bin = new DataTable();
            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
                dtgrid_corrDisplay.Items.Refresh();
                bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

            }));


            //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\phone\phoneBin_template.csv");
            // PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\Reduscer.csv");
            // DataTable binTable = new DataTable();
            // Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            // {
            //   dtgrid_corrDisplay.Items.Refresh();
            //   binTable = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

            // }));

            //// DataTable binTable = new DataTable();
            // //binTable = bin.Clone();
            // Random random = new Random();
            // for (int j = 0; j < bin.Rows.Count; j = j++)
            // {

            //   for (int i = 0; i < binTable.Rows.Count; i = i+200)
            //   {
            //     int bgno = Convert.ToInt32(binTable.Rows[i][0].ToString());
            //     int bgref = Convert.ToInt32(bin.Rows[j][3].ToString());
            //     if (bgno == bgref)
            //       binTable.ImportRow(bin.Rows[j]);
            //   }
            // }


            DataTable binTable = new DataTable();
            binTable = bin.Clone();
            Random random = new Random();

            for (int i = 0; i < 200; i++)
            {
                int j = random.Next(6, bin.Rows.Count);
                binTable.ImportRow(bin.Rows[j]);
            }

            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = binTable.DefaultView));
            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));


            btn_ExportGrid.IsEnabled = true;
        }

        private void FileConvertor_Click(object sender, RoutedEventArgs e)
        {

            //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\phone\phoneBin_full.csv");
            PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\Reduscer_BPND_cal.csv");
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
            int randomNumber;
            DataTable binTable = new DataTable();
            binTable = bin.Clone();

            int startPt = 0;
            int EndPt = 0;
            Random random = new Random();
            for (int bg = 1; bg < 24; bg++)
            {

                for (int i = startPt; i < bin.Rows.Count; i = i + 832)//417)
                {
                    if (i > 19273)//9612)
                    {
                        EndPt = i;
                        break;
                    }
                    while (bg > Convert.ToInt32(bin.Rows[i][0].ToString()))
                        i++;
                    if (Convert.ToInt32(bin.Rows[i][0].ToString()) == bg)
                        EndPt = i;
                    else
                        break;
                  
                }

                int index = 0;
                for (int k = 0; k < 200; k++)
                {
                    randomNumber = random.Next(startPt, EndPt);

                    binTable.ImportRow(bin.Rows[randomNumber]);


                    index++;
                }
                int p = index;
                startPt = EndPt;
                if (startPt > 19273)//9612)
                    break;
            }

            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = binTable.DefaultView));
            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));


            btn_ExportGrid.IsEnabled = true;

        }

        private void PhoneCompensation_Click(object sender, RoutedEventArgs e)
        {
            dtgrid_corrDisplay.IsEnabled = true;


            double X, Y, Z;

            int binnumber = 0;
            int GammutRangeCheck3, GammutRangeCheck4;

            PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\phone\BaseBinFile.csv");
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

            RGBValue BincolorRGB = new RGBValue();

            CIEXYZ BINCOLOR = new CIEXYZ(0, 0, 0);
            CIEXYZ BgND = new CIEXYZ(0, 0, 0);
            CIEXYZ BgTD = new CIEXYZ(0, 0, 0);
            CIEXYZ BP_TD = new CIEXYZ(0, 0, 0);
            CIEXYZ BP_ND = new CIEXYZ(0, 0, 0);

            CIEXYZ MBgND = new CIEXYZ(0, 0, 0);
            CIEXYZ MBgTD = new CIEXYZ(0, 0, 0);
            CIEXYZ MBP_TD = new CIEXYZ(0, 0, 0);
            CIEXYZ MBP_ND = new CIEXYZ(0, 0, 0);

            PerceptionLib.Color BINColor;
            PerceptionLib.Color BINTDColor;
            PerceptionLib.Color BINNDColor;

            PerceptionLib.Color MBINColor;
            PerceptionLib.Color MBINTDColor;
            PerceptionLib.Color MBINNDColor;


            //FgNo = 400;
            PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\PerceptionLib\bin\previous data\cs-200 data\color compensation\phone input\17bg_ip.csv");

            DataTable dt_DataCollection = new DataTable();
            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
                dtgrid_corrDisplay.Items.Refresh();
                dt_DataCollection = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
            }));

            {
                PerceptionLib.RGBValue rgb = new PerceptionLib.RGBValue();

                foreach (DataRow dr in dt_DataCollection.Rows)
                {


                    X = Convert.ToDouble(dr["BGX"].ToString());
                    Y = Convert.ToDouble(dr["BGY"].ToString());
                    Z = Convert.ToDouble(dr["BGZ"].ToString());

                    BgND = new CIEXYZ(X, Y, Z);

                    X = Convert.ToDouble(dr["BGsX"].ToString());
                    Y = Convert.ToDouble(dr["BGsY"].ToString());
                    Z = Convert.ToDouble(dr["BGsZ"].ToString());

                    BgTD = new CIEXYZ(X, Y, Z);

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    //bin model
                    X = Convert.ToDouble(dr["Mx"].ToString());
                    Y = Convert.ToDouble(dr["My"].ToString());
                    Z = Convert.ToDouble(dr["Mz"].ToString());
                    BINCOLOR = new CIEXYZ(X, Y, Z);
                    BincolorRGB = new RGBValue();
                    BINColor = new PerceptionLib.Color();

                    BINColor.LA = Convert.ToDouble(dr["Ml"].ToString());
                    BINColor.A = Convert.ToDouble(dr["Ma"].ToString());
                    BINColor.B = Convert.ToDouble(dr["Mlb"].ToString());

                    MBP_ND = ColorSpaceConverter.SubtractXYZ(BINCOLOR, BgND);

                    MBINNDColor = PerceptionLib.Color.ToLUV(MBP_ND);

                    dr["ComS_Fg_L"] = MBINNDColor.LA.ToString();
                    dr["ComS_Fg_a"] = MBINNDColor.A.ToString();
                    dr["ComS_Fg_b"] = MBINNDColor.B.ToString();
                    GammutRangeCheck3 = 1;
                    if ((MBINNDColor.LA >= minLLevel) & (MBINNDColor.LA <= maxLLevel) & (MBINNDColor.A >= minALevel) & (MBINNDColor.A <= maxALevel) & (MBINNDColor.B >= minBLevel) & (MBINNDColor.B <= maxBLevel))
                    {
                        for (int index = 0; index < bin.Rows.Count; index++)
                        {
                            double tempL, tempA, tempB;
                            tempL = Convert.ToDouble(bin.Rows[index][9].ToString());
                            tempA = Convert.ToDouble(bin.Rows[index][10].ToString());
                            tempB = Convert.ToDouble(bin.Rows[index][11].ToString());

                            if ((MBINNDColor.LA >= tempL - 5) & (MBINNDColor.LA <= tempL + 5) & (MBINNDColor.A >= tempA - 5) & (MBINNDColor.A <= tempA + 5) & (MBINNDColor.B >= tempB - 5) & (MBINNDColor.B <= tempB + 5))
                            {
                                GammutRangeCheck3 = 0;
                                break;
                            }
                            GammutRangeCheck3 = 1;
                        }
                    }
                    else
                        GammutRangeCheck3 = 1;

                    binnumber = CATCalulation.MatchWithBinnedColors(MBINNDColor, bin, BgND, BINColor);

                    BINNDColor = new PerceptionLib.Color();
                    BINNDColor.LA = Convert.ToDouble(bin.Rows[binnumber][3].ToString());
                    BINNDColor.A = Convert.ToDouble(bin.Rows[binnumber][4].ToString());
                    BINNDColor.B = Convert.ToDouble(bin.Rows[binnumber][5].ToString());

                    dr["Com_Fg_L"] = BINNDColor.LA.ToString();
                    dr["Com_Fg_a"] = BINNDColor.A.ToString();
                    dr["Com_Fg_b"] = BINNDColor.B.ToString();

                    BincolorRGB = PerceptionLib.Color.ToRBGFromLAB(BINNDColor);
                    string Hex = PerceptionLib.Color.RGBtoHEX(BincolorRGB.R, BincolorRGB.G, BincolorRGB.B);
                    dr["Com_HEX"] = "0x" + Hex;




                    dr["GammutFlag"] = GammutRangeCheck3.ToString();

                    MBP_TD = ColorSpaceConverter.SubtractXYZ(BINCOLOR, BgTD);

                    MBINTDColor = PerceptionLib.Color.ToLUV(MBP_TD);
                    dr["TDComS_Fg_L"] = MBINNDColor.LA.ToString();
                    dr["TDComS_Fg_a"] = MBINNDColor.A.ToString();
                    dr["TDComS_Fg_b"] = MBINNDColor.B.ToString();
                    GammutRangeCheck4 = 1;
                    if ((MBINTDColor.LA >= minLLevel) & (MBINTDColor.LA <= maxLLevel) & (MBINTDColor.A >= minALevel) & (MBINTDColor.A <= maxALevel) & (MBINTDColor.B >= minBLevel) & (MBINTDColor.B <= maxBLevel))
                    {
                        for (int index = 0; index < bin.Rows.Count; index++)
                        {
                            double tempL, tempA, tempB;
                            tempL = Convert.ToDouble(bin.Rows[index][9].ToString());
                            tempA = Convert.ToDouble(bin.Rows[index][10].ToString());
                            tempB = Convert.ToDouble(bin.Rows[index][11].ToString());

                            if ((MBINTDColor.LA >= tempL - 5) & (MBINTDColor.LA <= tempL + 5) & (MBINTDColor.A >= tempA - 5) & (MBINTDColor.A <= tempA + 5) & (MBINTDColor.B >= tempB - 5) & (MBINTDColor.B <= tempB + 5))
                            {
                                GammutRangeCheck4 = 0;
                                break;
                            }
                            GammutRangeCheck4 = 1;
                        }
                    }
                    else
                        GammutRangeCheck4 = 1;

                    binnumber = CATCalulation.MatchWithBinnedColors(MBINTDColor, bin, BgND, BINColor);
                    BINTDColor = new PerceptionLib.Color();

                    BINTDColor.LA = Convert.ToDouble(bin.Rows[binnumber][3].ToString());
                    BINTDColor.A = Convert.ToDouble(bin.Rows[binnumber][4].ToString());
                    BINTDColor.B = Convert.ToDouble(bin.Rows[binnumber][5].ToString());

                    dr["TDCom_Fg_L"] = BINTDColor.LA.ToString();
                    dr["TDCom_Fg_a"] = BINTDColor.A.ToString();
                    dr["TDCom_Fg_b"] = BINTDColor.B.ToString();

                    BincolorRGB = PerceptionLib.Color.ToRBGFromLAB(BINTDColor);
                    Hex = PerceptionLib.Color.RGBtoHEX(BincolorRGB.R, BincolorRGB.G, BincolorRGB.B);
                    dr["TDCom_HEX"] = "0x" + Hex;


                    dr["TDGammutFlag"] = GammutRangeCheck4.ToString();
                }


            }
            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = dt_DataCollection.DefaultView));
            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));


            btn_ExportGrid.IsEnabled = true;

        }

        private void HueDifference_Click(object sender, RoutedEventArgs e)
        {
            PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\Reduscer_angles.csv");
            DataTable bin = new DataTable();
            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
                dtgrid_corrDisplay.Items.Refresh();
                bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

            }));

            double m, l, p, aa, am, al, ap;

            PerceptionLib.Color Acolor = new PerceptionLib.Color();
            PerceptionLib.Color Mcolor = new PerceptionLib.Color();
            PerceptionLib.Color Lcolor = new PerceptionLib.Color();
            PerceptionLib.Color Pcolor = new PerceptionLib.Color();

            foreach (DataRow dr in bin.Rows)
            {
                //i++;

                Acolor.LA = Convert.ToDouble(dr["AL"].ToString());
                Acolor.A = Convert.ToDouble(dr["AA"].ToString());
                Acolor.B = Convert.ToDouble(dr["AB"].ToString());

                Mcolor.LA = Convert.ToDouble(dr["ML"].ToString());
                Mcolor.A = Convert.ToDouble(dr["MA"].ToString());
                Mcolor.B = Convert.ToDouble(dr["MB"].ToString());

                Lcolor.LA = Convert.ToDouble(dr["LL"].ToString());
                Lcolor.A = Convert.ToDouble(dr["LA"].ToString());
                Lcolor.B = Convert.ToDouble(dr["LB"].ToString());

                Pcolor.LA = Convert.ToDouble(dr["PL"].ToString());
                Pcolor.A = Convert.ToDouble(dr["PA"].ToString());
                Pcolor.B = Convert.ToDouble(dr["PB"].ToString());


                m = CATCalulation.hueDifference(Mcolor, Acolor);
                l = CATCalulation.hueDifference(Mcolor, Lcolor);
                p = CATCalulation.hueDifference(Mcolor, Pcolor);

                aa = CATCalulation.HueAngle(Acolor);
                am = CATCalulation.HueAngle(Mcolor);
                al = CATCalulation.HueAngle(Lcolor);
                ap = CATCalulation.HueAngle(Pcolor);

                dr["m"] = m.ToString();
                dr["l"] = l.ToString();
                dr["p"] = p.ToString();
                dr["aacc"] = aa.ToString();
                dr["am"] = am.ToString();
                dr["alum"] = al.ToString();
                dr["ap"] = ap.ToString();



            }
            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = bin.DefaultView));
            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
            btn_ExportGrid.IsEnabled = true;

        }

        private void EqDistance_Click(object sender, RoutedEventArgs e)
        {

            // PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\bigProjecor\BaseBinFile.csv");
            PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Data\Phone_Prediction_WhitePointsApplied_v2.csv");
            DataTable bin = new DataTable();
            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
                dtgrid_corrDisplay.Items.Refresh();
                bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

            }));

            PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\Predicition_Anova.txt");
            DataTable binTable = new DataTable();
            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
                dtgrid_corrDisplay.Items.Refresh();
                binTable = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

            }));

            DataRow newRow;
            int index = 0;

            for (int i = 0; i < bin.Rows.Count; i++)
            {
                newRow = binTable.NewRow();
                newRow[0] = "Phone";
                newRow[1] = "DirectModel";
                newRow[2] = "Behind";
                newRow[3] = bin.Rows[i][76];
                newRow[4] = index.ToString();
                binTable.Rows.Add(newRow);

                newRow = binTable.NewRow();
                newRow[0] = "Phone";
                newRow[1] = "DirectModel";
                newRow[2] = "front";
                newRow[3] = bin.Rows[i][77];
                newRow[4] = index.ToString();
                binTable.Rows.Add(newRow);

                newRow = binTable.NewRow();
                newRow[0] = "Phone";
                newRow[1] = "BinModel";
                newRow[2] = "Behind";
                newRow[3] = bin.Rows[i][84];
                newRow[4] = index.ToString();
                binTable.Rows.Add(newRow);

                newRow = binTable.NewRow();
                newRow[0] = "Phone";
                newRow[1] = "BinModel";
                newRow[2] = "front";
                newRow[3] = bin.Rows[i][85];
                newRow[4] = index.ToString();
                binTable.Rows.Add(newRow);
                index++;
            }
            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = binTable.DefaultView));
            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

            //CIEXYZ bg = new CIEXYZ(0,0,0);
            ////Acolor.LA = 10.10715853;
            ////Acolor.A = 0.053429388;
            ////Acolor.B = -10.79067387;

            //Acolor.LA = 100;
            //Acolor.A = 0;
            //Acolor.B = 0;

            //Stopwatch stopwatch = new Stopwatch();
            //Dispatcher.Invoke((Action) delegate()
            //{
            //  stopwatch.Start();
            //  CATCalulation.MatchWithBinnedColors_noSubration(bin, bg, Acolor);
            //  stopwatch.Stop();
            //}, DispatcherPriority.Render);

            //Console.WriteLine(stopwatch.ElapsedMilliseconds);

            //for (int i = 0; i < bin.Rows.Count; i++)
            //{
            //  Acolor.LA = Convert.ToDouble(bin.Rows[i][3].ToString());
            //  Acolor.A = Convert.ToDouble(bin.Rows[i][4].ToString());
            //  Acolor.B = Convert.ToDouble(bin.Rows[i][5].ToString());

            //  PerceptionLib.ColorRegion cr = PerceptionLib.Color.ToFindColorRegion(Acolor);
            //  if (cr.NetralValueFlag == 0)
            //    binTable.ImportRow(bin.Rows[i]);
            // Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = binTable.DefaultView));
            //  Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));


            //}


            //foreach (DataRow dr in bin.Rows)
            //{
            //  Acolor.LA = Convert.ToDouble(dr["Fg_L"].ToString());
            //  Acolor.A = Convert.ToDouble(dr["Fg_A"].ToString());
            //  Acolor.B = Convert.ToDouble(dr["Fg_B"].ToString());

            //  CDcolor.LA = Convert.ToDouble(dr["CD_Fg_L"].ToString());
            //  CDcolor.A = Convert.ToDouble(dr["CD_Fg_A"].ToString());
            //  CDcolor.B = Convert.ToDouble(dr["CD_Fg_B"].ToString());

            //  Mcolor.LA = Convert.ToDouble(dr["Measured_L"].ToString());
            //  Mcolor.A = Convert.ToDouble(dr["Measured_A"].ToString());
            //  Mcolor.B = Convert.ToDouble(dr["Measured_B"].ToString());

            //  double bgL = Convert.ToDouble(dr["Bg_L"].ToString());

            //  PerceptionLib.ColorRegion cr = PerceptionLib.Color.ToFindColorRegion(Acolor);

            //  if(bgL<50)
            //  dr["BGLValue"] = "0";
            //  else
            //    dr["BGLValue"] = "1";


            //  dr["FGLValue"] = cr.LValueFlag.ToString();
            //  dr["NuetralValue"] = cr.NetralValueFlag.ToString();
            //  dr["Region"] = cr.RegionValue.ToString();

            //  dr["Distance"] = (PerceptionLib.Color.ColorDistanceCalAB(CDcolor, Mcolor)).ToString();

            //}

            //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = bin.DefaultView));
            //Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
            btn_ExportGrid.IsEnabled = true;

        }

        private void BgCal_Click(object sender, RoutedEventArgs e)
        {

            PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\DataGrid\DataGrid\bin\value\Predicted_Bg_input.csv");
            DataTable bin = new DataTable();
            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
                dtgrid_corrDisplay.Items.Refresh();
                bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

            }));

            double OL, OA, OB, px, py, pz, LL, LA, LB;

            PerceptionLib.Color BgColor = new PerceptionLib.Color();
            PerceptionLib.Color LBgColor = new PerceptionLib.Color();


            foreach (DataRow dr in bin.Rows)
            {
                OL = Convert.ToDouble(dr["BGL"].ToString());
                OA = Convert.ToDouble(dr["BGA"].ToString());
                OB = Convert.ToDouble(dr["BGB"].ToString());

                LL = Convert.ToDouble(dr["LumistyL"].ToString());
                LA = Convert.ToDouble(dr["LumistyA"].ToString());
                LB = Convert.ToDouble(dr["LumistyB"].ToString());

                px = Convert.ToDouble(dr["PX"].ToString());
                py = Convert.ToDouble(dr["PY"].ToString());
                pz = Convert.ToDouble(dr["PZ"].ToString());

                PerceptionLib.CIEXYZ xyz = new CIEXYZ(px, py, pz);

                PerceptionLib.Color PLBgColor = PerceptionLib.Color.ToLAB(xyz);

                BgColor.LA = OL;
                BgColor.A = OA;
                BgColor.B = OB;


                LBgColor.LA = LL;
                LBgColor.A = LA;
                LBgColor.B = LB;

                PerceptionLib.ColorRegion cr = PerceptionLib.Color.ToFindColorRegion(BgColor);

                dr["BGLValue"] = cr.LValueFlag.ToString();
                dr["SaturationValue"] = cr.NetralValueFlag.ToString();
                dr["Region"] = cr.RegionValue.ToString();
                dr["PXYZ_EqDistance"] = (PerceptionLib.Color.ColorDistanceCalAB(PLBgColor, LBgColor)).ToString();
            }
            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = bin.DefaultView));
            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
            btn_ExportGrid.IsEnabled = true;


        }

        /// <summary>
        /// data structure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DSCal_Click(object sender, RoutedEventArgs e)
        {
          //data structure for the whole LAB space  
          PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\DataGrid\DataGrid\data\DS.csv");
            DataTable labDS = new DataTable();
            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
                dtgrid_corrDisplay.Items.Refresh();
                labDS = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
            }));

          // our bin data
            PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\DataGrid\DataGrid\data\binnedData_bigProjector.csv");
            DataTable bin = new DataTable();
            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
              dtgrid_corrDisplay.Items.Refresh();
              bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
            }));

          // the template to fit in all the lab space
            PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\DataGrid\DataGrid\data\Template.csv");
            DataTable template = new DataTable();
            Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            {
              dtgrid_corrDisplay.Items.Refresh();
              template = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
            }));
          
            DataRow newRow;
            double l, a, b, lx, ax, bx, sl, sa, sb, distance=0;
            int LABSpaceNo=0;
            for (int i = 0; i < bin.Rows.Count; i++)
            {
              l = Convert.ToDouble(bin.Rows[i][0].ToString());
              a = Convert.ToDouble(bin.Rows[i][1].ToString());
              b = Convert.ToDouble(bin.Rows[i][2].ToString());

              double closestColorValue = double.MaxValue;
              for (int j = 0; j < labDS.Rows.Count; j++)
              {

                lx = Convert.ToDouble(labDS.Rows[j][0].ToString());
                ax = Convert.ToDouble(labDS.Rows[j][1].ToString());
                bx = Convert.ToDouble(labDS.Rows[j][2].ToString());

                sl = l - lx;
                sa = a - ax;
                sb = b - bx;

                sl = sl * sl;
                sa = sa * sa;
                sb = sb * sb;

                double result = sl + sa + sb;
                distance = Math.Sqrt(result);

                if (distance > closestColorValue)
                {
                  continue;
                }

                closestColorValue = distance;
                LABSpaceNo = j;
              }
              newRow = template.NewRow();
              newRow[0] = i.ToString(); //here i is bin index
              newRow[1] = LABSpaceNo.ToString();
              newRow[2] = closestColorValue.ToString();
              newRow[3] = bin.Rows[i][0].ToString();
              newRow[4] = bin.Rows[i][1].ToString();
              newRow[5] = bin.Rows[i][2].ToString();
              newRow[6] = labDS.Rows[LABSpaceNo][0].ToString();
              newRow[7] = labDS.Rows[LABSpaceNo][1].ToString();
              newRow[8] = labDS.Rows[LABSpaceNo][2].ToString();

              template.Rows.Add(newRow);
            }

            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = template.DefaultView));
            Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
            btn_ExportGrid.IsEnabled = true;

        }

        private void DsMAtching_Click(object sender, RoutedEventArgs e)
        {
          PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\DataGrid\DataGrid\data\DS.csv");
          DataTable labDS = new DataTable();
          Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
          {
            dtgrid_corrDisplay.Items.Refresh();
            labDS = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
          }));

          // our bin data
          PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\DataGrid\DataGrid\data\binnedData_bigProjector.csv");
          DataTable bin = new DataTable();
          Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
          {
            dtgrid_corrDisplay.Items.Refresh();
            bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
          }));

          // the template to fit in all the lab space
          PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\DataGrid\DataGrid\data\DSVSBIN_ORIGINAL.csv");
          DataTable DSvsBin = new DataTable();
          Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
          {
            dtgrid_corrDisplay.Items.Refresh();
            DSvsBin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
          }));

          PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\DataGrid\DataGrid\data\Template.csv");
          DataTable template = new DataTable();
          Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
          {
            dtgrid_corrDisplay.Items.Refresh();
            template = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
          }));
           
          DataRow newRow;
          int Dsno, BinNo;
          double Distance;
          int reboundIndex=1;
          int reboundTimes = 11;
          for (int j = 0; j < DSvsBin.Rows.Count; j++)
          {
            BinNo=Convert.ToInt32(DSvsBin.Rows[j][0].ToString());
            Dsno = Convert.ToInt32(DSvsBin.Rows[j][1].ToString());
            Distance = Convert.ToDouble(DSvsBin.Rows[j][2].ToString());
            if (labDS.Rows[Dsno][8].ToString() == "-1")
            {
              labDS.Rows[Dsno][8] = BinNo.ToString();
              labDS.Rows[Dsno][9] = bin.Rows[BinNo][0];//lab
              labDS.Rows[Dsno][10] = bin.Rows[BinNo][1];
              labDS.Rows[Dsno][11] = bin.Rows[BinNo][2];
              labDS.Rows[Dsno][12] = bin.Rows[BinNo][3];//xyz
              labDS.Rows[Dsno][13] = bin.Rows[BinNo][4];
              labDS.Rows[Dsno][14] = bin.Rows[BinNo][5];
              labDS.Rows[Dsno][15] = bin.Rows[BinNo][6];//rgb
              labDS.Rows[Dsno][16] = bin.Rows[BinNo][7];
              labDS.Rows[Dsno][17] = bin.Rows[BinNo][8];
              labDS.Rows[Dsno][18] = Distance.ToString();//distance
              labDS.Rows[Dsno][7] = 1;//distance
              reboundIndex = 1;
            }
            else
            {
              newRow = labDS.NewRow();
              reboundIndex = Convert.ToInt32(labDS.Rows[Dsno][7].ToString());
              //int addFactor = reboundIndex * reboundTimes;
              newRow[0] = labDS.Rows[Dsno][0];
              newRow[1] = labDS.Rows[Dsno][1];
              newRow[2] = labDS.Rows[Dsno][2];
              newRow[3] = labDS.Rows[Dsno][3];
              newRow[4] = labDS.Rows[Dsno][4];
              newRow[5] = labDS.Rows[Dsno][5];
              newRow[6] = labDS.Rows[Dsno][6];
              reboundIndex++;
              newRow[7] = reboundIndex.ToString();//distance
              labDS.Rows[Dsno][7] = reboundIndex.ToString();
              newRow[8] = BinNo.ToString();
              newRow[9] = bin.Rows[BinNo][3];//lab
              newRow[10] = bin.Rows[BinNo][4];
              newRow[11] = bin.Rows[BinNo][5];
              newRow[12] = bin.Rows[BinNo][6];//xyz
              newRow[13] = bin.Rows[BinNo][7];
              newRow[14] = bin.Rows[BinNo][8];
              newRow[15] = bin.Rows[BinNo][9];//rgb
              newRow[16] = bin.Rows[BinNo][10];
              newRow[17] = bin.Rows[BinNo][11];
              newRow[18] = Distance.ToString();//distance
              labDS.Rows.Add(newRow);
              //labDS.Rows[Dsno][8 + addFactor] = BinNo.ToString();
              //labDS.Rows[Dsno][9 + addFactor] = bin.Rows[BinNo][0];//lab
              //labDS.Rows[Dsno][10 + addFactor] = bin.Rows[BinNo][1];
              //labDS.Rows[Dsno][11 + addFactor] = bin.Rows[BinNo][2];
              //labDS.Rows[Dsno][12 + addFactor] = bin.Rows[BinNo][3];//xyz
              //labDS.Rows[Dsno][13 + addFactor] = bin.Rows[BinNo][4];
              //labDS.Rows[Dsno][14 + addFactor] = bin.Rows[BinNo][5];
              //labDS.Rows[Dsno][15 + addFactor] = bin.Rows[BinNo][6];//rgb
              //labDS.Rows[Dsno][16 + addFactor] = bin.Rows[BinNo][7];
              //labDS.Rows[Dsno][17 + addFactor] = bin.Rows[BinNo][8];
              //labDS.Rows[Dsno][18 + addFactor] = Distance.ToString();//distance
            }
            
          }
          Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = labDS.DefaultView));
          Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
          btn_ExportGrid.IsEnabled = true;
          
        }

        private void button11_Click(object sender, RoutedEventArgs e)
        {

          PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\DataGrid\DataGrid\data\Ds_correction.txt");
          DataTable DS = new DataTable();
          Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
          {
            dtgrid_corrDisplay.Items.Refresh();
            DS = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
          }));

          List<LABbin> bin =PerceptionLib.Color.RGBBinneddData_David();

          int count = bin.Count();
          
          //int temp = -1;
          DataRow newRow;
          for (int i = 0; i < count; i++)
          {

            newRow = DS.NewRow();
            newRow[0] = bin[i].L.ToString();
            newRow[1] = bin[i].A.ToString();
            newRow[2] = bin[i].B.ToString();
            newRow[3] = PerceptionLib.Color.OriginalRGB[i].R.ToString();
            newRow[4] = PerceptionLib.Color.OriginalRGB[i].G.ToString();
            newRow[5] = PerceptionLib.Color.OriginalRGB[i].B.ToString();

            DS.Rows.Add(newRow);
          }
          Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = DS.DefaultView));
          Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

          PerceptionLib.Color lab = new PerceptionLib.Color();
          RGBValue temp = new RGBValue();

          for (int i = 0; i < DS.Rows.Count; i++)
          {
            lab.LA = bin[i].L;
            lab.A = bin[i].A;
            lab.B = bin[i].B;


            temp = PerceptionLib.Color.ToRBGFromLAB(lab);

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

              DS.Rows[i][6] = temp.R.ToString();
              DS.Rows[i][7] = temp.G.ToString();
              DS.Rows[i][8] = temp.B.ToString();

            }
            
          }
          Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = DS.DefaultView));
          Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
        }

        private void button12_Click(object sender, RoutedEventArgs e)
        {

          PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\DataGrid\DataGrid\data\Template_for_lab.csv");
          DataTable DS = new DataTable();
          Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
          {
            dtgrid_corrDisplay.Items.Refresh();
            DS = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
          }));
                 
          PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\DataGrid\DataGrid\data\binnedData_bigProjector.csv");
          DataTable bin = new DataTable();
          Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
          {
            dtgrid_corrDisplay.Items.Refresh();
            bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
          }));

          PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\DataGrid\DataGrid\data\template_for_bin_lab_matching.txt");
          DataTable template = new DataTable();
          Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
          {
            dtgrid_corrDisplay.Items.Refresh();
            template = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();
          }));


          DataRow newRow;

          int GivenR, GivenG, GivenB, MatchingR, MatchingG, MatchingB;

          for (int i = 0; i < bin.Rows.Count; i++)
          {
            GivenR = Convert.ToInt32(bin.Rows[i][9].ToString());
            GivenG = Convert.ToInt32(bin.Rows[i][10].ToString());
            GivenB = Convert.ToInt32(bin.Rows[i][11].ToString());
            int j = 0;
            for(j = 0; j < DS.Rows.Count-1; j++)
            {
              MatchingR = Convert.ToInt32(DS.Rows[j][6].ToString());
              MatchingG = Convert.ToInt32(DS.Rows[j][7].ToString());
              MatchingB = Convert.ToInt32(DS.Rows[j][8].ToString());

              if (GivenR == MatchingR && GivenG == MatchingG && GivenB == MatchingB)
                break;
            }

            newRow = template.NewRow();
            newRow[0] = DS.Rows[j][0];
            newRow[1] = DS.Rows[j][1];
            newRow[2] = DS.Rows[j][2];
            newRow[3] = bin.Rows[i][3];
            newRow[4] = bin.Rows[i][4];
            newRow[5] = bin.Rows[i][5];
            newRow[6] = bin.Rows[i][6];
            newRow[7] = bin.Rows[i][7];
            newRow[8] = bin.Rows[i][8];
            newRow[9] = DS.Rows[j][3];
            newRow[10] = DS.Rows[j][4];
            newRow[11] = DS.Rows[j][5];
            newRow[12] = GivenR.ToString();
            newRow[13] = GivenG.ToString();
            newRow[14] = GivenB.ToString();
            template.Rows.Add(newRow);
          }
          Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = template.DefaultView));
          Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

          btn_ExportGrid.IsEnabled=true;



        }
    }
}
     



