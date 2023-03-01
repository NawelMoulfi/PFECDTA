using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;




public class SaveAnimation : MonoBehaviour
{
    public class Data
    {
        public string name;
        public int age;

    }

     Transform[] articu = new Transform[22];


    string[] nomArticu = new string[22];

    string path;
    AnimationData sp2;

    // Start is called before the first frame update
    void Start()
    {

        sp2 = new AnimationData();
        sp2.squelleteData = new List<SquelleteData>();


        /*path = "RoundTripTime.txt";
        //créer le fichier s'il n'existe pas
        if (!(File.Exists(path)))
        {
            string Entete = "Round Time Trip for the client :\n";
            File.WriteAllText(path, Entete);
        }*/

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
    void getJoint()
    {
        SquelleteData sp1 = new SquelleteData();
        sp1.articuPosition = new Vector3[22];
        sp1.articuRotation = new Quaternion[22];
        sp1.scale = new Vector3[22];

        var animator = this.GetComponent<Animator>();
        for (int boneIndex = 0; boneIndex < articu.Length; boneIndex++)
        {
            if (!boneIndex2MecanimMap.ContainsKey(boneIndex))
                continue;
            articu[boneIndex] = animator.GetBoneTransform(boneIndex2MecanimMap[boneIndex]);

        }
        for (int i = 0; i < articu.Length; i++)
        {

            sp1.articuPosition[i] = articu[i].localPosition;
            sp1.articuRotation[i] = articu[i].localRotation;
            sp1.scale[i] = articu[i].localScale;
        }

        getNameArticu();
        sp2.squelleteData.Add(sp1);

    }
    string content;


    int i = 0;


    bool start = false;

    // Update is called once per frame
    void Update()
    {

        /*
        if (i == 3001)
            return;

        
        if (i == 3000)
        {
            String content = JsonUtility.ToJson(sp2);
            Debug.Log(content);
            path = Application.streamingAssetsPath + "/fusionData.json";
            File.AppendAllText(path, content);

        }*/
        if (start)
        {
            
            getJoint();
        }
        

       // i++;

    }

    public void StartSaving()
    {
        start = true;
    }

    public void StopSaving()
    {
        start = false;
    }

    public void SaveData()
    {
        String content = JsonUtility.ToJson(sp2);
        Debug.Log(content);
        path = Application.streamingAssetsPath + "/RotationAvatarData.json";
        File.WriteAllText(path, content);
    }





    private readonly Dictionary<int, HumanBodyBones> boneIndex2MecanimMap = new Dictionary<int, HumanBodyBones>
     {
         {0, HumanBodyBones.Hips},
         {1, HumanBodyBones.Spine},
         {2, HumanBodyBones.Neck},
         {3, HumanBodyBones.Head},

         {4, HumanBodyBones.LeftShoulder},
         {5, HumanBodyBones.LeftUpperArm},
         {6, HumanBodyBones.LeftLowerArm},
         {7, HumanBodyBones.LeftHand},
         {8, HumanBodyBones.LeftIndexProximal},

         {9, HumanBodyBones.RightShoulder},
         {10, HumanBodyBones.RightUpperArm},
         {11, HumanBodyBones.RightLowerArm},
         {12, HumanBodyBones.RightHand},
         {13, HumanBodyBones.RightIndexProximal},

         {14, HumanBodyBones.LeftUpperLeg},
         {15, HumanBodyBones.LeftLowerLeg},
         {16, HumanBodyBones.LeftFoot},
         {17, HumanBodyBones.LeftToes},

         {18, HumanBodyBones.RightUpperLeg},
         {19, HumanBodyBones.RightLowerLeg},
         {20, HumanBodyBones.RightFoot},
         {21, HumanBodyBones.RightToes},
     };

    public class SaveArticulation
    {
        public Vector3[] Articulations;
        public string[] NomArticulation;
        public SaveArticulation(Vector3[] Articulation, string[] nomArticulation)
        {
            this.Articulations = Articulation;
            this.NomArticulation = nomArticulation;
        }
    }

    [Serializable]
    public class SaveArticulation2
    {
        public List<Vector3[]> articuPosition;
        public List<Vector3[]> articuRotation;

        public string[] nomArticu;

        public SaveArticulation2(List<Vector3[]> articuPosition, List<Vector3[]> articuRotation, string[] nomArticu)
        {
            //Debug.Log("articu[0] " + articu[0]);

            this.articuPosition = articuPosition;
            this.articuRotation = articuRotation;
            this.nomArticu = nomArticu;
        }
    }



    [Serializable]
    public class SquelleteData
    {
        public Vector3[] articuPosition;
        public Quaternion[] articuRotation;
        public Vector3[] scale;

    }
    [Serializable]
    public class AnimationData
    {
        public List<SquelleteData> squelleteData;
    }



    void readWrite()
    {
        string line;

        //Pass the file path and file name to the StreamReader constructor
        StreamReader sr = new StreamReader("RoundTripTime.txt");

        //Read the first line of text
        line = sr.ReadLine();

        //Continue to read until you reach end of file
        while (line != null)
        {
            //write the lie to console window
            Debug.Log(line);

            //Read the next line
            line = sr.ReadLine();
        }

        //close the file
        sr.Close();



    }
}
