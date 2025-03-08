using System;
using DualityGame.Audio;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

namespace DualityGame.Editor.Audio
{
    [CustomEditor(typeof(VolumePreference))]
    public class VolumePreferenceEditor : UnityEditor.Editor
    {
        private VolumePreference _volumePreference;
        private SerializedProperty _audioMixerProperty;
        private SerializedProperty _exposedParameterProperty;
        private SerializedProperty _playerPrefsKeyProperty;

        private PropertyField _audioMixerField;
        private PropertyField _exposedParameterField;
        private PropertyField _playerPrefsKeyField;
        private Slider _volumeSlider;
        private VisualElement _deleteFrame;

        private void OnEnable()
        {
            _volumePreference = target as VolumePreference;
            _audioMixerProperty = serializedObject.FindProperty(VolumePreference.Fields.AudioMixer);
            _exposedParameterProperty = serializedObject.FindProperty(VolumePreference.Fields.ExposedParameter);
            _playerPrefsKeyProperty = serializedObject.FindProperty(VolumePreference.Fields.PlayerPrefsKey);
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement
            {
                dataSource = _volumePreference
            };
            _audioMixerField = new PropertyField
            {
                bindingPath = VolumePreference.Fields.AudioMixer,
            };
            _audioMixerField.RegisterValueChangeCallback(_ => UpdateUI());
            root.Add(_audioMixerField);

            _exposedParameterField = new PropertyField
            {
                bindingPath = VolumePreference.Fields.ExposedParameter,
            };
            _exposedParameterField.RegisterValueChangeCallback(_ => UpdateUI());
            root.Add(_exposedParameterField);

            _playerPrefsKeyField = new PropertyField
            {
                bindingPath = VolumePreference.Fields.PlayerPrefsKey,
            };
            _playerPrefsKeyField.RegisterValueChangeCallback(_ => UpdateUI());
            root.Add(_playerPrefsKeyField);

            _volumeSlider = new Slider("Volume", VolumePreference.MinDb, VolumePreference.MaxDb, SliderDirection.Horizontal, 1)
            {
                showInputField = true
            };
            _volumeSlider.RegisterValueChangedCallback(SetVolume);
            root.Add(_volumeSlider);

            _deleteFrame = new VisualElement();
            _deleteFrame.Add(new HelpBox("Remember: This value is saved in PlayerPrefs; not in this object", HelpBoxMessageType.Info));

            var deleteButton = new Button(OnDeleteButtonClick)
            {
                text = "Delete"
            };
            _deleteFrame.Add(deleteButton);

            root.Add(_deleteFrame);
            UpdateUI();
            return root;
        }

        private void OnDeleteButtonClick()
        {
            _volumePreference.ClearPlayerPrefs();
            UpdateUI();
        }

        private void UpdateUI()
        {
            var validAudioMixer = _audioMixerProperty.objectReferenceValue is AudioMixer;
            var validPlayerPrefsKey = !string.IsNullOrEmpty(_playerPrefsKeyProperty.stringValue);

            _exposedParameterField.SetEnabled(validAudioMixer);
            _volumeSlider.SetEnabled(validAudioMixer && validPlayerPrefsKey);

            _deleteFrame.style.display = _volumePreference.HasPlayerPrefs ? DisplayStyle.Flex : DisplayStyle.None;
            _volumeSlider.SetValueWithoutNotify(_volumePreference.Volume);
        }

        private void SetVolume(ChangeEvent<float> evt)
        {
            _volumePreference.Volume = evt.newValue;
            UpdateUI();
        }
    }
}
