using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePlayerLabel : MonoBehaviour
{
    private Player player;
    public TMP_Text playerLabel;

    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        //设置玩家label

        playerLabel = transform.Find("Text").GetComponent<TMP_Text>();
        UpdateLabel();
    }

    public void UpdateLabel()
    {
        Player player = transform.parent.GetComponent<Player>();

        if (player == null)
        {
            //线上玩家
            //线上玩家和本地玩家应该继承一个基类
            OnlinePlayer onlinePlayer = transform.parent.GetComponent<OnlinePlayer>();
            playerLabel.text = onlinePlayer.playerName;
        }
        else
        {
            playerLabel.text = player.PlayerOnlineData.playerName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(mainCamera.transform);
    }
}