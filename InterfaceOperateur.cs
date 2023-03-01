using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;
using System;

public class InterfaceOperateur : MonoBehaviour
{
    public GameObject accueil;
    public GameObject interfaceOperateur;
    public GameObject interfaceMedecin;
	public GameObject accueilOperateur;
    public GameObject infoMedecin;
    public GameObject infoPatient;
    public GameObject infoExamen;
    public GameObject ligne;
    public GameObject table;

    public GameObject lignePatient;
    public GameObject tablePatient;

    public GameObject ligneExamen;
    public GameObject tableExamen;

    public GameObject panelAddMedecin;
    public GameObject panelAddPatient;

    public InputField nvNom;
    public InputField nvPrenom;
    public InputField nvSexe;
    public InputField nvDateNaiss;
    public InputField nvNumTel;
    public InputField nvAdresse;
    public InputField nvEmail;
    public InputField nvMdp;

    public InputField nvNomPatient;
    public InputField nvPrenomPatient;
    public InputField nvSexePatient;
    public InputField nvDateNaissPatient;
    public InputField nvNumTelPatient;
    public InputField nvIdMedecinPatient;

    public RawImage ImageMedecin;
    public RawImage ImagePatient;

    public string path;

    private int idSelectionne = 0;

    // Start is called before the first frame update
    public void starting()
    {
    	retourVersAccueil();
        //interfaceConnexion.SetActive(true);
        //interfaceOperateur.SetActive(false);
    }

    public void deconnexion()
    {
        accueil.SetActive(true);
        interfaceOperateur.SetActive(false);
        interfaceMedecin.SetActive(false);
    }

    public void retourVersAccueil()
    {
    	accueilOperateur.SetActive(true);
    	infoMedecin.SetActive(false);
    	infoPatient.SetActive(false);
    	infoExamen.SetActive(false);
		panelAddMedecin.SetActive (false);
		panelAddPatient.SetActive (false);
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
    
    public void upload_image(RawImage pic)
    {
        if( pic.name == "ImageMedecin" )
        {
            path = EditorUtility.OpenFilePanel("Upload Image", "", "jpg");
            print(path);
            if (path !="")
            {
                WWW ww = new WWW("file:///" + path);
                pic.texture = ww.texture;
            }
            else
            {
                update_image(pic);
                path="C:/Users/pc/Desktop/PFE_Master/Projet_PFE/Assets/Images/MedecinParDefaut.jpg";
            }
        }
        else
        {
            path = EditorUtility.OpenFilePanel("Upload Image", "", "png");
            if (path != "")
            {
                WWW ww = new WWW("file:///" + path);
                pic.texture = ww.texture;
            }
            else
            {
                update_image(pic);
                path="C:/Users/pc/Desktop/PFE_Master/Projet_PFE/Assets/Images/PatientParDefaut.jpg";
            }
        }
    }
    public void listeMedecin()
    {
    	accueilOperateur.SetActive(false);
    	infoMedecin.SetActive(true);
    	infoPatient.SetActive(false);
    	infoExamen.SetActive(false);

        int i;
        for (i = 1; i < table.transform.childCount; i++)
        {
            Destroy(table.transform.GetChild(i).gameObject);
        }

        ManagerMedecin managerMedecin = new ManagerMedecin(ConnexionDB.DB());
        List <Medecin>  medecins = managerMedecin.getAllMedecin();

        i = 1;
        foreach (Medecin medecin in medecins)
        {
            GameObject test = Instantiate(ligne);
            test.transform.SetParent(table.transform, false);
            test.name = "row" + i;
            test.SetActive(true);

            GameObject id = GameObject.Find("IDMedecin");
            id.name = "ID" + i;
            id.GetComponent<Text>().text = medecin.idM.ToString();

            GameObject nom = GameObject.Find("NomMedecin");
            nom.name = "Nom" + i;
            nom.GetComponent<Text>().text = medecin.nomM;

            GameObject prenom = GameObject.Find("PrenomMedecin");
            prenom.name = "Prenom" + i;
            prenom.GetComponent<Text>().text = medecin.prenomM;

            GameObject dateNaiss = GameObject.Find("DateMedecin");
            dateNaiss.name = "Date" + i;
            dateNaiss.GetComponent<Text>().text = medecin.dateNaissM;

            //GameObject sexe = GameObject.Find("Sexe");
            //sexe.name = "Sexe" + i;
            //sexe.GetComponent<Text>().text = medecin.sexeM;

            GameObject numTel = GameObject.Find("NumTelMedecin");
            numTel.name = "NumTel" + i;
            numTel.GetComponent<Text>().text = medecin.numTelM.ToString();

            /*
            GameObject adresse = GameObject.Find("Adresse");
            adresse.name = "Adresse" + i;
            adresse.GetComponent<Text>().text = medecin.adresseM;
            */
            GameObject email = GameObject.Find("EmailMedecin");
            email.name = "Email" + i;
            email.GetComponent<Text>().text = medecin.emailM;
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
        idSelectionne = int.Parse(g.transform.GetChild(0).GetComponent<Text>().text);
    }

    public void crudMedecin(GameObject g)
    {
        ManagerMedecin managerMedecin = new ManagerMedecin(ConnexionDB.DB());
        switch (g.name)
        {
            case "Ajouter":
                idSelectionne = 0;
                panelAddMedecin.SetActive(true);
                update_image(ImageMedecin);
                nvNom.text = "";
                nvPrenom.text = "";
                nvSexe.text = "";
                nvDateNaiss.text = "";
                nvNumTel.text = "";
                nvAdresse.text = "";
                nvEmail.text = "";
                nvMdp.text = "";
                break;

            case "Modifier":
                if (idSelectionne > 0)
                {
                    panelAddMedecin.SetActive(true);
                    
                    Medecin medecin = managerMedecin.getMedecinById(idSelectionne);

                    nvNom.text = medecin.nomM;
                    nvPrenom.text = medecin.prenomM;
                    nvSexe.text = medecin.sexeM;
                    nvDateNaiss.text = medecin.dateNaissM;
                    nvNumTel.text = medecin.numTelM.ToString();
                    nvAdresse.text = medecin.adresseM;
                    nvEmail.text = medecin.emailM;
                    nvMdp.text = medecin.mdpM;
                    WWW ww = new WWW("file:///" + medecin.imageM);
                    ImageMedecin.texture = ww.texture;

                }
                break;

            case "Supprimer":
                if (idSelectionne > 0)
                {
                    managerMedecin.deleteMedecin(idSelectionne);
                    listeMedecin();
                    idSelectionne = 0;
                }
                break;

            case "Annuler":
                idSelectionne = 0;
                panelAddMedecin.SetActive(false);
                //infoMedecin.SetActive(true);
                break;

            case "Valider":
                string nom = nvNom.text;
                string prenom = nvPrenom.text;
                string sexe = nvSexe.text;
                string dateNaiss = nvDateNaiss.text;
                int numTel = int.Parse(nvNumTel.text);
                string adresse = nvAdresse.text;
                string email = nvEmail.text;
                string mdp = nvMdp.text;

                //string img = upload_image(Image);

                if (idSelectionne == 0)
                {
                    Medecin medecin = new Medecin(nom, prenom, dateNaiss, sexe, numTel, adresse, email, mdp, path);
                    managerMedecin.addMedecin(medecin);
                }
                else
                {
                    Medecin medecin = new Medecin(idSelectionne, nom, prenom, dateNaiss, sexe, numTel, adresse, email, mdp, path);
                    managerMedecin.updateMedecin(medecin);
                }

                nvNom.text = "";
                nvPrenom.text = "";
                nvSexe.text = "";
                nvDateNaiss.text = "";
                nvNumTel.text = "";
                nvAdresse.text = "";
                nvEmail.text = "";
                nvMdp.text = "";

                panelAddMedecin.SetActive(false);
                update_image(ImageMedecin);
                listeMedecin();
                idSelectionne = 0;
                break;
        }
    }

    public void listePatient()
    {
    	accueilOperateur.SetActive(false);
    	infoMedecin.SetActive(false);
    	infoPatient.SetActive(true);
    	infoExamen.SetActive(false);

        int i;
        for (i = 1; i < tablePatient.transform.childCount; i++)
        {
            Destroy(tablePatient.transform.GetChild(i).gameObject);
        }
        ManagerPatient managerPatient = new ManagerPatient(ConnexionDB.DB());
        List<Patient> patients = managerPatient.getAllPatient();

        i = 1;
        foreach (Patient patient in patients)
        {
            GameObject test = Instantiate(lignePatient);
            test.transform.SetParent(tablePatient.transform, false);
            test.name = "row" + i;
            test.SetActive(true);

            GameObject id = GameObject.Find("IDPatient");
            id.name = "IDPatient" + i;
            id.GetComponent<Text>().text = patient.idP.ToString();

            GameObject nom = GameObject.Find("NomPatient");
            nom.name = "NomPatient" + i;
            nom.GetComponent<Text>().text = patient.nomP;

            GameObject prenom = GameObject.Find("PrenomPatient");
            prenom.name = "PrenomPatient" + i;
            prenom.GetComponent<Text>().text = patient.prenomP;

            GameObject dateNaiss = GameObject.Find("DatePatient");
            dateNaiss.name = "DatePatient" + i;
            dateNaiss.GetComponent<Text>().text = patient.dateNaissP;

            /*
            GameObject sexe = GameObject.Find("SexePatient");
            sexe.name = "SexePatient" + i;
            sexe.GetComponent<Text>().text = patient.sexeP;
            */
            GameObject numTel = GameObject.Find("NumTelPatient");
            numTel.name = "NumTelPatient" + i;
            numTel.GetComponent<Text>().text = patient.numTelP.ToString();

            /*
            GameObject adresse = GameObject.Find("Adresse");
            adresse.name = "Adresse" + i;
            adresse.GetComponent<Text>().text = medecin.adresseM;
            */
            GameObject medecin = GameObject.Find("MedecinPatient");
            medecin.name = "MedecinPatient" + i;

            ManagerMedecin managerMedecin = new ManagerMedecin(ConnexionDB.DB());
            Medecin medecinPatient = managerMedecin.getMedecinById(patient.idMedecin);
            medecin.GetComponent<Text>().text = medecinPatient.prenomM.Substring(0,1) + "." + medecinPatient.nomM;
            /*
            GameObject mdp = GameObject.Find("Mdp");
            mdp.name = "Mdp" + i;
            mdp.GetComponent<Text>().text = medecin.mdpM;
            */
            i++;
        }
    }

    public void crudPatient(GameObject g)
    {
        ManagerPatient managerPatient = new ManagerPatient(ConnexionDB.DB());
        switch (g.name)
        {
            case "Ajouter":
                idSelectionne = 0;
                panelAddPatient.SetActive(true);
                update_image(ImagePatient);
                nvNomPatient.text = "";
                nvPrenomPatient.text = "";
                nvSexePatient.text = "";
                nvDateNaissPatient.text = "";
                nvNumTelPatient.text = "";
                nvIdMedecinPatient.text = "";
                //nvAdresse.text = "";
                //nvEmail.text = "";
                //nvMdp.text = "";
                break;

            case "Modifier":
                if (idSelectionne > 0)
                {
                    print("ook");
                    panelAddPatient.SetActive(true);

                    Patient patient = managerPatient.getPatientById(idSelectionne);

                    nvNomPatient.text = patient.nomP;
                    nvPrenomPatient.text = patient.prenomP;
                    nvSexePatient.text = patient.sexeP;
                    nvDateNaissPatient.text = patient.dateNaissP;
                    nvNumTelPatient.text = patient.numTelP.ToString();
                    nvIdMedecinPatient.text = patient.idMedecin.ToString();
                }
                break;

            case "PasserExamen":
                if (idSelectionne > 0)
                {
                    newExamControlor.selectedPatient = idSelectionne;
                    ManagerExamen managerExamen = new ManagerExamen(ConnexionDB.DB());
                    newExamControlor.idExamen = managerExamen.getNextId();
                    string chemin = "report" + newExamControlor.idExamen + ".csv";
                    newExamControlor.examen = new Examen(DateTime.Now.Date.ToString(),idSelectionne,chemin,"");
                    CubemanController.side = "Serveur";
					//TestTransportNetwork.side = "Serveur";
                    idSelectionne = 0;
                    SceneManager.LoadScene("AnimationLive");
					//SceneManager.LoadScene("TransportNetworkTest");

                }
                break;

            case "Supprimer":
                if (idSelectionne > 0)
                {
                    managerPatient.deletePatient(idSelectionne);
                    listePatient();
                    idSelectionne = 0;
                }
                break;

            case "Annuler":
                idSelectionne = 0;
                panelAddPatient.SetActive(false);
                //infoPatient.SetActive(true);
                break;

            case "Valider":
                string nom = nvNomPatient.text;
                string prenom = nvPrenomPatient.text;
                string sexe = nvSexePatient.text;
                string dateNaiss = nvDateNaissPatient.text;
                int numTel = int.Parse(nvNumTelPatient.text);
                int medecin = int.Parse(nvIdMedecinPatient.text);
                
                string img1 = "    ";
                if (idSelectionne == 0)
                {
                    Patient patient = new Patient(nom, prenom, dateNaiss, sexe, numTel, medecin, path);
                    managerPatient.addPatient(patient);
                }
                else
                {
                    Patient patient = new Patient(idSelectionne, nom, prenom, dateNaiss, sexe, numTel, medecin, path);
                    managerPatient.updatePatient(patient);
                }

                nvNomPatient.text = "";
                nvPrenomPatient.text = "";
                nvSexePatient.text = "";
                nvDateNaissPatient.text = "";
                nvNumTelPatient.text = "";
                nvIdMedecinPatient.text = "";

                panelAddPatient.SetActive(false);
                listePatient();
                idSelectionne = 0;
                break;
        }
    }

    public void listeExamen()
    {
    	accueilOperateur.SetActive(false);
    	infoMedecin.SetActive(false);
    	infoPatient.SetActive(false);
    	infoExamen.SetActive(true);

        int i;
        for (i = 1; i < tableExamen.transform.childCount; i++)
        {
            Destroy(tableExamen.transform.GetChild(i).gameObject);
        }
        ManagerExamen managerExamen = new ManagerExamen(ConnexionDB.DB());
        List<Examen> examens = managerExamen.getAllExamen();

        i = 1;
        foreach (Examen examen in examens)
        {
            GameObject test = Instantiate(ligneExamen);
            test.transform.SetParent(tableExamen.transform, false);
            test.name = "row" + i;
            test.SetActive(true);

            GameObject id = GameObject.Find("IDExamen");
            id.name = "IDExamen" + i;
            id.GetComponent<Text>().text = examen.idE.ToString();

            GameObject dateE = GameObject.Find("DateExamen");
            dateE.name = "DateExamen" + i;
            dateE.GetComponent<Text>().text = examen.dateE;

            GameObject patient = GameObject.Find("PatientExamen");
            patient.name = "PatientExamen" + i;

            ManagerPatient managerPatient = new ManagerPatient(ConnexionDB.DB());
            Patient patientExamen = managerPatient.getPatientById(examen.idPatient);
            patient.GetComponent<Text>().text = patientExamen.prenomP.Substring(0, 1) + "." + patientExamen.nomP;
            
            GameObject noteE = GameObject.Find("NoteExamen");
            noteE.name = "NoteExamen" + i;
            noteE.GetComponent<Text>().text = examen.note;

            i++;
        }
    }

    public void crudExamen(GameObject g)
    {
        ManagerExamen managerExamen = new ManagerExamen(ConnexionDB.DB());
        switch (g.name)
        {
            case "Supprimer":
                if (idSelectionne > 0)
                {
                    managerExamen.deleteExamen(idSelectionne);
                    listeExamen();
                    idSelectionne = 0;
                }
                break;
        }
    }
}
