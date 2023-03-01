using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
class CalaculTransformationMatrix
{
        float[][] A, B;



        public  float[][] calculate(ref KinectWrapper.NuiSkeletonFrame[] MySkeletonFrameTable, int id1, int id2)
        {
            float[][] result = new float[9][];


            copyData(MySkeletonFrameTable, id1, id2);

            CalaculesMatrices cal = new CalaculesMatrices(A, B, 9);

          

            return cal.getResult(); 

        }
        

        void copyData( KinectWrapper.NuiSkeletonFrame[] MySkeletonFrameTable, int id1, int id2)
        {
             A = new float[60][];
            B = new float[60][];
            
            
            for (int i = 0; i < A.Length; i++)
            {
                B[i] = new float[1];
                A[i] = new float[9];
                for (int j = 0; j < 9; j++)
                {
                    A[i][j] = 0f;
                }
            }


            int k = 0;
            for (int i = 0 ; i < MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions.Length; i++ )
            {

                A[k][0]     = MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[i].x;    A[k][1]     = MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[i].y;   A[k][2]     = MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[i].z;
                A[k + 1][3] = MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[i].x;    A[k + 1][4] = MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[i].y;   A[k + 1][5] = MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[i].z;
                A[k + 2][6] = MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[i].x;    A[k + 2][7] = MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[i].y;   A[k + 2][8] = MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[i].z;

                k += 3;
            }

            k = 0;
            for (int i = 0; i < MySkeletonFrameTable[id2].SkeletonData[0].SkeletonPositions.Length; i++)
            {
                B[k][0]   = MySkeletonFrameTable[id2].SkeletonData[0].SkeletonPositions[i].x;
                B[k+1][0] = MySkeletonFrameTable[id2].SkeletonData[0].SkeletonPositions[i].y;
                B[k+2][0] = MySkeletonFrameTable[id2].SkeletonData[0].SkeletonPositions[i].z;
                k +=3;
            }


        }

}
