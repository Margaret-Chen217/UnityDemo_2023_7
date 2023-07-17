using System;
using System.Collections.Generic;
using PickUpSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIInventoryPage : MonoBehaviour
    {
        [SerializeField] private UIInventoryItem itemPrefab;
        [SerializeField] private RectTransform contentPanel;
        [SerializeField] private UIInventoryDescription itemDescription;

        [SerializeField] private UIMouseFollower mouseFollower;
        public Button button;


        //存储所有实例化的item UI
        private List<UIInventoryItem> listOfItems = new List<UIInventoryItem>();

        //无拖拽选择
        private int currentDraggedItemIndex = -1;
        
        //当前选择
        public int currentSelectItemIndex = -1;

        public event Action<int>
            OnDescriptionRequested,
            OnItemActionRequested,
            OnStartDragging;

        public event Action<int, int> OnSwapItems;

        private void Awake()
        {
            //隐藏MyBag界面
            Hide();
            //隐藏拖拽鼠标
            mouseFollower.Toggle(false);
            //重置描述面板
            itemDescription.ResetDescription();
            

        }
        


        /// <summary>
        /// 初始化背包界面
        /// </summary>
        /// <param name="inventorySize"></param>
        public void InitializeInventoryUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                UIInventoryItem item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                item.transform.SetParent(contentPanel);
                listOfItems.Add(item);

                //
                item.OnItemBeginDrag += HandleBeginDrag;
                item.OnItemEndDrag += HandleEndDrag;
                item.OnItemDroppedOn += HandleSwap;
                item.OnLeftMouseClicked += HandleItemSelection;

            }
        }

        /// <summary>
        /// 根据list index 更新item数据
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <param name="itemImage"></param>
        /// <param name="itemCount"></param>
        public void UpdateData(int itemIndex, Sprite itemImage, int itemCount)
        {
            //判断当前要更新的item下标是否超出item总数
            if (listOfItems.Count > itemIndex)
            {
                listOfItems[itemIndex].SetData(itemImage, itemCount);
            }
        }

        private void HandleSwap(UIInventoryItem inventoryItemUI)
        {
            int index = listOfItems.IndexOf(inventoryItemUI);

            if (index == -1)
            {
                return;
            }

            OnSwapItems?.Invoke(currentDraggedItemIndex, index);
            HandleItemSelection(inventoryItemUI);
        }

        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            currentDraggedItemIndex = -1;
        }

        private void HandleEndDrag(UIInventoryItem inventoryItemUI)
        {
            ResetDraggedItem();
        }

        private void HandleBeginDrag(UIInventoryItem inventoryItemUI)
        {
            //获取点击item的index
            int index = listOfItems.IndexOf(inventoryItemUI);
            if (index == -1)
            {
                return;
            }

            currentDraggedItemIndex = index;
            HandleItemSelection(inventoryItemUI);
            OnStartDragging?.Invoke(index);
        }

        /// <summary>
        /// 设置浮动item
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="count"></param>
        public void CreateDraggedItem(Sprite sprite, int count)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, count);
        }

        public void HandleItemSelection(UIInventoryItem inventoryItemUI)
        {
            int index = listOfItems.IndexOf(inventoryItemUI);
            if (index == -1)
            {
                return;
            }
            this.currentSelectItemIndex = index;
            OnDescriptionRequested?.Invoke(index);
        }

        public void SelectItem(int selectItemIndex)
        {
            UIInventoryItem item = listOfItems[selectItemIndex];
            item.Select();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            itemDescription.ResetDescription();
            
            ResetSelection();
        }

        /// <summary>
        /// 重置desciption面板 并取消所有选择
        /// </summary>
        public void ResetSelection()
        {
            itemDescription.ResetDescription();
            DeselectAllItems();
        } 

        private void DeselectAllItems()
        {
            foreach (UIInventoryItem item in listOfItems)
            {
                item.Deselect();
            }
            currentSelectItemIndex = -1;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            ResetDraggedItem();
        }

        /// <summary>
        /// 更新description面板
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <param name="itemImage"></param>
        /// <param name="itemName"></param>
        /// <param name="description"></param>
        public void UpdateDescription(int itemIndex, Sprite itemImage, string itemName, string description)
        {
            itemDescription.SetDescription(itemImage, itemName, description);
            DeselectAllItems();
            listOfItems[itemIndex].Select();
            //TODO: 
            currentSelectItemIndex = itemIndex;
        }


        public void ResetAllItems()
        {
            foreach (var item in listOfItems)
            {
                item.ResetData();
                item.Deselect();
                //TODO: 
                currentSelectItemIndex = -1;
            }
        }
    }
}