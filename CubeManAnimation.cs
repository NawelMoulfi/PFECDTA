using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CubeManAnimation : MonoBehaviour
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
    
    private List<LineRenderer>[] lines2 = new List<LineRenderer>[20];
    private List<LineRenderer>[] lines3 = new List<LineRenderer>[20];
    private int[] parIdxs;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialPosOffset = Vector3.zero;
    private uint initialPosUserID = 0;


    //Transform avatarFusion;
    public static string idExamen;

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
                lines2[i] = new List<LineRenderer>();
                lines3[i] = new List<LineRenderer>();
                lasPosJoint[i] = new Vector3();
            }
        }

        initialPosition = transform.position;
        initialRotation = transform.rotation;
        //transform.rotation = Quaternion.identity;
    }

    int k = 0;
    bool anim = true, startAnimation = false;

    Vector3[] lasPosJoint = new Vector3[20];


    float pas = 0;
    // Update is called once per frame
    void Update()
    {

        if (!startAnimation || k == animationData.squelleteData.Count)
            return;

        if (!anim)
            return;

        /*
        for (int f = 0; f<5;f++)
        {
            Vector3 posJoint1 = animationData.squelleteData[k].articuPosition[f];
            pas += 0.002f;
            posJoint1.z = pas;
            if (k != 0)
            {

                LineRenderer line = Instantiate(SkeletonLine) as LineRenderer;
                line.transform.parent = transform;



                line.SetPosition(0, lasPosJoint[f]);
                line.SetPosition(1, posJoint1);


                lines2[f].Add(line);



                lasPosJoint[f] = posJoint1;



            }
            else
            {

                lasPosJoint[f] = posJoint1;
            }

            
            int parI = parIdxs[f];
            Vector3 posParent = animationData.squelleteData[k].articuPosition[parI];
                

            LineRenderer linel = Instantiate(SkeletonLine) as LineRenderer;
            linel.transform.parent = transform;



            linel.SetPosition(0, posParent);
            linel.SetPosition(1, posJoint1);


            lines3[f].Add(linel);
            //lines[i].SetVertexCount(2);
                
        
            
        }
       
       */

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
                Debug.Log("animateJoint.Contains(" + i+ ") = "+ animateJoint.Contains(i));
                //if (manager.IsJointTracked(playerID, joint))
                if (animateJoint.Contains(i))
                {
                    
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
                else
                {
                    bones[i].gameObject.SetActive(false);
                }
            }
        }

        if (SkeletonLine)
        {
            pas += 0.4f;
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

                            LineRenderer line = Instantiate(SkeletonLine) as LineRenderer;
                            line.transform.parent = transform;

                            
                            posParent.x = posParent.x + pas;
                            posJoint.x = posJoint.x + pas;

                            line.SetPosition(0, posParent);
                            line.SetPosition(1, posJoint);

                           

                            lines2[i].Add(line);


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







    List<int> animateJoint = new List<int>();


    public void animerSelection()
    {
        
        if (GameObject.Find("shoulderleft").GetComponent<Image>().color.Equals(new Color(0, 1, 0, 1)))
        {
            animateJoint.Add(4);
        }
        if (GameObject.Find("shoulderright").GetComponent<Image>().color.Equals(new Color(0, 1, 0, 1)))
        {
            animateJoint.Add(8);
        }
        if (GameObject.Find("elbowleft").GetComponent<Image>().color.Equals(new Color(0, 1, 0, 1)))
        {
            animateJoint.Add(5);
        }
        if (GameObject.Find("wristleft").GetComponent<Image>().color.Equals(new Color(0, 1, 0, 1)))
        {
            animateJoint.Add(6);
        }
        if (GameObject.Find("elbowright").GetComponent<Image>().color.Equals(new Color(0, 1, 0, 1)))
        {
            animateJoint.Add(9);
        }
        if (GameObject.Find("wristright").GetComponent<Image>().color.Equals(new Color(0, 1, 0, 1)))
        {
            animateJoint.Add(10);
        }
        if (GameObject.Find("shouldercenter").GetComponent<Image>().color.Equals(new Color(0, 1, 0, 1)))
        {
            animateJoint.Add(2);
        }
        if (GameObject.Find("head").GetComponent<Image>().color.Equals(new Color(0, 1, 0, 1)))
        {
            animateJoint.Add(3);
        }
        if (GameObject.Find("hipCenter").GetComponent<Image>().color.Equals(new Color(0, 1, 0, 1)))
        {
            animateJoint.Add(0);
        }
        if (GameObject.Find("spine").GetComponent<Image>().color.Equals(new Color(0, 1, 0, 1)))
        {
            animateJoint.Add(1);
        }
        if (GameObject.Find("hipleft").GetComponent<Image>().color.Equals(new Color(0, 1, 0, 1)))
        {
            animateJoint.Add(12);
        }
        if (GameObject.Find("hipright").GetComponent<Image>().color.Equals(new Color(0, 1, 0, 1)))
        {
            animateJoint.Add(16);
        }

        if (GameObject.Find("kneeleft").GetComponent<Image>().color.Equals(new Color(0, 1, 0, 1)))
        {
            animateJoint.Add(13);
        }

        if (GameObject.Find("kneeright").GetComponent<Image>().color.Equals(new Color(0, 1, 0, 1)))
        {
            animateJoint.Add(17);
        }


        if (GameObject.Find("ankleright").GetComponent<Image>().color.Equals(new Color(0, 1, 0, 1)))
        {
            animateJoint.Add(18);
        }


        if (GameObject.Find("ankleleft").GetComponent<Image>().color.Equals(new Color(0, 1, 0, 1)))
        {
            animateJoint.Add(14);
        }


        if (GameObject.Find("footleft").GetComponent<Image>().color.Equals(new Color(0, 1, 0, 1)))
        {
            animateJoint.Add(15);
        }


        if (GameObject.Find("footright").GetComponent<Image>().color.Equals(new Color(0, 1, 0, 1)))
        {
            animateJoint.Add(19);
        }


        if (GameObject.Find("handleft").GetComponent<Image>().color.Equals(new Color(0, 1, 0, 1)))
        {
            animateJoint.Add(7);
        }


        if (GameObject.Find("handright").GetComponent<Image>().color.Equals(new Color(0, 1, 0, 1)))
        {
            animateJoint.Add(11);
        }

        GameObject.Find("jointSelection").SetActive(false);
        startAnimation = true;

        Debug.Log("------------------------------------------------------------------------------------------------------------------------------------------------");
        Debug.Log("animateJoint count = " + animateJoint.Count);
    }




    public void FermerRepresentation3D()
    {
        //DBManager.fromRepresentation3D = true;
        SceneManager.LoadScene(0);
    }



}
