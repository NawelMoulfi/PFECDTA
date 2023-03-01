using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AnimationDifferee : MonoBehaviour
{

    Transform avatarFusion;
    public static string idExamen;

    string path, content;

    AnimationData2 animationData;

    Transform[] bones = new Transform[22];


    // Use this for initialization
    void Start()
    {
        avatarFusion = gameObject.transform;


        path = Application.streamingAssetsPath + "/RotationAvatarData" + InterfaceMedecin.examenSelected.ToString() + ".json";
        content = File.ReadAllText(path);

        animationData = JsonUtility.FromJson<AnimationData2>(content);

     }

    int i = 0;
    SquelleteData2 st;


    // Update is called once per frame
    void Update()
    {
       
        if (i == animationData.squelleteData.Count)
            return;



        if (anim)
        {
            st = animationData.squelleteData[i];

            animationAvatar(st);
            i++;
            StartCoroutine(waitForAWhile());
            anim = false;
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
        yield return new WaitForSeconds((((float)st.timeExec)/2000));
        anim = true;
    }

    

    void animationAvatar(SquelleteData2 st)
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

        avatarFusion.position = st.avatarPosition;
        avatarFusion.rotation = st.avatarRotation;


        //Donner les transforms des bones du premier client à l'avatar de fusion
        for (int h = 0; h < bones.Length; h++)
        {
           
            bones[h].localRotation = st.articuRotation[h];
            bones[h].localPosition = st.articuPosition[h];

        }


    }


    [Serializable]
    public class SquelleteData
    {
        public Vector3[] articuPosition;
        public Quaternion avatarRotation;
        public Quaternion[] articuRotation;
        public Vector3[] scale;
        public double timeExec;
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
        public Quaternion avatarRotation;
        public Vector3[] articuPosition;
        public Quaternion[] articuRotation;
        public Vector3[] scale;
        public double timeExec;
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

   
    public void FermerAnimationDifferee()
    {
        //DBManager.fromAnimationDifferee = true;
        SceneManager.LoadScene(0);
		//GetComponent<InterfaceAccueil>().AllerVersDetailExamen();
    }
}

