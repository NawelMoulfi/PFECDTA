using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnexionMenu : NetworkBehaviour
{
    


    NetworkInstanceId playerNetID;

    GameObject miniJeu, KinectConfig;


    uint ID;
    [SyncVar]uint MyID;


    public override void OnStartLocalPlayer()
    {
        playerNetID = GetComponent<NetworkIdentity>().netId;
        ID = playerNetID.Value;
    }
    // Use this for initialization
    Transform playerTransform,  playerCameraTransform;

    void Start()
    {

        if(GameObject.Find("ButtonReturnClient"))
        {
            Button b = GameObject.Find("ButtonReturnClient").GetComponent<Button>();
            b.onClick.AddListener(delegate { ReturnClient(ID); });
        }

        



        if (!isServer && isLocalPlayer)
        {
            Debug.Log("Envoyé avec ID =" + ID);
            MyID = ID;
            CmdSpawnMyMenu(ID);
            
            playerTransform = GameObject.Find("Player " + ID).transform;
            playerCameraTransform = GameObject.Find("Main Camera").transform;
            playerCameraTransform.position = new Vector3(playerTransform.position.x, 1, 4);
            
        }

        if (isClient && isLocalPlayer)
        {
            if (GameObject.Find("ClientText"))
            {
                GameObject.Find("ClientText").GetComponent<Text>().text = "Client " + (ID-2);
            }
            playerTransform = GameObject.Find("Player " + ID).transform;
            playerCameraTransform = GameObject.Find("Main Camera").transform;
            playerCameraTransform.position = new Vector3(playerTransform.position.x, 1, 0);

        }


    }



    public void ReturnClient(uint id)
    {
        Debug.Log("ReturnClient id =  " + ID + "destroyME = " + destroyME + "id =  " + id);
        destroyME = true;
        CmdDisplayMe2(id);
        CmdDeleteMyMenu2(id);
        Debug.Log("wait 1 ");
        StartCoroutine(waitMe());

        Debug.Log("wait 2");

       
    }

    [Command]
    void CmdDisplayMe2(uint id)
    {
        Debug.Log("Hello this is me  id =  " + id + "destroyME = "  + "SyncId =  " + MyID);
    }
    [Command]
    void CmdDeleteMyMenu2(uint id)
    {
        Debug.Log("CmdDeleteMyMenu");

       
        Destroy(GameObject.Find("MenuItem " + id));
        Destroy(GameObject.Find("Kinect " + id));
        Destroy(GameObject.Find("Client3DText "+ id));
     
    }



    IEnumerator waitMe()
    {
        while (true)
        {

           


            yield return new WaitForSeconds(2f);
            GameObject.Find("NetManager").GetComponent<NetworkManager>().StopClient();

            SceneManager.LoadScene(0);
            Debug.Log("waitMe 1 ");

        }

    }





    void CheckForKinectState()
    {
        if (isServer)
        {
            
            for (int i = 0; i < nbKinectMax; i++)
            {
                
                if (GameObject.Find("KinectsConfig"))
                {
                    if (GameObject.Find("Kinect " + i))
                    {
                        GameObject KinectState = GameObject.Find("Button " + i);



                        GameObject KinectStateConfig = GameObject.Find("KinectStateButtonConfig " + i);

                        GameObject Player = GameObject.Find("Player " + i);
                        string stateOfKinect = Player.GetComponent<KinectManager>().KinectState;
                   

                        switch (stateOfKinect)
                        {
                            case "":
                                KinectState.GetComponent<Image>().color = new Color(1, 0, 0, 1);
                                KinectStateConfig.GetComponent<Image>().color = new Color(1, 0, 0, 1);
                                GameObject.Find("KinectState " + i).GetComponent<Text>().text = "Erreur d'init";
                                GameObject.Find("KinectStateConfig " + i).GetComponent<Text>().text = "Erreur d'init";
                                break;
                            case null:
                                KinectState.GetComponent<Image>().color = new Color(1, 0, 0, 1);
                                KinectStateConfig.GetComponent<Image>().color = new Color(1, 0, 0, 1);
                                GameObject.Find("KinectState " + i).GetComponent<Text>().text = "Erreur d'init";
                                GameObject.Find("KinectStateConfig " + i).GetComponent<Text>().text = "Erreur d'init";
                                break;
                            case "Suivi":
                                KinectState.GetComponent<Image>().color = new Color(0, 0.78f, 1.0f, 1);
                                KinectStateConfig.GetComponent<Image>().color = new Color(0, 0.78f, 1.0f, 1);

                                GameObject.Find("KinectState " + i).GetComponent<Text>().text = "Joueur Suivi";
                                GameObject.Find("KinectStateConfig " + i).GetComponent<Text>().text = "Joueur Suivi";
                          

                                break;
                            case "Ready":
                                KinectState.GetComponent<Image>().color = new Color(0, 1, 0, 1);
                                KinectStateConfig.GetComponent<Image>().color = new Color(0, 1, 0, 1);
                                GameObject.Find("KinectState " + i).GetComponent<Text>().text = "Pret";
                                GameObject.Find("KinectStateConfig " + i).GetComponent<Text>().text = "Pret";
                                break;
                            case "Exit":
                                cheekForExit("" + i);
                                break;
                            case "ErreurInit":
                                KinectState.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                                KinectStateConfig.GetComponent<Image>().color = new Color(1, 0.27f, 0, 1);
                                GameObject.Find("KinectState " + i).GetComponent<Text>().text = "Not Connected";
                                GameObject.Find("KinectStateConfig " + i).GetComponent<Text>().text = "Not Connected";
                                break;

                        }




                    }

                }
                else
                {
                    
                    if (GameObject.Find("Kinect " + i))
                    {
                        GameObject KinectState = GameObject.Find("Button " + i);

                        GameObject Player = GameObject.Find("Player " + i);
                        if (!GameObject.Find("Player " + i))
                        {

                            Debug.Log("!player " + i);
                            if (GameObject.Find("MenuItem " + i))
                            {
                                Debug.Log("MenuItem exist" + i);
                                GameObject.Find("MenuItem " + i).SetActive(false);
                            }
                            break;
                        }
                        string stateOfKinect = Player.GetComponent<KinectManager>().KinectState;



                        switch (stateOfKinect)
                        {
                            case "":
                                KinectState.GetComponent<Image>().color = new Color(1, 0, 0, 1);
                                GameObject.Find("KinectState " + i).GetComponent<Text>().text = "Erreur d'init";
                                break;
                            case null:
                                KinectState.GetComponent<Image>().color = new Color(1, 0, 0, 1);
                                GameObject.Find("KinectState " + i).GetComponent<Text>().text = "Erreur d'init";
                                break;
                            case "Suivi":
                                KinectState.GetComponent<Image>().color = new Color(0, 0.78f, 1.0f, 1);
                                GameObject.Find("KinectState " + i).GetComponent<Text>().text = "Joueur Suivi";
                            
                                break;
                            case "Ready":
                                KinectState.GetComponent<Image>().color = new Color(0, 1, 0, 1);
                                GameObject.Find("KinectState " + i).GetComponent<Text>().text = "Pret";
                                break;
                            case "Exit":
                                cheekForExit("" + i);
                                break;
                            case "ErreurInit":
                                KinectState.GetComponent<Image>().color = new Color(1, 0.27f, 0, 1);
                                GameObject.Find("KinectState " + i).GetComponent<Text>().text = "Not Connected";
                                break;

                        }




                    }
                    
                    
                    
                }

             
            }
        }
    }

    void cheekForExit(string id)
    {
        Debug.Log("Destroy kinect ");
        //clientIDs.Remove(int.Parse(id));
        Destroy(GameObject.Find("Kinect " + id));
        Destroy(GameObject.Find("Kinect " + id));
        Destroy(GameObject.Find("MenuItem " + id));

    }
    [SyncVar] bool destroyME = false;
    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer && isClient)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                LocalCheckPingViaCmdRpc();
            }
        }
       
        CheckForKinectState();
        if (!isServer && isLocalPlayer)
        {
            for (int i = 0; i<10; i++)
            {
                if (GameObject.Find("Player " + i)&& i!=ID)
                {
                    GameObject.Find("Player " + i).SetActive(false);
                }
            }

        }
 

    }
    void setPosition()
    {
       
        playerTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 10, playerTransform.position.z);
        CmdSetPositionInServeur(ID);

        
    }

    [Command]
    void CmdSetPositionInServeur(uint ctr)
    {
        //Transform playerTransform = GameObject.Find("Player " + ctr).transform;
        //playerTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 10, playerTransform.position.z);
    }


    public static int nbKinectMax = 10;
    Transform KinectListContentConfigTransform;
    public GameObject item;
    public GameObject itemConfig;
    //ButtonManager buttonManager;


    public void ReturnClient()
    {
        destroyME = true;
        CmdDisplayMe(ID, destroyME);
        CmdDeleteMyMenu();

        GameObject.Find("NetManager").GetComponent<NetworkManager>().StopClient();
        
        SceneManager.LoadScene(0);
 
    }

    [Command]
    void CmdDisplayMe(uint id, bool destroyMe)
    {
        MyID = id;
        Debug.Log("Hello this is me  id =  " + id + "destroyME = " + destroyMe+ "SyncId =  " + MyID);
    }
    [Command]
    void CmdDeleteMyMenu()
    {
        GameObject.Find("MenuItem " + MyID).SetActive(false);
        GameObject.Find("Kinect " + MyID).SetActive(false);
        Destroy(GameObject.Find("MenuItem " + MyID));
        Destroy(GameObject.Find("Kinect " + MyID));
    }

    public GameObject Client3DText;


    [Command]
    void CmdSpawnMyMenu(uint ctr)
    {
        //Transform playerTransform = GameObject.Find("Player " + ctr).transform;
       // playerTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 10, playerTransform.position.z);

        Debug.Log("  SpawnMyMenu ");
        
        if (GameObject.Find("KinectsConfig"))
        {
            GameObject myItemConfig = Instantiate(itemConfig);
            myItemConfig.SetActive(true);
            myItemConfig.transform.SetParent(GameObject.Find("RefParentConfig").transform.parent, false);

            myItemConfig.name = "Kinect " + ctr;


            GameObject KinectIDConfig = GameObject.Find("KinectIDConfig");
            KinectIDConfig.name = "KinectIDConfig " + ctr;
            KinectIDConfig.GetComponent<Text>().text = "Kinect " + ctr;

            GameObject KinectIDConfigD = GameObject.Find("KinectIDConfigD");
            KinectIDConfigD.name = "KinectIDConfigD " + (ctr - 2);
            KinectIDConfigD.GetComponent<Text>().text = "Kinect " + (ctr-2);

            GameObject.Find("KinectStateButtonConfig").name = "KinectStateButtonConfig " + ctr;
            GameObject.Find("KinectStateConfig").name = "KinectStateConfig " + ctr;


        }

        GameObject ClientText = Instantiate(Client3DText);
        ClientText.SetActive(true);
        ClientText.name = "Client3DText " + ctr;
        ClientText.GetComponent<TextMesh>().text = "Client " + (ctr-2);


        GameObject myItem = Instantiate(item);
        myItem.SetActive(true);
        myItem.transform.SetParent(GameObject.Find("RefParent").transform.parent, false);

        myItem.name = "MenuItem " + ctr;


        GameObject txt = GameObject.Find("KinectID");
        Text c = txt.GetComponent<Text>();
        c.name = "Kinect " + ctr;
        c.text = "Kinect " + (ctr);

        GameObject txtD = GameObject.Find("KinectIDD");
        Text cD = txtD.GetComponent<Text>();
        cD.name = "KinectD " + (ctr-2);
        cD.text = "Kinect " + (ctr - 2);




        GameObject mybutton = GameObject.Find("KinectStateButton");
        mybutton.name = "Button " + ctr;

        GameObject myStates = GameObject.Find("KinectState");
        myStates.name = "KinectState " + ctr;

        GameObject focusButton = GameObject.Find("Focus");
        focusButton.name = "Focus " + ctr;

        GameObject repereButton = GameObject.Find("Repere");
        repereButton.name = "Repere " + ctr;
    }













    float _sentTime;
    void LocalCheckPingViaCmdRpc()
    {
        _sentTime = Time.time;
        CmdServerReceiveCheckingPingRequest();
    }

    [Command]
    void CmdServerReceiveCheckingPingRequest()
    {
        RpcClientReceiveCheckingPingMessageFromServer();
    }

    [ClientRpc]
    void RpcClientReceiveCheckingPingMessageFromServer()
    {
        if (isLocalPlayer)
        {
            var RTT = (Time.time - _sentTime) * 1000f;
            Debug.LogFormat("Cmd, Rpc -> RTT : {0}", RTT);
        }
    }






}
