/* 
 * This software is based upon the book CUDA By Example by Sanders and Kandrot
 * and source code provided by NVIDIA Corporation.
 * It is a good idea to read the book while studying the examples!
*/
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
using System.Windows.Threading;
using GenericParsing;
using System.Data;
using System.Windows.Media.Media3D;
using System.Diagnostics;
using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;


namespace CudafyByExample
{
    class Program
    {
        public const int RANGEL = 21;
        public const int RANGEA = 41;
        public const int RANGEB = 45;
        private static DataTable profile = new DataTable();

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                CudafyModule km = CudafyModule.TryDeserialize();
                if (km == null || !km.TryVerifyChecksums())
                {
                    km = CudafyTranslator.Cudafy();
                    km.TrySerialize();
                }

                CudafyTranslator.GenerateDebug = true;
                // cuda or emulator
                GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId);
                gpu.LoadModule(km);



                //set up color profile to have a measure LAB lookup working
                #region

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
                    using (GenericParserAdapter parser = new GenericParserAdapter(@"C:\lev\STColorCorrection\Data\PROFILE\p3700.csv"))
                    {
                        System.Data.DataSet dsResult = parser.GetDataSet();
                        profile = dsResult.Tables[0];
                    }
                }
                catch
                { }



                //  #region
                for (int i = 1; i < profile.Rows.Count; i++)
                {
                    //lab vale as got form profile index
                    Point3D labBin = new Point3D();
                    labBin.X = Convert.ToDouble(profile.Rows[i][0].ToString());
                    labBin.Y = Convert.ToDouble(profile.Rows[i][1].ToString());
                    labBin.Z = Convert.ToDouble(profile.Rows[i][2].ToString());


                    //trasfered points
                    Point3D labCoordinate = navigationMatrix.Transform(labBin);
                    if (labCoordinate.X == 20 && labCoordinate.Y == 20 && labCoordinate.Z == 22)
                    {
                        Console.WriteLine("empty");
                    }

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



                #endregion

                //CVS FILE CREATING AND INICIALIZATION
                #region
                //create the CSV file
                CsvFileWriter output_file_1 = new CsvFileWriter(@"C:\lev\STColorCorrection\Data\CUDA performance analysis\out_file1.csv");
                CsvFileWriter output_file_2 = new CsvFileWriter(@"C:\lev\STColorCorrection\Data\CUDA performance analysis\out_file2.csv");

                //create the header
                CsvRow header = new CsvRow();
                header.Add("R_fg_in");
                header.Add("G_fg_in");
                header.Add("B_fg_in");
                header.Add("L_fg_in");
                header.Add("A_fg_in");
                header.Add("B_fg_in");

                header.Add("X_bg_in");
                header.Add("Y_bg_in");
                header.Add("Z_bg_in");

                header.Add("BF_Dist");
                header.Add("Cuda BF time");
                header.Add("QC_Dist");
                header.Add("Cuda QC time");
                header.Add("Snake_Dist");
                header.Add("Cuda Snake time");
                header.Add("DecreaseStep_DS");
                header.Add("Cuda DS time");

                output_file_1.WriteRow(header);
                header = new CsvRow();
                header.Add("R_fg_in");
                header.Add("G_fg_in");
                header.Add("B_fg_in");
                header.Add("L_fg_in");
                header.Add("A_fg_in");
                header.Add("B_fg_in");

                header.Add("X_bg_in");
                header.Add("Y_bg_in");
                header.Add("Z_bg_in");

                header.Add("BF_Dist");
                header.Add("R_fg_out");
                header.Add("G_fg_out");
                header.Add("B_fg_out");

                header.Add("QC_Dist");
                header.Add("R_fg_out");
                header.Add("G_fg_out");
                header.Add("B_fg_out");

                header.Add("Snake_Dist");
                header.Add("R_fg_out");
                header.Add("G_fg_out");
                header.Add("B_fg_out");

                header.Add("DecreaseStep_DS");
                header.Add("R_fg_out");
                header.Add("G_fg_out");
                header.Add("B_fg_out");
                output_file_2.WriteRow(header);
                //write the header to the CSV file
                #endregion


                Random randomGenerater = new Random();
                for (int num_colors = 0; num_colors < 500; num_colors++)
                {
                    //create a new csv row
                    CsvRow new_row_file_1 = new CsvRow();
                    CsvRow new_row_file_2 = new CsvRow();



                    //colour selection
                    Byte[] rgb = new Byte[3];
                    randomGenerater.NextBytes(rgb);
                    System.Drawing.Color foreground = System.Drawing.Color.FromArgb(rgb[0], rgb[1], rgb[2]);

                    Point3D backgroundCIEXYZ = new Point3D(0, 0, 0);
                    backgroundCIEXYZ.X = randomGenerater.NextDouble() * 0.9504;
                    backgroundCIEXYZ.Y = randomGenerater.NextDouble() * 1.0000;
                    backgroundCIEXYZ.Z = randomGenerater.NextDouble() * 1.0888;
                    Point3D background = new Point3D(backgroundCIEXYZ.X, backgroundCIEXYZ.Y, backgroundCIEXYZ.Z);

                    Bin foregroundBin = FindForegroundBin(p3700, navigationMatrix, foreground);
                    PerceptionLib.Color foregroundLAB = new PerceptionLib.Color();

                    foregroundLAB.LA = foregroundBin.measuredLAB.X;
                    foregroundLAB.A = foregroundBin.measuredLAB.Y;
                    foregroundLAB.B = foregroundBin.measuredLAB.Z;

                    //write the input colors
                    #region
                    new_row_file_1.Add(foreground.R.ToString());
                    new_row_file_1.Add(foreground.G.ToString());
                    new_row_file_1.Add(foreground.B.ToString());

                    new_row_file_1.Add(foregroundLAB.LA.ToString());
                    new_row_file_1.Add(foregroundLAB.A.ToString());
                    new_row_file_1.Add(foregroundLAB.B.ToString());

                    new_row_file_1.Add(background.X.ToString());
                    new_row_file_1.Add(background.Y.ToString());
                    new_row_file_1.Add(background.Z.ToString());

                    new_row_file_2.Add(foreground.R.ToString());
                    new_row_file_2.Add(foreground.G.ToString());
                    new_row_file_2.Add(foreground.B.ToString());
                                 
                    new_row_file_2.Add(foregroundLAB.LA.ToString());
                    new_row_file_2.Add(foregroundLAB.A.ToString());
                    new_row_file_2.Add(foregroundLAB.B.ToString());
                                 
                    new_row_file_2.Add(background.X.ToString());
                    new_row_file_2.Add(background.Y.ToString());
                    new_row_file_2.Add(background.Z.ToString());
                    #endregion


                    //get the brute force values
                    Color.TestingStructure[] results_brute_force = Color.CorrectColour(foreground, background.X, background.Y, background.Z);

                    new_row_file_1.Add(results_brute_force[0].distance.ToString());
                    new_row_file_1.Add(results_brute_force[0].execution_time.ToString());

                    Point3D labBin = new Point3D();
                    labBin.X = results_brute_force[0].Given_R;
                    labBin.Y = results_brute_force[0].Given_G;
                    labBin.Z = results_brute_force[0].Given_B;

                    Bin actualBin = GetProfileBin(p3700, labBin);

                    new_row_file_2.Add(results_brute_force[0].distance.ToString());
                    new_row_file_2.Add(actualBin.binRGB.X.ToString());
                    new_row_file_2.Add(actualBin.binRGB.Y.ToString());
                    new_row_file_2.Add(actualBin.binRGB.Z.ToString());


                    quick_corr.TestingStructure[] results_quick_corr = quick_corr.CorrectColour(foreground, background.X, background.Y, background.Z);

                    new_row_file_1.Add(results_quick_corr[0].distance.ToString());
                    new_row_file_1.Add(results_quick_corr[0].execution_time.ToString());

                    labBin = new Point3D();
                    labBin.X = results_quick_corr[0].Given_R;
                    labBin.Y = results_quick_corr[0].Given_G;
                    labBin.Z = results_quick_corr[0].Given_B;

                    actualBin = GetProfileBin(p3700, labBin);

                    new_row_file_2.Add(results_quick_corr[0].distance.ToString());
                    new_row_file_2.Add(actualBin.binRGB.X.ToString());
                    new_row_file_2.Add(actualBin.binRGB.Y.ToString());
                    new_row_file_2.Add(actualBin.binRGB.Z.ToString());


                    snake.TestingStructure[] results_snake = snake.CorrectColour(foreground, background.X, background.Y, background.Z);

                    new_row_file_1.Add(results_snake[0].distance.ToString());
                    new_row_file_1.Add(results_snake[0].execution_time.ToString());

                    labBin = new Point3D();
                    labBin.X = results_snake[0].Given_R;
                    labBin.Y = results_snake[0].Given_G;
                    labBin.Z = results_snake[0].Given_B;

                    actualBin = GetProfileBin(p3700, labBin);

                    new_row_file_2.Add(results_snake[0].distance.ToString());
                    new_row_file_2.Add(actualBin.binRGB.X.ToString());
                    new_row_file_2.Add(actualBin.binRGB.Y.ToString());
                    new_row_file_2.Add(actualBin.binRGB.Z.ToString());

                    half_step.TestingStructure[] results_half_step = half_step.CorrectColour(foreground, background.X, background.Y, background.Z);

                    new_row_file_1.Add(results_half_step[0].distance.ToString());
                    new_row_file_1.Add(results_half_step[0].execution_time.ToString());

                    labBin = new Point3D();
                    labBin.X = results_half_step[0].Given_R;
                    labBin.Y = results_half_step[0].Given_G;
                    labBin.Z = results_half_step[0].Given_B;

                    actualBin = GetProfileBin(p3700, labBin);

                    new_row_file_2.Add(results_half_step[0].distance.ToString());
                    new_row_file_2.Add(actualBin.binRGB.X.ToString());
                    new_row_file_2.Add(actualBin.binRGB.Y.ToString());
                    new_row_file_2.Add(actualBin.binRGB.Z.ToString());





                    //write the results
                    output_file_1.WriteRow(new_row_file_1);
                    output_file_2.WriteRow(new_row_file_2);
                    
                }

                

               //Color.Execute();
               //quick_corr.Execute();

                //close the CSV files
                output_file_1.Close();
                output_file_2.Close();

                Console.WriteLine("Done!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
theEnd:
            Console.ReadKey();
        }

        private static Bin outOfBounds = new Bin(-1, -1, -1);
        private static Bin GetProfileBin(Bin[, ,] profile, Point3D coordinates)
        {
            if (coordinates.X < 0 || coordinates.X >= RANGEL)
                return outOfBounds;
            if (coordinates.Y < 0 || coordinates.Y >= RANGEA)
                return outOfBounds;
            if (coordinates.Z < 0 || coordinates.Z >= RANGEB)
                return outOfBounds;

            Bin returnBin = profile[(int)coordinates.X, (int)coordinates.Y, (int)coordinates.Z];
            returnBin.isMoreAccurateThanOrigin = false;
            return returnBin;
        }

        private static Bin FindForegroundBin(Bin[, ,] profile, Matrix3D navigationMatrix, System.Drawing.Color foregroundRGB)
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

    }

    /// <summary>
    /// Class to store one CSV row
    /// </summary>
    public class CsvRow : List<string>
    {
        public string LineText { get; set; }
    }

    /// <summary>
    /// Class to write data to a CSV file
    /// </summary>
    public class CsvFileWriter : StreamWriter
    {
        public CsvFileWriter(Stream stream)
            : base(stream)
        {
        }

        public CsvFileWriter(string filename)
            : base(filename)
        {
        }

        /// <summary>
        /// Writes a single row to a CSV file.
        /// </summary>
        /// <param name="row">The row to be written</param>
        public void WriteRow(CsvRow row)
        {
            StringBuilder builder = new StringBuilder();
            bool firstColumn = true;
            foreach (string value in row)
            {
                // Add separator if this isn't the first value
                if (!firstColumn)
                    builder.Append(',');
                // Implement special handling for values that contain comma or quote
                // Enclose in quotes and double up any double quotes
                if (value.IndexOfAny(new char[] { '"', ',' }) != -1)
                    builder.AppendFormat("\"{0}\"", value.Replace("\"", "\"\""));
                else
                    builder.Append(value);
                firstColumn = false;
            }
            row.LineText = builder.ToString();
            WriteLine(row.LineText);
        }
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
        public double distanceLAB;

        public double weight;

        public Bin(int l, int a, int b)
        {
            binLAB = new Point3D(l, a, b);
            isEmpty = true;
            distanceLAB = Double.MaxValue;

            isMoreAccurateThanOrigin = false;
        }

        public override string ToString()
        {
            return String.Format("Coordinates: {0}, IsEmpty: {1}, Location: {2}", binLAB, isEmpty);
        }
    }
   
}
