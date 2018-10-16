using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatController : MonoBehaviour
{
    private Dictionary<int, string> namesByConnectionID = new Dictionary<int, string>();
    private Dictionary<string, int> connectionIDsByName = new Dictionary<string, int>();
    public int maxNameLength = 20;
    public int maxMessages = 5;
    private string localPlayerName = "Local PLayer";

    public List<string> messages = new List<string>();
    private CustomNetworkControl myNetworkControl;

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

    internal void AddMessage(string message)
    {
        if (myNetworkControl == null)
        {
            OnChatMessageReceived(localPlayerName, message);
            if (messages.Count > maxMessages)
            {
                messages.RemoveAt(messages.Count - 1);
            }
            else
            {
                myNetworkControl.SendChatMessage(message);
            }
        }
    }

    public void AnnouncePlayer(string playerName)
    {
        messages.Add(string.Format("Player {0} joined.", playerName));
    }
    
    internal string SetPlayerName(string playerName, int connectionID)
    {
        
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
    /*
    private void OnGUI()
    {
        foreach (string message in messages)
        {
            GUILayout.Label(message);

        }
    }
*/
    // Use this for initialization
    void Start()
    {
        GameObject networkControllerObj = GameObject.FindGameObjectWithTag("NetworkController");
        if (networkControllerObj != null)
        {
            myNetworkControl = networkControllerObj.GetComponent<CustomNetworkControl>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
