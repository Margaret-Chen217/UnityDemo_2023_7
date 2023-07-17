using System;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model.ItemParameters;

namespace Inventory.Model
{
    /// <summary>
    /// 声明 ItemSO 数据类型
    /// </summary>
    // [CreateAssetMenu(fileName = "ItemSO", menuName = "ScriptableObject/ItemSO", order = 0)]
    public abstract class ItemSO : ScriptableObject
    {
        public int ID => GetInstanceID();

        [field: SerializeField] public string Name { get; set; }

        [field: SerializeField] public bool IsStackable { get; set; }
        [field: SerializeField] public int MaxStackSize { get; set; } = 1;

        [field: SerializeField] public GameObject ItemPrefab { get; set; }
        [field: SerializeField] public Sprite ItemImage { get; set; }

        [field: SerializeField]
        [field: TextArea]
        public string Description { get; set; }

        [field: SerializeField] public List<ItemParameter> DefaultParametersList { get; set; }

    }

    [Serializable]
    public struct ItemParameter : IEquatable<ItemParameter>
    {
        public ItemParameterSO itemParameter;
        public float value;

        public bool Equals(ItemParameter other)
        {
            return other.itemParameter == itemParameter;
        }
    }

    public interface IDestroyableItem
    {
    }


    public interface IItemAction
    {
        public string ActionName { get; }
        public AudioClip actionSFX { get; }
        bool PerformAction(GameObject character,out bool isSuccess, List<ItemParameter> itemState = null);
    }

    public interface IDropItem
    {
        bool Drop(GameObject character, ItemSO inventoryItemItem);
    }

}