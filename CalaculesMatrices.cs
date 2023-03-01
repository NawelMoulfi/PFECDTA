using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class CalaculesMatrices
{

        float[][] matrix, identity;
        float[][] B;
        int tailleIdentity = -1;


        float[][] mymatrix, original;
        float[][] myidentity;
        float[][] invers;
        
        float[][] ATXACopy;


        public CalaculesMatrices(float[][] matrixOriginal , float[][] B,int  tailleIdentity)
        {

            this.matrix = copyMatrix(matrixOriginal);
            //Debug.Log("**********************************    matrix    **************************************************");
            //afficherMatrix(matrix);


            this.B = copyMatrix(B);
            //Debug.Log("**********************************     --B--    **************************************************");
            //afficherMatrix(this.B);


            this.tailleIdentity = tailleIdentity;

            initIdentity(ref identity, tailleIdentity);
            //Debug.Log("**********************************     identity    **************************************************");
            //afficherMatrix(this.identity);

   

            float[][] Atranspose = transpose(matrixOriginal);
            //Debug.Log("**********************************     Atranspose    **************************************************");
            //afficherMatrix(Atranspose);


            float[][] ATXA = multipleMatrix(Atranspose, matrixOriginal);
            //Debug.Log("**********************************     ATXA    **************************************************");
            //afficherMatrix(ATXA);


            ATXACopy = copyMatrix(ATXA);
            //Debug.Log("**********************************     ATXACopy    **************************************************");
            //afficherMatrix(ATXACopy);


            invers = calculateInversMatrix(ATXA, identity);
            //Debug.Log("**********************************     invers    **************************************************");
            //afficherMatrix(invers);


            if (verifierInverse(ATXACopy, invers))
            {
                Debug.Log(" ----------------------    --------------------       Inversible     -------------------------------------------------------------");
            }
            else{
                Debug.Log("------------------------   ----------- --- --- --    Non Inversible ----------------------------------------------------------------------");
            }

            float[][] ATXB = multipleMatrix(Atranspose, B);
            //Debug.Log("**********************************     ATXB    **************************************************");
            //afficherMatrix(ATXB);


            inversXATXB = multipleMatrix(invers, ATXB);
            Debug.Log("**********************************     inversXATXB    **************************************************");
            afficherMatrix(inversXATXB);

          
            

        }
        float[][] inversXATXB;
        public float[][] getResult()
        {
            return inversXATXB;
        }

        void initIdentity(ref float[][] identity, int tailleIdentity)
        {
            int n = tailleIdentity;
           

            identity = new float[n][];

            for (int i = 0; i < n; i++)
            {
                identity[i] = new float[n];

                for (int j = 0; j < n; j++)
                {
                    if(i==j)
                        identity[i][j] = 1;
                    else
                        identity[i][j] = 0;
                }
            }
        }
        public void afficherMatrix(float[][] matrixPar)
        {
            for (int i = 0; i < matrixPar.Length; i++)
            {
                string s = "";
                for (int jj = 0; jj < matrixPar[i].Length; jj++)
                {
                    s = s + "[" + i + "][" + jj + "] = " + matrixPar[i][jj] + "    ";
                }
                Debug.Log(s + "\n");
            }
        }


       

        float[][] MyMatrix = new float[3][];
        float[][] MyIdentity = new float[3][];


        public float[][] calculateInversMatrix(float[][] matrix, float[][] identity)
        {
           
            
            //afficherMatrix(matrix);
            //afficherMatrix(identity);



            bool terminer = false;
            int j = 1;
            for (int i = 0; i < matrix.Length; i++)
            {
                j = 1;
                terminer = false;
                while (!terminer)// && i<9 && (i+j)<9) 
                {
                    if (matrix[i][i] != 0)
                    {
                        bool pivotChoisi = false;
                        float pivo = 1f;
                        for (int k = 0; k < matrix.Length; k++)
                        {
                            if (!pivotChoisi)
                            {
                                pivo = matrix[i][i];
                                pivotChoisi = true;
                            }

                            matrix[i][k] = matrix[i][k] / pivo;

                            identity[i][k] = identity[i][k] / pivo;

                        }
                        //Debug.Log("L[" + i + "]" + i + "<> L[" + i + "] /" + pivo);
                        //afficherMatrix(matrix);
                       // afficherMatrix(identity);

                        for (int g = (i + 1); g < matrix.Length; g++)
                        {
                            float piv = matrix[g][i];
                            
                            for (int m = 0; m < matrix.Length; m++)
                            {

                                matrix[g][m] = matrix[g][m] - piv * matrix[i][m];
                                identity[g][m] = identity[g][m] - piv * identity[i][m];
                                //Debug.Log("matrix[" + g + "][" + m + "] = matrix[" + g + "][" + m + "] - matrix[" + g + "][" + i + "] * matrix[" + i + "][" + m + "]");

                            }


                            //afficherMatrix(matrix);
                            //afficherMatrix(identity);
                        }
                        terminer = true;
                    }
                    else
                    {
                       // Debug.Log("i + j = " +(i + j));
                        if ((i+j)>= matrix.Length)
                        {
                            break;
                        }
                        if (matrix[i + j][i] != 0)
                        {
                            bool pivotChoisi = false;
                            float pivo = 1f;
                            for (int k = 0; k < matrix.Length; k++)
                            {
                                if (!pivotChoisi)
                                {
                                    pivo = matrix[i + j][i];
                                    pivotChoisi = true;
                                }

                                float temp = matrix[i][k];
                                matrix[i][k] = matrix[i + j][k] / pivo;
                                matrix[i + j][k] = temp;

                                float temp1 = identity[i][k];
                                identity[i][k] = identity[i + j][k] / pivo;
                                identity[i + j][k] = temp1;

                            }

                            //Debug.Log(" L[" + (i + j) + "] <>" + " L[" + i + "] " + "    L[" + i + "]" + i + "<> L[" + (i + j) + "] /" + pivo + "       ");
                           // afficherMatrix(matrix);
                            //afficherMatrix(identity);

                            terminer = true;

                            for (int g = (i + 1); g < matrix.Length; g++)
                            {
                                identity[g][0] = identity[g][0] - matrix[g][i] * identity[i][0];
                                for (int m = 1; m < matrix.Length; m++)
                                {
                                    matrix[g][m] = matrix[g][m] - matrix[g][i] * matrix[i][m];
                                    identity[g][m] = identity[g][m] - matrix[g][i] * identity[i][m];
                                   // Debug.Log("matrix[" + g + "][" + m + "] = matrix[" + g + "][" + m + "] - matrix[" + g + "][" + i + "] * matrix[" + i + "][" + m + "]");

                                }
                                matrix[g][0] = matrix[g][0] - matrix[g][i] * matrix[i][0];
                                //afficherMatrix(matrix);
                                //afficherMatrix(identity);
                            }

                        }
                        else j++;


                    }
                }



                

            }

            //afficherMatrix(matrix);
            //afficherMatrix(identity);


            // 2eme partie

            for (int i = matrix.Length - 1; i >= 0; i--)
            {
                j = 1;

                terminer = false;
                while (!terminer)
                {
                    
                    for (int g = i - 1; g >= 0; g--)
                    {

                        bool pivotChoisi = false;
                        float pivo = 1f;
                        if (!pivotChoisi)
                        {
                            pivo = matrix[g][i];
                            pivotChoisi = true;
                        }
                      
                        for (int m = matrix.Length - 1; m >= 0; m--)
                        {

                            matrix[g][m] = matrix[g][m] - matrix[i][m] * pivo;
                            identity[g][m] = identity[g][m] - identity[i][m] * pivo;
                            //Debug.Log("matrix[" + g+"]["+m+"] = MyMatrix["+g+"]["+m+ "] - matrix[" + i+"]["+m+"] *"+ pivo);
                        }
                    }
                    //afficherMatrix(matrix);
                    //afficherMatrix(identity);
                    terminer = true;
                
                    j++;


                }





            }

            return identity;

        }
        void calculateInversMatrix3X3()
        { 
                    
            for (int i = 0; i < 3; i++)
            {
                MyMatrix[i] = new float[3];
                MyIdentity[i] = new float[3];
            }

            MyMatrix[0][0] = -1; MyMatrix[1][0] = 1;  MyMatrix[2][0] = -2;
            MyMatrix[0][1] = 2;  MyMatrix[1][1] = 2;  MyMatrix[2][1] = 8;
            MyMatrix[0][2] = 5;  MyMatrix[1][2] = 3;  MyMatrix[2][2] = 10;

            MyIdentity[0][0] = 1; MyIdentity[1][0] = 0; MyIdentity[2][0] = 0;
            MyIdentity[0][1] = 0; MyIdentity[1][1] = 1; MyIdentity[2][1] = 0;
            MyIdentity[0][2] = 0; MyIdentity[1][2] = 0; MyIdentity[2][2] = 1;

            afficherMatrix(MyMatrix);
            afficherMatrix(MyIdentity);


            float[] premiereLigne = new float[3];
            bool terminer = false;
            int j = 1;
            for (int i = 0; i < MyMatrix.Length; i++)
            {
                j = 1;
                terminer = false;
                while (!terminer)// && i<9 && (i+j)<9) 
                {
                    if (MyMatrix[i][i] != 0)
                    {
                        bool pivotChoisi = false;
                        float pivo = 1f;
                        for (int k = 0; k < MyMatrix.Length; k++)
                        {
                            if (!pivotChoisi)
                            {
                                pivo = MyMatrix[i][i];
                                pivotChoisi = true;
                            }

                            MyMatrix[i][k] = MyMatrix[i][k] / pivo;

                            MyIdentity[i][k] = MyIdentity[i][k] / pivo;

                        }
                        Debug.Log("L[" + i + "]" + i + "<> L[" + i + "] /" + pivo);
                        afficherMatrix(MyMatrix);
                        afficherMatrix(MyIdentity);

                        for (int g = (i + 1); g < MyMatrix.Length; g++)
                        {
                            float piv = MyMatrix[g][i];

                            for (int m = 0; m < MyMatrix.Length; m++)
                            {

                                MyMatrix[g][m] = MyMatrix[g][m] - piv * MyMatrix[i][m];
                                MyIdentity[g][m] = MyIdentity[g][m] - piv * MyIdentity[i][m];
                                Debug.Log("MyMatrix[" + g + "][" + m + "] = matrix[" + g + "][" + m + "] - matrix[" + g + "][" + i + "] * matrix[" + i + "][" + m + "]");

                            }


                            afficherMatrix(MyMatrix);
                            afficherMatrix(MyIdentity);
                        }
                        terminer = true;
                    }
                    else
                    {
                        if (MyMatrix[i + j][i] != 0)
                        {
                            bool pivotChoisi = false;
                            float pivo = 1f;
                            for (int k = 0; k < MyMatrix.Length; k++)
                            {
                                if (!pivotChoisi)
                                {
                                    pivo = MyMatrix[i + j][i];
                                    pivotChoisi = true;
                                }

                                float temp = MyMatrix[i][k];
                                MyMatrix[i][k] = MyMatrix[i + j][k] / pivo;
                                MyMatrix[i + j][k] = temp;

                                float temp1 = MyIdentity[i][k];
                                MyIdentity[i][k] = MyIdentity[i + j][k] / pivo;
                                MyIdentity[i + j][k] = temp1;

                            }

                            Debug.Log(" L[" + (i + j) + "] <>" + " L[" + i + "] " + "    L[" + i + "]" + i + "<> L[" + (i + j) + "] /" + pivo + "       ");
                            afficherMatrix(MyMatrix);
                            afficherMatrix(MyIdentity);

                            terminer = true;

                            for (int g = (i + 1); g < MyMatrix.Length; g++)
                            {
                                MyIdentity[g][0] = MyIdentity[g][0] - MyMatrix[g][i] * MyIdentity[i][0];
                                for (int m = 1; m < MyMatrix.Length; m++)
                                {
                                    MyMatrix[g][m] = MyMatrix[g][m] - MyMatrix[g][i] * MyMatrix[i][m];
                                    MyIdentity[g][m] = MyIdentity[g][m] - MyMatrix[g][i] * MyIdentity[i][m];
                                    Debug.Log("MyMatrix[" + g + "][" + m + "] = matrix[" + g + "][" + m + "] - matrix[" + g + "][" + i + "] * matrix[" + i + "][" + m + "]");

                                }
                                MyMatrix[g][0] = MyMatrix[g][0] - MyMatrix[g][i] * MyMatrix[i][0];

                                afficherMatrix(MyMatrix);
                                afficherMatrix(MyIdentity);
                            }

                        }
                        else j++;


                    }
                }





            }

            afficherMatrix(MyMatrix);
            afficherMatrix(MyIdentity);


            // 2eme partie

            for (int i = MyMatrix.Length - 1; i >= 0; i--)
            {
                j = 1;

                terminer = false;
                while (!terminer)
                {

                    for (int g = i - 1; g >= 0; g--)
                    {

                        bool pivotChoisi = false;
                        float pivo = 1f;
                        if (!pivotChoisi)
                        {
                            pivo = MyMatrix[g][i];
                            pivotChoisi = true;
                        }

                        for (int m = MyMatrix.Length - 1; m >= 0; m--)
                        {

                            MyMatrix[g][m] = MyMatrix[g][m] - MyMatrix[i][m] * pivo;
                            MyIdentity[g][m] = MyIdentity[g][m] - MyIdentity[i][m] * pivo;
                            Debug.Log("MyMatrix[" + g + "][" + m + "] = MyMatrix[" + g + "][" + m + "] - MyMatrix[" + i + "][" + m + "] *" + pivo);
                        }
                    }
                    afficherMatrix(MyMatrix);
                    afficherMatrix(MyIdentity);
                    terminer = true;
                
                    j++;


              
                }





            }



        }

        
        bool verifierInverse(float[][] original, float[][] invers)
        {
            //Debug.Log("*****************    original      ******************************************************");
            //afficherMatrix(original);
            //Debug.Log("*****************     invers     ******************************************************");
            //afficherMatrix(invers);

            int n = original.Length;

            float[][] result = new float[n][];


            for (int j = 0; j < n; j++)
            {
                result[j] = new float[n];
            }
           
            
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    result[i][j] = 0;
                    for (int k = 0; k < n; k++)
                            result[i][j] += original[i][k] * invers[k][j];

                    result[i][j] = (float)Math.Round(result[i][j]);
                    if(i==j && result[i][j] != 1)
                    {
                        Debug.Log("return false ");
                        return false;
                    }
                    else
                    {
                        if (i != j && result[i][j] != 0)
                        {
                            Debug.Log("return false ");
                            return false;

                        }

                    }
                }
                



       
            Debug.Log("*****************     Result   verifierInverse      ******************************************************");
            afficherMatrix(result);
            Debug.Log("return true ");
            return true;
        }


        public static float[][] multipleMatrix(float[][] m1, float[][] m2)
        {
            int n = m1.Length;
            int m = m2[0].Length;

            float[][] result = new float[n][];

            for (int j = 0; j < n; j++)
            {
                result[j] = new float[m];
            }


            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    result[i][j] = 0;
                    for (int k = 0; k < m1[i].Length; k++)
                        result[i][j] += m1[i][k] * m2[k][j]; 
                }

            return result;
        }

        float[][] transpose(float[][] matrix)
        {

            int n = matrix.Length;
            int m = matrix[0].Length;

            float[][] result = new float[m][];
            for (int i = 0; i < m; i++)
            {
                result[i] = new float[n];
            }
          
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i][j] = matrix[j][i];
                }
            }

            return result;
        }

        float[][] copyMatrix(float[][] matrix)
        {
            int n = matrix.Length;
            int m = matrix[0].Length;

            float[][] result = new float[n][];

            for (int i = 0; i < n; i++)
            {
                result[i] = new float[m];

                for (int j = 0; j < m; j++)
                {
                    result[i][j] = matrix[i][j];
                }
            }

            return result;
        }

}


