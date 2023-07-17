using UnityEngine;

namespace Inventory.Model.ItemParameters
{
    [CreateAssetMenu(fileName = "ItemParameterSO", menuName = ("ScriptableObject/ItemParameter"))]
    public class ItemParameterSO : ScriptableObject{
    
        [field:SerializeField]
        public string ParameterName { get; private set; }
    }
}
