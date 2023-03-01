using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class DataCollecter : NetworkBehaviour
{
    public static int nbKinectMax = 10;

    string[] kinectState;
    bool[] faceTrackingState;


    initCalibration initCalibration;

    KinectWrapper.NuiSkeletonFrame MySkeletonFrame;
    public KinectWrapper.NuiSkeletonFrame[] MySkeletonFrameTable = new KinectWrapper.NuiSkeletonFrame[nbKinectMax];
    public static object locker = new object();


    // Use this for initialization
    void Start()
    {
        kinectState = new string[nbKinectMax];
        faceTrackingState = new bool[nbKinectMax];
        initCalibration = GetComponent<initCalibration>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isServer)
        {
            //Debug.Log("IsServeur");
            for (int i = 0; i < nbKinectMax; i++)
            {
                faceTrackingState[i] = false;


                if (GameObject.Find("Player " + i))
                {
                    if (!clientIDs.Contains(i))
                    {
                        clientIDs.Add(i);
                    }

                    GameObject Player = GameObject.Find("Player " + i);
                    if (GameObject.Find("Client3DText " + i))
                    {
                        Transform Client3DTextTransform = GameObject.Find("Client3DText " + i).transform;

                        Client3DTextTransform.position = new Vector3(Player.transform.GetChild(9).transform.position.x + 0.3f, Player.transform.GetChild(9).transform.position.y + 1.2f, Player.transform.GetChild(9).transform.position.z);

                    }



                    KinectManager manager = GameObject.Find("Player " + i).GetComponent<KinectManager>();



                    //// Debug.Log("Player " + i + " is Founded");

                    // Debug.Log("isFrameInitialised " + manager.isFrameInitialised);

                    if (manager.isFrameInitialised)
                    {
                        //Debug.Log("frame " + manager.frameString);
                        //Debug.Log("skeletonDataString " + manager.skeletonDataString);

                        lock (locker)
                        {

                            MySkeletonFrameTable[i] = JsonUtility.FromJson<KinectWrapper.NuiSkeletonFrame>(manager.frameString);
                            MySkeletonFrameTable[i].SkeletonData = new KinectWrapper.NuiSkeletonData[6];


                            MySkeletonFrameTable[i].SkeletonData[0] = JsonUtility.FromJson<KinectWrapper.NuiSkeletonData>(manager.skeletonDataString);

                            kinectState[i] = manager.KinectState;
                            faceTrackingState[i] = manager.IsTrackingFace();
                            // Debug.Log("faceTrackingState = " + faceTrackingState[i]);

                            if (GameObject.Find("Focus " + i))
                            {
                                if (faceTrackingState[i])
                                {
                                    GameObject.Find("Focus " + i).GetComponent<Image>().color = new Color(0, 1, 0, 1);
                                }
                                else
                                {
                                    GameObject.Find("Focus " + i).GetComponent<Image>().color = new Color(1, 1, 1, 1);
                                }
                            }
                        }



                    }





                }
                else
                {
                    if (clientIDs.Contains(i))
                    {
                        clientIDs.Remove(i);
                    }
                }
            }
            if (StartSavingData && frame < 100)
            {
                Debug.Log("Frame = " + frame);



                for (int i = 0; i < 20; i++)
                {
                    Debug.Log(" -------------------------------------------------      Joint   i = " + i);



                    Sx[i] = Sx[i] + MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[i].x;
                    Sy[i] = Sy[i] + MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[i].y;
                    Sz[i] = Sz[i] + MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[i].z;

                    Sx1[i] = Sx1[i] + MySkeletonFrameTable[id2].SkeletonData[0].SkeletonPositions[i].x;
                    Sy1[i] = Sy1[i] + MySkeletonFrameTable[id2].SkeletonData[0].SkeletonPositions[i].y;
                    Sz1[i] = Sz1[i] + MySkeletonFrameTable[id2].SkeletonData[0].SkeletonPositions[i].z;




                    Debug.Log(" Sx  = " + Sx[i]);
                    Debug.Log(" Sy  = " + Sx[i]);
                    Debug.Log(" Sz  = " + Sx[i]);

                }

                frame++;

                if (frame == 100)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        Sx[i] = Sx[i] / 100;
                        Sy[i] = Sy[i] / 100;
                        Sz[i] = Sz[i] / 100;

                        Sx1[i] = Sx1[i] / 100;
                        Sy1[i] = Sy1[i] / 100;
                        Sz1[i] = Sz1[i] / 100;

                        Debug.Log(" Sx = " + Sx[i]);
                        Debug.Log(" Sy  = " + Sx[i]);
                        Debug.Log(" Sz = " + Sx[i]);

                        MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[i].x = (float)Sx[i];
                        MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[i].y = (float)Sy[i];
                        MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[i].z = (float)Sz[i];

                        MySkeletonFrameTable[id2].SkeletonData[0].SkeletonPositions[i].x = (float)Sx1[i];
                        MySkeletonFrameTable[id2].SkeletonData[0].SkeletonPositions[i].y = (float)Sy1[i];
                        MySkeletonFrameTable[id2].SkeletonData[0].SkeletonPositions[i].z = (float)Sz1[i];
                    }
                    initCalibration.MySkeletonFrameTable = MySkeletonFrameTable;
                    initCalibration.calculer();
                }
            }
        }

       
    }

    public bool StartSavingData = false;


    double[] Sx, Sy, Sz;
    double[] Sx1, Sy1, Sz1;

    int id1 = -1, id2 = -1;

    public int frame = 0;

    public void StoreData(int id1, int id2)
    {
        frame = 0;
        Sx = new double[20];
        Sy = new double[20];
        Sz = new double[20];
        Sx1 = new double[20];
        Sy1 = new double[20];
        Sz1 = new double[20];

        this.id1 = id1;
        this.id2 = id2;

        StartSavingData = true;
    }

    public KinectWrapper.NuiSkeletonFrame[] GetMySkeletonFrameTable()
    {
        lock (locker)
        {
            return MySkeletonFrameTable;
        }

    }

    public bool[] GetFaceTrackingState()
    {
        lock (locker)
        {
            return faceTrackingState;
        }

    }



    public int GetnbKinectMax()
    {
        return nbKinectMax;
    }

    public List<int> clientIDs = new List<int>();
    public List<int> GetclientIDs()
    {
        return clientIDs;
    }
}





