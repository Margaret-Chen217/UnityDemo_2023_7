using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;

public class PlayerInfo
{
    public String PlayerName { get; set; }
    public float x, y, z;

    public PlayerInfo(String name, Vector3 vector3)
    {
        this.PlayerName = name;
        this.x = vector3.x;
        this.y = vector3.y;
        this.z = vector3.z;
    }
}

public class Message
{
    public string type;
    public string info;

    public Message(string type, string info)
    {
        this.type = type;
        this.info = info;
    }
}


public class UserClient : MonoBehaviour
{
    public Button button;

    private Socket socket;

    private void Awake()
    {
        button.onClick.AddListener(delegate { OnNetworkButtonClick(); });
    }

    void Start()
    {
    }

    private void OnNetworkButtonClick()
    {
        if (socket == null)
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.Connect("127.0.0.1", 8888);

                Debug.Log("Connected to Server");

                PlayerInfo p = new PlayerInfo("Margaret", transform.position);
                Debug.Log(p);
                string str = JsonConvert.SerializeObject(p);
                byte[] bytes = System.Text.Encoding.Default.GetBytes(str);
                socket.Send(bytes);
            }
            catch (Exception exception)
            {
                Debug.Log("Not Found Server");
            }
        }
        else
        {
            socket = null;
        }
    }
}