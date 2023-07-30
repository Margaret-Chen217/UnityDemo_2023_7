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

    private static readonly object toDoListLock = new object();

    private static void listenServer(object obj)
    {
        Socket socket = (Socket)obj;
        byte[] readbuffer = new byte[1024];
        while (true)
        {
            int len = socket.Receive(readbuffer);
            string str = System.Text.Encoding.Default.GetString(readbuffer, 0, len);
            foreach (var s in str.Split('&'))
            {
                if (s.Length > 0)
                {
                    lock (toDoListLock)
                    {
                        Message msg = JsonConvert.DeserializeObject<Message>(s);
                        todoList.Enqueue(msg);
                        //Debug.Log($"Enqueue: {msg}");
                    }
                }
            }
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
            Message msg;
            bool flag = false;
            lock (toDoListLock)
            {
                msg = todoList.Dequeue();
                flag = true;
            }

            if (flag)
            {
                Debug.Log($"message type: {msg.type}");
                switch (msg.type)
                {
                    case "AllPlayerInfo":
                        List<PlayerInfo> listInfo = JsonConvert.DeserializeObject<List<PlayerInfo>>(msg.info);
                        OnlinePlayerPool.ins.updateOnlinePlayer(listInfo);
                        break;
                }

                flag = false;
            }
        }
    }
}