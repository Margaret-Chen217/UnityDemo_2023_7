using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using UnityEngine;

public class Listener : MonoBehaviour
{
    private static Queue<Message> todoList = new Queue<Message>();
    private static void listenServer(object obj)
    {
        Socket socket = (Socket)obj;
        byte[] readbuffer = new byte[1024];
        while (true)
        {
            int len = socket.Receive(readbuffer);
            string str = System.Text.Encoding.Default.GetString(readbuffer, 0, len);
            Message msg = JsonConvert.DeserializeObject<Message>(str);
        }
    }

    public static void startListen(Socket socket)
    {
        Thread listen = new Thread(new ParameterizedThreadStart(listenServer));
        listen.Start(socket);
    }

    private void Update()
    {
        if (todoList.Count > 0)
        {
            Message msg = todoList.Dequeue();
            switch (msg.type)
            {
                case "AllPlayerInfo":
                    List<PlayerInfo> listInfo = JsonConvert.DeserializeObject<List<PlayerInfo>>(msg.info);
                    OnlinePlayerPool.ins.updateOnlinePlayer(listInfo);
                    break;
            }
        }
    }
}