using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(fileName = "WeaponItemSO", menuName = ("ScriptableObject/WeaponItemSO"))]
    public class WeaponItemSO : ItemSO, IItemAction, IDropItem, IDestroyableItem
    {
        public string ActionName => "Equip";

        public AudioClip actionSFX { get; private set; }

        private float yOffset = 0f;

        /// <summary>
        /// 为了在面版中不删除
        /// </summary>
        /// <param name="character"></param>
        /// <param name="itemState"></param>
        /// <returns></returns>
        public bool PerformAction(GameObject character, out bool isSuccess, List<ItemParameter> itemState = null)
        {
            AgentWeapon weaponSystem = character.GetComponent<AgentWeapon>();
            if (weaponSystem != null)
            {
                Debug.Log("Equip Weapon");
                //TODO: 装备武器
                weaponSystem.SetWeapon(this, itemState == null ? DefaultParametersList : itemState);
                //(success 表示是否执行成功)
                //返回值表示是否删除
                isSuccess = true;
                return false;
            }

            isSuccess = false;
            return false;
        }


        public bool Drop(Player player, ItemSO inventoryItem)
        {
            //TODO:需要判定是否正在使用
            WeaponItemSO weapon = player.gameObject.GetComponent<AgentWeapon>().weapon;
            if (weapon != null)
            {
                if (weapon == this)
                {
                    //该武器正在使用
                    return false;
                }
            }

            Vector3 playerPoint = player.gameObject.transform.position;
            Vector3 instantialPoint = new Vector3(playerPoint.x, playerPoint.y + 2, playerPoint.z);
            Instantiate(inventoryItem.ItemPrefab, instantialPoint, Quaternion.identity);
            return true;
        }
    }
}