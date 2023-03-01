using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Networking;

public class AnimationSqueleton : MonoBehaviour
{
    public Transform SLeftToe, SLeftToeBase, SLeftFoot, SLeftLeg, SLeftUpLeg;
    public Transform SRightToe, SRightToeBase, SRightFoot, SRightLeg, RightUpLeg;



    Transform avatarFusion;

    string path, content;

    AnimationData animationData;

    Transform[] bones = new Transform[22];
    public LineRenderer lineRendere;

    // Use this for initialization
    void Start()
    {
        avatarFusion = gameObject.transform;

        path = Application.streamingAssetsPath + "/fusionData3.json";
        content = File.ReadAllText(path);

        animationData = JsonUtility.FromJson<AnimationData>(content);

        lines = new LineRenderer[20];

        if (SkeletonLine)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = Instantiate(SkeletonLine) as LineRenderer;
                lines[i].transform.parent = transform;
            }
        }
    }

    int i = 0;
    SquelleteData st;
    public LineRenderer SkeletonLine;
    private LineRenderer[] lines;
    //public System.Diagnostics.Stopwatch time_execution = new System.Diagnostics.Stopwatch();

    // Update is called once per frame
    void Update()
    {

        if (i == animationData.squelleteData.Count)
            return;
        /*
                TimeSpan ts = time_execution.Elapsed;


                if ((ts.TotalSeconds * 1000) == 4f)
                {

                }*/


        if (anim)
        {
            st = animationData.squelleteData[i];

            animationAvatar(st);
            i++;
            StartCoroutine(waitForAWhile());
            anim = false;

      
            lines[0].SetPosition(0, SLeftToe.position);
            lines[0].SetPosition(1, SLeftToeBase.position);



            lines[1].SetPosition(0, SLeftToeBase.position);
            lines[1].SetPosition(1, SLeftFoot.position);

            lines[2].SetPosition(0, SLeftFoot.position);
            lines[2].SetPosition(1, SLeftLeg.position); 
        }




    }   

    bool anim = true;

    void animmer()
    {
        st = animationData.squelleteData[i];

        animationAvatar(st);
        i++;
    }

    private IEnumerator waitForAWhile()
    {
        yield return new WaitForSeconds((((float)st.timeExec) / 1100));
        anim = true;
    }



    void animationAvatar(SquelleteData st)
    {


        //recupération du Animator de l'avatar de fusion
        var AnimatorFusion = avatarFusion.GetComponent<Animator>();

        //recupération des transforms de tous les bones de l'avatar de fusion et les assosier a la variable bones
        for (int boneIndex = 0; boneIndex < bones.Length; boneIndex++)
        {
            //s'il n'existe pas ne fait rien
            if (!boneIndexMap.ContainsKey(boneIndex))
                continue;
            bones[boneIndex] = AnimatorFusion.GetBoneTransform(boneIndexMap[boneIndex]);
        }




        //Donner les transforms des bones du premier client à l'avatar de fusion
        for (int h = 0; h < bones.Length; h++)
        {
            bones[h].localRotation = st.articuRotation[h];

        }
    }


    [Serializable]
    public class SquelleteData
    {
        public Vector3[] articuPosition;
        public Quaternion[] articuRotation;
        public Vector3[] scale;
        public double timeExec;
    }
    [Serializable]
    public class AnimationData
    {
        public List<SquelleteData> squelleteData;
    }

    //Association entre les indices et les noms des bones
    private readonly Dictionary<int, HumanBodyBones> boneIndexMap = new Dictionary<int, HumanBodyBones>
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
}

