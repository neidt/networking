using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatController : MonoBehaviour {
    private Dictionary<int, string> namesByConnectionID = new Dictionary<int, string>();
    private Dictionary<string, int> connectionIDsByName = new Dictionary<string, int>();
    public int maxNameLength = 20;
    private string localPlayerName;

    public List<string> messages = new List<string>();

    public void OnChatMessageReceived(string sender, string message)
    {
        messages.Add(string.Format("[User chat] {0}: {1}", sender, message));
    }

    public string GetNameByConnectionId(int connectionId)
    {
        return namesByConnectionID[connectionId];
    }

    public void SetLocalPlayerName(string playerName)
    {
        localPlayerName = playerName;
    }

    public void AnnouncePlayer(string playerName)
    {
        messages.Add(string.Format("Player {0} joined.", playerName));
    }

    internal string SetPlayerName(string playerName, int connectionID)
    {
        //playername = ensureunique(playername, connectionID);
        if (namesByConnectionID.ContainsKey(connectionID))
        {
            //remove dfrom both indexes firest
            connectionIDsByName.Remove(namesByConnectionID[connectionID]);
            namesByConnectionID.Remove(connectionID);
        }
        connectionIDsByName.Add(playerName, connectionID);
        namesByConnectionID.Add(connectionID, playerName);
        return playerName;
    }

    private void OnGUI()
    {
        foreach (string message in messages)
        {
            GUILayout.Label(message);
        }
    }

    // Use this for initialization
    void Start ()
    {
            
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
