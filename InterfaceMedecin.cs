using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class InterfaceMedecin : MonoBehaviour
{
    public GameObject interfaceConnexionServeur;
    public GameObject accueil;
    public GameObject interfaceOperateur;
    public GameObject interfaceMedecin;
    public GameObject infoPersoMedecin;
    public GameObject listePatientM;
    public GameObject listeExamenM;
    public GameObject affichageExamenM;
    public GameObject lignePatient;
    public GameObject tablePatient;

    public GameObject ligneExamen;
    public GameObject tableExamen;

    public GameObject nomM;
    public GameObject prenomM;
    public GameObject sexeM;
    public GameObject dateNaissM;
    public GameObject numTelM;
    public GameObject adresseM;

    public GameObject nomP;
    public GameObject prenomP;
    public GameObject sexeP;
    public GameObject dateNaissP;
    public GameObject numTelP;
    //public GameObject adresseP;

    public GameObject nomP2;
    public GameObject prenomP2;
    public GameObject sexeP2;
    public GameObject dateNaissP2;
    public GameObject numTelP2;
    public GameObject dateExamen;
    public InputField nvNote;
    public InputField animationPath;


    private int idM;
    private int repeat = 1;
    public static int patientSelected = 0;
    public static int examenSelected = 0;
    //public GameObject texteAccueil;

    public RawImage ImageMedecin;
    public RawImage ImagePatient1;
    public RawImage ImagePatient2;

    public void starting()
    {
        infoPersoMedecin.SetActive(true);
        listePatientM.SetActive(true);
        listeExamenM.SetActive(false);
		affichageExamenM.SetActive (false);
		idM = int.Parse(SessionState.GetString("SessionIdMedecin", "0"));
		listePatient ();
    }


    void Update()
    {
		/*
        idM = int.Parse(SessionState.GetString("SessionIdMedecin", "0"));
        //print("idM = " + idM);
		if (idM > 0 && repeat == 1 && examenSelected == 0) {
			print ("ouii");
			RemplirChampsMedecin ();
			listePatient ();
			repeat--;
		} else
			print ("idm = " + idM + " repeat = " + repeat + " examen = " + examenSelected); */
        //texteAccueil.GetComponent<Text>().text = "Bonjour " + idM +" !"; 
    }

    public void update_image(RawImage pic)
    {   
        WWW ww;
        if( pic.name == "ImageMedecin" )
        {
            ww = new WWW("file:///" + "C:/Users/pc/Desktop/PFE_Master/Projet_PFE/Assets/Images/MedecinParDefaut.jpg");
        }
        else
        {
            ww = new WWW("file:///" + "C:/Users/pc/Desktop/PFE_Master/Projet_PFE/Assets/Images/PatientParDefaut.jpg");
        }
        pic.texture = ww.texture;
    }

    public void upload_image(RawImage pic, string path)
    {
        if( pic.name == "ImageMedecin" )
        {
            if (path !="")
            {
                WWW ww = new WWW("file:///" + path);
                pic.texture = ww.texture;
            }
            else
            {
                update_image(pic);
            }
        }
        else
        {
            if (path != "")
            {
                WWW ww = new WWW("file:///" + path);
                pic.texture = ww.texture;
            }
            else
            {
                update_image(pic);
            }
        }
    }

    public void RemplirChampsMedecin()
    {
        ManagerMedecin managerMedecin = new ManagerMedecin(ConnexionDB.DB());
        Medecin medecin = managerMedecin.getMedecinById(idM);

        nomM.GetComponent<Text>().text = "Nom :" + medecin.nomM;
        prenomM.GetComponent<Text>().text = "Prenom :" + medecin.prenomM;
        sexeM.GetComponent<Text>().text = "Sexe :" + medecin.sexeM;
        dateNaissM.GetComponent<Text>().text = "Date Naissance :" + medecin.dateNaissM;
        numTelM.GetComponent<Text>().text = "Numéro Tel :" + medecin.numTelM;
        adresseM.GetComponent<Text>().text = "Adresse :" + medecin.adresseM;
        upload_image(ImageMedecin,medecin.imageM);
    }

    public void RemplirChampsPatient()
    {
        ManagerPatient managerPatient = new ManagerPatient(ConnexionDB.DB());
        Patient patient = managerPatient.getPatientById(patientSelected);

        nomP.GetComponent<Text>().text = "Nom :" + patient.nomP;
        prenomP.GetComponent<Text>().text = "Prenom :" + patient.prenomP;
        sexeP.GetComponent<Text>().text = "Sexe :" + patient.sexeP;
        dateNaissP.GetComponent<Text>().text = "Date Naissance :" + patient.dateNaissP;
        numTelP.GetComponent<Text>().text = "Numéro Tel :" + patient.numTelP;
        upload_image(ImagePatient1,patient.imageP);
    }


    public void listePatient()
    {
		RemplirChampsMedecin ();

        listePatientM.SetActive(true);
        listeExamenM.SetActive(false);

        int i;
        for (i = 1; i < tablePatient.transform.childCount; i++)
        {
            Destroy(tablePatient.transform.GetChild(i).gameObject);
        }
        ManagerPatient managerPatient = new ManagerPatient(ConnexionDB.DB());
        List<Patient> patients = managerPatient.getPatientByMedecin(idM);

        i = 1;
        foreach (Patient patient in patients)
        {
            GameObject test = Instantiate(lignePatient);
            test.transform.SetParent(tablePatient.transform, false);
            test.name = "rowP" + i;
            test.SetActive(true);

            GameObject id = GameObject.Find("IDPatientM");
            id.name = "IDPatientM" + i;
            id.GetComponent<Text>().text = patient.idP.ToString();

            GameObject nom = GameObject.Find("NomPatientM");
            nom.name = "NomPatientM" + i;
            nom.GetComponent<Text>().text = patient.nomP;

            GameObject prenom = GameObject.Find("PrenomPatientM");
            prenom.name = "PrenomPatientM" + i;
            prenom.GetComponent<Text>().text = patient.prenomP;

            GameObject dateNaiss = GameObject.Find("DatePatientM");
            dateNaiss.name = "DatePatientM" + i;
            dateNaiss.GetComponent<Text>().text = patient.dateNaissP;

            /*
            GameObject sexe = GameObject.Find("SexePatient");
            sexe.name = "SexePatient" + i;
            sexe.GetComponent<Text>().text = patient.sexeP;
            */
            GameObject numTel = GameObject.Find("NumTelPatientM");
            numTel.name = "NumTelPatientM" + i;
            numTel.GetComponent<Text>().text = patient.numTelP.ToString();

            /*
            GameObject adresse = GameObject.Find("Adresse");
            adresse.name = "Adresse" + i;
            adresse.GetComponent<Text>().text = medecin.adresseM;
            */
            /*
            GameObject mdp = GameObject.Find("Mdp");
            mdp.name = "Mdp" + i;
            mdp.GetComponent<Text>().text = medecin.mdpM;
            */
            i++;
        }
    }

    public void selectionner(GameObject g)
    {
        if (g.name.Contains("P")) patientSelected = int.Parse(g.transform.GetChild(0).GetComponent<Text>().text);
        else examenSelected = int.Parse(g.transform.GetChild(0).GetComponent<Text>().text);    
    }

    public void listeExamen()
    {
        print("patientSelected = " + patientSelected);
        if (patientSelected > 0)
        {
            listePatientM.SetActive(false);
            listeExamenM.SetActive(true);
            affichageExamenM.SetActive(false);
            RemplirChampsPatient();

            int i;
            for (i = 1; i < tableExamen.transform.childCount; i++)
            {
                Destroy(tableExamen.transform.GetChild(i).gameObject);
            }
            ManagerExamen managerExamen = new ManagerExamen(ConnexionDB.DB());
            List<Examen> examens = managerExamen.getExamenByPatient(patientSelected);

            i = 1;
            foreach (Examen examen in examens)
            {
                GameObject test = Instantiate(ligneExamen);
                test.transform.SetParent(tableExamen.transform, false);
                test.name = "rowE" + i;
                test.SetActive(true);

                GameObject id = GameObject.Find("IDExamenM");
                id.name = "IDExamenM" + i;
                id.GetComponent<Text>().text = examen.idE.ToString();

                GameObject dateE = GameObject.Find("DateExamenM");
                dateE.name = "DateExamenM" + i;
                dateE.GetComponent<Text>().text = examen.dateE;
                i++;
            }

        }
    }

    public void affichageExamen()
    {
        //print("ExamenSelected = " + examenSelected);
        if (examenSelected >= 0)
        {
            listePatientM.SetActive(false);
            listeExamenM.SetActive(false);
            affichageExamenM.SetActive(true);

            ManagerPatient managerPatient = new ManagerPatient(ConnexionDB.DB());
            Patient patient = managerPatient.getPatientById(patientSelected);

            nomP2.GetComponent<Text>().text = "Nom :" + patient.nomP;
            prenomP2.GetComponent<Text>().text = "Prenom :" + patient.prenomP;
            sexeP2.GetComponent<Text>().text = "Sexe :" + patient.sexeP;
            dateNaissP2.GetComponent<Text>().text = "Date Naissance :" + patient.dateNaissP;
            numTelP2.GetComponent<Text>().text = "Numéro Tel :" + patient.numTelP;

            ManagerExamen managerExamen = new ManagerExamen(ConnexionDB.DB());
            Examen examen = managerExamen.getExamenById(examenSelected);
            dateExamen.GetComponent<Text>().text = "Date d'Examen : " +  examen.dateE;
            upload_image(ImagePatient2,patient.imageP);

            nvNote.text = examen.note;
            animationPath.text = examen.chemin;
        }
    }

    public void modifierNoteExamen()
    {
        ManagerExamen managerExamen = new ManagerExamen(ConnexionDB.DB());
        Examen examen = managerExamen.getExamenById(examenSelected);
        examen.note = nvNote.text;
        managerExamen.updateExamen(examen);
    }

    public void ExplorerCSV()
    {
        ManagerExamen managerExamen = new ManagerExamen(ConnexionDB.DB());
        Examen examen = managerExamen.getExamenById(examenSelected);
        Application.OpenURL(Application.dataPath + "/Reports/" + "report" + examenSelected + ".csv");
    }

    public void retour(GameObject g)
    {
        if (g.name == "Retour1")
        {
            listePatientM.SetActive(true);
            listeExamenM.SetActive(false);
            patientSelected = 0;
        }
        else if (g.name == "Retour2")
        {
            listePatientM.SetActive(false);
            listeExamenM.SetActive(true);
            
        }
        examenSelected = 0;
        affichageExamenM.SetActive(false);
    }


    public void deconnexion()
    {
        SessionState.EraseString("SessionIdMedecin");
        idM = 0;
        patientSelected = 0;
        examenSelected = 0;
        repeat = 1;

        accueil.SetActive(true);
        interfaceOperateur.SetActive(false);
        interfaceMedecin.SetActive(false);
        listeExamenM.SetActive(false);
        affichageExamenM.SetActive(false);
    }

}
