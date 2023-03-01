using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CubeMan : MonoBehaviour
{
    public bool MoveVertically = false;
    public bool MirroredMovement = false;

    //public GameObject debugText;

    public GameObject Hip_Center;
    public GameObject Spine;
    public GameObject Shoulder_Center;
    public GameObject Head;
    public GameObject Shoulder_Left;
    public GameObject Elbow_Left;
    public GameObject Wrist_Left;
    public GameObject Hand_Left;
    public GameObject Shoulder_Right;
    public GameObject Elbow_Right;
    public GameObject Wrist_Right;
    public GameObject Hand_Right;
    public GameObject Hip_Left;
    public GameObject Knee_Left;
    public GameObject Ankle_Left;
    public GameObject Foot_Left;
    public GameObject Hip_Right;
    public GameObject Knee_Right;
    public GameObject Ankle_Right;
    public GameObject Foot_Right;

    public LineRenderer SkeletonLine;

    private GameObject[] bones;
    private LineRenderer[] lines;
    private int[] parIdxs;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialPosOffset = Vector3.zero;
    private uint initialPosUserID = 0;

    string path, content;

    AnimationData2 animationData;

    void Start()
    {

        // avatarFusion = gameObject.transform;

        path = Application.streamingAssetsPath + "/fusionDataFromAM" + InterfaceMedecin.examenSelected.ToString() + ".json";
        content = File.ReadAllText(path);

        animationData = JsonUtility.FromJson<AnimationData2>(content);




        //store bones in a list for easier access
        bones = new GameObject[] {
            Hip_Center, Spine, Shoulder_Center, Head,  // 0 - 3
			Shoulder_Left, Elbow_Left, Wrist_Left, Hand_Left,  // 4 - 7
			Shoulder_Right, Elbow_Right, Wrist_Right, Hand_Right,  // 8 - 11
			Hip_Left, Knee_Left, Ankle_Left, Foot_Left,  // 12 - 15
			Hip_Right, Knee_Right, Ankle_Right, Foot_Right  // 16 - 19
		};

        parIdxs = new int[] {
            0, 0, 1, 2,
            2, 4, 5, 6,
            2, 8, 9, 10,
            0, 12, 13, 14,
            0, 16, 17, 18
        };

        // array holding the skeleton lines
        lines = new LineRenderer[bones.Length];

        if (SkeletonLine)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = Instantiate(SkeletonLine) as LineRenderer;
                lines[i].transform.parent = transform;
            }
        }

        initialPosition = transform.position;
        initialRotation = transform.rotation;
        //transform.rotation = Quaternion.identity;
    }



    int k = 0;
    bool anim = true, startAnimation = false;



    // Update is called once per frame
    void Update()
    {

        if (k == animationData.squelleteData.Count)
            return;


        if (!anim)
            return;


        //KinectManager manager = KinectManager.Instance;

        // get 1st player
        //uint playerID = manager != null ? manager.GetPlayer1ID() : 0;
        uint playerID = 1;

        if (playerID <= 0)
        {
            // reset the pointman position and rotation
            if (transform.position != initialPosition)
            {
                transform.position = initialPosition;
            }

            if (transform.rotation != initialRotation)
            {
                transform.rotation = initialRotation;
            }

            for (int i = 0; i < bones.Length; i++)
            {
                bones[i].gameObject.SetActive(true);

                bones[i].transform.localPosition = Vector3.zero;
                bones[i].transform.localRotation = Quaternion.identity;

                if (SkeletonLine)
                {
                    lines[i].gameObject.SetActive(false);
                }
            }

            return;
        }

        // set the user position in space
        Vector3 posPointMan = animationData.squelleteData[k].avatarPosition;
        posPointMan.z = !MirroredMovement ? -posPointMan.z : posPointMan.z;

        // store the initial position
        if (initialPosUserID != playerID)
        {
            initialPosUserID = playerID;
            initialPosOffset = transform.position - (MoveVertically ? posPointMan : new Vector3(posPointMan.x, 0, posPointMan.z));
        }

        transform.position = initialPosOffset + (MoveVertically ? posPointMan : new Vector3(posPointMan.x, 0, posPointMan.z));

        // update the local positions of the bones
        for (int i = 0; i < bones.Length; i++)
        {
            if (bones[i] != null)
            {
                //int joint = MirroredMovement ? KinectWrapper.GetSkeletonMirroredJoint(i) : i;
              
                //if (manager.IsJointTracked(playerID, joint))
                

                bones[i].gameObject.SetActive(true);

                //Vector3 posJoint = manager.GetJointPosition(playerID, joint);
                Vector3 posJoint = animationData.squelleteData[k].articuPosition[i];
                posJoint.z = !MirroredMovement ? -posJoint.z : posJoint.z;

                //Quaternion rotJoint = manager.GetJointOrientation(playerID, joint, !MirroredMovement);
                Quaternion rotJoint = animationData.squelleteData[k].articuRotation[i];
                rotJoint = initialRotation * rotJoint;

                posJoint -= posPointMan;  //???

                if (MirroredMovement)
                {
                    posJoint.x = -posJoint.x;
                    posJoint.z = -posJoint.z;
                }

                bones[i].transform.localPosition = posJoint;
                bones[i].transform.rotation = rotJoint;
               
            }
        }

        if (SkeletonLine)
        {
          
            for (int i = 0; i < bones.Length; i++)
            {
                bool bLineDrawn = false;

                if (bones[i] != null)
                {
                    if (bones[i].gameObject.activeSelf)
                    {
                        Vector3 posJoint = bones[i].transform.position;

                        int parI = parIdxs[i];
                        Vector3 posParent = bones[parI].transform.position;

                        if (bones[parI].gameObject.activeSelf)
                        {
                            lines[i].gameObject.SetActive(true);

                            //lines[i].SetVertexCount(2);
                            lines[i].SetPosition(0, posParent);
                            lines[i].SetPosition(1, posJoint);
                            /*
                            LineRenderer line = Instantiate(SkeletonLine) as LineRenderer;
                            line.transform.parent = transform;



                            line.SetPosition(0, posParent);
                            line.SetPosition(1, posJoint);

                            */



                            //lines2[i].Add(lines[i]);
                            bLineDrawn = true;
                        }
                    }
                }

                if (!bLineDrawn)
                {
                    lines[i].gameObject.SetActive(false);
                }
            }
        }
     
        StartCoroutine(waitForAWhile());
        anim = false;
        k++;
    }
    private IEnumerator waitForAWhile()
    {
        yield return new WaitForSeconds((((float)animationData.squelleteData[k].timeExec) / 1100));
        anim = true;
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


    public void FermerAnimationSquelettique()
    {
        //DBManager.fromRepresentation3D = true;
        SceneManager.LoadScene(0);
    }
}
