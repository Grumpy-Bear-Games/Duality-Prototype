using UnityEngine;
using UnityEngine.Audio;

namespace DualityGame.Audio
{
    [CreateAssetMenu(menuName = "Duality/Audio/Volume Preference", fileName = "Volume Preference")]
    public sealed class VolumePreference: ScriptableObject
    {
        public const float MinDb  = -80f;
        //public const float MaxDb  =  20f;
        public const float MaxDb  =  0f;

        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private string _exposedParameter = "";
        [SerializeField] private string _playerPrefsKey = "Settings/Audio/SFXVolume";

        public float MinDB => MinDb;
        public float MaxDB => MaxDb;

        public void RestoreVolume()
        {
            if (_audioMixer == null) return;
            if (!HasPlayerPrefs) return;
            var volume = Mathf.Clamp(PlayerPrefs.GetFloat(_playerPrefsKey), MinDb, MaxDb);
            ApplyVolume(volume);
        }

        public float Volume
        {
            get
            {
                if (HasPlayerPrefs)
                {
                    var prefVolume = PlayerPrefs.GetFloat(_playerPrefsKey);
                    ApplyVolume(prefVolume);
                    return prefVolume;
                }

                if (_audioMixer == null || !_audioMixer.GetFloat(_exposedParameter, out var mixerVolume)) return 0;

                PlayerPrefs.SetFloat(_playerPrefsKey, mixerVolume);
                PlayerPrefs.Save();
                return mixerVolume;
            }
            set
            {
                var volume = Mathf.Clamp(value, MinDb, MaxDb);
                ApplyVolume(volume);
                PlayerPrefs.SetFloat(_playerPrefsKey, volume);
                PlayerPrefs.Save();
            }
        }

        private void ApplyVolume(float volume)
        {
            if (_audioMixer == null) return;
            _audioMixer.SetFloat(_exposedParameter, volume);
        }

        public void ClearPlayerPrefs() => PlayerPrefs.DeleteKey(_playerPrefsKey);

        public bool HasPlayerPrefs => PlayerPrefs.HasKey(_playerPrefsKey);

        #if UNITY_EDITOR
        public static class Fields
        {
            public const string AudioMixer = nameof(_audioMixer);
            public const string ExposedParameter = nameof(_exposedParameter);
            public const string PlayerPrefsKey = nameof(_playerPrefsKey);
        }
        #endif
    }
}
