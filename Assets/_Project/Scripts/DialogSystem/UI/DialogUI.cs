﻿using System;
using System.Collections;
using DualityGame.Utilities;
using NodeCanvas.DialogueTrees;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace DualityGame.DialogSystem.UI {
	public class DialogUI : MonoBehaviour
	{
		[Header("Statement")]
		[SerializeField] private StatementAudioPlayer _statementAudioPlayer;
		
		[Header("Events")]
		[FormerlySerializedAs("_onShow")][SerializeField] private UnityEvent _onDialogBegin;
		[FormerlySerializedAs("_onHide")][SerializeField] private UnityEvent _onDialogEnd;
		
		private VisualElement _dialogFrame;
		private VisualElement _actorPortrait;
		private Label _statementText;
		private Label _actorName;
		private VisualElement _options;

		private void Awake()
		{
			var root = GetComponent<UIDocument>().rootVisualElement;
			
			_dialogFrame = root.Q<VisualElement>("DialogFrame");
			_actorPortrait = root.Q<VisualElement>("PortraitFrame");
			_statementText = root.Q<Label>("StatementText");
			_actorName = root.Q<Label>("ActorName");
			_options = root.Q<VisualElement>("Options");
			
			_dialogFrame.PreventLoosingFocus();
		}

		private void OnEnable() {
			DialogueTree.OnDialogueStarted       += OnDialogueStarted;
			DialogueTree.OnDialoguePaused        += OnDialoguePaused;
			DialogueTree.OnDialogueFinished      += OnDialogueFinished;
			DialogueTree.OnSubtitlesRequest      += OnSubtitlesRequest;
			DialogueTree.OnMultipleChoiceRequest += OnMultipleChoiceRequest;
		}

		private void OnDisable() {
			DialogueTree.OnDialogueStarted       -= OnDialogueStarted;
			DialogueTree.OnDialoguePaused        -= OnDialoguePaused;
			DialogueTree.OnDialogueFinished      -= OnDialogueFinished;
			DialogueTree.OnSubtitlesRequest      -= OnSubtitlesRequest;
			DialogueTree.OnMultipleChoiceRequest -= OnMultipleChoiceRequest;
		}

		private bool _dialogJustStarted;

		private void OnDialogueStarted(DialogueTree dlg)
		{
			// Workaround to avoid bug in the DialogueTree
			_dialogJustStarted = true;
		}

		private void OnDialoguePaused(DialogueTree dlg)
		{
			Hide();
			CleanupChoiceButton();
		}

		private void OnDialogueFinished(DialogueTree dlg)
		{
			Hide();
			CleanupChoiceButton();
			_onDialogEnd.Invoke();
		}
		
		private void Hide() => _dialogFrame.RemoveFromClassList("Shown");

		private void Show() => _dialogFrame.AddToClassList("Shown");

		private void CleanupChoiceButton() => _options.Clear();

		private void OnSubtitlesRequest(SubtitlesRequestInfo info) => StartCoroutine(Internal_OnSubtitlesRequestInfo(info));

		private void SetActor(IDialogueActor actor, Mood mood)
		{
			var portrait = actor.PortraitByMood(mood);
			_actorPortrait.style.backgroundImage = portrait ? new StyleBackground(portrait) : new StyleBackground(StyleKeyword.Initial);
			_actorName.text = actor.name;
		}

		private IEnumerator Internal_OnSubtitlesRequestInfo(SubtitlesRequestInfo info)
		{
			// Skip one frame to avoid double-triggering in the Input system
			yield return null;

			// Workaround to avoid bug in the DialogueTree
			if (_dialogJustStarted)
			{
				_dialogJustStarted = false;
				_onDialogBegin.Invoke();
			}

			Show();
			
			var text = info.statement.text;
			var audio = info.statement.audio;
			var actor = info.actor;

			SetActor(actor, info.statement.mood);

			_statementText.text = text;
			if (audio != null){
				yield return _statementAudioPlayer.Play(audio);
			}

			CleanupChoiceButton();
			CreateOption("Continue", info.Continue);
			FocusFirstOption();
		}

		private void FocusFirstOption() => _options[0].Focus();

		private void OnMultipleChoiceRequest(MultipleChoiceRequestInfo info)
		{
			// Workaround to avoid bug in the DialogueTree
			if (_dialogJustStarted)
			{
				_dialogJustStarted = false;
				_onDialogBegin.Invoke();
			}

			Show();
			SetActor(info.actor, info.statement.mood);
			_statementText.text = info.statement.text;
			
			// TODO: Play audio
			
			CleanupChoiceButton();
			
			foreach (var (statement, value) in info.options)
			{
				CreateOption($"- {statement.text}", () => info.SelectOption(value));
			}
			FocusFirstOption();
		}

		private void CreateOption(string text, Action onClick)
		{
			var button = new Button(onClick)
			{
				text = text,
			};
			_options.Add(button);
		}
	}
}
