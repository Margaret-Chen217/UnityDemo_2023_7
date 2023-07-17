using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIInventoryDescription : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text description;

    public void Awake()
    {
        ResetDescription();
    }

    /// <summary>
    /// 重置description,数据为0,字符串为null
    /// </summary>
    public void ResetDescription()
    {
        this.itemImage.gameObject.SetActive(false);
        this.itemName.text = "";
        this.description.text = "";
    }

    /// <summary>
    /// 设置description
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="itemName"></param>
    /// <param name="itemDescription"></param>
    public void SetDescription(Sprite sprite, string itemName, string itemDescription)
    {
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.itemName.text = itemName;
        this.description.text = itemDescription;
    }
}
