using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemControlleur : MonoBehaviour {

    public static string selectedItem = "";
    public static string selectedPatientID = "";
    static string selectedItemPrevious = "";

	// Use this for initialization
	void Start () {
        Button b = this.GetComponent<Button>();

        string j = gameObject.transform.GetChild(0).name;
        string i = gameObject.name;
        //Debug.Log(j);
        b.onClick.AddListener(delegate { Test(i,j); });
    }

    void Test(string id, string name)
    {
       // Debug.Log("item = "+id.ToString());
        //Debug.Log("name = "+ name.ToString());

        //Debug.Log("selectedItem = " + selectedItem);
        selectedPatientID = "" + GameObject.Find(name).GetComponent<Text>().text;
        selectedItem = id;
        //Debug.Log("------------------------------------------**********   selectedPatientID = " + selectedPatientID);


        if (selectedItem != id)
        {      
            if (GameObject.Find(selectedItemPrevious))
            {
                GameObject.Find(selectedItemPrevious).GetComponent<Image>().color = new Color(0.97f, 0.97f, 0.97f, 1);
            }
            selectedItemPrevious = selectedItem;     
        }
        if (GameObject.Find("ModifierPatient") && GameObject.Find("SupprimerPatient"))
        {
            GameObject.Find("ModifierPatient").GetComponent<Button>().interactable = true;
            GameObject.Find("SupprimerPatient").GetComponent<Button>().interactable = true;
            GameObject.Find("AfficherInfoPatient").GetComponent<Button>().interactable = true;
        }
        if (GameObject.Find("ConsulterExamentBu"))
        {
            GameObject.Find("ConsulterExamentBu").GetComponent<Button>().interactable = true;
            GameObject.Find("SupprimerExamentBu").GetComponent<Button>().interactable = true;
        }
        if (GameObject.Find("AfficherInfoPatientCE"))
        {
            GameObject.Find("AfficherInfoPatientCE").GetComponent<Button>().interactable = true;
        }

        if (GameObject.Find("SelectPatirntCont"))
        {
            GameObject.Find("SelectPatirntCont").GetComponent<Button>().interactable = true;
        }



    }


    // Update is called once per frame
    void Update () {

    }
}
