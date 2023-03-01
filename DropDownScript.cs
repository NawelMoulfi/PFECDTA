using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

        Button b = this.GetComponent<Button>();

        string j = gameObject.transform.GetChild(0).GetComponent<Text>().text;
   
        string i = gameObject.name;
        //Debug.Log(j);
        b.onClick.AddListener(delegate { SelectElement(j); });
    }


    bool remplie = false;
    // Update is called once per frame
    void Update () {
    
	}


    public Text MainElement;



    void SelectElement(string s)
    {
        ((gameObject.transform.parent).parent).parent.parent.parent.parent.parent.GetChild(0).GetChild(0).GetComponent<Text>().text = s;
        ((gameObject.transform.parent).parent).parent.parent.parent.parent.gameObject.SetActive(false);
        ((gameObject.transform.parent).parent).parent.parent.parent.parent.parent.parent.gameObject.GetComponent<FillDropDown>().SelectedElement = s;
       
        Debug.Log("Hello s =" + s);
    }


}
