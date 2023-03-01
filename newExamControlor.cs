using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class newExamControlor : MonoBehaviour {

    public Button SelectPatirntCont;
    public static bool fromSaveAnimation = false;

    Fusion saveAnimationFusion;

    public static int selectedPatient;

    // Use this for initialization
    void Start () {

        if (SelectPatirntCont!=null) {
            SelectPatirntCont.interactable = false;
        }
        if (GameObject.Find("PatientNom"))
        {
            ManagerPatient managerPatient = new ManagerPatient(ConnexionDB.DB());
            Patient patient = managerPatient.getPatientById(selectedPatient);
            GameObject.Find("PatientNom").GetComponent<Text>().text = patient.nomP + " " + patient.prenomP;
        }
        if (GameObject.Find("ServerSide"))
        {
            saveAnimationFusion = GameObject.Find("ServerSide").GetComponent<Fusion>();
        }
        //GameObject.Find("NetManager").GetComponent<NetworkManager>().StartServer();
        //GameObject.Find("NetManager").GetComponent<NetworkManager>().StopServer();


    }
	
    public static  Examen examen;

    public void NewExamScene()
    {
        ManagerPatient managerPatient = new ManagerPatient(ConnexionDB.DB());
        Patient patient = managerPatient.getPatientById(selectedPatient);

        Debug.Log("Patient est selectionné id == " + patient.idP + " nom = " + patient.nomP + " prenom = " + patient.prenomP);
        //Debug.Log("---------------------------------------------     New examen medecin nom = " + InscriptionControleur.medecin.nom + " prenom = " + InscriptionControleur.medecin.prenom);
        //examen = new Examen("11-08-2022",selectedPatient);
        //idExamen = examen.GetNextId();
        //idExamen = examen.SaveExamen();


        SceneManager.LoadScene(1);
    }

  

    public static string idExamen = null;
    public InputField noteContent;

    public void EnregistrerAnimation()
    {
        ManagerExamen managerExamen = new ManagerExamen(ConnexionDB.DB());
        examen.note = noteContent.text;
        managerExamen.addExamen(examen);
        //examen.dureeAnimation = "" + ButtonManager.dureeAnimation;
        //ButtonManager.dureeAnimation = 0;
        //idExamen = examen.SaveExamen();
        //Debug.Log("NewExamen id = " + examen.idExamen);


        GameObject.Find("NetManager").GetComponent<NetworkManager>().StopServer();
        saveAnimationFusion.SaveData();
        saveAnimationFusion.SaveData2();

      
        //Note note = new Note(idExamen, InscriptionControleur.medecin.id,noteContent.text);
        //note.SaveNote();
        //Debug.Log("Examen et note sont enregistés");
        GameObject.Find("NetManager").GetComponent<NetworkManagerHUD>().enabled = true;
        fromSaveAnimation = true;
        SceneManager.LoadScene(0);

    }


    public GameObject ConfirmationEnregAnimationPanel, EnregistrerNotePanel;

    public void AfficherEnregistrerNote()
    {

        ConfirmationEnregAnimationPanel.SetActive(false);
        EnregistrerNotePanel.SetActive(true);

    }

}
