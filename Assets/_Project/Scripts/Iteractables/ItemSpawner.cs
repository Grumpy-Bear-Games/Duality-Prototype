using System.Collections.Generic;
using DG.Tweening;
using DualityGame.SaveSystem;
using UnityEngine;

namespace DualityGame.Iteractables
{
    [RequireComponent(typeof(SaveableEntity))]
    public class ItemSpawner : MonoBehaviour, IInteractable, ISaveableComponent
    {
        [Header("Items to spawn")]
        [SerializeField] private List<ItemToSpawn> _itemsToSpawn = new();

        [Header("Interaction prompt")]
        [SerializeField] private string _prompt;
        [SerializeField] private Vector3 _promptOffset;

        [Header("Spawn animation")]
        [SerializeField] private float _jumpPower = 1f;
        [SerializeField] private float _jumpDuration = 1f;
        [SerializeField] private Ease _jumpEasing = Ease.Linear;

        private bool _hasSpawned;

        private void Awake() => _itemsToSpawn.ForEach(itemToSpawn => itemToSpawn.Item.gameObject.SetActive(false));

        public void Interact(GameObject actor)
        {
            if (_hasSpawned) return;
            
            _hasSpawned = true;
            var seq = DOTween.Sequence();
            
            _itemsToSpawn.ForEach(itemToSpawn =>
            {
                itemToSpawn.Item.gameObject.SetActive(true);
                var tween = itemToSpawn.Item.transform
                    .DOJump(itemToSpawn.SpawnTo.position, _jumpPower, 1, _jumpDuration, false)
                    .SetEase(_jumpEasing)
                    .OnComplete(() => itemToSpawn.Item.UpdateInitialPosition());
                seq.Insert(0f, tween);
            });
        }

        public string Prompt => _hasSpawned ? null : _prompt;

        public Vector3 PromptPosition => transform.position + _promptOffset;

        [System.Serializable]
        public class ItemToSpawn
        {
            public Inventory.Item Item;
            public Transform SpawnTo;
        }

        object ISaveableComponent.CaptureState() => _hasSpawned;

        void ISaveableComponent.RestoreState(object state) => _hasSpawned = (bool)state;
    }
}
