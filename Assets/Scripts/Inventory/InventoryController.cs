using System;
using System.Collections.Generic;
using System.Text;
using Inventory.Model;
using UnityEngine;
using Inventory.UI;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private UIInventoryPage inventoryUI;
        [SerializeField] private InventorySO inventoryData;
        [SerializeField] private Button bagButton;

        [SerializeField] private Button dropButton;
        [SerializeField] private Button useButton;

        public List<InventoryItem> initialItems = new List<InventoryItem>();

        private void Awake()
        {
            //对背包按钮添加监听
            bagButton.onClick.AddListener(delegate { OnButtonClicked(bagButton.gameObject); });

            //按钮添加监听
            dropButton.onClick.AddListener(delegate { OnButtonClicked(dropButton.gameObject); });
            useButton.onClick.AddListener(delegate { OnButtonClicked(useButton.gameObject); });
        }

        private void OnButtonClicked(GameObject buttonObject)
        {
            //点击背包按钮
            if (buttonObject == bagButton.gameObject)
            {
                if (inventoryUI.isActiveAndEnabled)
                {
                    inventoryUI.Hide();
                }
                else
                {
                    //显示inventoryUI前更新所有item数据
                    inventoryUI.Show();
                    foreach (var item in inventoryData.GetCurrentInventoryState())
                    {
                        inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.count);
                    }
                }
            }
            //点击丢弃按钮
            else if (buttonObject == dropButton.gameObject)
            {
                int itemIndex = inventoryUI.currentSelectItemIndex;
                if (itemIndex < 0)
                {
                    return;
                }

                InventoryItem inventoryItem = inventoryData.GetItemAt(inventoryUI.currentSelectItemIndex);
                if (inventoryItem.IsEmpty)
                {
                    return;
                }

                IDropItem dropItem = inventoryItem.item as IDropItem;
                if (dropItem != null)
                {
                    bool actionResult = dropItem.Drop(this.GetComponent<Player>(), inventoryItem.item);
                    if (actionResult)
                    {
                        UpdateInventoryUI(inventoryItem, itemIndex);
                    }
                }
            }
            //点击使用按钮
            else if (buttonObject == useButton.gameObject)
            {
                int itemIndex = inventoryUI.currentSelectItemIndex;
                if (itemIndex < 0)
                {
                    return;
                }

                InventoryItem inventoryItem = inventoryData.GetItemAt(inventoryUI.currentSelectItemIndex);
                if (inventoryItem.IsEmpty)
                {
                    return;
                }

                bool actionResult = false;
                bool isPerformSuccess = false;
                IItemAction itemAction = inventoryItem.item as IItemAction;
                if (itemAction != null)
                {
                    actionResult =
                        itemAction.PerformAction(this.gameObject, out isPerformSuccess, inventoryItem.itemState);
                }

                if (actionResult && isPerformSuccess)
                {
                    //因为WeaponItem没有实现这个接口，所以是null
                    UpdateInventoryUI(inventoryItem, itemIndex);
                }
            }
        }

        private void UpdateInventoryUI(InventoryItem inventoryItem, int itemIndex)
        {
            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                Debug.Log("not null");
                int remainer = inventoryData.RemoveItem(itemIndex, 1);

                if (remainer <= 0)
                {
                    inventoryUI.ResetSelection();
                }
                else
                {
                    //当前物品没有使用完，为了方便用户点击自动选择当前物品
                    inventoryUI.currentSelectItemIndex = itemIndex;
                    inventoryUI.SelectItem(itemIndex);
                }
            }
        }

        void Start()
        {
            PrepareUI();
            PrepareInventoryData();
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdatedInventoryUI;
            if (initialItems.Count > 0)
            {
                foreach (InventoryItem item in initialItems)
                {
                    if (item.IsEmpty)
                    {
                        continue;
                    }

                    //整理初始化数据
                    inventoryData.AddItem(item);
                }
            }
        }

        private void UpdatedInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.count);
            }
        }


        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);
            this.inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            this.inventoryUI.OnSwapItems += HandlwSwapItems;
            this.inventoryUI.OnStartDragging += HandleDragging;
            this.inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleItemActionRequest(int itemIndex)
        {
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            {
                if (inventoryItem.IsEmpty)
                {
                    return;
                }

                inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.count);
            }
        }

        private void HandlwSwapItems(int itemIndex1, int itemIndex2)
        {
            inventoryData.SwapItems(itemIndex1, itemIndex2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            //获取指定下标的item
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            //item为空则取消所有selection
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }

            //item不为空 将desciption更新为当前item的数据
            ItemSO item = inventoryItem.item;
            string description = PrepareDescription(inventoryItem);
            print(description);
            inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.name, description);
        }

        /// <summary>
        /// 根据itemState生成description
        /// </summary>
        /// <param name="inventoryItem"></param>
        /// <returns></returns>
        private string PrepareDescription(InventoryItem inventoryItem)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(inventoryItem.item.Description + "    ");
            for (int i = 0; i < inventoryItem.itemState.Count; i++)
            {
                stringBuilder.Append($"{inventoryItem.itemState[i].itemParameter.ParameterName}" +
                                     $":{inventoryItem.itemState[i].value}/" +
                                     $"{inventoryItem.item.DefaultParametersList[i].value}");
            }

            return stringBuilder.ToString();
        }
    }
}