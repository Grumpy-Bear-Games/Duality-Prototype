using Codice.Client.BaseCommands.Merge;
using UnityEngine;
using UnityEngine.Audio;

namespace DualityGame.Audio
{
    [CreateAssetMenu(menuName = "Duality/Audio/Volume Preference", fileName = "Volume Preference")]
    public sealed class VolumePreference: ScriptableObject
    {
        public const float MinDb  = -80f;
        public const float MaxDb  =  20f;

        /*
        private static readonly float _minAmp = Mathf.Pow(10f, MinDb / 20f);  // ~0.0001
        private static readonly float _maxAmp = Mathf.Pow(10f, MaxDb / 20f);  // 10
        public static float Linear0To100ToDb(float linear)
        {
            // First, normalize the 0..100 to a 0..1 parameter
            var t = Mathf.Clamp01(linear / 100f);

            // Convert 0..1 back into amplitude
            var amplitude = Mathf.Lerp(_minAmp, _maxAmp, t);

            // Finally, convert amplitude ratio to dB
            // dB = 20 * log10(amplitudeRatio)
            var db = 20f * Mathf.Log10(amplitude);

            return db;
        }

        public static float DbToLinear0To100(float db)
        {
            // Convert dB to amplitude ratio
            // (dB = 20 * log10(amplitudeRatio)  =>  amplitudeRatio = 10^(dB/20))
            var amplitude = Mathf.Pow(10f, db / 20f);

            // Normalize amplitude to 0..1 range, then scale up to 0..100
            var t = (amplitude - _minAmp) / (_maxAmp - _minAmp);
            return t * 100f;
        }
        */

        [SerializeField] private AudioMixer _audioMixer = null;
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
