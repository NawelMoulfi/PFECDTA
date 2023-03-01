using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Diagnostics;

public class PatientManager : MonoBehaviour {

    // Use this for initialization
    void Start () {

        
    }
	
	// Update is called once per frame
	void Update () {


		
	}

    //public Text nomInfo, prenomInfo, dateNaisInfo, telInfo, sexeInfoPatient, adresseInfo, situationFamInfo;

    /*
    public void ModifierNote()
    {
        if (note != null)
        {
            note.noteContent = noteContent.text;
            note.UpdateNote();
            Debug.Log("Note Not null");
        }
        else
        {
            Debug.Log("Note is null");
        }
       
    }*/


    public void AfficherAnimationDifferee()
    {
        //AnimationDifferee.idExamen = InterfaceMedecin.examenSelected.ToString();
        SceneManager.LoadScene(2);
    }

    public void AfficherRepresentation3D()
    {
        SceneManager.LoadScene(4);
    }

    public void AfficherAnimationSquelettique()
    {
        //CubeManAnimation.idExamen = InterfaceMedecin.examenSelected.ToString();
        SceneManager.LoadScene(3);
    }

    public void AnalyseMarche()
    {

        Process p = new Process();
        ProcessStartInfo pInfo;
        pInfo = new ProcessStartInfo("cmd.exe", "/K " + "Python -m streamlit run " + @"Assets\ScriptPython\test.py [-- " + @"Assets\Reports\report" + InterfaceMedecin.examenSelected.ToString() + ".csv ]");
        pInfo.CreateNoWindow = true;
        pInfo.WindowStyle = ProcessWindowStyle.Hidden;
        pInfo.UseShellExecute = true;
        p = Process.Start(pInfo);
    }

    
    
    public void FermerRepresentation3D()
    {
        SceneManager.LoadScene(0);
    }

    /*
    public void ExplorerCSV()
    {

        Application.OpenURL(Application.dataPath + "/Reports/" + "report" + examen.idExamen + ".csv");
    }
    */

}
