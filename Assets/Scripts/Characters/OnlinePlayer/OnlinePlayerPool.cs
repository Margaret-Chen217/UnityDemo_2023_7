using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator;
using Unity.Mathematics;
using UnityEngine;

public class OnlinePlayerPool : MonoBehaviour
{
    public GameObject onlinePlayerPrefab;
    public PlayerOnlineInfoSO CurrentPlayerData;
    private Dictionary<string, GameObject> onlinePlayerDic = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    public void updateOnlinePlayer(List<PlayerInfo> playerInfoList)
    {
        foreach (PlayerInfo playerInfo in playerInfoList)
        {
            if (playerInfo.PlayerName == CurrentPlayerData.playerName)
            {
                continue;
            }
            if (onlinePlayerDic.ContainsKey(playerInfo.PlayerName))
            {
                onlinePlayerDic[playerInfo.PlayerName].transform.position =
                    new Vector3(playerInfo.x, playerInfo.y, playerInfo.z);
            }
            else
            {
                onlinePlayerDic[playerInfo.PlayerName] = Instantiate(onlinePlayerPrefab,
                    new Vector3(playerInfo.x, playerInfo.y, playerInfo.z), quaternion.identity);
            }
            
        }
    }

    public static OnlinePlayerPool ins;

    private void Awake()
    {
        ins = this;
    }
}