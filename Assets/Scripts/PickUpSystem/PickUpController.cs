using System;
using System.Collections.Generic;
using Inventory.Model;
using UnityEngine;
using UnityEngine.UI;

namespace PickUpSystem
{
    public class PickUpController : MonoBehaviour
    {
        [SerializeField] private InventorySO inventoryData;
        [SerializeField] private PickUpButton itemButtonPrefab;
        [SerializeField] private GameObject buttonContainer;
        private Dictionary<Item, PickUpButton> itemsButtonDictionary;
        private TriggerDetection detectTrigger;

        private void Start()
        {
            itemsButtonDictionary = new Dictionary<Item, PickUpButton>();

            detectTrigger = transform.Find("DetectCollider").GetComponent<TriggerDetection>();
            detectTrigger.OnCollisionEnter_Action += detectTrigger_OnCollisionEnter;
            detectTrigger.OnCollisionExit_Action += detectTrigger_OnCollisionExit;
        }

        private void detectTrigger_OnCollisionExit(Collider other)
        {
            Item item = other.GetComponent<Item>();
            if (item != null)
            {
                if (itemsButtonDictionary.ContainsKey(item))
                {
                    RemovePickableButton(item);
                }
            }
        }

        private void detectTrigger_OnCollisionEnter(Collider other)
        {
            Item item = other.GetComponent<Item>();
            if (item != null)
            {
                PickUpButton pickUpButton = Instantiate(itemButtonPrefab, buttonContainer.transform, true);
                pickUpButton.initializePickableButton(item);
                pickUpButton.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClicked(item); });
                if (pickUpButton != null)
                {
                    itemsButtonDictionary.Add(item, pickUpButton);
                }
            }
        }
        

        private void OnButtonClicked(Item item)
        {
            int reminder = inventoryData.AddItem(item.InventoryItem, item.count);
            if (reminder == 0)
            {
                item.DestroyItem();
                RemovePickableButton(item);
            }
            else
            {
                item.count = reminder;
            }
        }
        

        private void RemovePickableButton(Item item)
        {
            PickUpButton pickUpButton;
            itemsButtonDictionary.TryGetValue(item, out pickUpButton);
            pickUpButton.DestroyButton();
            itemsButtonDictionary.Remove(item);
        }
    }
}