using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class CustomNetworkControl : NetworkManager
{
    public string playerName;
    private ChatController myChat;

    private const short PlayerNameMessage = 2001;
    private const short AssignPlayerNameMessage = 2002;
    private const short PlayerJoinedGameMessage = 2003;

    public class ChatMessage : MessageBase
    {
        public string sender;
        public string message;
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        myChat = GameObject.FindGameObjectWithTag("ChatSystem").GetComponent<ChatController>();
        myChat.AnnouncePlayer(playerName);
        //client.Send(PlayerNameMessage, new StringMessage(playerName));
    }

    public void StartNetworkHost()
    {
        StartHost();
        NetworkServer.RegisterHandler(PlayerJoinedGameMessage, OnOtherPlayerJoinedGame);
       
    }

    public void StartNetworkClient()
    {
        StartClient();
        NetworkServer.RegisterHandler(PlayerJoinedGameMessage, OnOtherPlayerJoinedGame);
       // NetworkServer.RegisterHandler(3000,OnChatMessageReceived);
    }

    public void OnOtherPlayerJoinedGame(NetworkMessage netMsg)
    {
        string playerName = netMsg.ReadMessage<StringMessage>().value;
        print("other poalyer joinged messsgae reveiced, name is " + playerName);
        myChat.AnnouncePlayer(playerName);
    }

    public void OnPlayerNameRecieved(NetworkMessage netMsg)
    {
        StringMessage specificMessage = netMsg.ReadMessage<StringMessage>();
        playerName = specificMessage.value;
        NetworkServer.SendToAll(PlayerJoinedGameMessage, new StringMessage(playerName));
    }

    public void SendChatMessage(string message)
    {
        client.Send(3000, new StringMessage(message));
    }

    public void OnChatMessageReceived(NetworkMessage netMsg)
    {
        ChatMessage received = netMsg.ReadMessage<ChatMessage>();

        myChat.OnChatMessageReceived(received.sender, received.message);
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
