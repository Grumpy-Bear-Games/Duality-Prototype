using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Audio.UIElements
{
    [UxmlElement]
    public partial class VolumeSlider : Slider
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
                lowValue = _volumePreference.MinDB;
                highValue = _volumePreference.MaxDB;
                SetValueWithoutNotify(_volumePreference.Volume);
                SetEnabled(true);
            }
        }

        public VolumeSlider() : base()
        {
            lowValue = 0;
            highValue = 0;
            this.RegisterValueChangedCallback(SetVolume);
            RegisterCallback<GeometryChangedEvent>(UpdateOnShow);
            UpdateUI();
        }
        
        private void UpdateOnShow(GeometryChangedEvent evt)
        {
            if (evt.oldRect == Rect.zero && evt.newRect != Rect.zero) UpdateUI();
        }


        private void SetVolume(ChangeEvent<float> evt)
        {
            if (_volumePreference == null) return;
            _volumePreference.Volume = evt.newValue;
        }
    }
}
