#include <stdio.h>
#include <time.h>
#include <math.h>
#include <float.h>

#include "cuda.h"
#include "cuda_runtime.h"
#include "device_launch_parameters.h"
 
const int blocksize = 1; 
const int N = 16; 


const int PROFILE_SIZE = 8376;
const int PROFILE_ARRAY_SIZE = PROFILE_SIZE * 6;
__device__ const int GPU_PROFILE_SIZE = 8376;

const int FRAME_DIMENSIONS = 800 * 600;
const int FRAME_ARRAY_SIZE = FRAME_DIMENSIONS * 3;
__device__ const int GPU_FRAME_DIMENSIONS = 800 * 600;

__device__ double X,Y,Z,L,A,B;

__device__ double distance(double L1,double A1,double B1,double L2,double A2,double B2)
{
	 double l, a, b, result,sqresult;
      l = L1 - L2;
      a = A1 - A2;
      b = B1 - B2;
      l = l * l;
      a = a * a;
      b = b * b;
	  result = l + a + b;

	  sqresult = sqrt(result);
	  return sqresult;
}

__device__ void addXYZ(double X1,double Y1,double Z1,double X2,double Y2,double Z2)
{
	X=X1+X2;
	Y=Y1+Y2;
	Z=Z1+Z2;
/*
	if(X>0.9504)
		X=0.9504;
	else if (X<0)
		X=0;
	else
		X=X;
	
	if(X>1)
		Y=1
	else if (Y<0)
		Y=0;
	else
		Y=Y;

	if(Z>1.0888)
		Z=1.0888;
	else if (Z<0)
		Z=0;
	else
		Z=Z;
*/
}

// rgb to xyz
//__device__ void RGBToXYZ(unsigned char R,unsigned char G,unsigned char B )
__device__ void RGBToXYZ(int R,int G,int B )
    {
      // by the formula given the the web page http://www.brucelindbloom.com/index.html [XYZ]=[M][RGB]
      //In order to properly use this matrix, the RGB values must be linear and in the nominal range [0.0, 1.0].
      // RGB values may first need conversion (for example, dividing by 255 and then raising them to a power)
      // Where M for D65:	 0.4124564  0.3575761  0.1804375
      //0.2126729  0.7151522  0.0721750
      //0.0193339  0.1191920  0.9503041

      //// to make rgb values linear red, green, blue values
      double rLinear = (double)R / 255.0;
      double gLinear = (double)G / 255.0;
      double bLinear = (double)B / 255.0;

      // convert to a sRGB form

      //double r =  Math.pow((rLinear ), 2.2) ;
      //double g =  Math.pow((gLinear ), 2.2) ;
      //double b = Math.pow((bLinear ), 2.2) ;
      double r, g, b;

      if (rLinear > 0.04045)
        r = pow(((rLinear + 0.055) / 1.055), 2.2);
      else
        r = rLinear / 12.92;

      if (gLinear > 0.04045)
        g = pow(((gLinear + 0.055) / 1.055), 2.2);
      else
        g = gLinear / 12.92;

      if (bLinear > 0.04045)
        b = pow(((bLinear + 0.055) / 1.055), 2.2);
      else
        b = bLinear / 12.92;


		X=(r * 0.4124564 + g * 0.3575761 + b * 0.1804375);
        Y=(r * 0.2126729 + g * 0.7151522 + b * 0.0721750);
        Z=(r * 0.0193339 + g * 0.1191920 + b * 0.9503041);

		/*
	if(X>0.9504)
		X=0.9504;
	else if (X<0)
		X=0;
	else
		X=X;
	
	if(X>1)
		Y=1
	else if (Y<0)
		Y=0;
	else
		Y=Y;

	if(Z>1.0888)
		Z=1.0888;
	else if (Z<0)
		Z=0;
	else
		Z=Z;
*/
    }

__device__  double FX(double e)
    {
		if(e > 0.008856) 
			e=pow(e, (1.0 / 3.0));
		else
			e=(903.3 * e + 16) / 116;
			return e;
    }

__device__ double Lxyz(double e)
    {
      if (e > 0.008856) 
	  e= (116 * pow(e, (1.0 / 3.0))) - 16 ;
	  else
	  e= (903.3 * e);
	  return e;
    }

//__device__ void ToLAB(unsigned char R,unsigned char G,unsigned char B )
__device__ void ToLAB(int R,int G,int B )
    {
      double Fx, Fy, Fz;
     
      RGBToXYZ(R,G,B);

      double yr = Y / 1.0000;
      double xr = X /0.9504;
      double zr = Z / 1.0888;

      Fx = FX(xr);
      Fy = FX(yr);
      Fz = FX(zr);

      L = Lxyz(yr);
      A = 500 * (Fx - Fy);
      B = 200 * (Fy - Fz);

     
    }

__device__ void XYZtoLAB(double X1, double Y1,double Z1)
{
	  double Fx, Fy, Fz;
           
      double yr = Y1 / 1.0000;
      double xr = X1 /0.9504;
      double zr = Z1 / 1.0888;

      Fx = FX(xr);
      Fy = FX(yr);
      Fz = FX(zr);

      L = Lxyz(yr);
      A = 500 * (Fx - Fy);
      B = 200 * (Fy - Fz);
}

__device__ void LABToXYZ(double L1,double A1,double B1)
    {
      double  xr, yr, zr, Xr, Yr, Zr, Fx, Fy, Fz;

      Fy = (L1 + 16) / 116;

      Fx = (A1 / 500) + Fy;

      Fz = Fy - (B1 / 200);

      if (pow(Fx, 3) > (0.008856)) 
		xr=pow(Fx, 3);
	  else
	    xr=((116 * Fx - 16) / 903.3);

      if(L1 > (0.008856 * 903.3))
		yr= pow(Fy, 3) ; 
	  else
		yr=L1 / 903.3;

      if (pow(Fz, 3) > (0.008856)) 
		zr= pow(Fz, 3);
	  else
		zr= ((116 * Fz - 16) / 903.3);

      //for D65
      Xr = 0.9504;

      Yr = 1;

      Zr = 1.0888;

      X = xr * Xr;
      Y = yr * Yr;
      Z = zr * Zr;

	/*
	if(X>0.9504)
		X=0.9504;
	else if (X<0)
		X=0;
	else
		X=X;
	
	if(X>1)
		Y=1
	else if (Y<0)
		Y=0;
	else
		Y=Y;

	if(Z>1.0888)
		Z=1.0888;
	else if (Z<0)
		Z=0;
	else
		Z=Z;
*/
	  }
	
 __global__ void correct(int *block_frame, double *block_background, double *block_profile,char *a, int *b) 
{
	a[threadIdx.x] += b[threadIdx.x];
	//GPU_FRAME_DIMENSIONS
	for(int pixel = 0 ; pixel < 1 ; pixel++)
	{
		int R = block_frame[3*pixel + 0];
		int G = block_frame[3*pixel + 1];
		int B = block_frame[3*pixel + 2];

		double bgX = block_background[3*pixel + 0];
		double bgY = block_background[3*pixel + 1];
		double bgZ = block_background[3*pixel + 2];

		/*double bgX1 = block_background[3*pixel + 0];
		double bgY1 = block_background[3*pixel + 1];
		double bgZ1 = block_background[3*pixel + 2];

		block_frame[3*pixel + 0]=(int)(block_background[3*pixel + 0]*block_background[3*pixel + 0]);
		block_frame[3*pixel + 1]=(int)(block_background[3*pixel + 1]*block_background[3*pixel + 1]);
		block_frame[3*pixel + 2]=(int)(block_background[3*pixel + 2]*block_background[3*pixel + 2]);

	}*/

		
		double FrameX,FrameY,FrameZ,FrameL,FrameA,FrameB,BlendL,BlendA,BlendB;
		
		
		// to find the best fit bin color
		// SAVES LAB in device global L,A,B variables
		ToLAB(R,G,B);
		FrameL=L;
		FrameA=A;
		FrameB=B;

		double DistanceInBin =  DBL_MIN;
		double ClosestBinDistance;
		int BinIndex=0;
		
		double keyL;
		double keyA;
		double keyB;

		for(int bin = 0 ; bin < PROFILE_SIZE ; bin++)
		{			
			 keyL = block_profile[6*bin + 0];
			 keyA = block_profile[6*bin + 1];
			 keyB = block_profile[6*bin + 2];

			 ClosestBinDistance=distance(FrameL,FrameA,FrameB,keyL,keyA,keyB);

			 if (DistanceInBin >= ClosestBinDistance)
	               continue;
				
	          DistanceInBin = ClosestBinDistance;
			  BinIndex=bin;
		}

		FrameL=block_profile[6*BinIndex + 3];
		FrameA=block_profile[6*BinIndex + 4];
		FrameB=block_profile[6*BinIndex + 5];

		//DO YOUR MAGIC
		DistanceInBin =  DBL_MIN;
		ClosestBinDistance=0;
		BinIndex=0;
		for(int bin = 0 ; bin < PROFILE_SIZE ; bin++)
		{			
			
			double valueL = block_profile[6*bin + 3];
			double valueA = block_profile[6*bin + 4];
			double valueB = block_profile[6*bin + 5];

			//getting the xyz values of the chocen bin
			LABToXYZ(valueL,valueA,valueB);

			FrameX=X;
			FrameY=Y;
			FrameZ=Z;

			addXYZ(FrameX,FrameY,FrameZ,bgX,bgY,bgZ);
			
			XYZtoLAB(X,Y,Z);
			BlendL=L;
			BlendA=A;
			BlendB=B;


			ClosestBinDistance=distance(FrameL,FrameA,FrameB,BlendL,BlendA,BlendB);

			 if (DistanceInBin >= ClosestBinDistance)
	               continue;
				
	             DistanceInBin = ClosestBinDistance;
			  BinIndex=bin;

		}

		block_frame[3*pixel + 0]=(int)block_profile[6*BinIndex + 3];
		block_frame[3*pixel + 1]=(int)block_profile[6*BinIndex + 4];
		block_frame[3*pixel + 2]=(int)block_profile[6*BinIndex + 5];
	
	}
}

//basic cuda whihc runs on a single thread 
int main(int argc, char** argv)
{
	//
	char a[N] = "Hello \0\0\0\0\0\0";
	int b[N] = {15, 10, 6, 0, -11, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

	char *ad;
	int *bd;
	const int csize = N*sizeof(char);
	const int isize = N*sizeof(int);
 
	printf("%s", a);

	///0
	clock_t tstart;
	clock_t end;
	double runTime;

	double *background,*profile;
	//unsigned char *frame;
	int *frame;

	//var display-profile -- lookup table in LAB
	profile = (double*) malloc(PROFILE_ARRAY_SIZE * sizeof(double));

	for(int index = 0 ; index < PROFILE_ARRAY_SIZE ; index++)
		profile[index] = 30;

	//var frame-image -- image to correct in RGB
	/*frame=	(unsigned char*)malloc(FRAME_ARRAY_SIZE * sizeof(unsigned char));
	for(int index = 0 ; index < FRAME_ARRAY_SIZE ; index++)
		frame[index] = '3';
	*/

	frame =	(int*)malloc(FRAME_ARRAY_SIZE * sizeof(int));
	for(int index = 0 ; index < FRAME_ARRAY_SIZE ; index++)
		frame[index] = 1;
	//var background-image -- background image in YXZ
	background= (double*)malloc(FRAME_ARRAY_SIZE * sizeof(double));
	for(int index = 0 ; index < FRAME_ARRAY_SIZE ; index++)
		background[index] = 0;

	//pointers on the device
	double *gpu_profile;
	//unsigned char *gpu_frame;
	int *gpu_frame;

	double *gpu_background;
	printf("prg starting\n");

	const int psize = PROFILE_ARRAY_SIZE * sizeof(double);
	//const int fsize = FRAME_ARRAY_SIZE * sizeof(unsigned char);
	const int fsize = FRAME_ARRAY_SIZE * sizeof(int);
	const int bgsize = FRAME_ARRAY_SIZE * sizeof(double);
	
	//memory allocation on the GPU
	cudaMalloc(&gpu_profile, PROFILE_ARRAY_SIZE * sizeof(double)); 
	//cudaMalloc(&gpu_frame, FRAME_ARRAY_SIZE * sizeof(unsigned char)); 
	cudaMalloc(&gpu_frame, FRAME_ARRAY_SIZE * sizeof(int)); 
	cudaMalloc(&gpu_background, FRAME_ARRAY_SIZE * sizeof(double)); 

	//1- copy the profile
	cudaMemcpy(gpu_profile, profile, psize, cudaMemcpyHostToDevice); 

	// start outer timer

	//-- for 100 frames
	for(int f = 0 ; f < 1 ; f++)
	{
		//start inner timer
		cudaEvent_t start, stop;
		cudaEventCreate(&start);
		cudaEventCreate(&stop);

		
		//2- pass the image to correct to the GPU
		//cudaMemcpy(gpu_frame, frame, FRAME_ARRAY_SIZE * sizeof(unsigned char), cudaMemcpyHostToDevice); 
		cudaMemcpy(gpu_frame, frame, FRAME_ARRAY_SIZE * sizeof(int), cudaMemcpyHostToDevice); 

		//3- pass the background image to the GPU
		cudaMemcpy(gpu_background, background, FRAME_ARRAY_SIZE * sizeof(double), cudaMemcpyHostToDevice); 
		
		////////////////////
		cudaMalloc((void**)&ad, csize ); 
		cudaMalloc((void**)&bd, isize ); 
		cudaMemcpy(ad, a, csize, cudaMemcpyHostToDevice ); 
		cudaMemcpy(bd, b, isize, cudaMemcpyHostToDevice ); 
		/////////////////////
		//4- call the kernel
		dim3 dimBlock( blocksize, 1 );
		dim3 dimGrid( 1, 1 );
	
		tstart = clock();
				// Start record
		cudaEventRecord(start, 0);
		correct<<<dimGrid, dimBlock>>>(gpu_frame, gpu_background,gpu_profile,ad, bd);
		// Stop event
		cudaEventRecord(stop, 0);
		
		cudaEventSynchronize(stop);

		float elapsedTime=-1;
		cudaEventElapsedTime(&elapsedTime, start, stop);
		// that's our time!
		
		printf("Run time is: %f \n",elapsedTime);
		// Clean up:
		cudaEventDestroy(start);
		cudaEventDestroy(stop);
		
		end = clock();
	
		runTime = ((end-tstart));
		///////////////////////////
		cudaMemcpy( a, ad, csize, cudaMemcpyDeviceToHost ); 
		cudaFree(ad);
		cudaFree(bd);
		
		printf("%s\n", a);
		/////////////////////////////////
		//cudaMemcpy(frame, gpu_frame, FRAME_ARRAY_SIZE * sizeof(unsigned char), cudaMemcpyDeviceToHost); 
		cudaMemcpy(frame, gpu_frame, FRAME_ARRAY_SIZE * sizeof(int), cudaMemcpyDeviceToHost); 
		//cudaMemcpy(background, gpu_background, FRAME_ARRAY_SIZE * sizeof(unsigned char), cudaMemcpyDeviceToHost); 
		cudaFree( gpu_frame );
		cudaFree( gpu_background );

		//5- copy the corrected image back to the CPU
		
		for(int index = 0 ; index < 3 ; index++)
		printf("%d\n", frame[index]);
		//stop inner timer
		//print inner timer -- 33 milliseconds MAX
	}

	//printf("Run time is %g seconds\n",runTime);
	cudaFree( gpu_profile );
	//system("PAUSE");	
	// stop outer timer
	// print outer timer -- 3333 millisenconds MAX
	return EXIT_SUCCESS;
}

int main1(int argc, char** argv)
{
	return 0;
}