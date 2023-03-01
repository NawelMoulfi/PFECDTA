using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class initComponant : MonoBehaviour {

    public Text ipAdresse;
    public static string IPServer="";
    NetworkClient Client;
    // Use this for initialization
    void Start () {
        if (CubemanController.side == "Serveur")
        {
            GameObject.Find("NetManager").GetComponent<NetworkManager>().StartServer();
            GameObject.Find("NetManager").GetComponent<NetworkManagerHUD>().enabled = false;

            ipAdresse.text = GetLocalIPAddress();
        }
        if (CubemanController.side == "Client")
        {
            if (IPServer=="")
            {
                IPServer = "localhost";
            }
            GameObject.Find("NetManager").GetComponent<NetworkManager>().networkAddress = CubemanController.ipServeur;
            Client = GameObject.Find("NetManager").GetComponent<NetworkManager>().StartClient();

            StartCoroutine(waitMe());

            GameObject.Find("NetManager").GetComponent<NetworkManagerHUD>().enabled = false;
        }
        
    }
	
	// Update is called once per frame
	void Update () {

        if (CubemanController.side == "Client" && hasConnected)
        {
            if (!Client.isConnected)
            {
                GameObject.Find("NetManager").GetComponent<NetworkManager>().StopClient();
                SceneManager.LoadScene(0);
            }
        }
        
	}


    bool hasConnected = false;

    public static string GetLocalIPAddress()
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }

    IEnumerator waitMe()
    {
        yield return new WaitForSeconds(10f);

        if (!Client.isConnected)
        {
            GameObject.Find("NetManager").GetComponent<NetworkManager>().StopClient();
            SceneManager.LoadScene(0);
        }
        else
        {
            hasConnected = true; 
        }
        
        
    }










  



}
