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

    public override string ToString()
    {
        string str = ($"{this.type}, {this.info} ");
        return str;
    }
}

[Serializable]
public class UserClient
{
    private Socket socket;

    public void Initialze()
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect("127.0.0.1", 8888);
            Listener.startListen(socket);
            Debug.Log("Connected to Server");
        }
        catch (Exception exception)
        {
            Debug.Log("Not Found Server");
        }
    }

    public void SendMessageToServer(Message message)
    {
        //message.Print();
        //Debug.Log("SendMessageToServer");
        string str = JsonConvert.SerializeObject(message);
        byte[] bytes = System.Text.Encoding.Default.GetBytes(str + "&");
        if (socket != null && socket.Connected)
        {
            Debug.Log($"Message in UserClient Send: {str}");
            socket.Send(bytes);
        }
    }
}