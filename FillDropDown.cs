using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillDropDown : MonoBehaviour {

    public GameObject dropDownElement;
    public GameObject TableContent;
    public string SelectedElement;

    // Use this for initialization
    void Start () {
        
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SetDropDownElementsDays()
    {
        for (int i = 1; i < 32; i++)
        {
            GameObject row = Instantiate(dropDownElement);
            row.SetActive(true);

            row.transform.SetParent(TableContent.transform, false);

            row.name = "row " + i;
            (row.GetComponent<Transform>().GetChild(0)).GetComponent<Text>().text = "" + i;
        }
    }
    public void SetDropDownElementsMounths()
    {
        for (int i = 1; i < 13; i++)
        {
            GameObject row = Instantiate(dropDownElement);
            row.SetActive(true);

            row.transform.SetParent(TableContent.transform, false);

            row.name = "row " + i;
            (row.GetComponent<Transform>().GetChild(0)).GetComponent<Text>().text = "" + i;
        }
    }

    public void SetDropDownElementsyears()
    {
        int currentYear = DateTime.Now.Year;
        for (int i = currentYear; i > 1930; i--)
        {
            GameObject row = Instantiate(dropDownElement);
            row.SetActive(true);

            row.transform.SetParent(TableContent.transform, false);

            row.name = "row " + i;
            (row.GetComponent<Transform>().GetChild(0)).GetComponent<Text>().text = "" + i;
        }
    }

    public GameObject tableDropDown;
    public static int x = 0;
        
    public void afficherDropDownTable()
    {
        
        if (tableDropDown.gameObject.active)
        {

            tableDropDown.SetActive(false);
        
        }
        else
        {
            tableDropDown.SetActive(true);
          
        }

    }

}
