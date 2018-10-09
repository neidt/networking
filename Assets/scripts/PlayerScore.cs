using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScore : NetworkBehaviour
{
    [SyncVar]
    public int score = 0;

    [SyncVar]
    public string playerName = string.Empty;

    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            if (isServer)
            {
                score = value;
            }
        }
    }

    public int spacePressCount = 0;
    private ChatController mychat;


	// Use this for initialization
	void Start ()
    {
        if (GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
