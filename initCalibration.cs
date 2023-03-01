using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class initCalibration : MonoBehaviour
{

    //Pour récuperer les données clients
    DataCollecter collector;

    //matrice contient tous les squeletes de tous les clients
    public KinectWrapper.NuiSkeletonFrame[] MySkeletonFrameTable;

    //matrice qui contient toutes les matrices de transformation (aller/retour)
    public float[][][][] transformationMatrixes;
    bool[][] matriceCalculed;

    Stack<int> clientCalibraded = new Stack<int>();


    int nbKinectMax = 0;
    bool matrixIsCalculated = false;
    /*

    [Serializable]
    public class MyTransformationMatrixes
    {
        public SubMyTransformationMatrixes[][] transformationMatrixes;
    }*/

    [Serializable]
    public class A1
    {
        public List<A2> a1;
    }
    [Serializable]
    public class A2
    {
        public List<A3> a2;
    }
    [Serializable]
    public class A3
    {
        public List<A4> a3;
    }
    [Serializable]
    public class A4
    {
        public float[] a4;
    }


    public void saveCalibrageMatrixes()
    {
        A1 a1 = new A1();
        a1.a1 = new List<A2>();


        for (int i = 0; i < transformationMatrixes.Length; i++)
        {
            A2 a2 = new A2();
            a2.a2 = new List<A3>();

            for (int j = 0; j < transformationMatrixes[0].Length; j++)
            {
                A3 a3 = new A3();
                a3.a3 = new List<A4>();

                for (int k = 0; k < transformationMatrixes[0][0].Length; k++)
                {
                    A4 a4 = new A4();
                    a4.a4 = new float[3];

                    for (int m = 0; m < transformationMatrixes[0][0][0].Length; m++)
                    {
                        a4.a4[m] = transformationMatrixes[i][j][k][m];
                    }
                    a3.a3.Add(a4);
                }
                a2.a2.Add(a3);
            }
            a1.a1.Add(a2);
        }

        string content = JsonUtility.ToJson(a1);
        Debug.Log(content);
        Debug.Log("-----------------------------------------------------------------------------------------------------------------------------");
        Debug.Log(Application.streamingAssetsPath + " /Calibrage.json");

        string path = Application.streamingAssetsPath + "/Calibrage.json";
        File.WriteAllText(path, content);
    }


    public void getCalibrageMatrixes()
    {

        string path = Application.streamingAssetsPath + "/Calibrage.json";

        string content = File.ReadAllText(path);

        A1 a1 = JsonUtility.FromJson<A1>(content);

        for (int i = 0; i < transformationMatrixes.Length; i++)
        {
        
            for (int j = 0; j < transformationMatrixes[0].Length; j++)
            {
               
                for (int k = 0; k < transformationMatrixes[0][0].Length; k++)
                {
                  
                    for (int m = 0; m < transformationMatrixes[0][0][0].Length; m++)
                    {
                        transformationMatrixes[i][j][k][m] = a1.a1[i].a2[j].a3[k].a4[m];
                    }
                   
                }
            }
            
        }
        matrixIsCalculated = true;

    }






    // Use this for initialization
    void Start()
    {


        collector = GetComponent<DataCollecter>();

        MySkeletonFrameTable = collector.GetMySkeletonFrameTable();


        nbKinectMax = collector.GetnbKinectMax();

        lock (locker)
        {
            matriceCalculed = new bool[nbKinectMax][];

            for (int i = 0; i < matriceCalculed.Length; i++)
            {
                matriceCalculed[i] = new bool[nbKinectMax];
                for (int j = 0; j < matriceCalculed[i].Length; j++)
                {
                    matriceCalculed[i][j] = false;
                }
            }
        }


        transformationMatrixes = new float[nbKinectMax][][][];

        for (int i = 0; i < transformationMatrixes.Length; i++)
        {
            transformationMatrixes[i] = new float[nbKinectMax][][];

            for (int j = 0; j < transformationMatrixes[0].Length; j++)
            {
                transformationMatrixes[i][j] = new float[3][];

                for (int h = 0;h < transformationMatrixes[0][0].Length; h++)
                {
                    transformationMatrixes[i][j][h] = new float[3];

                }
            }
        }


    }

    object locker = new object();


    public bool[][] getMatriceCalculed()
    {
        lock (locker)
        {
            return matriceCalculed;
        }

    }

    int id1 = -1, id2 = -1;

    public Button suivant, calculerMatrice;
    public Text compter;

    bool start = false;
    int cpt = 0;

    IEnumerator startCompteur()
    {
        while (start)
        {
            compter.text = "" +cpt;
            yield return new WaitForSeconds(1f);
            cpt++;
        }
        
    }
    public void initCalculer()
    {
        gameObject.GetComponent<ButtonManager>().modeCalculation = true;
        suivant.interactable = false;
        calculerMatrice.interactable = false;

        start = true;
        StartCoroutine(startCompteur());
        //recupeter les ids des clients


        if (GameObject.Find("Item1").GetComponent<Transform>().childCount > 0 && GameObject.Find("Item2").GetComponent<Transform>().childCount > 0)
        {
            string k1 = GameObject.Find("Item1").GetComponent<Transform>().GetChild(0).name;
            string k2 = GameObject.Find("Item2").GetComponent<Transform>().GetChild(0).name;
            Debug.Log("k1 = " + k1 + " k2 = " + k2);


            for (int i = 0; i < nbKinectMax; i++)
            {
                if (k1.Equals("Kinect " + i))
                {
                    id1 = i;
                }
                if (k2.Equals("Kinect " + i))
                {
                    id2 = i;
                }
            }
            Debug.Log("Ki.Id1 = " + id1 + "  Ki.Id2 = " + id2);


        }

        collector.StoreData(id1, id2);
        //StartCoroutine(waitMe());

    }



    public void calculer()
    {


        Debug.Log("  MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[19].x  " + MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[19].x);
        Debug.Log("  MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[19].y  " + MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[19].y);
        Debug.Log("  MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[19].z  " + MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[19].z);

        Debug.Log("         calculer     ");

        // pour récuperer le resultat de taille 9*1
        float[][] transformationMatrixDe1Vers2 = new float[9][];
        float[][] transformationMatrixDe2Vers1 = new float[9][];


        // pour transformer le resultat en matrice de transformation 3*3
        float[][] result1Vers2 = new float[3][];
        float[][] result2Vers1 = new float[3][];


        if (id1 != -1 && id2 != -1)
        {

            if (MySkeletonFrameTable[id1].SkeletonData[0].eTrackingState == MySkeletonFrameTable[id2].SkeletonData[0].eTrackingState)
            {
                CalaculTransformationMatrix cal = new CalaculTransformationMatrix();
                transformationMatrixDe1Vers2 = cal.calculate(ref MySkeletonFrameTable, id1, id2);
                transformationMatrixDe2Vers1 = cal.calculate(ref MySkeletonFrameTable, id2, id1);
            }

        }

        int k = 0;
        for (int i = 0; i < result2Vers1.Length; i++)
        {

            result1Vers2[i] = new float[3];

            result1Vers2[i][0] = transformationMatrixDe1Vers2[k][0];
            result1Vers2[i][1] = transformationMatrixDe1Vers2[k + 1][0];
            result1Vers2[i][2] = transformationMatrixDe1Vers2[k + 2][0];


            result2Vers1[i] = new float[3];

            result2Vers1[i][0] = transformationMatrixDe2Vers1[k][0];
            result2Vers1[i][1] = transformationMatrixDe2Vers1[k + 1][0];
            result2Vers1[i][2] = transformationMatrixDe2Vers1[k + 2][0];



            k += 3;
        }


        /*if (id1 < id2)
        {*/

        transformationMatrixes[id1][id2] = result1Vers2;
        transformationMatrixes[id2][id1] = result2Vers1;

        if (!clientCalibraded.Contains(id1))
        {
            clientCalibraded.Push(id1);
        }
        if (!clientCalibraded.Contains(id2))
        {
            clientCalibraded.Push(id2);
        }
     
        lock (locker)
        {
            matriceCalculed[id1][id2] = true;
            matriceCalculed[id2][id1] = true;
            matrixIsCalculated = true;
        }


        Debug.Log("CalculateCombinaitionMatrixes  id1  =" + id1 + "  id2 =   " + id2);

        CalculateCombinaitionMatrixes();










        if (GameObject.Find("CallibrationMatrix"))
        {
            GameObject.Find("CallibrationMatrix").GetComponent<Text>().text =
                " " + result1Vers2[0][0] + " " + result1Vers2[0][1] + " " + result1Vers2[0][2] + " " + "\n" +
                " " + result1Vers2[1][0] + " " + result1Vers2[1][1] + " " + result1Vers2[1][2] + " " + "\n" +
                " " + result1Vers2[2][0] + " " + result1Vers2[2][1] + " " + result1Vers2[2][2] + " " + "\n" +
                " " + result2Vers1[0][0] + " " + result2Vers1[0][1] + " " + result2Vers1[0][2] + " " + "\n" +
                " " + result2Vers1[1][0] + " " + result2Vers1[1][1] + " " + result2Vers1[1][2] + " " + "\n" +
                " " + result2Vers1[2][0] + " " + result2Vers1[2][1] + " " + result2Vers1[2][2] + " " + "\n";
        }


        float DX = 0, DY = 0, DZ = 0;
        float distanceCarree = 0f;/// 1 ere probleme 
        if (MySkeletonFrameTable[id1].SkeletonData[0].eTrackingState == MySkeletonFrameTable[id2].SkeletonData[0].eTrackingState)
        {

            for (int j = 0; j < MySkeletonFrameTable[id2].SkeletonData[0].SkeletonPositions.Length; j++)
            {
                if (MySkeletonFrameTable[id2].SkeletonData[0].eSkeletonPositionTrackingState[j] == MySkeletonFrameTable[id2].SkeletonData[0].eSkeletonPositionTrackingState[j] && 2 == (int)MySkeletonFrameTable[id1].SkeletonData[0].eSkeletonPositionTrackingState[j])
                {
                    float x = MySkeletonFrameTable[id2].SkeletonData[0].SkeletonPositions[j].x * result2Vers1[0][0] + MySkeletonFrameTable[id2].SkeletonData[0].SkeletonPositions[j].y * result2Vers1[0][1] + MySkeletonFrameTable[id2].SkeletonData[0].SkeletonPositions[j].z * result2Vers1[0][2];
                    float y = MySkeletonFrameTable[id2].SkeletonData[0].SkeletonPositions[j].x * result2Vers1[1][0] + MySkeletonFrameTable[id2].SkeletonData[0].SkeletonPositions[j].y * result2Vers1[1][1] + MySkeletonFrameTable[id2].SkeletonData[0].SkeletonPositions[j].z * result2Vers1[1][2];
                    float z = MySkeletonFrameTable[id2].SkeletonData[0].SkeletonPositions[j].x * result2Vers1[2][0] + MySkeletonFrameTable[id2].SkeletonData[0].SkeletonPositions[j].y * result2Vers1[2][1] + MySkeletonFrameTable[id2].SkeletonData[0].SkeletonPositions[j].z * result2Vers1[2][2];


                    DX = x - MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[j].x;
                    DY = y - MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[j].y;
                    DZ = z - MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[j].z;

                    distanceCarree = (float)Math.Sqrt(Math.Pow(DX, 2) + Math.Pow(DY, 2) + Math.Pow(DZ, 2));

                    Debug.Log(" j = " + j + " X =   " + MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[j].x + " Y = " + MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[j].y + " Z = " + MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[j].z);
                    Debug.Log(" j = " + j + " X' =  " + x + "  Y = " + y + "  Z' = " + z);
                    Debug.Log(" j = " + j + " DX =  " + DX + "  DY " + DY + "  DZ = " + DZ);
                    Debug.Log("Distance Carée  " + distanceCarree);


                }
            }
        }
        id1 = -1;
        id2 = -1;
        collector.StartSavingData = false;
        suivant.interactable = true;
        start = false;
        compter.text = "Calculer la matrice de callibration";
        calculerMatrice.interactable = true;
        gameObject.GetComponent<ButtonManager>().modeCalculation = false;
        cpt = 0;
    }

    void CalculateCombinaitionMatrixes()
    {
        Debug.Log("CalculateCombinaitionMatrixes");

        int clientDernier = clientCalibraded.Pop();
        lock (locker)
        {
            foreach (int client in clientCalibraded)
            {
                if (!matriceCalculed[clientDernier][client])
                {
                    Debug.Log("transformationMatrixes[" + clientDernier + "][" + client + "] = ");
                    transformationMatrixes[client][clientDernier] = CalaculesMatrices.multipleMatrix(transformationMatrixes[client + 1][clientDernier], transformationMatrixes[client][client + 1]);
                    transformationMatrixes[clientDernier][client] = CalaculesMatrices.multipleMatrix(transformationMatrixes[client + 1][client], transformationMatrixes[clientDernier][client + 1]);
                    matriceCalculed[clientDernier][client] = true;
                    matriceCalculed[client][clientDernier] = true;
                }
            }
        }



        clientCalibraded.Push(clientDernier);


    }


    // Update is called once per frame
    void Update()
    {

    }
}
