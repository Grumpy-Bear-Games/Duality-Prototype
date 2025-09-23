using DualityGame.Iteractables;
using UnityEngine;

namespace DualityGame.Player
{
    public class SimpleDoor : Interactable
    {
        [SerializeField] private SpawnPointReference _spawnPoint;
        [SerializeField] private SpawnSettings _spawnSettings;

        public override void Interact() => _spawnPoint.SpawnAt(_spawnSettings);

        public override IInteractable.InteractionType Type => IInteractable.InteractionType.Doorway;
    }
}
