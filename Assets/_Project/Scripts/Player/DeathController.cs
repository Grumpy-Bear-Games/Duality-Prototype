using System;
using System.Collections;
using System.Collections.Generic;
using DualityGame.Core;
using DualityGame.SaveSystem;
using DualityGame.VFX;
using UnityEngine;

namespace DualityGame.Player
{
    [RequireComponent(typeof(SpawnController))]
    public class DeathController : MonoBehaviour, IKillable
    {
        [SerializeField] private GameSession _gameSession;
        
        [Header("Game states")]
        [SerializeField] private GameState _deathGameState;
        [SerializeField] private GameState _startingGameState;

        [Header("Death Screen VFX")]
        [SerializeField] private ScreenFader _deathScreen;
        [SerializeField] private float _deathScreenDelay = 3f;

        [Header("Death Animations")]
        [SerializeField] private List<DeathAnimationEntry> _deathAnimations = new();
        
        public void Kill(CauseOfDeath causeOfDeath) => StartCoroutine(DeathScreen_CO(causeOfDeath));

        private IEnumerator DeathScreen_CO(CauseOfDeath causeOfDeath)
        {
            if (GameState.Current == _deathGameState) yield break; // Already dead
            var deathAnimation = _deathAnimations.Find(entry => entry.CauseOfDeath == causeOfDeath);
            
            _deathGameState.SetActive();
            deathAnimation?.DeathAnimation.Trigger();
            causeOfDeath.Trigger();
            yield return _deathScreen.Execute(ScreenFader.Direction.FadeOut);
            deathAnimation?.DeathAnimation.ResetPlayer();
            yield return _gameSession.Respawn();
            yield return new WaitForSeconds(_deathScreenDelay);
            yield return _deathScreen.Execute(ScreenFader.Direction.FadeIn);
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
