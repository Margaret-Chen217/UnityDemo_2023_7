using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory.Model
{
    /// <summary>
    /// 声明 InventorySO 数据类型
    /// </summary>
    [CreateAssetMenu(fileName = "InventorySO", menuName = "ScriptableObject/InventorySO", order = 0)]
    public class InventorySO : ScriptableObject
    {
        [SerializeField] private List<InventoryItem> inventoryItems;

        [field: SerializeField] public int Size { get; private set; } = 10;
        private InventorySO val = null;

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;

        /// <summary>
        /// 初始化 InventoryItem List
        /// </summary>
        public void Initialize()
        {
            inventoryItems = new List<InventoryItem>();
            for (int i = 0; i < Size; i++)
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }

        /// <summary>
        /// 在 InventorySO 数据中 添加 Item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="count"></param>
        public int AddItem(ItemSO item, int count, List<ItemParameter> itemState = null)
        {
            //不可堆叠item
            if (item.IsStackable == false)
            {
                //逐个item添加 
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    while (count > 0 && IsInventoryFull() == false)
                    {
                        count -= AddItemToFirstFreeSlot(item, 1, itemState);
                    }

                    InformAboutChange();
                    return count;
                }
            }

            //可堆叠item
            count = AddStackableItem(item, count);
            InformAboutChange();
            return count;
        }

        /// <summary>
        /// 背包中第一个空slot添加物品
        /// </summary>
        /// <param name="item"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private int  AddItemToFirstFreeSlot(ItemSO item, int count, List<ItemParameter> itemState = null)
        {
            InventoryItem newItem = new InventoryItem()
            {
                item = item,
                count = count,
                //这里添加防止对于每个可堆叠项目重复添加
                itemState = new List<ItemParameter>(itemState == null? item.DefaultParametersList: itemState)
            };
            //遍历背包寻找空位
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i] = newItem;
                    return count;
                }
            }

            //背包没有空位
            return 0;
        }

        /// <summary>
        /// 判断背包是否已满
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private bool IsInventoryFull() => inventoryItems.Where(item => item.IsEmpty).Any() == false;


        /// <summary>
        /// 背包中加入可堆叠的item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private int AddStackableItem(ItemSO item, int count)
        {
            //遍历所有item
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                //跳过空item
                if (inventoryItems[i].IsEmpty)
                {
                    continue;
                }

                //判断非空是否和待加入item ID一致
                if (inventoryItems[i].item.ID == item.ID)
                {
                    //当前空位可加入的item个数
                    int amountPossibleToTake = inventoryItems[i].item.MaxStackSize - inventoryItems[i].count;

                    //要放入的大于当前空位
                    if (count > amountPossibleToTake)
                    {
                        //更改当前空位物品个数
                        inventoryItems[i] = inventoryItems[i].ChangeCount(inventoryItems[i].item.MaxStackSize);
                        //修改待放入背包物品个数
                        count -= amountPossibleToTake;
                    }
                    else
                    {
                        //全部放入当前空位
                        inventoryItems[i] = inventoryItems[i].ChangeCount(inventoryItems[i].count + count);
                        InformAboutChange();
                        return 0;
                    }
                }
            }
            
            //将剩余的item加入到新的空位中
            while (count > 0 && IsInventoryFull() == false)
            {
                int newCount = Math.Clamp(count, 0, item.MaxStackSize);
                count -= newCount;
                AddItemToFirstFreeSlot(item, newCount);
            }
            return count;
        }

        public void AddItem(InventoryItem item)
        {
            AddItem(item.item, item.count);
        }

        /// <summary>
        /// 获取当前 Inventory 状态
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    continue;
                }

                returnValue[i] = inventoryItems[i];
            }

            return returnValue;
        }

        /// <summary>
        /// 获取指定下标的 InventoryItem
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <returns></returns>
        public InventoryItem GetItemAt(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }

        public void SwapItems(int itemIndex1, int itemIndex2)
        {
            InventoryItem item1 = inventoryItems[itemIndex1];
            inventoryItems[itemIndex1] = inventoryItems[itemIndex2];
            inventoryItems[itemIndex2] = item1;
            InformAboutChange();
        }

        private void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }

        public int RemoveItem(int itemIndex, int amount)
        {
            if (inventoryItems.Count > itemIndex)
            {
                if (inventoryItems[itemIndex].IsEmpty)
                {
                    return amount;
                }

                int reminder = inventoryItems[itemIndex].count - amount;
                if (reminder <= 0)
                {
                    inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
                    
                }
                else
                {
                    inventoryItems[itemIndex] = inventoryItems[itemIndex].ChangeCount(reminder);
                }
                InformAboutChange();
                return reminder;

            }
            return amount;
        }
    }

    /// <summary>
    /// 声明 InventoryItem 结构体
    /// </summary>
    [Serializable]
    public struct InventoryItem
    {
        public int count;
        public ItemSO item;
        //
        public List<ItemParameter> itemState;
        public bool IsEmpty => item == null;

        public InventoryItem ChangeCount(int newCount)
        {
            return new InventoryItem()
            {
                item = this.item,
                count = newCount,
                //
                itemState = new List<ItemParameter>(this.itemState)
            };
        }

        public static InventoryItem GetEmptyItem()
            => new InventoryItem()
            {
                item = null,
                count = 0,
                itemState =new List<ItemParameter>()
            };
    }
}