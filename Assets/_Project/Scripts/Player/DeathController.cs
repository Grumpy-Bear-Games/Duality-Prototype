using System.Collections;
using DualityGame.Core;
using DualityGame.Inventory;
using DualityGame.VFX;
using UnityEngine;

namespace DualityGame.Player
{
    [RequireComponent(typeof(SpawnController))]
    [RequireComponent(typeof(InventoryController))]
    public class DeathController : MonoBehaviour, IKillable
    {
        [Header("VFX")]
        [SerializeField] private ScreenFader _fadeVFX;

        private SpawnController _spawnController;
        private InventoryController _inventoryController;
        
        public void Kill() => StartCoroutine(_fadeVFX.Wrap(CO_Kill()));

        private IEnumerator CO_Kill()
        {
            _inventoryController.DropInventory();
            _spawnController.Respawn();
            yield return new WaitForSeconds(3f);
        }
        
        private void Awake()
        {
            _spawnController = GetComponent<SpawnController>();
            _inventoryController = GetComponent<InventoryController>();
        }
    }
}
