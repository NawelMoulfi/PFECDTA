using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Net;
using System.Net.Sockets;

public class InterfaceConnexionClient : MonoBehaviour
{
    public GameObject interfaceAccueil;
    public GameObject interfaceConnexionSeveur;
    public GameObject interfaceConnexionClient;
    public GameObject interfaceMedecin;
    public GameObject interfaceOperateur;
    public InputField ipToServeur;

    // Use this for initialization
    public void starting()
    {
        //Serveur.startProgramme();
        
        interfaceAccueil.SetActive(false);
        interfaceConnexionSeveur.SetActive(false);
        interfaceConnexionClient.SetActive(true);
        interfaceMedecin.SetActive(false);
        interfaceOperateur.SetActive(false);
    }

    public void connexionNetwork()
    {
        CubemanController.side = "Client";
        CubemanController.ipServeur = ipToServeur.text;
		//TestTransportNetwork.side = "Client";
		//TestTransportNetwork.ipServeur = ipToServeur.text;
		SceneManager.LoadScene("AnimationLive");
		//SceneManager.LoadScene("TransportNetworkTest");
    }

	public void retour()
	{
		ipToServeur.text = "";
	}
}

