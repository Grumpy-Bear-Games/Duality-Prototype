using System;
using System.Collections.Generic;
using DG.Tweening;
using DualityGame.Inventory;
using DualityGame.SaveSystem;
using UnityEngine;

namespace DualityGame.Iteractables
{
    [RequireComponent(typeof(SaveableEntity))]
    public class ItemSpawner : MonoBehaviour, IInteractable, ISaveableComponent
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private List<ItemPickup> _itemsToSpawn = new();

        [Header("Interaction prompt")]
        [SerializeField] private string _prompt;
        [SerializeField] private Vector3 _promptOffset;

        [Header("Spawn animation")]
        [SerializeField] private float _jumpPower = 1f;
        [SerializeField] private float _jumpDuration = 1f;
        [SerializeField] private Ease _jumpEasing = Ease.Linear;

        private bool _hasSpawned;

        private void Awake() => _itemsToSpawn.ForEach(itemToSpawn => itemToSpawn.gameObject.SetActive(false));

        public void Interact(GameObject actor)
        {
            if (_hasSpawned) return;
            
            _hasSpawned = true;
            var seq = DOTween.Sequence();
            
            _itemsToSpawn.ForEach(itemToSpawn =>
            {
                var spawnTarget = itemToSpawn.transform.position;
                itemToSpawn.transform.position = _spawnPoint.position;
                itemToSpawn.transform.localScale = Vector3.zero;
                itemToSpawn.gameObject.SetActive(true);
                seq.Insert(0f, itemToSpawn.transform
                    .DOJump(spawnTarget, _jumpPower, 1, _jumpDuration, false)
                    .SetEase(_jumpEasing));
                seq.Insert(0f, itemToSpawn.transform
                    .DOScale(Vector3.one, _jumpDuration)
                    .SetEase(_jumpEasing));
            });
        }

        public string Prompt => _hasSpawned ? null : _prompt;

        public Vector3 PromptPosition => transform.position + _promptOffset;


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            foreach (var item in _itemsToSpawn)
            {
                var itemCollider = item.GetComponent<Collider>();
                var bounds = itemCollider.bounds;
                Gizmos.DrawWireCube(bounds.center, bounds.size);
                Gizmos.DrawLine(transform.position, bounds.center);
            }
        }

        object ISaveableComponent.CaptureState() => _hasSpawned;

        void ISaveableComponent.RestoreState(object state) => _hasSpawned = (bool)state;
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (_spawnPoint == null)
            {
                _spawnPoint = transform;
            }
        }
        #endif
    }
}
