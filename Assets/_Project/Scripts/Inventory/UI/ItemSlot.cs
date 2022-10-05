using System;
using UnityEngine;
using UnityEngine.UI;

namespace DualityGame.Inventory.UI
{
    [RequireComponent(typeof(Button))]
    public class ItemSlot : MonoBehaviour
    {
        [SerializeField] private Image _image;
        
        public event Action OnClicked;
        
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => OnClicked?.Invoke());
        }


        public void SetSelected(bool selected)
        {
            // Something visual should happen here
            Debug.Log(selected ? "Selecting" : "Deselecting");
        }

        public void SetItem(Item item)
        {
            _image.sprite = item != null ? item.Type.InventorySprite : null;
            _image.enabled = item != null;
        }

        public void Clear()
        {
            _image.sprite = null;
            _image.enabled = false;
        }
    }
}
