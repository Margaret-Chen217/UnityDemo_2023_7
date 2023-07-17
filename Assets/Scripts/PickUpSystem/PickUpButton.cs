using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Model;
using PickUpSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PickUpButton : MonoBehaviour 
{
    //[SerializeField] private AudioSource audioSource;
    [SerializeField] private TMP_Text ItemName;
    [SerializeField] private Image ItemImage;
    [SerializeField] private TMP_Text ItemCount;

    private void Start()
    {
        Button button = GetComponent<Button>();
    }



    public void initializePickableButton(Item item)
    {
        this.ItemName.text = item.InventoryItem.Name;
        this.ItemImage.sprite = item.InventoryItem.ItemImage;
        this.ItemCount.text = "x" + item.count;
    }

    public void DestroyButton()
    {
        Destroy(gameObject);
    }
}