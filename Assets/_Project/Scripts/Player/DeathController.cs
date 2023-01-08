using DualityGame.Core;
using UnityEngine;

namespace DualityGame.Player
{
    public class DeathController : MonoBehaviour, IKillable
    {
        public void Kill() => PlayState.Current.Set(PlayState.State.Death);
    }
}
