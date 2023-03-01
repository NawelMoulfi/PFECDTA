using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DisplayManager : NetworkBehaviour {

    
    

    public GameObject ClientCanvas;

    // Use this for initialization
    void Start () {



        Debug.Log("isClient = "+ isClient);
        Debug.Log("isLocalPlayer = " + isLocalPlayer);
        Debug.Log("isServer = " + isServer);
        Debug.Log(" GameObject.Find ClientCanvas = " + GameObject.Find("ClientCanvas"));

        if (isServer)
        {
            ClientCanvas.SetActive(false);
        }
        if (isClient )
        {
            ClientCanvas.SetActive(true);
        }
            
        
       
	}
	
	// Update is called once per frame
	void Update () {

    }
}
