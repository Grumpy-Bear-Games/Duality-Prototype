using UnityEngine;

namespace DualityGame.Player
{
    public class PuffDeath : DeathAnimationBase
    {
        [SerializeField] private ParticleSystem _particleSystem;

        public override void Trigger()
        {
            base.Trigger();
            _particleSystem.Play();
        }

        private void Reset()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }
    }
}
