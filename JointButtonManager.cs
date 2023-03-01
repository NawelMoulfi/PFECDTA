using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JointButtonManager : MonoBehaviour {
    Image image;

    public void selectJoint(string name)
    {
        if (GameObject.Find(name))
        {
            image = GameObject.Find(name).GetComponent<Image>();
            if (image.color.Equals(  new Color(0, 1, 0, 1)))
            {
                image.color = new Color(1, 1, 1, 1);
            }
            else
            {
                image.color = new Color(0, 1, 0, 1);
            }
        }
       
    }

    bool isSelectAll = false, isSelectUpperPart = false, isSelectLowerrPart = false, isSelectArms = false, isSelectLegs = false;

    public void selectAllJoint()
    {
        if (!isSelectAll)
        {
            GameObject.Find("shoulderleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("shoulderright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("elbowleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("wristleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("elbowright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("wristright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));

            GameObject.Find("shouldercenter").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("head").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("spine").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("hipleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("hipright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));


            GameObject.Find("kneeleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("kneeright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("ankleright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("ankleleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("footleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("footright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("handleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("handright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));

            GameObject.Find("hipCenter").GetComponent<Image>().color = (new Color(0, 1, 0, 1));

            isSelectAll = true;
        }
        else
        {
            DeSelectAll();
            isSelectAll = false;
        }
    
        
    }

    public void selectUpperPart()
    {
        if (!isSelectUpperPart)
        {
            DeSelectAll();

            GameObject.Find("shoulderleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("shoulderright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("elbowleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("wristleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("elbowright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("wristright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));

            GameObject.Find("shouldercenter").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("head").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("spine").GetComponent<Image>().color = (new Color(0, 1, 0, 1));

           


            GameObject.Find("handleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("handright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));

            GameObject.Find("hipCenter").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            isSelectUpperPart = true;
        }
        else
        {
            DeSelectAll();

            isSelectUpperPart = false;
        }

    }

    public void selectLowerPart()
    {
        if (!isSelectLowerrPart)
        {
            DeSelectAll();

            GameObject.Find("hipleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("hipright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));


            GameObject.Find("kneeleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("kneeright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("ankleright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("ankleleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("footleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("footright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));


            GameObject.Find("hipCenter").GetComponent<Image>().color = (new Color(0, 1, 0, 1));

            isSelectLowerrPart = true;
        }
        else
        {
            DeSelectAll();
            isSelectLowerrPart = false;
        }


    }

    public void selectArms()
    {
        if (!isSelectArms)
        {
            DeSelectAll();

            GameObject.Find("shoulderleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("shoulderright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("elbowleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("wristleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("elbowright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("wristright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));

            GameObject.Find("shouldercenter").GetComponent<Image>().color = (new Color(0, 1, 0, 1));

            GameObject.Find("handleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("handright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));


            isSelectArms = true;
        }
        else
        {
            DeSelectAll();
            isSelectArms = false;
        }


    }
    public void selectLegs()
    {
        if (!isSelectLegs)
        {
            DeSelectAll();


            GameObject.Find("hipleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("hipright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));


            GameObject.Find("kneeleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("kneeright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("ankleright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("ankleleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("footleft").GetComponent<Image>().color = (new Color(0, 1, 0, 1));
            GameObject.Find("footright").GetComponent<Image>().color = (new Color(0, 1, 0, 1));


            

            isSelectLegs = true;
        }
        else
        {
            DeSelectAll();
            isSelectLegs = false;
        }


    }
    void DeSelectAll()
    {
        GameObject.Find("shoulderleft").GetComponent<Image>().color = (new Color(1, 1, 1, 1));
        GameObject.Find("shoulderright").GetComponent<Image>().color = (new Color(1, 1, 1, 1));
        GameObject.Find("elbowleft").GetComponent<Image>().color = (new Color(1, 1, 1, 1));
        GameObject.Find("wristleft").GetComponent<Image>().color = (new Color(1, 1, 1, 1));
        GameObject.Find("elbowright").GetComponent<Image>().color = (new Color(1, 1, 1, 1));
        GameObject.Find("wristright").GetComponent<Image>().color = (new Color(1, 1, 1, 1));

        GameObject.Find("shouldercenter").GetComponent<Image>().color = (new Color(1, 1, 1, 1));
        GameObject.Find("head").GetComponent<Image>().color = (new Color(1, 1, 1, 1));
        GameObject.Find("spine").GetComponent<Image>().color = (new Color(1, 1, 1, 1));
        GameObject.Find("hipleft").GetComponent<Image>().color = (new Color(1, 1, 1, 1));
        GameObject.Find("hipright").GetComponent<Image>().color = (new Color(1, 1, 1, 1));


        GameObject.Find("kneeleft").GetComponent<Image>().color = (new Color(1, 1, 1, 1));
        GameObject.Find("kneeright").GetComponent<Image>().color = (new Color(1, 1, 1, 1));
        GameObject.Find("ankleright").GetComponent<Image>().color = (new Color(1, 1, 1, 1));
        GameObject.Find("ankleleft").GetComponent<Image>().color = (new Color(1, 1, 1, 1));
        GameObject.Find("footleft").GetComponent<Image>().color = (new Color(1, 1, 1, 1));
        GameObject.Find("footright").GetComponent<Image>().color = (new Color(1, 1, 1, 1));
        GameObject.Find("handleft").GetComponent<Image>().color = (new Color(1, 1, 1, 1));
        GameObject.Find("handright").GetComponent<Image>().color = (new Color(1, 1, 1, 1));

        GameObject.Find("hipCenter").GetComponent<Image>().color = (new Color(1, 1, 1, 1));
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
