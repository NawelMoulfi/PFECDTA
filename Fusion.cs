using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;

//using UnityEditor;

public class Fusion : MonoBehaviour {

    //Pour récuperer les données clients
    DataCollecter collector;

    Calibration calibration;
	BestKmeans  bestkmeans ;

    AnimationManager animationManager;

    initCalibration initCalibration;

    ButtonManager buttonManager;

    public Transform cap, avatar;


    //matrice contient tous les squeletes de tous les clients
    public KinectWrapper.NuiSkeletonFrame[] MySkeletonFrameTable;
    public KinectWrapper.NuiSkeletonFrame[] PrevMySkeletonFrameTable;


    bool[] faceTrackingState;


    public List<int> clientIDs = new List<int>();
    List<int> clientsTracked = new List<int>();
    int nbKinectMax = 0;

    bool isTransformationCalculated;
    bool[][] matriceCalculed;


    Transform[] avatarHips = new Transform[10];



    public System.Diagnostics.Stopwatch Mtime_execution;
    string Mpath = "";
    int Mnb_frames;
    double Mtemps_dexec_moyen = 0;



    // Use this for initialization
    void Start() {


        //****************Pour le calcul du temps d'execution********************
        Mtime_execution = new System.Diagnostics.Stopwatch();
        //Création du fichier d'évaluation
        Mpath = Application.streamingAssetsPath +"/Evaluation/TempsDexecAnimationFusion.txt";

        //créer le fichier s'il n'existe pas
        if ((File.Exists(Mpath)))
        {
            String Entete = "TEMPS D'EXECUTION DE LA FUSION ET SON ANIMATION :\n";
            File.WriteAllText(Mpath, Entete);
        }








        collector = GetComponent<DataCollecter>();

        calibration = GetComponent<Calibration>();

        animationManager = avatarFusion.GetComponent<AnimationManager>();

        initCalibration = GetComponent<initCalibration>();

        buttonManager = GetComponent<ButtonManager>();
		bestkmeans =  GetComponent<BestKmeans>(); 


        nbKinectMax = collector.GetnbKinectMax();

        faceTrackingState = new bool[nbKinectMax];

        clientIDs = collector.GetclientIDs();

        if (clientIDs.Count > 0)
        {
            MySkeletonFrameTable = collector.GetMySkeletonFrameTable();

            faceTrackingState = collector.GetFaceTrackingState();
        }




        isTransformationCalculated = false;

        //Debug.Log("IsServeur");

        sp2 = new AnimationData();
        sp2.squelleteData = new List<SquelleteData>();
        sp = new AnimationData2();
        sp.squelleteData = new List<SquelleteData2>();


        //*************************************************************************************************************
        //recupération du Animator de l'avatar de fusion
        var myAnimatorFusion = myAvatarFusion.GetComponent<Animator>();

        //recupération des transforms de tous les bones de l'avatar de fusion et les assosier a la variable bones
        for (int boneIndex = 0; boneIndex < bones1.Length; boneIndex++)
        {
            //s'il n'existe pas ne fait rien
            if (!boneIndexMap.ContainsKey(boneIndex))
                continue;
            bones1[boneIndex] = myAnimatorFusion.GetBoneTransform(boneIndexMap[boneIndex]);
        }
        //recupération du Animator de l'avatar de fusion
        var myAnimatorFusion2 = myAvatarFusion2.GetComponent<Animator>();

        //recupération des transforms de tous les bones de l'avatar de fusion et les assosier a la variable bones
        for (int boneIndex = 0; boneIndex < bones2.Length; boneIndex++)
        {
            //s'il n'existe pas ne fait rien
            if (!boneIndexMap.ContainsKey(boneIndex))
                continue;
            bones2[boneIndex] = myAnimatorFusion2.GetBoneTransform(boneIndexMap[boneIndex]);
        }

        intialPosition = myAnimatorFusion.transform.position;
        intialPosition2 = myAnimatorFusion2.transform.position;
        avatarFusionPosition = new Vector3(0, 0, 8);
    }

    List<int> ClientTracked()
    {
        List<int> result = new List<int>();

        foreach (int client in clientIDs)
        {
            if (MySkeletonFrameTable[client].SkeletonData != null)
            {
                if (MySkeletonFrameTable[client].SkeletonData[0].eTrackingState == KinectWrapper.NuiSkeletonTrackingState.SkeletonTracked)
                {
                    result.Add(client);
                }
            }

        }


        return result;
    }

    int GetNbJointsTracked(int id)
    {
        int cpt = 0;
        if (MySkeletonFrameTable[id].SkeletonData[0].eTrackingState == KinectWrapper.NuiSkeletonTrackingState.SkeletonTracked)
        {
            for (int k = 0; k < MySkeletonFrameTable[id].SkeletonData[0].eSkeletonPositionTrackingState.Length; k++)
            {
                if (2 == (int)MySkeletonFrameTable[id].SkeletonData[0].eSkeletonPositionTrackingState[k])
                {
                    cpt++;
                }
            }
        }
        return cpt;
    }





    ArrayList FaceTrackedSkeleton()
    {
        ArrayList squelettes_indices = new ArrayList();
        for (int i = 0; i < nbKinectMax; i++)
        {
            if (faceTrackingState[i]) squelettes_indices.Add(i);
        }

        return squelettes_indices;
    }


    //la premiere kinect qui a le face tracking true parmi les nb_kinect
    int Indice_FaceTRackingFirst()
    {
        for (int i = 0; i < nbKinectMax; i++)
        {
            if (faceTrackingState[i]) return i;
        }
        return -1;
    }


    int SeuilMax = 5;

    int Kinect_SqueletteDeBase()
    {
       
        int first = Indice_FaceTRackingFirst();

        //s'il y a au moins une kinect qui a le faceTracking
        if (first != -1)
        {
            int max = GetNbJointsTracked(first);
            int kinect = first;

            for (int i = 0; i < clientsTracked.Count; i++)
            {
                int temp = GetNbJointsTracked(clientsTracked[i]);
                if (temp > max + SeuilMax && faceTrackingState[clientsTracked[i]])
                {
                    max = temp; kinect = clientsTracked[i];
                }
            }

            return kinect;

        }
        else
        {
            //si aucune kinect n'a le faceTracking alors choisir ceelle qui a le plus grand nombre de joints true
            int max = GetNbJointsTracked(clientsTracked[0]); int kinect = clientsTracked[0];

            for (int i = 0; i < clientsTracked.Count; i++)
            {
                int temp = GetNbJointsTracked(clientsTracked[i]);
                if (temp > max + SeuilMax)
                {
                    max = temp; kinect = clientsTracked[i];
                }
            }

            return kinect;
        }

    }

    int GetBaseSkeletonID()
    {
        int baseSkeleton = -1;
        List<int> faceDetected = new List<int>();


        if (clientsTracked.Count > 0)
        {
            baseSkeleton = clientsTracked[0];

            int max = GetNbJointsTracked(baseSkeleton);

            Debug.Log("*****************************************   Selection du squelete de Base  ******************************************");
            Debug.Log("baseSkeleton = clientIDs[0] =" + baseSkeleton);
            for (int k = 0; k < clientsTracked.Count; k++)
            {
                if (faceTrackingState[clientsTracked[k]])
                {
                    return clientsTracked[k];
                }
                int temp = GetNbJointsTracked(clientsTracked[k]);
                // Debug.Log("nbJointTracked = " + temp + " MySkeletonFrameTable[k].SkeletonData[0].nbJointTracked = " + MySkeletonFrameTable[clientsTracked[k]].SkeletonData[0].nbJointTracked );
                if (max < temp)
                {
                    max = temp;
                    baseSkeleton = clientsTracked[k];
                    Debug.Log(k + "baseSkeleton " + baseSkeleton);
                }
            }
        }

        return baseSkeleton;
    }

    KinectWrapper.NuiSkeletonData GetBaseSkeleton(int id)
    {
        return MySkeletonFrameTable[id].SkeletonData[0];

    }

    public KinectWrapper.NuiSkeletonFrame[] GetMySkeletonFrameTable()
    {
        return MySkeletonFrameTable;
    }

    Quaternion tournerAvatar(string Direction, Quaternion initDirect)
    {
        Quaternion newRotation = Quaternion.identity;
        switch (Direction)
        {
            case "3":
                newRotation = initDirect * Quaternion.Euler(0, 90f, 0);//----------------------------------------------------------------------
                break;
            case "4":
                newRotation = initDirect * Quaternion.Euler(0, 270f, 0);
                break;
            case "5":
                newRotation = initDirect;
                break;
            case "6":
                newRotation = initDirect * Quaternion.Euler(0, 180f, 0);
                break;
            default:
                break;
        }
        return newRotation;
    }


    int baseSkeletonID = -1;



    public Transform avatarFusion;
    Transform[] bones = new Transform[22];
    public System.Diagnostics.Stopwatch time_execution = new System.Diagnostics.Stopwatch();
    double temps_dexec_moyen = 0;

    double t1 = 0, t2 = 0;
    string MyTXTPath;

    bool touner = false;
    public Transform myAvatarFusion;
    public Transform myAvatarFusion2;

    Transform[] bones1 = new Transform[22];
    Transform[] bones2 = new Transform[22];

    Vector3 intialPosition;
    Vector3 intialPosition2;
    Vector3 avatarFusionPosition;

    // Update is called once per frame
    void Update() {

        if (calculerLeTempsDexecution)
        {
            Mnb_frames++;
            Mtime_execution.Start();
        }


        if (Input.GetKeyDown(KeyCode.O))
        {
            Mtemps_dexec_moyen = Mtemps_dexec_moyen / Mnb_frames;
            UnityEngine.Debug.Log("le temps d'execution=" + Mtemps_dexec_moyen);
            UnityEngine.Debug.Log(Mnb_frames);
        }






        //recupération du Animator de l'avatar de fusion
        var myAnimatorFusion = myAvatarFusion.GetComponent<Animator>();

        //recupération des transforms de tous les bones de l'avatar de fusion et les assosier a la variable bones
        for (int boneIndex = 0; boneIndex < bones1.Length; boneIndex++)
        {
            //s'il n'existe pas ne fait rien
            if (!boneIndexMap.ContainsKey(boneIndex))
                continue;
            bones1[boneIndex] = myAnimatorFusion.GetBoneTransform(boneIndexMap[boneIndex]);
        }


        clientIDs = new List<int>();
        clientsTracked = new List<int>();

        for (int i = 0; i < 10; i++)
        {
            if (GameObject.Find("Player " + i))
            {
                avatarHips[i] = GameObject.Find("Player " + i).GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Hips);

            }
        }

        clientIDs = collector.GetclientIDs();

        if (clientIDs.Count > 0)
        {
            faceTrackingState = collector.GetFaceTrackingState();

            MySkeletonFrameTable = collector.GetMySkeletonFrameTable();



            clientsTracked.Clear();
            clientsTracked = ClientTracked();

            if (clientsTracked.Count == 0)
                return;

            if (clientsTracked.Count == 1)
            {
                
                //recupération des transforms de tous les bones de l'avatar de fusion et les assosier a la variable bones
                for (int boneIndex = 0; boneIndex < bones2.Length; boneIndex++)
                {
                    //s'il n'existe pas ne fait rien
                    if (!boneIndexMap.ContainsKey(boneIndex))
                        continue;
                    bones2[boneIndex].localRotation = bones1[boneIndex].localRotation;
                }

                myAvatarFusion2.position = new Vector3(myAvatarFusion.position.x, myAvatarFusion.position.y, myAvatarFusion.position.z - 11);

              


                //Debug.Log("clientsTracked[0] = " + clientsTracked[0]);
                animationManager.ProcessSkeleton(MySkeletonFrameTable, clientsTracked[0]);




            }
            else
            {

                if (buttonManager.calculateMatrixesIsDone)
                {


                    baseSkeletonID = Kinect_SqueletteDeBase();


                    if (!isTransformationCalculated)
                    {
                        PrevMySkeletonFrameTable = MySkeletonFrameTable;
                        matriceCalculed = initCalibration.getMatriceCalculed();
                         time_execution.Start();
                         TimeSpan t = time_execution.Elapsed;
                        //time_execution.Stop();
                        // GameObject.Find("Latence").GetComponent<Text>().text = "FusionRunTime=" + ts.TotalSeconds*1000 + "ms";
                         t1 =  t.TotalSeconds * 1000 ;

                        prevBaseSkeletonID = baseSkeletonID;
                        if (GameObject.Find("Repere " + baseSkeletonID))
                        {
                            GameObject.Find("Repere " + baseSkeletonID).GetComponent<Image>().color = new Color(0, 1, 0, 1);
                        }

                        MyTXTPath = Application.streamingAssetsPath + "/fusionExecution.txt";
                        //créer le fichier s'il n'existe pas
                        if (!(File.Exists(MyTXTPath)))
                        {
                            string Entete = "Hello this is My first Line :\n";
                            File.WriteAllText(MyTXTPath, Entete);
                        }
                    }
                    isTransformationCalculated = true;

                
                    GetFusionedSkeletonWithOutAverage();

                    getOrientationJoints();

                   


                    TimeSpan ts = time_execution.Elapsed;
                    //time_execution.Stop();
                     // GameObject.Find("Latence").GetComponent<Text>().text = "FusionRunTime=" + ts.TotalSeconds*1000 + "ms";
                    //String contenu = "temps d'exec=" + ts.TotalSeconds * 1000 + " ms\n";

                    /*
                    temps_dexec_moyen += ts.TotalSeconds;
                    UnityEngine.Debug.Log("temps d'exec de la frame=" + ts.TotalSeconds);
                    */
                

                    t2 = (ts.TotalSeconds*1000) - t1;
                    //UnityEngine.Debug.Log("temps entre update = " + t2);

                    //UnityEngine.Debug.Log("avant t1= " + t1);
                    t1 = ts.TotalSeconds * 1000;
                    //UnityEngine.Debug.Log("apres t1= " + t1);




                    if (baseSkeletonID != prevBaseSkeletonID  )
                    {
                        if (GameObject.Find("Repere " + baseSkeletonID))
                        {
                            GameObject.Find("Repere " + baseSkeletonID).GetComponent<Image>().color = new Color(0, 1, 0, 1);
                            if (GameObject.Find("Repere " + prevBaseSkeletonID))
                            {
                                GameObject.Find("Repere " + prevBaseSkeletonID).GetComponent<Image>().color = new Color(1, 1, 1, 1);
                            }
                        }
                    }

                    if (start)
                    {

                        //Ancien avant modification 
                         //* work well

                        SquelleteData2 sp1 = new SquelleteData2();
                        sp1.articuPosition = new Vector3[20];
                        sp1.articuRotation = new Quaternion[20];
                        sp1.scale = new Vector3[20];

                        uint id = animationManager.GetPlayer1ID();

                        // Sauvgarder les données de l'animation manager POSITIONS + Articulations 
                        sp1.avatarPosition = animationManager.GetUserPosition((uint)repereGlobal);
                        for (int i = 0; i < 20; i++)
                        {
                            sp1.articuPosition[i] = animationManager.GetJointPosition(id, i);
                            sp1.articuRotation[i] = animationManager.GetJointOrientation(id, i, false);
                        }
                        sp1.timeExec = t2;
                        sp.squelleteData.Add(sp1);




                        getJoint();


                        FrameCpt++;
                        string[] finalString = new string[61];

                        finalString[0] = "Frame " + FrameCpt;

                        // Enregistrement des positions fusionnées dans un Fichier CSV pour l'exploiter par EXCEL
                        ctr = 0;
                        for (int i = 1; i < 61; i+=3)
                        {
                            finalString[i ] = ("" + MySkeletonFrameTable[4].SkeletonData[0].SkeletonPositions[ctr].x);
                            finalString[i + 1] = ("" + MySkeletonFrameTable[4].SkeletonData[0].SkeletonPositions[ctr].y);
                            finalString[i + 2] = ("" + MySkeletonFrameTable[4].SkeletonData[0].SkeletonPositions[ctr].z);
                            ctr++;
                        }
                        DEV_AppendToReport(finalString);

                    }
                
                }
                prevBaseSkeletonID = baseSkeletonID;
                PrevMySkeletonFrameTable = MySkeletonFrameTable;
            }
        }





        if (calculerLeTempsDexecution)
        {
            TimeSpan Mts = Mtime_execution.Elapsed;
            Mtime_execution.Stop();

            String contenu = Mts.TotalSeconds * 1000 + "\n";
            Mtemps_dexec_moyen += Mts.TotalSeconds;
            //UnityEngine.Debug.Log("temps d'exec de la frame=" + ts.TotalSeconds);
            File.AppendAllText(Mpath, contenu);
            Mtime_execution.Reset();
        }

        


    }

    bool calculerLeTempsDexecution = false;



    int FrameCpt = 0;
    int ctr = 0;
    void DEV_AppendToReport(string[] data)
    {
        Debug.Log("data = "+data);
        CSVManager.AppandToReport(data);
        //EditorApplication.Beep();
    }






    Vector3 hipRight = new Vector3();
    Vector3 hipLeft = new Vector3();

    void getOrientationJoints()
    {
        hipRight = new Vector3();
        hipLeft = new Vector3();

        if (baseSkeletonID != repereGlobal)
        {
            //left 12
            //right 16


            int f = 12;

            if (baseSkeletonID == 5 || baseSkeletonID == 6 )
            {
                f = 16;
            }
            
            int idSource = baseSkeletonID;

            float x = MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].x * initCalibration.transformationMatrixes[idSource][repereGlobal][0][0] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[idSource][repereGlobal][0][1] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[idSource][repereGlobal][0][2];
            float y = MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].x * initCalibration.transformationMatrixes[idSource][repereGlobal][1][0] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[idSource][repereGlobal][1][1] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[idSource][repereGlobal][1][2];
            float z = MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].x * initCalibration.transformationMatrixes[idSource][repereGlobal][2][0] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[idSource][repereGlobal][2][1] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[idSource][repereGlobal][2][2];

            hipLeft = new Vector3(x,y,z);

            f = 16;

           if (baseSkeletonID == 5 || baseSkeletonID == 6)
            {
                f = 12;
            }
            
            float x2 = MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].x * initCalibration.transformationMatrixes[idSource][repereGlobal][0][0] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[idSource][repereGlobal][0][1] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[idSource][repereGlobal][0][2];
            float y2 = MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].x * initCalibration.transformationMatrixes[idSource][repereGlobal][1][0] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[idSource][repereGlobal][1][1] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[idSource][repereGlobal][1][2];
            float z2 = MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].x * initCalibration.transformationMatrixes[idSource][repereGlobal][2][0] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[idSource][repereGlobal][2][1] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[idSource][repereGlobal][2][2];

            hipRight = new Vector3(x2, y2, z2);

           
        }
        else
        {
            int f = 12;
            hipLeft = new Vector3(MySkeletonFrameTable[repereGlobal].SkeletonData[0].SkeletonPositions[f].x, MySkeletonFrameTable[repereGlobal].SkeletonData[0].SkeletonPositions[f].y, MySkeletonFrameTable[repereGlobal].SkeletonData[0].SkeletonPositions[f].z);

            f = 16;
            hipRight = new Vector3(MySkeletonFrameTable[repereGlobal].SkeletonData[0].SkeletonPositions[f].x, MySkeletonFrameTable[repereGlobal].SkeletonData[0].SkeletonPositions[f].y, MySkeletonFrameTable[repereGlobal].SkeletonData[0].SkeletonPositions[f].z);
        }

        bones2[0].localRotation = Quaternion.Euler(bones1[0].localRotation.eulerAngles.x, 0, bones1[0].localRotation.eulerAngles.z);

        //recupération des transforms de tous les bones de l'avatar de fusion et les assosier a la variable bones
        for (int boneIndex = 1; boneIndex < bones2.Length; boneIndex++)
        {
            //s'il n'existe pas ne fait rien
            if (!boneIndexMap.ContainsKey(boneIndex))
                continue;
            bones2[boneIndex].localRotation = bones1[boneIndex].localRotation;
        }

        
        float angle = caluleAngle();
        

        Debug.Log("Angle= " + angle  +"Repere globale"+ repereGlobal+"repere de base "+ baseSkeletonID);
        
         if (baseSkeletonID == 5)
        {
            Debug.Log(" -----------                   baseSkeletonID = " + baseSkeletonID);
            angle = -angle;
        }
      
        if (baseSkeletonID == 4)
        {
            angle = -10+angle;
        }
        if (baseSkeletonID == 6)
        {
            angle = 50+ angle;
        }
        if (baseSkeletonID == 7)
        {
            angle = -angle-15;
        }

        myAvatarFusion2.rotation = Quaternion.Euler(0, angle, 0);

        /*
        
                switch (baseSkeletonID)
                {
                    case 3:
                        angle = -angle;
                        myAvatarFusion2.rotation = Quaternion.Euler(0, angle, 0);


                        break;
                    case 4:
                        float ddd = 80 - angle;
                        angle = -(angle + ddd);

                        myAvatarFusion2.rotation = Quaternion.Euler(0, angle, 0);
                        break;
                    case 5:
                        float d = 150 - angle;
                        angle = -(angle+d);
                        myAvatarFusion2.rotation = Quaternion.Euler(0, angle, 0);
                        break;
                    case 6:
                        float dd = 250 - angle;
                        angle = -(angle+dd);
                        myAvatarFusion2.rotation = Quaternion.Euler(0, angle, 0);
                        break;
                }

                angle = -angle;*/
        //myAvatarFusion2.rotation = Quaternion.Euler(0,angle, 0);

        //bones2[0].localRotation = Quaternion.Euler(bones2[0].localEulerAngles.x, angle, bones2[0].localEulerAngles.z);

        //if (baseSkeletonID != perviousRepere)
        //  {
        //    
        //myAvatarFusion2.rotation = Quaternion.Slerp(myAvatarFusion2.rotation, Quaternion.Euler(myAvatarFusion2.rotation.eulerAngles.x, angle, myAvatarFusion2.rotation.eulerAngles.z), smoothFactor);
        //myAvatarFusion2.Rotate(new Vector3(myAvatarFusion2.rotation.eulerAngles.x, angle-myAvatarFusion2.rotation.eulerAngles.y, myAvatarFusion2.rotation.eulerAngles.z));
        // }


        //myAvatarFusion2.rotation = Quaternion.Slerp(myAvatarFusion2.rotation, Quaternion.Euler(myAvatarFusion2.rotation.eulerAngles.x, angle, myAvatarFusion2.rotation.eulerAngles.z), 0.5f);

        /* if (baseSkeletonID == 3)
         {
             //myAvatarFusion2.rotation = Quaternion.Slerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 0), 1f);
             myAvatarFusion2.rotation = Quaternion.Euler(0, 0, 0);
         }
         else
         {
             //myAvatarFusion2.rotation = Quaternion.Slerp(Quaternion.Euler(0, 180, 0), Quaternion.Euler(0, 180, 0), 1f);
             myAvatarFusion2.rotation = Quaternion.Euler(0, 180, 0);
         }
         */
        /*   
         switch (baseSkeletonID)
          {
              case 3:
                  //myAvatarFusion2.rotation = Quaternion.Slerp(myAvatarFusion2.rotation, Quaternion.Euler(0, 0, 0), 0.2f);
                  myAvatarFusion2.rotation = Quaternion.Euler(0, 0, 0);
                  break;
              case 4:
                  //myAvatarFusion2.rotation = Quaternion.Slerp(myAvatarFusion2.rotation, Quaternion.Euler(0, -90f, 0), 0.2f);
                  myAvatarFusion2.rotation = Quaternion.Euler(0, -40, 0);
                  break;
              case 5:
                  //myAvatarFusion2.rotation = Quaternion.Slerp(myAvatarFusion2.rotation, Quaternion.Euler(0, -180f, 0), 1);
                  myAvatarFusion2.rotation = Quaternion.Euler(0, -120, 0);
                  break;
              case 6:
                  //myAvatarFusion2.rotation = Quaternion.Slerp(myAvatarFusion2.rotation, Quaternion.Euler(0, -270f, 0), 1);
                  myAvatarFusion2.rotation = Quaternion.Euler(0, -210, 0);
                  break;
          }*/


        myAvatarFusion2.position = new Vector3(myAvatarFusion.position.x, myAvatarFusion.position.y, myAvatarFusion.position.z-11);

        animationManager.ProcessSkeleton(MySkeletonFrameTable, repereGlobal);

        perviousRepere = baseSkeletonID;


    }

    int perviousRepere = 3;
    public float smoothFactor = 1f;






    float caluleAngle()
    {

        double delta = hipRight.z - hipLeft.z;


        double Dx = hipRight.x - hipLeft.x;

        double Dz = hipRight.z - hipLeft.z;


        double distanceCarree = (float)Math.Sqrt(Math.Pow(Dx, 2)  + Math.Pow(Dz, 2));


        if (distanceCarree == 0)
            return -1;



        Debug.Log("delta = " + delta);
        Debug.Log("Dx = " + Dx);
        Debug.Log("Dz = " + Dz);
        Debug.Log("distanceCarree = " + distanceCarree);


        float angle = (float)((Math.Asin(Math.Abs(delta) / distanceCarree) * 180) / Math.PI);


        if (delta == 0)
        {
            if (Dx > 0)
            {
                angle = 0;
            }
            else
            {
                angle = -180;
            }
        }
        else
        {
            if(delta < 0)
            {
                if (Dx >0)
                {
                    angle = -angle;
                }
                else
                {
                    if (Dx == 0)
                    {
                        angle = -90;
                    }
                    else
                    {
                        angle = angle -180;
                    }
                }
            }
            else
            {
                if (Dx < 0 )
                {
                    angle = 180 - angle;
                }
                else
                {
                    if (Dx == 0)
                    {
                        angle = 90;
                    }
                    else
                    {
                        angle = angle;
                    }
                }
            }
        }



        return angle;
    }


















    int prevBaseSkeletonID = -1;

    bool AreAllTransformationMatrixesCalculated()
    {

        foreach (int client in clientsTracked)
        {
            foreach (int client2 in clientsTracked)
            {
                if (client != client2)
                {
                    if (!matriceCalculed[client][client2] || !matriceCalculed[client2][client])
                        return false;
                }
            }
        }


        return true;
    }

    KinectWrapper.NuiSkeletonData baseSkeletonData, fusionedSkeletonData;
    KinectWrapper.NuiSkeletonData[] calibratedClientsSkeletonData, ajustedClientsSkeletonData;

    KinectWrapper.NuiSkeletonData GetFusionedSkeleton()
    {
        calibratedClientsSkeletonData = new KinectWrapper.NuiSkeletonData[nbKinectMax];

        KinectWrapper.NuiSkeletonData fusionedSkeletonData = new KinectWrapper.NuiSkeletonData();
        fusionedSkeletonData.SkeletonPositions = new Vector4[20];
        fusionedSkeletonData.eSkeletonPositionTrackingState = new KinectWrapper.NuiSkeletonPositionTrackingState[20];

        // Pour chaque client estimer ces valeurs dans le repère de base 
        for (int i = 0; i < clientsTracked.Count; i++)
        {
            if (clientsTracked[i] != baseSkeletonID && baseSkeletonID != -1)
                calibratedClientsSkeletonData[clientsTracked[i]] = calibration.EstimateSkeleton(clientsTracked[i], baseSkeletonID, MySkeletonFrameTable);
        }

        float sumX = 0f, sumY = 0f, sumZ = 0f;
        int cpt = 0;
        foreach (int client in clientsTracked)
        {
            if (client != baseSkeletonID)
            {
                Debug.Log("client = " + client + "calibratedClientsSkeletonData[client].Position.x= " + calibratedClientsSkeletonData[client].Position.x);
                Debug.Log("client = " + client + "calibratedClientsSkeletonData[client].Position.y= " + calibratedClientsSkeletonData[client].Position.y);
                Debug.Log("client = " + client + "calibratedClientsSkeletonData[client].Position.z= " + calibratedClientsSkeletonData[client].Position.z);
                sumX += calibratedClientsSkeletonData[client].Position.x;
                sumY += calibratedClientsSkeletonData[client].Position.y;
                sumZ += calibratedClientsSkeletonData[client].Position.z;
                cpt++;
            }
        }
        cpt++;
        Debug.Log("baseSkeletonData.Position.x = " + baseSkeletonData.Position.x);
        Debug.Log("baseSkeletonData.Position.y = " + baseSkeletonData.Position.y);
        Debug.Log("baseSkeletonData.Position.z = " + baseSkeletonData.Position.z);


        fusionedSkeletonData.Position.x = (baseSkeletonData.Position.x + sumX) / cpt;
        Debug.Log("fusionedSkeletonData.Position.x = " + fusionedSkeletonData.Position.x);
        fusionedSkeletonData.Position.y = (baseSkeletonData.Position.y + sumY) / cpt;
        Debug.Log("fusionedSkeletonData.Position.y = " + fusionedSkeletonData.Position.y);
        fusionedSkeletonData.Position.z = (baseSkeletonData.Position.z + sumZ) / cpt;
        Debug.Log("fusionedSkeletonData.Position.z = " + fusionedSkeletonData.Position.z);



        for (int i = 0; i < 20; i++)
        {
            cpt = 0;
            sumX = 0f; sumY = 0f; sumZ = 0f;

            foreach (int client in clientsTracked)
            {
                if (client != baseSkeletonID)
                {
					
                    if (MySkeletonFrameTable[client].SkeletonData[0].eSkeletonPositionTrackingState[i] == KinectWrapper.NuiSkeletonPositionTrackingState.Tracked)
                    {
                        sumX += calibratedClientsSkeletonData[client].SkeletonPositions[i].x;
                        Debug.Log("joint = " + i + " x = " + calibratedClientsSkeletonData[client].SkeletonPositions[i].x);
                        sumY += calibratedClientsSkeletonData[client].SkeletonPositions[i].y;
                        Debug.Log("joint = " + i + " y = " + calibratedClientsSkeletonData[client].SkeletonPositions[i].y);
                        sumZ += calibratedClientsSkeletonData[client].SkeletonPositions[i].z;
                        Debug.Log("joint = " + i + " z = " + calibratedClientsSkeletonData[client].SkeletonPositions[i].z);
                        cpt++;
                    }
                }

            }
            cpt++;
            fusionedSkeletonData.SkeletonPositions[i].x = (baseSkeletonData.SkeletonPositions[i].x + sumX) / cpt;
            fusionedSkeletonData.SkeletonPositions[i].y = (baseSkeletonData.SkeletonPositions[i].y + sumY) / cpt;
            fusionedSkeletonData.SkeletonPositions[i].z = (baseSkeletonData.SkeletonPositions[i].z + sumZ) / cpt;

            Debug.Log("baseSkeletonData.SkeletonPositions[" + i + "].x = " + baseSkeletonData.SkeletonPositions[i].x);
            Debug.Log("baseSkeletonData.SkeletonPositions[" + i + "].y = " + baseSkeletonData.SkeletonPositions[i].y);
            Debug.Log("baseSkeletonData.SkeletonPositions[" + i + "].z = " + baseSkeletonData.SkeletonPositions[i].z);


            Debug.Log("fusionedSkeletonData.SkeletonPositions X = " + fusionedSkeletonData.SkeletonPositions[i].x);
            Debug.Log("fusionedSkeletonData.SkeletonPositions Y = " + fusionedSkeletonData.SkeletonPositions[i].y);
            Debug.Log("fusionedSkeletonData.SkeletonPositions Z = " + fusionedSkeletonData.SkeletonPositions[i].z);

            fusionedSkeletonData.eSkeletonPositionTrackingState[i] = KinectWrapper.NuiSkeletonPositionTrackingState.Tracked;
            fusionedSkeletonData.dwTrackingID = baseSkeletonData.dwTrackingID;
            fusionedSkeletonData.dwQualityFlags = baseSkeletonData.dwQualityFlags;
            fusionedSkeletonData.dwUserIndex = baseSkeletonData.dwUserIndex;
            fusionedSkeletonData.eTrackingState = KinectWrapper.NuiSkeletonTrackingState.SkeletonTracked;

        }

        return fusionedSkeletonData;
    }

    KinectWrapper.NuiSkeletonData GetFusionedSkeletonLocalCal()
    {
        //calibratedClientsSkeletonData = new KinectWrapper.NuiSkeletonData[nbKinectMax];

        KinectWrapper.NuiSkeletonData fusionedSkeletonData = new KinectWrapper.NuiSkeletonData();
        fusionedSkeletonData.SkeletonPositions = new Vector4[20];
        fusionedSkeletonData.eSkeletonPositionTrackingState = new KinectWrapper.NuiSkeletonPositionTrackingState[20];
        int idSource = 0;

        if (baseSkeletonID == 1)
        {
            idSource = 2;
        }
        else
        {
            idSource = 1;
        }


        Debug.Log("----------------------------------------  ID_CLIENTS   ------------------------------------------");
        Debug.Log("idSource = " + idSource);
        Debug.Log("baseSkeletonID = " + baseSkeletonID);

        //Debug.Log("idSource nbJointTracked = " + MySkeletonFrameTable[idSource].SkeletonData[0].nbJointTracked);
        //Debug.Log("baseSkeletonID nbJointTracked = " + MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].nbJointTracked);

        Debug.Log("");

        Debug.Log("  M[00] =  " + initCalibration.transformationMatrixes[idSource][baseSkeletonID][0][0] + "   M[01] " + initCalibration.transformationMatrixes[idSource][baseSkeletonID][0][1] + "  M[02] = " + initCalibration.transformationMatrixes[idSource][baseSkeletonID][0][2]);
        Debug.Log("  M[10] =  " + initCalibration.transformationMatrixes[idSource][baseSkeletonID][1][0] + "   M[11]" + initCalibration.transformationMatrixes[idSource][baseSkeletonID][1][1] + "   M[12] = " + initCalibration.transformationMatrixes[idSource][baseSkeletonID][1][2]);
        Debug.Log("  M[20] =  " + initCalibration.transformationMatrixes[idSource][baseSkeletonID][2][0] + "   M[21] " + initCalibration.transformationMatrixes[idSource][baseSkeletonID][2][1] + "  M[22] = " + initCalibration.transformationMatrixes[idSource][baseSkeletonID][2][2]);

        // Estimer la position du squelette 
        float xSkeleton = MySkeletonFrameTable[idSource].SkeletonData[0].Position.x * initCalibration.transformationMatrixes[idSource][baseSkeletonID][0][0] + MySkeletonFrameTable[idSource].SkeletonData[0].Position.y * initCalibration.transformationMatrixes[idSource][baseSkeletonID][0][1] + MySkeletonFrameTable[idSource].SkeletonData[0].Position.z * initCalibration.transformationMatrixes[idSource][baseSkeletonID][0][2];
        float ySkeleton = MySkeletonFrameTable[idSource].SkeletonData[0].Position.x * initCalibration.transformationMatrixes[idSource][baseSkeletonID][1][0] + MySkeletonFrameTable[idSource].SkeletonData[0].Position.y * initCalibration.transformationMatrixes[idSource][baseSkeletonID][1][1] + MySkeletonFrameTable[idSource].SkeletonData[0].Position.z * initCalibration.transformationMatrixes[idSource][baseSkeletonID][1][2];
        float zSkeleton = MySkeletonFrameTable[idSource].SkeletonData[0].Position.x * initCalibration.transformationMatrixes[idSource][baseSkeletonID][2][0] + MySkeletonFrameTable[idSource].SkeletonData[0].Position.y * initCalibration.transformationMatrixes[idSource][baseSkeletonID][2][1] + MySkeletonFrameTable[idSource].SkeletonData[0].Position.z * initCalibration.transformationMatrixes[idSource][baseSkeletonID][2][2];

        fusionedSkeletonData.Position.x = (MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].Position.x + xSkeleton) / 2;
        fusionedSkeletonData.Position.y = (MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].Position.y + ySkeleton) / 2;
        fusionedSkeletonData.Position.z = (MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].Position.z + zSkeleton) / 2;


        // Estimer la position Pour Chaque Joint du squelette 
        for (int f = 0; f < 20; f++)
        {
            //Si la joint est bien traquée Alors calculer l'estimation de ces coordonnée dans le repère du squelete destination
            if (KinectWrapper.NuiSkeletonPositionTrackingState.Tracked == MySkeletonFrameTable[idSource].SkeletonData[0].eSkeletonPositionTrackingState[f])
            {

                float x2 = MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].x * initCalibration.transformationMatrixes[idSource][baseSkeletonID][0][0] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[idSource][baseSkeletonID][0][1] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[idSource][baseSkeletonID][0][2];
                float y2 = MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].x * initCalibration.transformationMatrixes[idSource][baseSkeletonID][1][0] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[idSource][baseSkeletonID][1][1] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[idSource][baseSkeletonID][1][2];
                float z2 = MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].x * initCalibration.transformationMatrixes[idSource][baseSkeletonID][2][0] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[idSource][baseSkeletonID][2][1] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[idSource][baseSkeletonID][2][2];

                float DX = 0f;
                float DY = 0f;
                float DZ = 0f;

                DX = x2 - MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].SkeletonPositions[f].x;
                DY = y2 - MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].SkeletonPositions[f].y;
                DZ = z2 - MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].SkeletonPositions[f].z;

                //Debug.Log(" j = " + f + " X0 =   " + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].x + " Y0 = " + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].y + " Z0 = " + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].z);

                //Debug.Log(" j = " + f + " X =   " + MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].SkeletonPositions[f].x + " Y = " + MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].SkeletonPositions[f].y + " Z = " + MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].SkeletonPositions[f].z);
                //Debug.Log(" j = " + f + " X' =  " + x2 + "  Y = " + y2 + "  Z' = " + z2);
                Debug.Log(" j = " + f + " DX =  " + DX + "  DY " + DY + "  DZ = " + DZ);


                fusionedSkeletonData.SkeletonPositions[f].x = (x2 + MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].SkeletonPositions[f].x) / 2;
                fusionedSkeletonData.SkeletonPositions[f].y = (y2 + MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].SkeletonPositions[f].y) / 2;
                fusionedSkeletonData.SkeletonPositions[f].z = (z2 + MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].SkeletonPositions[f].z) / 2;


                fusionedSkeletonData.eSkeletonPositionTrackingState[f] = KinectWrapper.NuiSkeletonPositionTrackingState.Tracked;
                fusionedSkeletonData.dwTrackingID = MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].dwTrackingID;
                fusionedSkeletonData.dwQualityFlags = MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].dwQualityFlags;
                fusionedSkeletonData.dwUserIndex = MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].dwUserIndex;
                fusionedSkeletonData.eTrackingState = KinectWrapper.NuiSkeletonTrackingState.SkeletonTracked;
            }

        }



        return fusionedSkeletonData;
    }



    int repereGlobal = 3;

    void GetFusionedSkeletonWithOutAverage()
    {


        //Debug.Log("GetFusionedSkeletonWithOutAverage ");


        //Debug.Log("Mes clients sont : " + clientIDs.Count);
        //Debug.Log("Mes clients traqués sont : " + clientsTracked.Count);

       /// Debug.Log("Le client de base est = " + baseSkeletonID);


        if (repereGlobal == -1)
        {
            repereGlobal = baseSkeletonID;
        }
       // Debug.Log("Le repere Global est = " + repereGlobal);


        //Debug.Log("Pour toutes les joint : ");
        // Estimer la position Pour Chaque Joint du squelette 
        for (int f = 0; f < 20; f++)
        {

           /* if (KinectWrapper.NuiSkeletonPositionTrackingState.Tracked != MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].eSkeletonPositionTrackingState[f])
            {
                Debug.Log("Joint  : " + f + "    n'est pas traquée");

                float moyX = 0f, moyY = 0f, moyZ = 0f;
                int cpt = 0;
                foreach (int client in clientsTracked)
                {
                    int idSource = clientsTracked[0];

                    Debug.Log("Voir Client :  " + idSource);

                    if (idSource != baseSkeletonID)
                    {
                        

                        if (KinectWrapper.NuiSkeletonPositionTrackingState.Tracked == MySkeletonFrameTable[idSource].SkeletonData[0].eSkeletonPositionTrackingState[f])
                        {
                            cpt++;
                            Debug.Log("elle est courigée par ce client  = " + idSource);

                            float x2 = MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].x * initCalibration.transformationMatrixes[idSource][baseSkeletonID][0][0] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[idSource][baseSkeletonID][0][1] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[idSource][baseSkeletonID][0][2];
                            float y2 = MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].x * initCalibration.transformationMatrixes[idSource][baseSkeletonID][1][0] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[idSource][baseSkeletonID][1][1] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[idSource][baseSkeletonID][1][2];
                            float z2 = MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].x * initCalibration.transformationMatrixes[idSource][baseSkeletonID][2][0] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[idSource][baseSkeletonID][2][1] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[idSource][baseSkeletonID][2][2];

                            moyX = moyX + x2;
                            moyY = moyY + y2;
                            moyZ = moyZ + z2;

                            /*MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].SkeletonPositions[f].x = x2;
                            MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].SkeletonPositions[f].y = y2;
                            MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].SkeletonPositions[f].z = z2;
                            */
                /*        }

                    }
                }
                MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].SkeletonPositions[f].x = moyX/cpt;
                MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].SkeletonPositions[f].y = moyY/cpt;
                MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].SkeletonPositions[f].z = moyZ/cpt;
                MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].eSkeletonPositionTrackingState[f] = KinectWrapper.NuiSkeletonPositionTrackingState.Tracked;
            }

            else
            {
                Debug.Log("Joint  : " + f + "    est traquée");
            }
    */

            //Debug.Log("Changer le repère  :  ");
            if (baseSkeletonID != repereGlobal)
            {
                float DX = 0, DY = 0, DZ = 0;
                float distanceCarree = 0f;

                /* int temp = repereGlobal;
                 repereGlobal = baseSkeletonID;
                 baseSkeletonID = temp;*/

               // Debug.Log("                         Oui   ");
                int idSource = baseSkeletonID;

                float x2 = MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].x; //* initCalibration.transformationMatrixes[idSource][repereGlobal][0][0] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[idSource][repereGlobal][0][1] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[idSource][repereGlobal][0][2];
                float y2 = MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].y;// * initCalibration.transformationMatrixes[idSource][repereGlobal][1][0] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[idSource][repereGlobal][1][1] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[idSource][repereGlobal][1][2];
                float z2 = MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].z; //* initCalibration.transformationMatrixes[idSource][repereGlobal][2][0] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[idSource][repereGlobal][2][1] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[idSource][repereGlobal][2][2];

                /*temp = repereGlobal;
                repereGlobal = baseSkeletonID;
                baseSkeletonID = temp;*/




                DX = x2 - MySkeletonFrameTable[repereGlobal].SkeletonData[0].SkeletonPositions[f].x;
                DY = y2 - MySkeletonFrameTable[repereGlobal].SkeletonData[0].SkeletonPositions[f].y;
                DZ = z2 - MySkeletonFrameTable[repereGlobal].SkeletonData[0].SkeletonPositions[f].z;

                distanceCarree = (float)Math.Sqrt(Math.Pow(DX, 2) + Math.Pow(DY, 2) + Math.Pow(DZ, 2));

                //Debug.Log(" f = " + f + " X =   " + MySkeletonFrameTable[repereGlobal].SkeletonData[0].SkeletonPositions[f].x + " Y = " + MySkeletonFrameTable[repereGlobal].SkeletonData[0].SkeletonPositions[f].y + " Z = " + MySkeletonFrameTable[repereGlobal].SkeletonData[0].SkeletonPositions[f].z);
                //Debug.Log(" f = " + f + " X' =  " + x2 + "  Y = " + y2 + "  Z' = " + z2);
                //Debug.Log(" f = " + f + " DX =  " + DX + "  DY " + DY + "  DZ = " + DZ);
                //Debug.Log("Distance Carée  " + distanceCarree);



                MySkeletonFrameTable[repereGlobal].SkeletonData[0].eSkeletonPositionTrackingState[f] = KinectWrapper.NuiSkeletonPositionTrackingState.Tracked;



                MySkeletonFrameTable[repereGlobal].SkeletonData[0].SkeletonPositions[f].x = x2;
                MySkeletonFrameTable[repereGlobal].SkeletonData[0].SkeletonPositions[f].y = y2;
                MySkeletonFrameTable[repereGlobal].SkeletonData[0].SkeletonPositions[f].z = z2;
            }
            else
            {
                //Debug.Log("                         Non   ");
            }




        }
        /*
        if (baseSkeletonID != repereGlobal)
        {
                /*int temp = repereGlobal;
                repereGlobal = baseSkeletonID;
                baseSkeletonID = temp;
               */
      /*    int idSource = baseSkeletonID;



            float x2 = MySkeletonFrameTable[idSource].SkeletonData[0].Position.x *initCalibration.transformationMatrixes[idSource][repereGlobal][0][0] + MySkeletonFrameTable[idSource].SkeletonData[0].Position.y * initCalibration.transformationMatrixes[idSource][repereGlobal][0][1] + MySkeletonFrameTable[idSource].SkeletonData[0].Position.z * initCalibration.transformationMatrixes[idSource][repereGlobal][0][2];
            float y2 = MySkeletonFrameTable[idSource].SkeletonData[0].Position.x *initCalibration.transformationMatrixes[idSource][repereGlobal][1][0] + MySkeletonFrameTable[idSource].SkeletonData[0].Position.y * initCalibration.transformationMatrixes[idSource][repereGlobal][1][1] + MySkeletonFrameTable[idSource].SkeletonData[0].Position.z * initCalibration.transformationMatrixes[idSource][repereGlobal][1][2];
            float z2 = MySkeletonFrameTable[idSource].SkeletonData[0].Position.x *initCalibration.transformationMatrixes[idSource][repereGlobal][2][0] + MySkeletonFrameTable[idSource].SkeletonData[0].Position.y * initCalibration.transformationMatrixes[idSource][repereGlobal][2][1] + MySkeletonFrameTable[idSource].SkeletonData[0].Position.z * initCalibration.transformationMatrixes[idSource][repereGlobal][2][2];


   
            /*temp = repereGlobal;
            repereGlobal = baseSkeletonID;
            baseSkeletonID = temp;*/

          /*  MySkeletonFrameTable[repereGlobal].SkeletonData[0].Position.x = x2;
            MySkeletonFrameTable[repereGlobal].SkeletonData[0].Position.y = y2;
            MySkeletonFrameTable[repereGlobal].SkeletonData[0].Position.z = z2;


        }*/
        /*
        MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].Position.x = MySkeletonFrameTable[repereGlobal].SkeletonData[0].Position.x;
        MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].Position.y = MySkeletonFrameTable[repereGlobal].SkeletonData[0].Position.y;
        MySkeletonFrameTable[baseSkeletonID].SkeletonData[0].Position.z = MySkeletonFrameTable[repereGlobal].SkeletonData[0].Position.z;
        
        */
        
    }


    void GetFusionedSkeletonWithOutAverage2()
    {

        int id1 = 3;
        int id2 = 4;


        Debug.Log("GetFusionedSkeletonWithOutAverage2 ");
        

        for (int f = 0; f < 20; f++)
        {

            float DX = 0, DY = 0, DZ = 0;
            float distanceCarree = 0f;




            float x1 = MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[f].x * initCalibration.transformationMatrixes[id1][id2][0][0] + MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[id1][id2][0][1] + MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[id1][id2][0][2];
            float y1 = MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[f].x * initCalibration.transformationMatrixes[id1][id2][1][0] + MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[id1][id2][1][1] + MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[id1][id2][1][2];
            float z1 = MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[f].x * initCalibration.transformationMatrixes[id1][id2][2][0] + MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[id1][id2][2][1] + MySkeletonFrameTable[id1].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[id1][id2][2][2];





         

            MySkeletonFrameTable[id2].SkeletonData[0].eSkeletonPositionTrackingState[f] = KinectWrapper.NuiSkeletonPositionTrackingState.Tracked;



            MySkeletonFrameTable[id2].SkeletonData[0].SkeletonPositions[f].x = x1;
            MySkeletonFrameTable[id2].SkeletonData[0].SkeletonPositions[f].y = y1;
            MySkeletonFrameTable[id2].SkeletonData[0].SkeletonPositions[f].z = z1;





        }




        /*

        float x2 = MySkeletonFrameTable[id1].SkeletonData[0].Position.x * initCalibration.transformationMatrixes[id1][id2][0][0] + MySkeletonFrameTable[id1].SkeletonData[0].Position.y * initCalibration.transformationMatrixes[id1][id2][0][1] + MySkeletonFrameTable[id1].SkeletonData[0].Position.z * initCalibration.transformationMatrixes[id1][id2][0][2];
        float y2 = MySkeletonFrameTable[id1].SkeletonData[0].Position.x * initCalibration.transformationMatrixes[id1][id2][1][0] + MySkeletonFrameTable[id1].SkeletonData[0].Position.y * initCalibration.transformationMatrixes[id1][id2][1][1] + MySkeletonFrameTable[id1].SkeletonData[0].Position.z * initCalibration.transformationMatrixes[id1][id2][1][2];
        float z2 = MySkeletonFrameTable[id1].SkeletonData[0].Position.x * initCalibration.transformationMatrixes[id1][id2][2][0] + MySkeletonFrameTable[id1].SkeletonData[0].Position.y * initCalibration.transformationMatrixes[id1][id2][2][1] + MySkeletonFrameTable[id1].SkeletonData[0].Position.z * initCalibration.transformationMatrixes[id1][id2][2][2];



        MySkeletonFrameTable[id2].SkeletonData[0].Position.x = x2;
        MySkeletonFrameTable[id2].SkeletonData[0].Position.y = y2;
        MySkeletonFrameTable[id2].SkeletonData[0].Position.z = z2;


        */
    }



    void GetFusionedSkeletonTeste()
    {
        
        repereGlobal = 3;

        int idSource = 4;


        // Estimer la position Pour Chaque Joint du squelette 
        for (int f = 0; f < 20; f++)
        {
            


                float x2 = MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].x * initCalibration.transformationMatrixes[idSource][repereGlobal][0][0] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[idSource][repereGlobal][0][1] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[idSource][repereGlobal][0][2];
                float y2 = MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].x * initCalibration.transformationMatrixes[idSource][repereGlobal][1][0] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[idSource][repereGlobal][1][1] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[idSource][repereGlobal][1][2];
                float z2 = MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].x * initCalibration.transformationMatrixes[idSource][repereGlobal][2][0] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[idSource][repereGlobal][2][1] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[idSource][repereGlobal][2][2];

                MySkeletonFrameTable[repereGlobal].SkeletonData[0].eSkeletonPositionTrackingState[f] = KinectWrapper.NuiSkeletonPositionTrackingState.Tracked;





                MySkeletonFrameTable[repereGlobal].SkeletonData[0].SkeletonPositions[f].x = x2;
                MySkeletonFrameTable[repereGlobal].SkeletonData[0].SkeletonPositions[f].y = y2;
                MySkeletonFrameTable[repereGlobal].SkeletonData[0].SkeletonPositions[f].z = z2;


            




        }
    }



    
    bool hasChange = false;


    KinectWrapper.NuiSkeletonData[] GetAjustedClients()
    {

        KinectWrapper.NuiSkeletonData[] ajustedClientsSkeletonData = new KinectWrapper.NuiSkeletonData[nbKinectMax];
        KinectWrapper.NuiSkeletonData convertedFusionedSkeletonData = new KinectWrapper.NuiSkeletonData();

        // Pour chaque client estimer ces valeurs dans le repère de base 
        List<int> clients = ClientTracked();
        clients.Remove(baseSkeletonID);

        foreach (int client in clients)
        {
            ajustedClientsSkeletonData[client].SkeletonPositions = new Vector4[20];
            float moyX = 0f, moyY = 0f, moyZ = 0f;

            convertedFusionedSkeletonData = calibration.EstimateSkeleton(baseSkeletonID, client, MySkeletonFrameTable);


            moyX = (MySkeletonFrameTable[client].SkeletonData[0].Position.x + convertedFusionedSkeletonData.Position.x) / 2;
            moyY = (MySkeletonFrameTable[client].SkeletonData[0].Position.y + convertedFusionedSkeletonData.Position.y) / 2;
            moyZ = (MySkeletonFrameTable[client].SkeletonData[0].Position.z + convertedFusionedSkeletonData.Position.z) / 2;


            for (int i = 0; i < 20; i++)
            {

                if (MySkeletonFrameTable[client].SkeletonData[0].eSkeletonPositionTrackingState[i] == KinectWrapper.NuiSkeletonPositionTrackingState.Tracked)
                {

                    moyX = (MySkeletonFrameTable[client].SkeletonData[0].SkeletonPositions[i].x + convertedFusionedSkeletonData.SkeletonPositions[i].x) / 2;
                    moyY = (MySkeletonFrameTable[client].SkeletonData[0].SkeletonPositions[i].y + convertedFusionedSkeletonData.SkeletonPositions[i].y) / 2;
                    moyZ = (MySkeletonFrameTable[client].SkeletonData[0].SkeletonPositions[i].z + convertedFusionedSkeletonData.SkeletonPositions[i].z) / 2;


                }else
                {
                    moyX =  convertedFusionedSkeletonData.SkeletonPositions[i].x ;
                    moyY =  convertedFusionedSkeletonData.SkeletonPositions[i].y ;
                    moyZ = convertedFusionedSkeletonData.SkeletonPositions[i].z;

                }

                ajustedClientsSkeletonData[client].SkeletonPositions[i].x = moyX;
                ajustedClientsSkeletonData[client].SkeletonPositions[i].y = moyY;
                ajustedClientsSkeletonData[client].SkeletonPositions[i].z = moyZ;
            }
        }

        return ajustedClientsSkeletonData;
    }


    Quaternion angle, angle2;
    bool cal = false;
    float d = 0;

    float AvatarOrientation()
    {
        Matrix4x4[] jointOrients = new Matrix4x4[20];
        Matrix4x4[] jointOrients2 = new Matrix4x4[20];

        Vector3[] jointsPos;
        Vector3[] jointsPos2;

        
        
        if (baseSkeletonID == 1)
        {
            jointsPos = GetJointsPosV3(baseSkeletonID);
            jointsPos2 = GetJointsPosV3(2);
        }
        else
        {
            jointsPos = GetJointsPosV3(1);
            jointsPos2 = GetJointsPosV3(baseSkeletonID);
        }

    
        GeOrient(jointsPos, ref jointOrients);
        GeOrient(jointsPos2, ref jointOrients2);


        Quaternion angle  = ConvertMatrixToQuat(jointOrients[0], (int)KinectWrapper.NuiSkeletonPositionIndex.HipCenter, true);
        Quaternion angle2 = ConvertMatrixToQuat(jointOrients2[0], (int)KinectWrapper.NuiSkeletonPositionIndex.HipCenter, true);

        //Quaternion angle3 =

        //cap.transform.Rotate();
        float DY = angle.eulerAngles.y - angle2.eulerAngles.y;
        float DY1 = angle2.eulerAngles.y - angle.eulerAngles.y;

        float D =  angle.eulerAngles.y;

        if (!cal)
        {
            d = 360 - DY1;
        }
        

        // cap.transform.rotation = Quaternion.Euler(0, angle.eulerAngles.y, 0);
        // avatar.transform.rotation = Quaternion.Euler(avatar.transform.eulerAngles.x, angle.eulerAngles.y, avatar.transform.eulerAngles.z);

      
        cap.transform.rotation = Quaternion.Euler(0, angle.eulerAngles.y+50, 0); 
        avatar.transform.rotation = Quaternion.Euler(0, angle.eulerAngles.y+50, 0); 
        
        Vector3 eulerAngle = angle.eulerAngles;
        Vector3 eulerAngle2 = angle2.eulerAngles;
     
        /*
        Debug.Log("*******************************************************************************************************************************************");
        Debug.Log("*******************************************************************************************************************************************");
        Debug.Log("*******************************************************************************************************************************************");
        Debug.Log("*******************************************************************************************************************************************");
        Debug.Log("*******************************************************************************************************************************************");
        Debug.Log("*******************************************************************************************************************************************");
        Debug.Log("*******************************************************************************************************************************************");
        Debug.Log("*******************************************************************************************************************************************");
        Debug.Log("*******************************************************************************************************************************************");
        Debug.Log("*******************************************************************************************************************************************");
        Debug.Log("eulerAngle        X = "+ eulerAngle.x + "      Y = "+ eulerAngle.y+"      Z = "+ eulerAngle.z);
        Debug.Log("eulerAngle2        X = "+ eulerAngle2.x + "      Y = "+ eulerAngle2.y+"      Z = "+ eulerAngle2.z);
        Debug.Log("DY        DY = "+ DY);
        Debug.Log("DY1        DY1 = "+ DY1);*/
        return d;
    }

    bool[] jointsStates = new bool[20];

    Vector3[] GetJointsPosV3(int id)
    {
        
        Vector3[] jointsPos = new Vector3[20];
        

        for (int i=0; i<20; i++)
        {
            jointsPos[i].x = MySkeletonFrameTable[id].SkeletonData[0].SkeletonPositions[i].x;
            jointsPos[i].y = MySkeletonFrameTable[id].SkeletonData[0].SkeletonPositions[i].y;
            jointsPos[i].z = MySkeletonFrameTable[id].SkeletonData[0].SkeletonPositions[i].z;
            jointsStates[i] = 2 == (int)MySkeletonFrameTable[id].SkeletonData[0].eSkeletonPositionTrackingState[i];
        }

        return jointsPos;
    }


    void GeOrient(Vector3[] jointsPos, ref Matrix4x4[] jointOrients)
    {
        Vector3 vx;
        Vector3 vy;
        Vector3 vz;

       
        vy = GetPositionBetweenIndices(jointsPos, KinectWrapper.NuiSkeletonPositionIndex.HipCenter, KinectWrapper.NuiSkeletonPositionIndex.Spine);
        vx = GetPositionBetweenIndices(jointsPos, KinectWrapper.NuiSkeletonPositionIndex.HipLeft, KinectWrapper.NuiSkeletonPositionIndex.HipRight);
        MakeMatrixFromYX(vx, vy, ref jointOrients[(int)KinectWrapper.NuiSkeletonPositionIndex.HipCenter]);

        // make a correction of about 40 degrees back to the front
        Matrix4x4 mat = jointOrients[(int)KinectWrapper.NuiSkeletonPositionIndex.HipCenter];
        Quaternion quat = Quaternion.LookRotation(mat.GetColumn(2), mat.GetColumn(1));
        quat *= Quaternion.Euler(-40, 0, 0);
        jointOrients[(int)KinectWrapper.NuiSkeletonPositionIndex.HipCenter].SetTRS(Vector3.zero, quat, Vector3.one);
        
    }

    private static Vector3 GetPositionBetweenIndices(Vector3[] jointsPos, KinectWrapper.NuiSkeletonPositionIndex p1, KinectWrapper.NuiSkeletonPositionIndex p2)
    {
        Vector3 pVec1 = jointsPos[(int)p1];
        Vector3 pVec2 = jointsPos[(int)p2];

        return pVec2 - pVec1;
    }

    //constructs an orientation from 2 vectors: the first specifies the x axis, and the next specifies the y axis
    //uses the second vector as y axis, then constructs the other axes using cross products
    private static void MakeMatrixFromYX(Vector3 xUnnormalized, Vector3 yUnnormalized, ref Matrix4x4 jointOrientation)
    {
        //matrix columns
        Vector3 xCol;
        Vector3 yCol;
        Vector3 zCol;

        //set up the three different columns to be rearranged and flipped
        yCol = yUnnormalized.normalized;
        zCol = Vector3.Cross(xUnnormalized.normalized, yCol).normalized;
        xCol = Vector3.Cross(yCol, zCol).normalized;
        //xCol = xUnnormalized.normalized;
        //zCol = Vector3.Cross(xCol, yCol).normalized;

        //copy values into matrix
        PopulateMatrix(ref jointOrientation, xCol, yCol, zCol);
    }

    //populate matrix using the columns
    private static void PopulateMatrix(ref Matrix4x4 jointOrientation, Vector3 xCol, Vector3 yCol, Vector3 zCol)
    {
        jointOrientation.SetColumn(0, xCol);
        jointOrientation.SetColumn(1, yCol);
        jointOrientation.SetColumn(2, zCol);
    }


    // convert the matrix to quaternion, taking care of the mirroring
    private Quaternion ConvertMatrixToQuat(Matrix4x4 mOrient, int joint, bool flip)
    {
        Vector4 vZ = mOrient.GetColumn(2);
        Vector4 vY = mOrient.GetColumn(1);

        if (!flip)
        {
            vZ.y = -vZ.y;
            vY.x = -vY.x;
            vY.z = -vY.z;
        }
        else
        {
            vZ.x = -vZ.x;
            vZ.y = -vZ.y;
            vY.z = -vY.z;
        }

        if (vZ.x != 0.0f || vZ.y != 0.0f || vZ.z != 0.0f)
            return Quaternion.LookRotation(vZ, vY);
        else
            return Quaternion.identity;
    }

    //Association entre les indices et les noms des bones
    private readonly Dictionary<int, HumanBodyBones> boneIndexMap = new Dictionary<int, HumanBodyBones>
    {
        {0, HumanBodyBones.Hips},//HipCenter = 0
        {1, HumanBodyBones.Spine},// Spine = 1,
        {2, HumanBodyBones.Neck},//ShoulderCenter = 2,
        {3, HumanBodyBones.Head},//Head = 3,  

        {4, HumanBodyBones.LeftShoulder},//
        {5, HumanBodyBones.LeftUpperArm}, //ShoulderLeft = 4,
        {6, HumanBodyBones.LeftLowerArm},//  ElbowLeft = 5,
        {7, HumanBodyBones.LeftHand},// WristLeft = 6,
        {8, HumanBodyBones.LeftIndexProximal},//HandLeft = 7,

        {9, HumanBodyBones.RightShoulder},//
        {10, HumanBodyBones.RightUpperArm},// ShoulderRight = 8,
        {11, HumanBodyBones.RightLowerArm},//ElbowRight = 9,
        {12, HumanBodyBones.RightHand},//  WristRight = 10,
        {13, HumanBodyBones.RightIndexProximal},// HandRight = 11,

        {14, HumanBodyBones.LeftUpperLeg},//HipLeft = 12,
        {15, HumanBodyBones.LeftLowerLeg},// KneeLeft = 13,
        {16, HumanBodyBones.LeftFoot},// AnkleLeft = 14,
        {17, HumanBodyBones.LeftToes},//FootLeft = 15,

        {18, HumanBodyBones.RightUpperLeg},// HipRight = 16,
        {19, HumanBodyBones.RightLowerLeg},//KneeRight = 17,
        {20, HumanBodyBones.RightFoot},// AnkleRight = 18,
        {21, HumanBodyBones.RightToes},// FootRight = 19,
    };



    Transform[] articu = new Transform[22];
    AnimationData sp2;
    AnimationData2 sp;
    bool start = false;
    string[] nomArticu = new string[22];


    string path;


    void getJoint()
    {
        SquelleteData mysp1 = new SquelleteData();
        mysp1.articuPosition = new Vector3[22];
        mysp1.articuRotation = new Quaternion[22];
        mysp1.scale = new Vector3[22];
        mysp1.avatarPosition = new Vector3();

        var animator = myAvatarFusion2.GetComponent<Animator>();

        for (int boneIndex = 0; boneIndex < articu.Length; boneIndex++)
        {
            if (!boneIndexMap.ContainsKey(boneIndex))
                continue;
            articu[boneIndex] = animator.GetBoneTransform(boneIndexMap[boneIndex]);

        }

        //mysp1.avatarPosition = GameObject.Find("AvatarFusion").GetComponent<Transform>().transform.position;
        //mysp1.avatarPosition = myAvatarFusion2.transform.position;

        mysp1.avatarPosition = new Vector3(myAvatarFusion.position.x, myAvatarFusion.position.y, myAvatarFusion.position.z);
        mysp1.avatarRotation = myAvatarFusion2.rotation;

        for (int i = 0; i < articu.Length; i++)
        {
            mysp1.articuPosition[i] = articu[i].localPosition;
            mysp1.articuRotation[i] = articu[i].localRotation;
            mysp1.scale[i] = articu[i].localScale;
        }
        mysp1.timeExec = t2;
        getNameArticu();
        sp2.squelleteData.Add(mysp1);

    }

    public void SaveData()
    {
        String content = JsonUtility.ToJson(sp2);
        Debug.Log(content);
        Debug.Log("-----------------------------------------------------------------------------------------------------------------------------");
        Debug.Log(Application.streamingAssetsPath + " /RotationAvatarData" + newExamControlor.idExamen + ".json");

        path = Application.streamingAssetsPath + "/RotationAvatarData" + newExamControlor.idExamen + ".json";
        File.WriteAllText(path, content);
    }

    public void SaveData2()
    {
        String content = JsonUtility.ToJson(sp);
        Debug.Log(content);
        path = Application.streamingAssetsPath + "/fusionDataFromAM" + newExamControlor.idExamen + ".json";
        File.WriteAllText(path, content);
    }

    public void resetDataVariable()
    {
        sp2 = new AnimationData();
        sp2.squelleteData = new List<SquelleteData>();
        sp = new AnimationData2();
        sp.squelleteData = new List<SquelleteData2>();
    }

    void getNameArticu()
    {
        nomArticu[0] = "Hips";
        nomArticu[1] = "Spine";
        nomArticu[2] = "Neck";
        nomArticu[3] = "Head";
        nomArticu[4] = "LeftShoulder";
        nomArticu[5] = "LeftUpperArm";
        nomArticu[6] = "LeftLowerArm";
        nomArticu[7] = "LeftHand";
        nomArticu[8] = "LeftIndexProximal";
        nomArticu[9] = "RightShoulder";
        nomArticu[10] = "RightUpperArm";
        nomArticu[11] = "RightLowerArm";
        nomArticu[12] = "RightHand";
        nomArticu[13] = "RightIndexProximal";
        nomArticu[14] = "LeftUpperLeg";
        nomArticu[15] = "LeftLowerLeg";
        nomArticu[16] = "LeftFoot";
        nomArticu[17] = "LeftToes";
        nomArticu[18] = "RightUpperLeg";
        nomArticu[19] = "RightLowerLeg";
        nomArticu[20] = "RightFoot";
        nomArticu[21] = "RightToes";
    }
    [Serializable]
    public class SquelleteData
    {
        public Vector3 avatarPosition;
        public Quaternion avatarRotation;
        public Vector3[] articuPosition;
        public Quaternion[] articuRotation;
        public Vector3[] scale;
        public double timeExec;
    }

    public void StartSaving()
    {
        start = true;
    }

    public void StopSaving()
    {
        start = false;
    }


    [Serializable]
    public class AnimationData
    {
        public List<SquelleteData> squelleteData;
    }


    [Serializable]
    public class AnimationData2
    {
        public List<SquelleteData2> squelleteData;
    }

    [Serializable]
    public class SquelleteData2
    {
        public Vector3 avatarPosition;
        public Vector3[] articuPosition;
        public Quaternion[] articuRotation;
        public Vector3[] scale;
        public double timeExec;
    }


}
