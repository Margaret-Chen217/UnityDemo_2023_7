using System.Collections;
using Inventory.Model;
using UnityEngine;

namespace PickUpSystem
{
    public class Item : MonoBehaviour
    {
        [field: SerializeField] public ItemSO InventoryItem { get; private set; }

        [field: SerializeField] public int count { get; set; } = 1;

        [SerializeField] private AudioSource audioSource;
    
        [SerializeField] private float duration = 0.3f;

        private void Start()
        {

        }

        public void DestroyItem()
        {
            GetComponent<Collider>().enabled = false;
            StartCoroutine(PickUpItem());
        }

        private IEnumerator PickUpItem()
        {
            //audioSource.Play();
            Vector3 startScale = transform.localScale;
            Vector3 endScale = Vector3.zero;
            float current = 0;
            while (current < duration)
            {
                current += Time.deltaTime;
                transform.localScale =
                    Vector3.Lerp(startScale, endScale, current / duration);
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}