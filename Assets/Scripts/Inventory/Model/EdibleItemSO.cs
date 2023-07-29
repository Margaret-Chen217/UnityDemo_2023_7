using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(fileName = "EdibleItemSO", menuName = ("ScriptableObject/EdibleItemSO"))]
    public class EdibleItemSO : ItemSO, IDestroyableItem, IItemAction, IDropItem
    {
        [SerializeField] private List<ModifireData> modifireData = new List<ModifireData>();
        public string ActionName => "Consume";
        public AudioClip actionSFX { get; private set; }
        private float yOffset = 1f;


        public bool PerformAction(GameObject character, out bool isSuccess, List<ItemParameter> itemState = null)
        {
            bool res = false;
            foreach (ModifireData data in modifireData)
            {
                res = data.statModifier.AffectCharacter(character, data.value);
            }

            isSuccess = res;
            return res;
        }


        public bool Drop(Player player, ItemSO inventoryItem)
        {
            // Ray ray = new Ray(player.ColliderUtility.CapsuleColliderData.Collider.center, Vector3.down);
            // RaycastHit rayInfo;
            // Physics.Raycast(ray, out rayInfo, player.LayerData.GroundLayer);
            // Debug.DrawRay(player.ColliderUtility.CapsuleColliderData.Collider.center, Vector3.down, Color.green);
            //
            // Vector3 instantialPoint = new Vector3(rayInfo.point.x, rayInfo.point.y + yOffset, rayInfo.point.z);
            //
            // Debug.Log("Character Poing: " + player.ColliderUtility.CapsuleColliderData.Collider.center);
            // Debug.Log("Instantial Point :" + instantialPoint);

            Vector3 playerPoint = player.gameObject.transform.position;
            Vector3 instantialPoint = new Vector3(playerPoint.x, playerPoint.y + 1, playerPoint.z);
            Instantiate(inventoryItem.ItemPrefab, instantialPoint, quaternion.identity);
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