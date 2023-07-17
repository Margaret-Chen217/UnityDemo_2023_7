using UnityEngine;
using UnityEngine.UI;

namespace Rocker.UI
{
    public class UIRockerCenter : ScrollRect
    {
        protected float MRadius;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            MRadius = ((RectTransform)transform).sizeDelta.x * 0.5f;
        }

        public override void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
        {
            base.OnDrag(eventData);
            var contentPosition = this.content.anchoredPosition;
            if (contentPosition.magnitude > MRadius)
            {
                contentPosition = contentPosition.normalized * MRadius;
                SetContentAnchoredPosition(contentPosition);
            }
        }
    }
}