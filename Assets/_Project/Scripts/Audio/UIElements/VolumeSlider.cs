using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Audio.UIElements
{
    [UxmlElement]
    public sealed partial class VolumeSlider : SliderInt
    {
        private VolumePreference _volumePreference;

        [UxmlAttribute]
        public VolumePreference VolumePreference
        {
            get => _volumePreference;
            set
            {

                _volumePreference = value;
                UpdateUI();
            }
        }

        private static readonly float _minAmp = Mathf.Pow(10f, VolumePreference.MinDb / 20f);  // ~0.0001
        private static readonly float _maxAmp = Mathf.Pow(10f, VolumePreference.MaxDb / 20f);  // 10

        private static float Linear0To100ToDb(int linear)
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

        private static int DbToLinear0To100(float db)
        {
            // Convert dB to amplitude ratio
            // (dB = 20 * log10(amplitudeRatio)  =>  amplitudeRatio = 10^(dB/20))
            var amplitude = Mathf.Pow(10f, db / 20f);

            // Normalize amplitude to 0..1 range, then scale up to 0..100
            var t = (amplitude - _minAmp) / (_maxAmp - _minAmp);
            return Mathf.RoundToInt(t * 100f);
        }

        private void UpdateUI()
        {
            if (_volumePreference == null)
            {
                lowValue = 0;
                highValue = 0;
                SetValueWithoutNotify(0);
                SetEnabled(false);
            }
            else
            {
                lowValue = 0;
                highValue = 100;
                SetValueWithoutNotify(DbToLinear0To100(_volumePreference.Volume));
                SetEnabled(true);
            }
        }

        public VolumeSlider() : base()
        {
            pageSize = 10;
            this.RegisterValueChangedCallback(SetVolume);
            RegisterCallback<GeometryChangedEvent>(UpdateOnShow);
            UpdateUI();
        }
        
        private void UpdateOnShow(GeometryChangedEvent evt)
        {
            if (evt.oldRect == Rect.zero && evt.newRect != Rect.zero) UpdateUI();
        }

        private void SetVolume(ChangeEvent<int> evt)
        {
            if (_volumePreference == null) return;
            _volumePreference.Volume = Linear0To100ToDb(evt.newValue);
        }
    }
}
