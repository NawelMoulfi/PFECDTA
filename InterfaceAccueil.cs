using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using System;
using System.IO;
//using System.Threading.Tasks;
//using System.Windows.Forms;

public class InterfaceAccueil : MonoBehaviour
{
    public GameObject interfaceAccueil;
	public GameObject panelAccueil;
    public GameObject interfaceConnexionServeur;
    public GameObject interfaceConnexionClient;
	public GameObject panelServeur;
	public GameObject interfaceMedecin;


    // Start is called before the first frame update
    void Start()
    {
        interfaceAccueil.SetActive(true);
		panelAccueil.SetActive (true);
        interfaceConnexionServeur.SetActive(false);
        interfaceConnexionClient.SetActive(false);
		//SceneManager.LoadScene ("Representation Squelettique");

		if (InterfaceMedecin.examenSelected > 0) 
		{
			AllerVersDetailExamen ();
		}

    }

    void AllerVersDetailExamen()
	{
		interfaceAccueil.SetActive (false);
		//yield return new WaitForSeconds (3);
		interfaceConnexionServeur.SetActive (true);
		panelServeur.SetActive (false);
		interfaceMedecin.SetActive (true);
		interfaceMedecin.GetComponent<InterfaceMedecin> ().starting ();
		interfaceMedecin.GetComponent<InterfaceMedecin>().listePatient();
		interfaceMedecin.GetComponent<InterfaceMedecin>().listeExamen();
		interfaceMedecin.GetComponent<InterfaceMedecin>().affichageExamen();
	}
}
