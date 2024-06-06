using System;
using System.Collections;
using System.Collections.Generic;
using DualityGame.Core;
using DualityGame.SaveSystem;
using UnityEngine;

namespace DualityGame.Player
{
    [RequireComponent(typeof(SpawnController))]
    public class DeathController : MonoBehaviour, IKillable
    {
        public static event Action<CauseOfDeath, WaitForCompletion> OnPlayerDied;
        public static event Action<CauseOfDeath, WaitForCompletion> AfterRespawn;


        [SerializeField] private GameSession _gameSession;
        
        [Header("Game states")]
        [SerializeField] private GameState _deathGameState;
        [SerializeField] private GameState _startingGameState;

        [Header("Death Screen VFX")]
        [SerializeField] private float _deathScreenDelay = 3f;

        [Header("Death Animations")]
        [SerializeField] private List<DeathAnimationEntry> _deathAnimations = new();
        
        public void Kill(CauseOfDeath causeOfDeath) => StartCoroutine(DeathScreen_CO(causeOfDeath));

        private IEnumerator DeathScreen_CO(CauseOfDeath causeOfDeath)
        {
            if (GameState.Current == _deathGameState) yield break; // Already dead
            var deathAnimationEntry = _deathAnimations.Find(entry => entry.CauseOfDeath == causeOfDeath);
            var wfc = new WaitForCompletion();

            _deathGameState.SetActive();
            deathAnimationEntry?.DeathAnimation.Trigger();
            OnPlayerDied?.Invoke(causeOfDeath, wfc);
            yield return wfc;

            yield return _gameSession.Respawn();
            deathAnimationEntry?.DeathAnimation.ResetPlayer();
            yield return new WaitForSeconds(_deathScreenDelay);

            AfterRespawn?.Invoke(causeOfDeath, wfc);
            yield return wfc;
            _startingGameState.SetActive();
        }

        [Serializable]
        private class DeathAnimationEntry
        {
            [field: SerializeField] public CauseOfDeath CauseOfDeath { get; private set; }
            [field: SerializeField] public DeathAnimationBase DeathAnimation { get; private set; }
        }
    }
}
