using UnityEngine;

namespace Inventory.UI
{
    public class UIMouseFollower : MonoBehaviour
    {      
        [SerializeField] private Canvas canvas;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private UIInventoryItem item;

        public void Awake()
        {
            canvas = transform.root.GetComponent<Canvas>();
            mainCamera = Camera.main;
            item = GetComponentInChildren<UIInventoryItem>();
        }

        public void SetData(Sprite sprite, int count)
        {
            item.SetData(sprite, count);
        }

        private void Update()
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, Input.mousePosition,
                canvas.worldCamera, out position);
            transform.position = canvas.transform.TransformPoint(position);
        }

        public void Toggle(bool val)
        {
            //Debug.Log($"Item toggled {val}");
            item.gameObject.SetActive(val);
        }
    }
}
