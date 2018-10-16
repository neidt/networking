using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkMenuController : MonoBehaviour
{
    public CustomNetworkControl customNetworkControl;

    public InputField txtPlayerName;
    public InputField txtIPAddress;

    // Use this for initialization
    void Start()
    {
        if (customNetworkControl == null)
        {
            customNetworkControl = GameObject.FindGameObjectWithTag("NetworkController").GetComponent<CustomNetworkControl>();
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        string hostName = System.Net.Dns.GetHostName();
        foreach (System.Net.IPAddress ip in System.Net.Dns.GetHostEntry(hostName).AddressList)
        {
            sb.AppendLine(ip.ToString());
            if (IsValidIPAddress(ip.ToString()))
            {
                customNetworkControl.serverIPAddress = ip.ToString();
            }
        }
        print(sb.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void btnStartServer_Click()
    {
        customNetworkControl.StartNetworkHost();
    }

    public void btnStartClient_Click()
    {
        string ipAddress = txtIPAddress.text.Trim();
        if (IsValidIPAddress(ipAddress))
        {
            txtIPAddress.text = string.Empty;
        }
        else
        {
            customNetworkControl.serverIPAddress = txtIPAddress.text;
            customNetworkControl.networkAddress = txtIPAddress.text;
            customNetworkControl.StartNetworkClient();
        }
    }

    public void txtPlayerName_OnChange()
    {
        customNetworkControl.playerName = txtPlayerName.text;
    }

    private bool IsValidIPAddress(string ipAddress)
    {
        string[] parts = ipAddress.Split('.');
        if (parts.Length != 4)
        {
            return false;
        }

        uint a, b, c, d;
        if (uint.TryParse(parts[0], out a) == false)
        {
            return false;
        }
        if (uint.TryParse(parts[1], out b) == false)
        {
            return false;
        }
        if (uint.TryParse(parts[2], out c) == false)
        {
            return false;
        }
        if (uint.TryParse(parts[3], out d) == false)
        {
            return false;
        }

        if (a > 223 || a < 1)
        {
            return false;
        }
        if (b > 255 || b < 0)
        {
            return false;
        }
        if (c > 255 || c < 0)
        {
            return false;
        }
        if (d > 255 || d < 0)
        {
            return false;
        }

        if (a + b == 0)
        {
            return false;
        }

        return true;
    }
}
