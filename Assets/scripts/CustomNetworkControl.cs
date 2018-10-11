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
        client.Send(PlayerNameMessage, new StringMessage(playerName));
    }

    public void StartNetworkHost()
    {
        StartHost();
        //NetworkServer.RegisterHandler(PlayerJoinedGameMessage, OnOtherPlayerJoinedGame);
        RegisterServerListeners();
        RegisterClientListeners();
    }

    public void StartNetworkClient()
    {
        StartClient();
        RegisterClientListeners();
        // NetworkServer.RegisterHandler(PlayerJoinedGameMessage, OnOtherPlayerJoinedGame);
        // NetworkServer.RegisterHandler(3000,OnChatMessageReceived);
    }

    public void SendChatMessage(string message)
    {
        client.Send(3000, new StringMessage(message));
    }

    private void RegisterClientListeners()
    {
        client.RegisterHandler(2002, OnNameAssigned);
        client.RegisterHandler(2003, OnOtherPlayerJoinedGame);
        client.RegisterHandler(3001, OnChatMessageReceived);
    }

    public void OnNameAssigned(NetworkMessage netMsg)
    {
        string playerName = netMsg.ReadMessage<StringMessage>().value;
        print("Client player name assigned to " + playerName);
        myChat.SetLocalPlayerName(playerName);
    }

    public void OnOtherPlayerJoinedGame(NetworkMessage netMsg)
    {
        string playerName = netMsg.ReadMessage<StringMessage>().value;
        print("Other player joined message received, name is " + playerName);
        myChat.AnnouncePlayer(playerName);
    }

    public void OnChatMessageReceived(NetworkMessage netMsg)
    {
        ChatMessage received = netMsg.ReadMessage<ChatMessage>();

        myChat.OnChatMessageReceived(received.sender, received.message);
    }


    /*
    public void OnOtherPlayerJoinedGame(NetworkMessage netMsg)
    {
        string playerName = netMsg.ReadMessage<StringMessage>().value;
        print("other poalyer joinged messsgae reveiced, name is " + playerName);
        myChat.AnnouncePlayer(playerName);
    }*/

    private void RegisterServerListeners()
    {
        NetworkServer.RegisterHandler(2001, OnPlayerNameReceived);
        NetworkServer.RegisterHandler(3000, OnPlayerSendChatMessage);
    }

    public void OnPlayerNameReceived(NetworkMessage netMsg)
    {
        string playerName = netMsg.ReadMessage<StringMessage>().value;
        print("Received player name " + playerName);
        playerName = myChat.SetPlayerName(playerName, netMsg.conn.connectionId);
        NetworkServer.SendToClient(netMsg.conn.connectionId, 2002, new StringMessage(playerName));
        NetworkServer.SendToAll(2003, new StringMessage(playerName));
    }
    /*
    public void OnPlayerNameRecieved(NetworkMessage netMsg)
    {
        StringMessage specificMessage = netMsg.ReadMessage<StringMessage>();
        playerName = specificMessage.value;
        NetworkServer.SendToAll(PlayerJoinedGameMessage, new StringMessage(playerName));
    }*/

    public void OnPlayerSendChatMessage(NetworkMessage netMsg)
    {
        string message = netMsg.ReadMessage<StringMessage>().value.Trim();
        if (message.Length > 100)
        {
            message = message.Substring(0, 100).Trim();
        }
        string senderName = myChat.GetNameByConnectionId(netMsg.conn.connectionId);
        
        ChatMessage chatMessage = new ChatMessage() { sender = senderName, message = message };
        NetworkServer.SendToAll(3001, chatMessage);
    }
    /*
    public void OnChatMessageReceived(NetworkMessage netMsg)
    {
        ChatMessage received = netMsg.ReadMessage<ChatMessage>();

        myChat.OnChatMessageReceived(received.sender, received.message);
    }*/


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
