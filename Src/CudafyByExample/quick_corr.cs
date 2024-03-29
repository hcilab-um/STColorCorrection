﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cudafy;
using Cudafy.Host;
using Cudafy.Types;
using Cudafy.Translator;

using System.Data;
using GenericParsing;


namespace CudafyByExample
{
    class quick_corr
    {
        [Cudafy]
        public const int ImagreHeight = 800;
        [Cudafy]
        public const int ImagreWidth = 600;
        [Cudafy]
        public const int ImagreDimention = ImagreHeight * ImagreWidth;

        [Cudafy]
        public const double TRUE = 1.0;
        [Cudafy]
        public const double FALSE = -1.0;


        /// <summary>
        /// This struct is only used to return all the data to the testing 
        /// algorithm. 
        /// </summary>
        [Cudafy]
        public struct TestingStructure
        {
            public double Given_R;
            public double Given_G;
            public double Given_B;
            public double distance;
            public double execution_time;
        }


        [Cudafy]
        public struct ProfileStrucuture
        {
            public double L;
            public double A;
            public double B;
            //public double Given_R;
            //public double Given_G;
            //public double Given_B;
            public double ML;
            public double MA;
            public double MB;
            public double MX;
            public double MY;
            public double MZ;
            public double distance;
            public double weight;
            public double isempty;
            public double isMoreAccurateThanOrigin;


        }

        [Cudafy]
        public struct SampleStructure
        {
            public int L;
            public int A;
            public int B;
            public double MX;
            public double MY;
            public double MZ;
            public double distance;
            public double weight;
            public int isempty;
            public int isMoreAccurateThanOrigin;
        }


        [Cudafy]
        public struct BackGroundStrucuture
        {
            public double X;
            public double Y;
            public double Z;

        }

        [Cudafy]
        public struct Point3D
        {
            public double X;
            public double Y;
            public double Z;

            public Point3D(double x, double y, double z)
            {
                X = x;
                Y = y;
                Z = z;
            }
        }

        [Cudafy]
        public struct ForeGroundStrucuture
        {
            public byte R;
            public byte G;
            public byte B;
        }

        public const int LAxis = 21;
        public const int AAxis = 41;
        public const int BAxis = 45;


        [Cudafy]
        public static double FX(double e)
        {
            if (e > 0.008856)
                e = Math.Pow(e, (1.0 / 3.0));

            else
                e = ((903.3 * e) + 16) / 116;

            return e;
        }

        [Cudafy]
        public static double Lxyz(double e)
        {
            if (e > 0.008856)
                e = (116 * Math.Pow(e, (1.0 / 3.0))) - 16;
            else
                e = (double)(903.3 * e);
            return e;
        }

        [Cudafy]
        public static BackGroundStrucuture RGBToXYZ_St(ForeGroundStrucuture RGB)
        {
            // by the formula given the the web page http://www.brucelindbloom.com/index.html [XYZ]=[M][RGB]
            //In order to properly use this matrix, the RGB values must be linear and in the nominal range [0.0, 1.0].
            // RGB values may first need conversion (for example, dividing by 255 and then raising them to a power)
            // Where M for D65:	 0.4124564  0.3575761  0.1804375
            //0.2126729  0.7151522  0.0721750
            //0.0193339  0.1191920  0.9503041

            //// to make rgb values linear red, green, blue values
            BackGroundStrucuture XYZ = new BackGroundStrucuture();
            double rLinear = (double)(RGB.R / 255.0);
            double gLinear = (double)(RGB.G / 255.0);
            double bLinear = (double)(RGB.B / 255.0);

            // convert to a sRGB form

            //double r =  Math.pow((rLinear ), 2.2) ;
            //double g =  Math.pow((gLinear ), 2.2) ;
            //double b = Math.pow((bLinear ), 2.2) ;
            double r, g, b;

            if (rLinear > 0.04045)
            {
                r = (double)((double)(rLinear + 0.055) / 1.055);
                r = Math.Pow(r, (double)2.200000);
            }
            else
                r = (double)(rLinear / 12.92);

            if (gLinear > 0.04045)
            {
                g = (double)((double)(gLinear + 0.055) / 1.055);
                g = Math.Pow((double)g, (double)2.2);
            }
            else
                g = (double)(gLinear / 12.92);

            if (bLinear > 0.04045)
            {
                b = (double)((double)(bLinear + 0.055) / 1.055);
                b = Math.Pow((double)b, (double)2.2);
            }
            else
                b = (double)(bLinear / 12.92);


            XYZ.X = (double)(r * 0.4124564 + g * 0.3575761 + b * 0.1804375);
            XYZ.Y = (double)(r * 0.2126729 + g * 0.7151522 + b * 0.0721750);
            XYZ.Z = (double)(r * 0.0193339 + g * 0.1191920 + b * 0.9503041);

            //if (XYZ.X > 0.9504)
            //  XYZ.X = 0.9504F;
            //else if (XYZ.X < 0)
            //  XYZ.X = 0;
            //else
            //  XYZ.X = XYZ.X;

            //if (XYZ.Y > 1)
            //  XYZ.Y = 1;
            //else if (XYZ.Y < 0)
            //  XYZ.Y = 0;
            //else
            //  XYZ.Y = XYZ.Y;

            //if (XYZ.Z > 1.0888)
            //  XYZ.Z = 1.0888F;
            //else if (XYZ.Z < 0)
            //  XYZ.Z = 0;
            //else
            //  XYZ.Z = XYZ.Z;

            return XYZ;

        }

        [Cudafy]
        public static ProfileStrucuture ToLAB(ForeGroundStrucuture cRGB)
        {
            double Fx, Fy, Fz;

            BackGroundStrucuture xyz = RGBToXYZ_St(cRGB);

            double xr = (double)(xyz.X / 0.9504f);
            double yr = (double)(xyz.Y / 1.0000f);
            double zr = (double)(xyz.Z / 1.0888f);

            if (xr > 0.008856)
                xr = Math.Pow(xr, (1.0 / 3.0));

            else
                xr = ((903.3 * xr) + 16) / 116;

            if (yr > 0.008856)
                yr = Math.Pow(yr, (1.0 / 3.0));

            else
                yr = ((903.3 * yr) + 16) / 116;


            if (zr > 0.008856)
                zr = Math.Pow(zr, (1.0 / 3.0));

            else
                zr = ((903.3 * zr) + 16) / 116;

            Fx = xr;
            Fy = yr;
            Fz = zr;

            ProfileStrucuture rColor = new ProfileStrucuture();

            if (yr > 0.008856)
                yr = (116 * Math.Pow(yr, (1.0 / 3.0))) - 16;
            else
                yr = (double)(903.3 * yr);
            rColor.L = yr;
            rColor.A = 500 * (Fx - Fy);
            rColor.B = 200 * (Fy - Fz);

            return rColor;
        }

        [Cudafy]
        public static ProfileStrucuture XYZtoLAB_st(BackGroundStrucuture xyz)
        {
            double Fx, Fy, Fz;

            double xr = (double)(xyz.X / 0.9504);
            double yr = (double)(xyz.Y / 1.0000);
            double zr = (double)(xyz.Z / 1.0888);

            if (xr > 0.008856)
                xr = Math.Pow(xr, (1.0 / 3.0));

            else
                xr = ((903.3 * xr) + 16) / 116;

            if (yr > 0.008856)
                yr = Math.Pow(yr, (1.0 / 3.0));

            else
                yr = ((903.3 * yr) + 16) / 116;


            if (zr > 0.008856)
                zr = Math.Pow(zr, (1.0 / 3.0));

            else
                zr = ((903.3 * zr) + 16) / 116;

            Fx = xr;
            Fy = yr;
            Fz = zr;

            ProfileStrucuture rColor = new ProfileStrucuture();

            if (yr > 0.008856)
                yr = (116 * Math.Pow(yr, (1.0 / 3.0))) - 16;
            else
                yr = (double)(903.3 * yr);
            rColor.L = yr;
            rColor.A = 500 * (Fx - Fy);
            rColor.B = 200 * (Fy - Fz);

            return rColor;
        }

        [Cudafy]
        private static ProfileStrucuture GetProfileBin(double binL, double binA, double binB, ProfileStrucuture[, ,] profile_GPU1)
        {
            ProfileStrucuture outOfBounds = new ProfileStrucuture();
            outOfBounds.L = -1;
            outOfBounds.A = -1;
            outOfBounds.B = -1;

            outOfBounds.isempty = 0;
            if (binL < 0 || binL >= 21)
                return outOfBounds;
            if (binA < 0 || binA >= 41)
                return outOfBounds;
            if (binB < 0 || binB >= 45)
                return outOfBounds;

            outOfBounds = profile_GPU1[(int)binL, (int)binA, (int)binB];

            return outOfBounds;
        }

        [Cudafy]
        private static SampleStructure GetProfileBinForSample(double binL, double binA, double binB, ProfileStrucuture[, ,] profile_GPU1)
        {
            SampleStructure outOfBounds = new SampleStructure();

            outOfBounds.isempty = 0;
            if (binL < 0 || binL >= 21)
                return outOfBounds;
            if (binA < 0 || binA >= 41)
                return outOfBounds;
            if (binB < 0 || binB >= 45)
                return outOfBounds;


            outOfBounds.MX = profile_GPU1[(int)binL, (int)binA, (int)binB].MX;
            outOfBounds.MY = profile_GPU1[(int)binL, (int)binA, (int)binB].MY;
            outOfBounds.MZ = profile_GPU1[(int)binL, (int)binA, (int)binB].MZ;

            outOfBounds.L = (int)profile_GPU1[(int)binL, (int)binA, (int)binB].L;
            outOfBounds.A = (int)profile_GPU1[(int)binL, (int)binA, (int)binB].A;
            outOfBounds.B = (int)profile_GPU1[(int)binL, (int)binA, (int)binB].B;

            outOfBounds.isempty = (int)profile_GPU1[(int)binL, (int)binA, (int)binB].isempty;
            return outOfBounds;
        }

        [Cudafy]
        private static ProfileStrucuture FindForegroundBin(ForeGroundStrucuture foregorungRGB_GPU, ProfileStrucuture[, ,] profile_GPU)
        {
            ProfileStrucuture foregroundLAB = ToLAB(foregorungRGB_GPU);

            int binL = ((int)Math.Round(foregroundLAB.L / 5.0)) * 5;
            int binA = ((int)Math.Round(foregroundLAB.A / 5.0)) * 5;
            int binB = ((int)Math.Round(foregroundLAB.B / 5.0)) * 5;

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

            binL = (int)(binL * 0.2) + 0;
            binA = (int)(binA * 0.2) + 20;
            binB = (int)(binB * 0.2) + 22;

            ProfileStrucuture foregroundBin = GetProfileBin(binL, binA, binB, profile_GPU);

            return foregroundBin;
        }

        [Cudafy]
        private static BackGroundStrucuture addXYZ_st(double X1, double Y1, double Z1, BackGroundStrucuture XYZbg)
        {
            BackGroundStrucuture XYZ;
            XYZ.X = X1 + XYZbg.X;
            XYZ.Y = Y1 + XYZbg.Y;
            XYZ.Z = Z1 + XYZbg.Z;

            //if (XYZ.X > 0.9504)
            //    XYZ.X = 0.9504f;
            //else if (XYZ.X < 0)
            //    XYZ.X = 0;
            //else
            //    XYZ.X = XYZ.X;

            //if (XYZ.Y > 1)
            //    XYZ.Y = 1;
            //else if (XYZ.Y < 0)
            //    XYZ.Y = 0;
            //else
            //    XYZ.Y = XYZ.Y;

            //if (XYZ.Z > 1.0888)
            //    XYZ.Z = 1.0888f;
            //else if (XYZ.Z < 0)
            //    XYZ.Z = 0;
            //else
            //    XYZ.Z = XYZ.Z;

            return XYZ;
        }

        [Cudafy]
        private static Point3D HalfTheStep(Point3D step)
        {
            double truncated;
            if (step.X > 1)
            {
                step.X = step.X / 2.0;
                truncated = Math.Truncate(step.X);
                truncated = step.X - truncated;
                if (truncated > 0.4)
                {
                    step.X = step.X + 0.1;
                }

                step.X = (int)(Math.Round(step.X));
            }
            else if (step.X == 1)
            {
                step.X = 0;
            }

            if (step.Y > 1)
            {
                step.Y = step.Y / 2.0;
                truncated = Math.Truncate(step.Y);
                truncated = step.Y - truncated;
                if (truncated > 0.4)
                {
                    step.Y = step.Y + 0.1;
                }

                step.Y = (int)(Math.Round(step.Y));
            }
            else if (step.Y == 1)
            {
                step.Y = 0;
            }

            if (step.Z > 1)
            {
                step.Z = step.Z / 2.0;
                truncated = Math.Truncate(step.Z);
                truncated = step.Z - truncated;
                if (truncated > 0.4)
                {
                    step.Z = step.Z + 0.1;
                }
                step.Z = (int)(Math.Round(step.Z));
            }
            else if (step.Z == 1)
            {
                step.Z = 0;
            }

            return step;

        }

        ///
        /// ORIGINAL QUICK CORRECTION ALGORITHM
        ///

        //[Cudafy]
        //public static void QuickCorr(GThread thread, ProfileStrucuture[, ,] profile_GPU, ForeGroundStrucuture[] foregorungRGB_GPU, BackGroundStrucuture[] BackgroundXYZ_GPU, ProfileStrucuture[] ptr, SampleStructure[,] samples)
        //{
        //    // map from threadIdx/BlockIdx to pixel position
        //    int x = thread.threadIdx.x + thread.blockIdx.x * thread.blockDim.x;
        //    int y = thread.threadIdx.y + thread.blockIdx.y * thread.blockDim.y;
        //    int offset = x + y * thread.blockDim.x * thread.gridDim.x;
        //    double ox = (x - 1.0 / 2.0);
        //    double oy = (y - 1.0 / 2.0);

        //    // double closestColor = double.MaxValue;

        //    double diffL = 0.0;
        //    double diffA = 0.0;
        //    double diffB = 0.0;

        //    //int BestL = 0;
        //    //int BestA = 0;
        //    //int BestB = 0;

        //    //1 - Converts the foreground to how the display shows it
        //    ProfileStrucuture foregroundColorToShow = new ProfileStrucuture();
        //    ProfileStrucuture foregroundLAB = ToLAB(foregorungRGB_GPU[offset]);

        //    int binL = ((int)Math.Round(foregroundLAB.L / 5.0)) * 5;
        //    int binA = ((int)Math.Round(foregroundLAB.A / 5.0)) * 5;
        //    int binB = ((int)Math.Round(foregroundLAB.B / 5.0)) * 5;

        //    if (binL > 100)
        //        binL = 100;
        //    if (binA < -86.17385493791946)
        //        binA = -85;
        //    if (binA > 98.2448002875424)
        //        binA = 100;
        //    if (binB < -107.8619171648283)
        //        binB = -110;
        //    if (binB > 94.47705120353054)
        //        binB = 95;

        //    binL = (int)(binL * 0.2) + 0;
        //    binA = (int)(binA * 0.2) + 20;
        //    binB = (int)(binB * 0.2) + 22;


        //    foregroundColorToShow.ML = profile_GPU[binL, binA, binB].ML;
        //    foregroundColorToShow.MA = profile_GPU[binL, binA, binB].MA;
        //    foregroundColorToShow.MB = profile_GPU[binL, binA, binB].MB;

        //    //1.1 - Extra step for Quick Correction
        //    Point3D origin = new Point3D(10.0, 20.0, 22.0);
        //    Point3D step = new Point3D(10, 20, 22);

        //    //2 - Get the accuracy

        //    ProfileStrucuture actualBin = GetProfileBin(origin.X, origin.Y, origin.Z, profile_GPU);

        //    if (actualBin.isempty == TRUE)
        //        return;

        //    BackGroundStrucuture PredictionXYZ = addXYZ_st(actualBin.MX, actualBin.MY, actualBin.MZ, BackgroundXYZ_GPU[offset]);

        //    ProfileStrucuture PredictionLAB = XYZtoLAB_st(PredictionXYZ);

        //    diffL = PredictionLAB.L - foregroundColorToShow.ML;
        //    diffA = PredictionLAB.A - foregroundColorToShow.MA;
        //    diffB = PredictionLAB.B - foregroundColorToShow.MB;

        //    //diffL = PredictionXYZ.X - foregroundColorToShow.MX;
        //    //diffA = PredictionXYZ.Y - foregroundColorToShow.MY;
        //    //diffB = PredictionXYZ.Z - foregroundColorToShow.MZ;

        //    diffL = diffL * diffL;
        //    diffA = diffA * diffA;
        //    diffB = diffB * diffB;

        //    //originBin.distanceLAB
        //    actualBin.distance = Math.Sqrt(diffL + diffA + diffB);

        //    //declaring 6 separate samples - CUDA does not support array declaration in kernel device code

        //    Point3D top = new Point3D();
        //    Point3D bottom = new Point3D();
        //    Point3D left = new Point3D();
        //    Point3D right = new Point3D();
        //    Point3D forward = new Point3D();
        //    Point3D backward = new Point3D();

        //    while (step.X > 0 || step.Y > 0 && step.Z > 0)
        //    {

        //        //sample 6 bins
        //        top.X = origin.X + step.X;
        //        top.Y = origin.Y;
        //        top.Z = origin.Z;

        //        bottom.X = origin.X - step.X;
        //        bottom.Y = origin.Y;
        //        bottom.Z = origin.Z;

        //        left.X = origin.X;
        //        left.Y = origin.Y - step.Y;
        //        left.Z = origin.Z;

        //        right.X = origin.X;
        //        right.Y = origin.Y + step.Y;
        //        right.Z = origin.Z;

        //        forward.X = origin.X;
        //        forward.Y = origin.Y;
        //        forward.Z = origin.Z - step.Z;

        //        backward.X = origin.X;
        //        backward.Y = origin.Y;
        //        backward.Z = origin.Z + step.Z;

        //        samples[offset, 0] = GetProfileBinForSample(top.X, top.Y, top.Z, profile_GPU);
        //        samples[offset, 1] = GetProfileBinForSample(bottom.X, bottom.Y, bottom.Z, profile_GPU);
        //        samples[offset, 2] = GetProfileBinForSample(left.X, left.Y, left.Z, profile_GPU);
        //        samples[offset, 3] = GetProfileBinForSample(right.X, right.Y, right.Z, profile_GPU);
        //        samples[offset, 4] = GetProfileBinForSample(forward.X, forward.Y, forward.Z, profile_GPU);
        //        samples[offset, 5] = GetProfileBinForSample(backward.X, backward.Y, backward.Z, profile_GPU);


        //        int countSamplesClosestThanOrigin = 0;

        //        //calculate color correction for all samples
        //        #region
        //        for (int index = 0; index < 6; index++)
        //        {
        //            if (samples[offset,index].isempty == TRUE || samples[offset,index].isempty == 0.0)
        //                continue;

        //            BackGroundStrucuture temp_XYZ = addXYZ_st(samples[offset,index].MX, samples[offset,index].MY, samples[offset,index].MZ, BackgroundXYZ_GPU[offset]);

        //            ProfileStrucuture PredictionlAB = XYZtoLAB_st(temp_XYZ);

        //            diffL = PredictionlAB.L - foregroundColorToShow.ML;
        //            diffA = PredictionlAB.A - foregroundColorToShow.MA;
        //            diffB = PredictionlAB.B - foregroundColorToShow.MB;

        //            diffL = diffL * diffL;
        //            diffA = diffA * diffA;
        //            diffB = diffB * diffB;

        //            //curr_distanceLAB
        //            samples[offset,index].distance = Math.Sqrt(diffL + diffA + diffB);
        //            if (samples[offset,index].distance >= actualBin.distance)
        //            {
        //                continue;
        //            }
        //            else
        //            {
        //                samples[offset,index].isMoreAccurateThanOrigin = (int)TRUE;
        //                countSamplesClosestThanOrigin++;
        //            }
        //        }
        //        #endregion

                

        //        if (countSamplesClosestThanOrigin == 0)
        //        {
        //            step = HalfTheStep(step);
        //            continue;
        //        }

                

        //        //if there is at least one sample more accurate, it moves the origin in that direction, maintains the step and checks again
        //        else
        //        {
        //            //6.1 calculates weights
        //            double totalimprovements = 0;
        //            for (int index = 0; index < 6; index++)
        //            {
        //                if (!(samples[offset,index].isMoreAccurateThanOrigin == TRUE))
        //                {
        //                    continue;
        //                }
        //                totalimprovements += (actualBin.distance - samples[offset,index].distance);
        //            }
        //            for (int index = 0; index < 6; index++)
        //            {
        //                if (!(samples[offset, index].isMoreAccurateThanOrigin == 1))
        //                {
        //                    continue;
        //                }
        //                samples[offset,index].weight = ((actualBin.distance - samples[offset,index].distance) / totalimprovements);
        //            }
        //            //6.2 calculates displacement
        //            Point3D displacement = new Point3D(0, 0, 0);
        //            for (int index = 0; index < 6; index++)
        //            {
        //                if (!(samples[offset,index].isMoreAccurateThanOrigin == 1))
        //                {
        //                    continue;
        //                }


        //                displacement.X = displacement.X + (samples[offset,index].L - origin.X) * samples[offset,index].weight;
        //                displacement.Y = displacement.Y + (samples[offset,index].A - origin.Y) * samples[offset,index].weight;
        //                displacement.Z = displacement.Z + (samples[offset,index].B - origin.Z) * samples[offset,index].weight;
        //            }

        //            if (displacement.X > 0)
        //                displacement.X = Math.Ceiling(displacement.X);
        //            else
        //                displacement.X = Math.Floor(displacement.X);

        //            if (displacement.Y > 0)
        //                displacement.Y = Math.Ceiling(displacement.Y);
        //            else
        //                displacement.Y = Math.Floor(displacement.Y);

        //            if (displacement.Z > 0)
        //                displacement.Z = Math.Ceiling(displacement.Z);
        //            else
        //                displacement.Z = Math.Floor(displacement.Z);

        //            //6.3 pokes new origin
        //            Point3D newOriginLoc = new Point3D();

        //            newOriginLoc.X = origin.X + displacement.X;
        //            newOriginLoc.Y = origin.Y + displacement.Y;
        //            newOriginLoc.Z = origin.Z + displacement.Z;

        //            ProfileStrucuture newOriginBin = GetProfileBin(newOriginLoc.X, newOriginLoc.Y, newOriginLoc.Z, profile_GPU);
        //            while (newOriginBin.isempty == TRUE)
        //            {
        //                /////////////////////// round to even missing ///////////////////

        //                //6.4 moves half the magnitude in the given direction
        //                displacement.X = Math.Round(displacement.X / 2);
        //                displacement.Y = Math.Round(displacement.Y / 2);
        //                displacement.Z = Math.Round(displacement.Z / 2);

        //                newOriginLoc.X = origin.X + displacement.X;
        //                newOriginLoc.Y = origin.Y + displacement.Y;
        //                newOriginLoc.Z = origin.Z + displacement.Z;

        //                newOriginBin = GetProfileBin(newOriginLoc.X, newOriginLoc.Y, newOriginLoc.Z, profile_GPU);
        //            }
        //            //calclates the accuracy of the posible new origin
        //            if (newOriginBin.isempty == TRUE)
        //                return;

        //            PredictionXYZ = addXYZ_st(newOriginBin.MX, newOriginBin.MY, newOriginBin.MZ, BackgroundXYZ_GPU[offset]);

        //            ProfileStrucuture PredictionlAB = XYZtoLAB_st(PredictionXYZ);

        //            diffL = PredictionlAB.L - foregroundColorToShow.ML;
        //            diffA = PredictionlAB.A - foregroundColorToShow.MA;
        //            diffB = PredictionlAB.B - foregroundColorToShow.MB;

        //            //diffL = PredictionXYZ.X - foregroundColorToShow.MX;
        //            //diffA = PredictionXYZ.Y - foregroundColorToShow.MY;
        //            //diffB = PredictionXYZ.Z - foregroundColorToShow.MZ;

        //            diffL = diffL * diffL;
        //            diffA = diffA * diffA;
        //            diffB = diffB * diffB;

        //            //originBin.distanceLAB
        //            newOriginBin.distance = Math.Sqrt(diffL + diffA + diffB);

        //            if ((origin.X == newOriginLoc.X && origin.Z == newOriginLoc.X && origin.X == newOriginLoc.X) || actualBin.distance <= newOriginBin.distance) // it's the same location then just reduces the step
        //                step = HalfTheStep(step);
        //            else
        //            {
        //                origin = newOriginLoc;
        //                actualBin = newOriginBin;
        //            }

        //        }

        //    }
        //    ptr[offset] = actualBin;

        //}

        ///
        /// MODIFIED QUICK CORRECTION ALGORITHM
        ///

        [Cudafy]
        public static void QuickCorr(GThread thread, ProfileStrucuture[, ,] profile_GPU, ForeGroundStrucuture[] foregorungRGB_GPU, BackGroundStrucuture[] BackgroundXYZ_GPU, TestingStructure[] ptr, SampleStructure[,] samples)
        {
            // map from threadIdx/BlockIdx to pixel position
            int x = thread.threadIdx.x + thread.blockIdx.x * thread.blockDim.x;
            int y = thread.threadIdx.y + thread.blockIdx.y * thread.blockDim.y;
            int offset = x + y * thread.blockDim.x * thread.gridDim.x;
            double ox = (x - 1.0 / 2.0);
            double oy = (y - 1.0 / 2.0);

            // double closestColor = double.MaxValue;

            double diffL = 0.0;
            double diffA = 0.0;
            double diffB = 0.0;

            //int BestL = 0;
            //int BestA = 0;
            //int BestB = 0;

            //1 - Converts the foreground to how the display shows it
            ProfileStrucuture foregroundColorToShow = new ProfileStrucuture();
            ProfileStrucuture foregroundLAB = ToLAB(foregorungRGB_GPU[offset]);

            int binL = ((int)Math.Round(foregroundLAB.L / 5.0)) * 5;
            int binA = ((int)Math.Round(foregroundLAB.A / 5.0)) * 5;
            int binB = ((int)Math.Round(foregroundLAB.B / 5.0)) * 5;

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

            binL = (int)(binL * 0.2) + 0;
            binA = (int)(binA * 0.2) + 20;
            binB = (int)(binB * 0.2) + 22;


            foregroundColorToShow.ML = profile_GPU[binL, binA, binB].ML;
            foregroundColorToShow.MA = profile_GPU[binL, binA, binB].MA;
            foregroundColorToShow.MB = profile_GPU[binL, binA, binB].MB;

            //1.1 - Extra step for Quick Correction
            Point3D origin = new Point3D(10.0, 20.0, 22.0);
            Point3D step = new Point3D(10, 20, 22);

            //2 - Get the accuracy

            ProfileStrucuture actualBin = GetProfileBin(origin.X, origin.Y, origin.Z, profile_GPU);

            if (actualBin.isempty == TRUE)
                return;

            BackGroundStrucuture PredictionXYZ = addXYZ_st(actualBin.MX, actualBin.MY, actualBin.MZ, BackgroundXYZ_GPU[offset]);

            ProfileStrucuture PredictionLAB = XYZtoLAB_st(PredictionXYZ);

            //diffL = foregroundColorToShow.ML - PredictionLAB.L;
            //diffA = foregroundColorToShow.MA - PredictionLAB.A;
            //diffB = foregroundColorToShow.MB - PredictionLAB.B;

            diffL = PredictionLAB.L - foregroundColorToShow.ML;
            diffA = PredictionLAB.A - foregroundColorToShow.MA;
            diffB = PredictionLAB.B - foregroundColorToShow.MB;

            //diffL = PredictionXYZ.X - foregroundColorToShow.MX;
            //diffA = PredictionXYZ.Y - foregroundColorToShow.MY;
            //diffB = PredictionXYZ.Z - foregroundColorToShow.MZ;

            diffL = diffL * diffL;
            diffA = diffA * diffA;
            diffB = diffB * diffB;

            //originBin.distanceLAB
            actualBin.distance = Math.Sqrt(diffL + diffA + diffB);

            //declaring 6 separate samples - CUDA does not support array declaration in kernel device code

            Point3D top = new Point3D();
            Point3D bottom = new Point3D();
            Point3D left = new Point3D();
            Point3D right = new Point3D();
            Point3D forward = new Point3D();
            Point3D backward = new Point3D();

            while (step.X > 0 || step.Y > 0 && step.Z > 0)
            {

                //sample 6 bins
                top.X = origin.X + step.X;
                top.Y = origin.Y;
                top.Z = origin.Z;

                bottom.X = origin.X - step.X;
                bottom.Y = origin.Y;
                bottom.Z = origin.Z;

                left.X = origin.X;
                left.Y = origin.Y - step.Y;
                left.Z = origin.Z;

                right.X = origin.X;
                right.Y = origin.Y + step.Y;
                right.Z = origin.Z;

                forward.X = origin.X;
                forward.Y = origin.Y;
                forward.Z = origin.Z - step.Z;

                backward.X = origin.X;
                backward.Y = origin.Y;
                backward.Z = origin.Z + step.Z;

                samples[offset, 0] = GetProfileBinForSample(top.X, top.Y, top.Z, profile_GPU);
                samples[offset, 1] = GetProfileBinForSample(bottom.X, bottom.Y, bottom.Z, profile_GPU);
                samples[offset, 2] = GetProfileBinForSample(left.X, left.Y, left.Z, profile_GPU);
                samples[offset, 3] = GetProfileBinForSample(right.X, right.Y, right.Z, profile_GPU);
                samples[offset, 4] = GetProfileBinForSample(forward.X, forward.Y, forward.Z, profile_GPU);
                samples[offset, 5] = GetProfileBinForSample(backward.X, backward.Y, backward.Z, profile_GPU);


                int countSamplesClosestThanOrigin = 0;

                //calculate color correction for all samples
                #region
                for (int index = 0; index < 6; index++)
                {
                    if (samples[offset, index].isempty == TRUE || samples[offset, index].isempty == 0.0)
                        continue;

                    BackGroundStrucuture temp_XYZ = addXYZ_st(samples[offset, index].MX, samples[offset, index].MY, samples[offset, index].MZ, BackgroundXYZ_GPU[offset]);

                    ProfileStrucuture PredictionlAB = XYZtoLAB_st(temp_XYZ);

                    diffL = PredictionlAB.L - foregroundColorToShow.ML;
                    diffA = PredictionlAB.A - foregroundColorToShow.MA;
                    diffB = PredictionlAB.B - foregroundColorToShow.MB;

                    diffL = diffL * diffL;
                    diffA = diffA * diffA;
                    diffB = diffB * diffB;

                    //curr_distanceLAB
                    samples[offset, index].distance = Math.Sqrt(diffL + diffA + diffB);
                    if (samples[offset, index].distance >= actualBin.distance)
                    {
                        continue;
                    }
                    else
                    {
                        samples[offset, index].isMoreAccurateThanOrigin = (int)TRUE;
                        countSamplesClosestThanOrigin++;
                    }
                }
                #endregion



                if (countSamplesClosestThanOrigin == 0)
                {
                    step = HalfTheStep(step);
                    continue;
                }



                //if there is at least one sample more accurate, it moves the origin in that direction, maintains the step and checks again
                else
                {
                    //6.1 calculates weights
                    double totalimprovements = 0;
                    for (int index = 0; index < 6; index++)
                    {
                        if (!(samples[offset, index].isMoreAccurateThanOrigin == TRUE))
                        {
                            continue;
                        }
                        totalimprovements += (actualBin.distance - samples[offset, index].distance);
                    }
                    for (int index = 0; index < 6; index++)
                    {
                        if (!(samples[offset, index].isMoreAccurateThanOrigin == 1))
                        {
                            continue;
                        }
                        samples[offset, index].weight = ((actualBin.distance - samples[offset, index].distance) / totalimprovements);
                    }
                    //6.2 calculates displacement
                    Point3D displacement = new Point3D(0, 0, 0);
                    for (int index = 0; index < 6; index++)
                    {
                        if (!(samples[offset, index].isMoreAccurateThanOrigin == 1))
                        {
                            continue;
                        }


                        displacement.X = displacement.X + (samples[offset, index].L - origin.X) * samples[offset, index].weight;
                        displacement.Y = displacement.Y + (samples[offset, index].A - origin.Y) * samples[offset, index].weight;
                        displacement.Z = displacement.Z + (samples[offset, index].B - origin.Z) * samples[offset, index].weight;
                    }

                    if (displacement.X > 0)
                        displacement.X = Math.Ceiling(displacement.X);
                    else
                        displacement.X = Math.Floor(displacement.X);

                    if (displacement.Y > 0)
                        displacement.Y = Math.Ceiling(displacement.Y);
                    else
                        displacement.Y = Math.Floor(displacement.Y);

                    if (displacement.Z > 0)
                        displacement.Z = Math.Ceiling(displacement.Z);
                    else
                        displacement.Z = Math.Floor(displacement.Z);

                    //6.3 pokes new origin
                    Point3D newOriginLoc = new Point3D();

                    newOriginLoc.X = origin.X + displacement.X;
                    newOriginLoc.Y = origin.Y + displacement.Y;
                    newOriginLoc.Z = origin.Z + displacement.Z;

                    ProfileStrucuture newOriginBin = GetProfileBin(newOriginLoc.X, newOriginLoc.Y, newOriginLoc.Z, profile_GPU);
                    while (newOriginBin.isempty == TRUE)
                    {
                        /////////////////////// round to even missing ///////////////////

                        //6.4 moves half the magnitude in the given direction
                        displacement.X = Math.Round(displacement.X / 2);
                        displacement.Y = Math.Round(displacement.Y / 2);
                        displacement.Z = Math.Round(displacement.Z / 2);

                        newOriginLoc.X = origin.X + displacement.X;
                        newOriginLoc.Y = origin.Y + displacement.Y;
                        newOriginLoc.Z = origin.Z + displacement.Z;

                        newOriginBin = GetProfileBin(newOriginLoc.X, newOriginLoc.Y, newOriginLoc.Z, profile_GPU);
                    }
                    //calclates the accuracy of the posible new origin
                    if (newOriginBin.isempty == TRUE)
                        return;

                    PredictionXYZ = addXYZ_st(newOriginBin.MX, newOriginBin.MY, newOriginBin.MZ, BackgroundXYZ_GPU[offset]);

                    ProfileStrucuture PredictionlAB = XYZtoLAB_st(PredictionXYZ);

                    diffL = PredictionlAB.L - foregroundColorToShow.ML;
                    diffA = PredictionlAB.A - foregroundColorToShow.MA;
                    diffB = PredictionlAB.B - foregroundColorToShow.MB;

                    //diffL = PredictionXYZ.X - foregroundColorToShow.MX;
                    //diffA = PredictionXYZ.Y - foregroundColorToShow.MY;
                    //diffB = PredictionXYZ.Z - foregroundColorToShow.MZ;

                    diffL = diffL * diffL;
                    diffA = diffA * diffA;
                    diffB = diffB * diffB;

                    //originBin.distanceLAB
                    newOriginBin.distance = Math.Sqrt(diffL + diffA + diffB);

                    if ((origin.X == newOriginLoc.X && origin.Y == newOriginLoc.Y && origin.Z == newOriginLoc.Z) || actualBin.distance <= newOriginBin.distance) // it's the same location then just reduces the step
                        step = HalfTheStep(step);
                    else
                    {
                        origin = newOriginLoc;
                        actualBin = newOriginBin;
                    }

                }

            }

            TestingStructure ValueToReturn = new TestingStructure();
            ValueToReturn.Given_R = actualBin.L;
            ValueToReturn.Given_G = actualBin.A;
            ValueToReturn.Given_B = actualBin.B;

            ValueToReturn.distance = actualBin.distance;

            ptr[offset] = ValueToReturn;

        }


        public static TestingStructure[] CorrectColour(System.Drawing.Color rgb, double X, double Y, double Z)
        {
            //rgb = System.Drawing.Color.FromArgb(69, 77, 217);
            //X = 0.0630982813175294;
            //Y = 0.616476271122916;
            //Z = 0.667048468232457;

            //cuda intializer
            CudafyModule km = CudafyModule.TryDeserialize();
            if (km == null || !km.TryVerifyChecksums())
            {
                // km = CudafyTranslator.Cudafy((typeof(ForeGroundStrucuture)), (typeof(BackGroundStrucuture)), typeof(Color));
                km = CudafyTranslator.Cudafy((typeof(ProfileStrucuture)), (typeof(ForeGroundStrucuture)), (typeof(BackGroundStrucuture)), (typeof(SampleStructure)), (typeof(TestingStructure)), typeof(quick_corr));
                km.TrySerialize();
            }

            CudafyTranslator.GenerateDebug = true;
            // cuda or emulator
            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId);
            //GPGPU gpu = CudafyHost.GetDevice(eGPUType.Emulator);
            Console.WriteLine("Running quick correction using {0}", gpu.GetDeviceProperties(false).Name);
            gpu.LoadModule(km);

            TestingStructure[] distance_CPU = new TestingStructure[1];

            // allocate memory on the GPU for the bitmap (same size as ptr)
            #region
            DataTable profile = new DataTable();
            try
            {
                // add the csv bin file
                using (GenericParserAdapter parser = new GenericParserAdapter(@"C:\lev\STColorCorrection\Data\PROFILE\p3700.csv"))
                {
                    System.Data.DataSet dsResult = parser.GetDataSet();
                    profile = dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            { Console.WriteLine(ex); }
            #endregion

            // allocate temp memory, initialize it, copy to constant memory on the GPU
            // L 0-21 A 0-41 B 0-45

            ProfileStrucuture[, ,] profiles_CPU = new ProfileStrucuture[21, 41, 45];
            ForeGroundStrucuture[] foregorungRGB_CPU = new ForeGroundStrucuture[1];
            BackGroundStrucuture[] BackgroundXYZ_CPU = new BackGroundStrucuture[1];
            SampleStructure[,] samples_CPU = new SampleStructure[1, 6];

            //profile inicialization
            #region
            for (int indexL = 0; indexL < 21; indexL++)
            {
                for (int indexA = 0; indexA < 41; indexA++)
                {
                    for (int indexB = 0; indexB < 45; indexB++)
                    {
                        profiles_CPU[indexL, indexA, indexB].L = indexL;
                        profiles_CPU[indexL, indexA, indexB].A = indexA;
                        profiles_CPU[indexL, indexA, indexB].B = indexB;
                        //profiles_CPU[indexL, indexA, indexB].Given_R = 0;
                        //profiles_CPU[indexL, indexA, indexB].Given_G = 0;
                        //profiles_CPU[indexL, indexA, indexB].Given_B = 0;
                        profiles_CPU[indexL, indexA, indexB].ML = 0;
                        profiles_CPU[indexL, indexA, indexB].MA = 0;
                        profiles_CPU[indexL, indexA, indexB].MB = 0;
                        profiles_CPU[indexL, indexA, indexB].MX = 0;
                        profiles_CPU[indexL, indexA, indexB].MY = 0;
                        profiles_CPU[indexL, indexA, indexB].MZ = 0;
                        profiles_CPU[indexL, indexA, indexB].distance = -1.0;
                        profiles_CPU[indexL, indexA, indexB].weight = -1.0;

                        profiles_CPU[indexL, indexA, indexB].isempty = TRUE;
                        profiles_CPU[indexL, indexA, indexB].isMoreAccurateThanOrigin = FALSE;
                    }
                }
            }



            int lvalue, avalue, bvalue;
            try
            {
                for (int i = 1; i < profile.Rows.Count; i++)
                {
                    lvalue = Convert.ToInt32(profile.Rows[i][0].ToString());
                    avalue = Convert.ToInt32(profile.Rows[i][1].ToString());
                    bvalue = Convert.ToInt32(profile.Rows[i][2].ToString());

                    lvalue = (int)(lvalue * 0.2);
                    avalue = (int)(avalue * 0.2) + 20;
                    bvalue = (int)(bvalue * 0.2) + 22;

                    profiles_CPU[lvalue, avalue, bvalue].L = lvalue;
                    profiles_CPU[lvalue, avalue, bvalue].A = avalue;
                    profiles_CPU[lvalue, avalue, bvalue].B = bvalue;

                    //profiles_CPU[lvalue, avalue, bvalue].Given_R = (double)Convert.ToByte(profile.Rows[i][9].ToString());
                    //profiles_CPU[lvalue, avalue, bvalue].Given_G = (double)Convert.ToByte(profile.Rows[i][10].ToString());
                    //profiles_CPU[lvalue, avalue, bvalue].Given_B = (double)Convert.ToByte(profile.Rows[i][11].ToString());

                    profiles_CPU[lvalue, avalue, bvalue].ML = (double)Convert.ToDouble(profile.Rows[i][3].ToString());
                    profiles_CPU[lvalue, avalue, bvalue].MA = (double)Convert.ToDouble(profile.Rows[i][4].ToString());
                    profiles_CPU[lvalue, avalue, bvalue].MB = (double)Convert.ToDouble(profile.Rows[i][5].ToString());

                    profiles_CPU[lvalue, avalue, bvalue].MX = (double)Convert.ToDouble(profile.Rows[i][6].ToString());
                    profiles_CPU[lvalue, avalue, bvalue].MY = (double)Convert.ToDouble(profile.Rows[i][7].ToString());
                    profiles_CPU[lvalue, avalue, bvalue].MZ = (double)Convert.ToDouble(profile.Rows[i][8].ToString());


                    profiles_CPU[lvalue, avalue, bvalue].isempty = FALSE;


                }

            }
            catch (Exception ex)
            { Console.WriteLine(ex); }
            #endregion
            
            //grab the colors
            ProfileStrucuture[, ,] profile_GPU = gpu.CopyToDevice(profiles_CPU);
            SampleStructure[,] samples_GPU = gpu.CopyToDevice(samples_CPU);

            Point3D background = new Point3D(X, Y, Z);

            //foreground and background image inicialization
            #region
            try
            {
                for (int i = 0; i < 1; i++)
                {
                    foregorungRGB_CPU[i].R = rgb.R;
                    foregorungRGB_CPU[i].G = rgb.G;
                    foregorungRGB_CPU[i].B = rgb.B;

                    BackgroundXYZ_CPU[i].X = background.X;
                    BackgroundXYZ_CPU[i].Y = background.Y;
                    BackgroundXYZ_CPU[i].Z = background.Z;
                }
            }
            catch (Exception ex)
            { Console.WriteLine(ex); }
            #endregion

            //begin execution
            // capture the start time
            gpu.StartTimer();
            ForeGroundStrucuture[] foregorungRGB_GPU = gpu.CopyToDevice(foregorungRGB_CPU);
            BackGroundStrucuture[] BackgroundXYZ_GPU = gpu.CopyToDevice(BackgroundXYZ_CPU);

            //out put
            TestingStructure[] distance_GPU = gpu.Allocate(distance_CPU);

            // generate a bitmap from our sphere data
            //Image size: 1024 x 768

            //dim3 grids = new dim3(1024 / 16, 768 / 16);
            //dim3 threads = new dim3(16, 16);

            dim3 grids = new dim3(1, 1);
            dim3 threads = new dim3(1, 1);

            //quick_correct
            //gpu.Launch(grids, threads, ((Action<GThread, ProfileStrucuture[, ,], ForeGroundStrucuture[], BackGroundStrucuture[], ProfileStrucuture[], SampleStructure[,]>)QuickCorr), profile_GPU, foregorungRGB_GPU, BackgroundXYZ_GPU, distance_GPU, samples_GPU);

            //quick correct - testing
            gpu.Launch(grids, threads, ((Action<GThread, ProfileStrucuture[, ,], ForeGroundStrucuture[], BackGroundStrucuture[], TestingStructure[], SampleStructure[,]>)QuickCorr), profile_GPU, foregorungRGB_GPU, BackgroundXYZ_GPU, distance_GPU, samples_GPU);


            // copy our bitmap back from the GPU for display
            gpu.CopyFromDevice(distance_GPU, distance_CPU);


            // get stop time, and display the timing results
            double elapsedTime = gpu.StopTimer();
            distance_CPU[0].execution_time = elapsedTime;
            Console.WriteLine("Time to generate: {0} ms", elapsedTime);
            gpu.Free(foregorungRGB_GPU);
            gpu.Free(BackgroundXYZ_GPU);
            gpu.Free(distance_GPU);
            gpu.FreeAll();

            return distance_CPU;

        }

        //public static TestingStructure[] Execute()
        //{

        //    //cuda intializer
        //    CudafyModule km = CudafyModule.TryDeserialize();
        //    if (km == null || !km.TryVerifyChecksums())
        //    {
        //        // km = CudafyTranslator.Cudafy((typeof(ForeGroundStrucuture)), (typeof(BackGroundStrucuture)), typeof(Color));
        //        km = CudafyTranslator.Cudafy((typeof(ProfileStrucuture)), (typeof(ForeGroundStrucuture)), (typeof(BackGroundStrucuture)), (typeof(SampleStructure)), typeof(quick_corr));
        //        km.TrySerialize();
        //    }

        //    CudafyTranslator.GenerateDebug = true;
        //    // cuda or emulator
        //    //GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId);
        //    GPGPU gpu = CudafyHost.GetDevice(eGPUType.Emulator);
        //    Console.WriteLine("Running examples using {0}", gpu.GetDeviceProperties(false).Name);
        //    gpu.LoadModule(km);

        //    TestingStructure[] distance_CPU = new TestingStructure[786432];

        //    // allocate memory on the GPU for the bitmap (same size as ptr)
        //    #region
        //    DataTable profile = new DataTable();
        //    try
        //    {
        //        // add the csv bin file
        //        using (GenericParserAdapter parser = new GenericParserAdapter(@"C:\lev\STColorCorrection\Data\PROFILE\p3700.csv"))
        //        {
        //            System.Data.DataSet dsResult = parser.GetDataSet();
        //            profile = dsResult.Tables[0];
        //        }
        //    }
        //    catch (Exception ex)
        //    { Console.WriteLine(ex); }
        //    #endregion

        //    // allocate temp memory, initialize it, copy to constant memory on the GPU
        //    // L 0-21 A 0-41 B 0-45

        //    ProfileStrucuture[, ,] profiles_CPU = new ProfileStrucuture[21, 41, 45];
        //    ForeGroundStrucuture[] foregorungRGB_CPU = new ForeGroundStrucuture[786432];
        //    BackGroundStrucuture[] BackgroundXYZ_CPU = new BackGroundStrucuture[786432];
        //    SampleStructure[,] samples_CPU = new SampleStructure[786432, 6];

        //    //profile inicialization
        //    #region
        //    for (int indexL = 0; indexL < 21; indexL++)
        //    {
        //        for (int indexA = 0; indexA < 41; indexA++)
        //        {
        //            for (int indexB = 0; indexB < 45; indexB++)
        //            {
        //                profiles_CPU[indexL, indexA, indexB].L = indexL;
        //                profiles_CPU[indexL, indexA, indexB].A = indexA;
        //                profiles_CPU[indexL, indexA, indexB].B = indexB;
        //                //profiles_CPU[indexL, indexA, indexB].Given_R = 0;
        //                //profiles_CPU[indexL, indexA, indexB].Given_G = 0;
        //                //profiles_CPU[indexL, indexA, indexB].Given_B = 0;
        //                profiles_CPU[indexL, indexA, indexB].ML = 0;
        //                profiles_CPU[indexL, indexA, indexB].MA = 0;
        //                profiles_CPU[indexL, indexA, indexB].MB = 0;
        //                profiles_CPU[indexL, indexA, indexB].MX = 0;
        //                profiles_CPU[indexL, indexA, indexB].MY = 0;
        //                profiles_CPU[indexL, indexA, indexB].MZ = 0;
        //                profiles_CPU[indexL, indexA, indexB].distance = -1.0;
        //                profiles_CPU[indexL, indexA, indexB].weight = -1.0;

        //                profiles_CPU[indexL, indexA, indexB].isempty = TRUE;
        //                profiles_CPU[indexL, indexA, indexB].isMoreAccurateThanOrigin = FALSE;
        //            }
        //        }
        //    }



        //    int lvalue, avalue, bvalue;
        //    int temp_l, temp_a, temp_b;
        //    try
        //    {
        //        for (int i = 1; i < profile.Rows.Count; i++)
        //        {
        //            lvalue = Convert.ToInt32(profile.Rows[i][0].ToString());
        //            avalue = Convert.ToInt32(profile.Rows[i][1].ToString());
        //            bvalue = Convert.ToInt32(profile.Rows[i][2].ToString());

        //            temp_l = lvalue;
        //            temp_a = avalue;
        //            temp_b = bvalue;

        //            lvalue = (int)(lvalue * 0.2);
        //            avalue = (int)(avalue * 0.2) + 20;
        //            bvalue = (int)(bvalue * 0.2) + 22;

        //            profiles_CPU[lvalue, avalue, bvalue].L = lvalue;
        //            profiles_CPU[lvalue, avalue, bvalue].A = avalue;
        //            profiles_CPU[lvalue, avalue, bvalue].B = bvalue;

        //            //profiles_CPU[lvalue, avalue, bvalue].Given_R = (double)Convert.ToByte(profile.Rows[i][9].ToString());
        //            //profiles_CPU[lvalue, avalue, bvalue].Given_G = (double)Convert.ToByte(profile.Rows[i][10].ToString());
        //            //profiles_CPU[lvalue, avalue, bvalue].Given_B = (double)Convert.ToByte(profile.Rows[i][11].ToString());

        //            profiles_CPU[lvalue, avalue, bvalue].ML = (double)Convert.ToDouble(profile.Rows[i][3].ToString());
        //            profiles_CPU[lvalue, avalue, bvalue].MA = (double)Convert.ToDouble(profile.Rows[i][4].ToString());
        //            profiles_CPU[lvalue, avalue, bvalue].MB = (double)Convert.ToDouble(profile.Rows[i][5].ToString());

        //            profiles_CPU[lvalue, avalue, bvalue].MX = (double)Convert.ToDouble(profile.Rows[i][6].ToString());
        //            profiles_CPU[lvalue, avalue, bvalue].MY = (double)Convert.ToDouble(profile.Rows[i][7].ToString());
        //            profiles_CPU[lvalue, avalue, bvalue].MZ = (double)Convert.ToDouble(profile.Rows[i][8].ToString());


        //            profiles_CPU[lvalue, avalue, bvalue].isempty = FALSE;


        //        }

        //    }
        //    catch (Exception ex)
        //    { Console.WriteLine(ex); }
        //    #endregion



        //    ProfileStrucuture[, ,] profile_GPU = gpu.CopyToDevice(profiles_CPU);
        //    SampleStructure[,] samples_GPU = gpu.CopyToDevice(samples_CPU);

        //    System.Drawing.Color foreground = System.Drawing.Color.FromArgb(97, 195, 86);

        //    Point3D background = new Point3D(0.661546612768968, 0.878671692627795, 0.328385183960379);


        //        //foreground and background image inicialization
        //        #region
        //        try
        //        {
        //            for (int i = 0; i < 786432; i++)
        //            {
        //                foregorungRGB_CPU[i].R = foreground.R;
        //                foregorungRGB_CPU[i].G = foreground.G;
        //                foregorungRGB_CPU[i].B = foreground.B;

        //                BackgroundXYZ_CPU[i].X = background.X;
        //                BackgroundXYZ_CPU[i].Y = background.Y;
        //                BackgroundXYZ_CPU[i].Z = background.Z;
        //            }
        //        }
        //        catch (Exception ex)
        //        { Console.WriteLine(ex); }
        //        #endregion



        //        // capture the start time
        //        gpu.StartTimer();
        //        ForeGroundStrucuture[] foregorungRGB_GPU = gpu.CopyToDevice(foregorungRGB_CPU);
        //        BackGroundStrucuture[] BackgroundXYZ_GPU = gpu.CopyToDevice(BackgroundXYZ_CPU);

        //        //out put
        //        TestingStructure[] distance_GPU = gpu.Allocate(distance_CPU);




        //        // generate a bitmap from our sphere data
        //        //Image size: 1024 x 768

        //        //dim3 grids = new dim3(1024 / 16, 768 / 16);
        //        //dim3 threads = new dim3(16, 16);

        //        dim3 grids = new dim3(1, 1);
        //        dim3 threads = new dim3(1, 1);

        //        //brute force
        //        //gpu.Launch(grids, threads, ((Action<GThread, ProfileStrucuture[, ,], ForeGroundStrucuture[], BackGroundStrucuture[], double[]>)Bruteforce), profile_GPU, foregorungRGB_GPU, BackgroundXYZ_GPU, distance_GPU);

        //        //quick_correct
        //        gpu.Launch(grids, threads, ((Action<GThread, ProfileStrucuture[, ,], ForeGroundStrucuture[], BackGroundStrucuture[], TestingStructure[], SampleStructure[,]>)QuickCorr), profile_GPU, foregorungRGB_GPU, BackgroundXYZ_GPU, distance_GPU, samples_GPU);

        //        //gpu.Launch(grids, threads, ((Action<GThread, ForeGroundStrucuture[], BackGroundStrucuture[], double[]>)Bruteforce), foregorungRGB_GPU, BackgroundXYZ_GPU, distance_GPU);

        //        // copy our bitmap back from the GPU for display
        //        gpu.CopyFromDevice(distance_GPU, distance_CPU);


        //        // get stop time, and display the timing results
        //        double elapsedTime = gpu.StopTimer();
        //        return distance_CPU;
        //        Console.WriteLine("Time to generate: {0} ms", elapsedTime);
        //        gpu.Free(foregorungRGB_GPU);
        //        gpu.Free(BackgroundXYZ_GPU);
        //        gpu.Free(distance_GPU);





            
        //    gpu.FreeAll();

        //}

    }

}
