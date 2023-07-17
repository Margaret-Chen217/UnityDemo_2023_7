using System.Collections;
using System.Collections.Generic;
using Inventory.Model;
using UnityEngine;

public class AgentWeapon : MonoBehaviour
{
    [SerializeField] public WeaponItemSO weapon;
    [SerializeField] private InventorySO inventoryData;
    [SerializeField] private List<ItemParameter> parametersToModify, itemCurrentState;

    public void SetWeapon(WeaponItemSO weaponItemSO, List<ItemParameter> itemState)
    {
        // Debug.Log("SetWeapon");
        // if (weapon != null)
        // {
        //     Debug.Log("!11111");
        //     inventoryData.AddItem(weapon, 1, itemCurrentState);
        // }

        this.weapon = weaponItemSO;
        this.itemCurrentState = new List<ItemParameter>(itemState);
        ModifyParameters();
    }

    private void ModifyParameters()
    {
        foreach (var parameter in parametersToModify)
        {
            if (itemCurrentState.Contains(parameter))
            {
                int index = itemCurrentState.IndexOf(parameter);
                float newValue = itemCurrentState[index].value + parameter.value;
                itemCurrentState[index] = new ItemParameter
                    { itemParameter = parameter.itemParameter, value = newValue };
            }
        }
    }
}