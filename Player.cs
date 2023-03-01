using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{

	[SyncVar] public string playerUniqueIdentity;
	private NetworkInstanceId playerNetID;
	private Transform myTransform;
    public static string uniqueName;
    public static int nombrePlayer;
	public override void OnStartLocalPlayer()
	{
		GetNetIdentity();
		SetIdentity();
	}
	private void Awake()
	{
		myTransform = transform;
	}
	// Update is called once per frame
	void Update()
	{
		if (myTransform.name == "" || myTransform.name == "U_CharacterFront(Clone)" || myTransform.name == "Ch33_nonPBR(Clone)")
		{
			SetIdentity();

		}
		movePlayer();
	}


	void SetIdentity()
	{

		if (!isLocalPlayer)
		{

			myTransform.name = playerUniqueIdentity;
		}
		else
		{
			myTransform.name = MakeUniqueIdentity();

		}

	}
	private void movePlayer()
	{

		if (isLocalPlayer == true)
		{

			if (Input.GetKey(KeyCode.D))
			{
				Debug.Log("hello its me");
				myTransform.transform.Translate(Vector3.right * Time.deltaTime * 3f);
			}
			if (Input.GetKey(KeyCode.Q))
			{
				myTransform.transform.Translate(Vector3.left * Time.deltaTime * 3f);
			}
		}
	}
	[Client]
	void GetNetIdentity()
	{
		playerNetID = GetComponent<NetworkIdentity>().netId;

		CmdTellServerMyIdentity(MakeUniqueIdentity());
	}
	string MakeUniqueIdentity()
	{
		uniqueName = "Player " + playerNetID.ToString();
		return uniqueName;
	}
	[Command]
	void CmdTellServerMyIdentity(string name)
	{
		playerUniqueIdentity = name;
        print("player name = " + playerUniqueIdentity);
        nombrePlayer++;
	}
	// Use this for initialization
	void Start()
	{
        nombrePlayer = 0;

	}

}
