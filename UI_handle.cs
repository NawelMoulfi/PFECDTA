using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UI_handle : NetworkBehaviour
{

     Slider mainSlider;
     Text Angle;


    void Start()
    {
        if (isClient)
        {
            mainSlider = GameObject.Find("Slider").GetComponent<Slider>();
            Angle = GameObject.Find("TextAngle").GetComponent<Text>();

            Angle.text = string.Format("{0:N0}", mainSlider.value);
            Debug.Log("mainSlider.value");
            KinectWrapper.SetKinectElevationAngle((int)mainSlider.value);
            mainSlider.onValueChanged.AddListener(delegate { Changement_valeur(); });
        }
        
    }


    public void Changement_valeur()
    {
        if (isClient)
        {
            Angle.text = string.Format("{0:N0}", mainSlider.value);
            mainSlider.interactable = false;
            KinectWrapper.SetKinectElevationAngle((int)mainSlider.value);
            GetComponent<KinectManager>().ClearKinectUsers();
            mainSlider.interactable = true;
        }
    }


}
