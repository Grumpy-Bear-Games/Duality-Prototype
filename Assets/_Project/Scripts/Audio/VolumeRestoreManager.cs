using System.Collections.Generic;
using UnityEngine;

namespace DualityGame.Audio
{
    public class VolumeRestoreManager : MonoBehaviour
    {
        [SerializeField] private List<VolumePreference> _volumePreferences = new();

        private void Start() => _volumePreferences.ForEach(v => v.RestoreVolume());
    }
}
