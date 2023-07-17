using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(fileName = "EdibleItemSO", menuName = ("ScriptableObject/EdibleItemSO"))]
    public class EdibleItemSO : ItemSO, IDestroyableItem, IItemAction, IDropItem
    {
        [SerializeField] private List<ModifireData> modifireData = new List<ModifireData>();
        public string ActionName => "Consume";
        public AudioClip actionSFX { get; private set; }


        public bool PerformAction(GameObject character, out bool isSuccess,  List<ItemParameter> itemState = null)
        {
            bool res = false;
            foreach (ModifireData data in modifireData)
            {
                res = data.statModifier.AffectCharacter(character, data.value);
            }

            isSuccess = res;
            return res;
        }


        public bool Drop(GameObject character, ItemSO inventoryItem)
        {
            Instantiate(inventoryItem.ItemPrefab);
            return true;
        }
    }



    [Serializable]
    public class ModifireData
    {
        public CharacterStatModifierSO statModifier;
        public float value;
    }
}
