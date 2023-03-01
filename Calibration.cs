using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibration : MonoBehaviour {

    

    //matrice contient tous les squeletes de tous les clients
    public KinectWrapper.NuiSkeletonFrame[] MySkeletonFrameTable;
    List<int> clientIDs = new List<int>();

    initCalibration initCalibration;

    // Use this for initialization
    void Start () {
        initCalibration = GetComponent<initCalibration>();

    }
	

   
    public KinectWrapper.NuiSkeletonData EstimateSkeleton(int idSource,int idDestination, KinectWrapper.NuiSkeletonFrame[] MySkeletonFrameTable)
    {
        
        KinectWrapper.NuiSkeletonData skeletonData = new KinectWrapper.NuiSkeletonData();
        skeletonData.SkeletonPositions = new Vector4[20];
       
        
        Debug.Log("");
        // Estimer la position du squelette 
        float xSkeleton = MySkeletonFrameTable[idSource].SkeletonData[0].Position.x * initCalibration.transformationMatrixes[idSource][idDestination][0][0] + MySkeletonFrameTable[idSource].SkeletonData[0].Position.y * initCalibration.transformationMatrixes[idSource][idDestination][0][1] + MySkeletonFrameTable[idSource].SkeletonData[0].Position.z * initCalibration.transformationMatrixes[idSource][idDestination][0][2];
        float ySkeleton = MySkeletonFrameTable[idSource].SkeletonData[0].Position.x * initCalibration.transformationMatrixes[idSource][idDestination][1][0] + MySkeletonFrameTable[idSource].SkeletonData[0].Position.y * initCalibration.transformationMatrixes[idSource][idDestination][1][1] + MySkeletonFrameTable[idSource].SkeletonData[0].Position.z * initCalibration.transformationMatrixes[idSource][idDestination][1][2];
        float zSkeleton = MySkeletonFrameTable[idSource].SkeletonData[0].Position.x * initCalibration.transformationMatrixes[idSource][idDestination][2][0] + MySkeletonFrameTable[idSource].SkeletonData[0].Position.y * initCalibration.transformationMatrixes[idSource][idDestination][2][1] + MySkeletonFrameTable[idSource].SkeletonData[0].Position.z * initCalibration.transformationMatrixes[idSource][idDestination][2][2];

        skeletonData.Position.x = xSkeleton;
        skeletonData.Position.y = ySkeleton;
        skeletonData.Position.z = zSkeleton;


        // Estimer la position Pour Chaque Joint du squelette 
        for (int f = 0; f < 20; f++)
        {
            //Si la joint est bien traquée Alors calculer l'estimation de ces coordonnée dans le repère du squelete destination
            if (2 == (int)MySkeletonFrameTable[idSource].SkeletonData[0].eSkeletonPositionTrackingState[f])
            {

                float x2 = MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].x * initCalibration.transformationMatrixes[idSource][idDestination][0][0] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[idSource][idDestination][0][1] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[idSource][idDestination][0][2];
                float y2 = MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].x * initCalibration.transformationMatrixes[idSource][idDestination][1][0] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[idSource][idDestination][1][1] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[idSource][idDestination][1][2];
                float z2 = MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].x * initCalibration.transformationMatrixes[idSource][idDestination][2][0] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].y * initCalibration.transformationMatrixes[idSource][idDestination][2][1] + MySkeletonFrameTable[idSource].SkeletonData[0].SkeletonPositions[f].z * initCalibration.transformationMatrixes[idSource][idDestination][2][2];

                float DX = 0f;
                float DY = 0f;
                float DZ = 0f;

                DX = x2 - MySkeletonFrameTable[idDestination].SkeletonData[0].SkeletonPositions[f].x;
                DY = y2 - MySkeletonFrameTable[idDestination].SkeletonData[0].SkeletonPositions[f].y;
                DZ = z2 - MySkeletonFrameTable[idDestination].SkeletonData[0].SkeletonPositions[f].z;

                Debug.Log(" j = " + f + " X =   " + MySkeletonFrameTable[idDestination].SkeletonData[0].SkeletonPositions[f].x + " Y = " + MySkeletonFrameTable[idDestination].SkeletonData[0].SkeletonPositions[f].y + " Z = " + MySkeletonFrameTable[idDestination].SkeletonData[0].SkeletonPositions[f].z);
                Debug.Log(" j = " + f + " X' =  " + x2 + "  Y = " + y2 + "  Z' = " + z2);
                Debug.Log(" j = " + f + " DX =  " + DX + "  DX " + DX + "  DZ = " + DZ);


                skeletonData.SkeletonPositions[f].x = x2;
                skeletonData.SkeletonPositions[f].y = y2;
                skeletonData.SkeletonPositions[f].z = z2;
                   
            }

        }
        
        return skeletonData;
    }



    // Update is called once per frame
    void Update () {


        //clientIDs = fusion.clientIDs;

    }
}
