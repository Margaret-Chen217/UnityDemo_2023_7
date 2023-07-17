using PickUpSystem;
using UnityEngine;

namespace Inventory.Model.ItemModifiers
{
    [CreateAssetMenu(fileName = "HealthModifier", menuName = ("ScriptableObject/HealthModifier"))]
    public class CharacterStatHealthModifierSO : CharacterStatModifierSO
    {
        /// <summary>
        /// 返回是否执行成功
        /// </summary>
        /// <param name="character"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public override bool AffectCharacter(GameObject character, float val)
        {
            Health health = character.GetComponent<Health>();
            if (health != null)
            {
                bool res = health.AddHealth((int)val);
                return res;
            }

            return false;
        }
    }
}