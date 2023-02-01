using System.Collections;
using DualityGame.Core;
using DualityGame.VFX;
using UnityEngine;

namespace DualityGame.Player
{
    [RequireComponent(typeof(SpawnController))]
    public class DeathController : MonoBehaviour, IKillable
    {
        [Header("Game states")]
        [SerializeField] private GameState _deathGameState;
        [SerializeField] private GameState _startingGameState;

        [Header("Death Screen VFX")]
        [SerializeField] private ScreenFader _deathScreen;
        [SerializeField] private float _deathScreenDelay = 3f;
        
        private SpawnController _spawnController;
        private void Awake() => _spawnController = GetComponent<SpawnController>();

        public void Kill(CauseOfDeath causeOfDeath) => StartCoroutine(DeathScreen_CO(causeOfDeath));

        private IEnumerator DeathScreen_CO(CauseOfDeath causeOfDeath)
        {
            _deathGameState.SetActive();
            causeOfDeath.Trigger();
            yield return _deathScreen.Execute(ScreenFader.Direction.FadeOut);
            _spawnController.Respawn();
            yield return new WaitForSeconds(_deathScreenDelay);
            yield return _deathScreen.Execute(ScreenFader.Direction.FadeIn);
            _startingGameState.SetActive();
        }
    }
}
