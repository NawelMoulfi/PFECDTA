using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {


    

	// Use this for initialization
	void Start () {
        if (GameObject.Find("KinectListContentConfig"))
        {
            objj = GameObject.Find("KinectListContentConfig").GetComponent<Transform>();
        }
        //saveAnimation = avatarFusion.GetComponent<SaveAnimation>();
        saveAnimationFusion = GameObject.Find("ServerSide").GetComponent<Fusion>();

        
    }

    SaveAnimation saveAnimation;
    Fusion saveAnimationFusion;

    int nbChild = 0;

    string s1 = "", s11 = "";
    string s2 = "";
    public GameObject kinectException;
    public GameObject KinectsConfig, CalibrageList, Item1, Item2;
    public Button CalculateButton, SuivantButton;
    public Button RecorderButton;
    Transform objj;
    Transform obj1, obj2;
    bool calculationMode = false;
    Text itemText1, itemText2;
    public Text  RecordText;


    public GameObject CalibrageToUsePanel;

    public void afficherChoixCalibrage()
    {
        CalibrageToUsePanel.SetActive(true);

    }

    public void UtiliserLastCalibrage()
    {
        CalibrageToUsePanel.SetActive(false);
        KinectsConfig.SetActive(false);
        CalibrageList.SetActive(false);

        gameObject.GetComponent<initCalibration>().getCalibrageMatrixes();
        calculateMatrixesIsDone = true;


    }

    public void Suivant1()
    {
        CalibrageToUsePanel.SetActive(false);

        //KinectsConfig.SetActive(false);
        

        nbChild = objj.childCount;

        if (nbChild == 1)
        {
            kinectException.SetActive(true);
            return;
        }


        RecorderButton.interactable = true;
        RecorderButton.GetComponent<Image>().color = new Color(0, 1, 0, 1);
        if (nbChild <= 2)
        {
            KinectsConfig.SetActive(false);
            CalibrageList.SetActive(false);
            return;
        }

        CalibrageList.SetActive(true);

        s1 = objj.GetChild(1).name; s11 = s1;

        s2 = objj.GetChild(2).name;

        if (GameObject.Find(s1) && GameObject.Find(s2))
        {
            obj1 = GameObject.Find(s1).GetComponent<Transform>();
            obj2 = GameObject.Find(s2).GetComponent<Transform>();



            obj1.SetParent(Item1.transform, false);
            obj2.SetParent(Item2.transform, false);

            obj1.localPosition = new Vector3(Item1.transform.localPosition.x + 103.48f+162f, Item1.transform.localPosition.y - 2f, Item1.transform.localPosition.z); ;
            obj2.localPosition = new Vector3(Item2.transform.localPosition.x - 107.18f-166, Item2.transform.localPosition.y - 2f, Item2.transform.localPosition.z); ;


            itemText1 = ((obj1.GetChild(1)).transform).GetChild(0).GetComponent<Text>();
            itemText2 = ((obj2.GetChild(1)).transform).GetChild(0).GetComponent<Text>();

            calculationMode = true;


        }
        CalculateButton.interactable = false;

        b = true;




    }



    public void closeKinectException()
    {
        kinectException.SetActive(false);
    }

    public void CalibrerButton()
    {
        KinectsConfig.SetActive(true);
    }

    public Text compterText;
    public static int dureeAnimation = 0;
    int cpt = 0;

    IEnumerator startCompteur()
    {
        while (startCpt)
        {
            if (cpt<=9)
            {
                compterText.text = "00:0" + cpt;
            }
            else
            {
                int div = cpt / 60;
                int mod = cpt % 60;
                Debug.Log("div = " + div);
                Debug.Log("mod = " + mod);
                if (div >= 1)
                {
                    if (div<=9 )
                    {
                        if (mod <= 9)
                        {
                            compterText.text = "0" + div + ":0" + mod;
                        }
                        else
                        {
                            compterText.text = "0" + div + ":" + mod;
                        }
                       
                    }
                    else
                    {
                        if (mod <= 9)
                        {
                            compterText.text = "" + div + ":0" + mod;
                        }
                        else
                        {
                            compterText.text = "" + div + ":" + mod;
                        }
                    }

                }
                else
                {
                    compterText.text = "00:" + cpt;
                }
                
            }
            
            yield return new WaitForSeconds(1f);
            cpt++;
        }

    }

    bool firstPress = true;
    bool startCpt = false;




    public GameObject avatarFusion;
    public GameObject ConfirmationEnregAnimation;

    public void RecorderButtonD()
    {
        if (firstPress)
        {

            saveAnimationFusion.StartSaving();
            StartCoroutine(test1());
            startCpt = true;
            StartCoroutine(startCompteur());
            
            RecordText.text = "Stop";
            firstPress = false;
        }
        else
        {

            saveAnimationFusion.StopSaving();

            StopAllCoroutines();
            RecordText.text = "Enregistrer";
            firstPress = true;
            RecorderButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            ConfirmationEnregAnimation.SetActive(true);
            startCpt = false;
            dureeAnimation = cpt;
            cpt = 0;
            compterText.text = "00:00"; 
        }

    }


    bool start2 = true;

    IEnumerator test1()
    {
        while (true)
        {
            test2();
            yield return new WaitForSeconds(0.5f);

        }

    }
    
    void test2()
    {

        if (start2)
        {
            RecorderButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            start2 = false;
        }
        else
        {
            start2 = true;
            RecorderButton.GetComponent<Image>().color = new Color(1, 0, 0, 1);
        }            
          

    }


    public void annulerEnregAnimation()
    {
        GetComponent<Fusion>().resetDataVariable();
        ConfirmationEnregAnimation.SetActive(false);
    }















    bool b = false;
    public Image consignImage;

    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    public Sprite sprite4;



    public void ChangeImage(int imageId)
    {
        switch (imageId)
        {
            case 1:
                consignImage.sprite = sprite1;
                break;
            case 2:
                consignImage.sprite = sprite2;
                break;
            case 3:
                consignImage.sprite = sprite3;
                break;
            case 4:
                consignImage.sprite = sprite4;
                break;
        }
        
    }
    int imageId = 2;
    public void Suivant2()
    {

        ChangeImage( imageId);
        imageId++;

        SuivantButton.interactable = false;


        if (GameObject.Find("CallibrationMatrix"))
        {
            GameObject.Find("CallibrationMatrix").GetComponent<Text>().text = "";
        }

        if (nbChild <= 2)
         return;

        if (b)
        {
            obj1.SetParent(GameObject.Find("KinectListContentConfig").transform, false);
            obj2.SetParent(Item1.transform, false);
            obj2.localPosition = new Vector3(Item1.transform.localPosition.x + 103.48f+ 162f, Item1.transform.localPosition.y - 2f, Item1.transform.localPosition.z); ;


            s1 = s2;
            obj1 = obj2;
            itemText1 = itemText2;
        }

        if (!s11.Equals(objj.GetChild(1).name))
        {
            s2 = objj.GetChild(1).name;

            if (GameObject.Find(s2))
            {
                obj2 = GameObject.Find(s2).GetComponent<Transform>(); ;
                obj2.SetParent(Item2.transform, false);
                obj2.localPosition = new Vector3(Item2.transform.localPosition.x - 107.18f-166, Item2.transform.localPosition.y - 2f, Item2.transform.localPosition.z); ;

            }

        }
        else
        {
            obj1.SetParent(GameObject.Find("KinectListContentConfig").transform, false);
            obj2.SetParent(GameObject.Find("KinectListContentConfig").transform, false);
            calculationMode = false;
            b = false;
            calculateMatrixesIsDone = true;
            CalibrageList.SetActive(false);
            KinectsConfig.SetActive(false);

            gameObject.GetComponent<initCalibration>().saveCalibrageMatrixes();

        }

    }
    public bool calculateMatrixesIsDone = false;

    public bool modeCalculation = false;
    void cheekForKinectState()
    {
        if (modeCalculation)
            return;
        if (itemText1.text.Equals(itemText2.text))
        {
            CalculateButton.interactable = true;
        }
        else
        {
            CalculateButton.interactable = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (calculationMode)
        {
            cheekForKinectState();
        }
    }


    public void ReternNewExamScene()
    {
        newExamControlor.fromSaveAnimation = true;
        GameObject.Find("NetManager").GetComponent<NetworkManager>().StopServer();
        SceneManager.LoadScene(0);
    }


    public void ReternClient()
    {
        GameObject.Find("NetManager").GetComponent<NetworkManager>().StopClient();
        
        SceneManager.LoadScene(0);
    }



}
