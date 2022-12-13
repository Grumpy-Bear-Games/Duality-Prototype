using System;
using System.Collections;
using System.Linq;
using DualityGame.Dialog;
using NodeCanvas.DialogueTrees;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace DualityGame.DialogSystem.UI {
	public class DialogUI : MonoBehaviour
	{
		[Header("Statement")]
		[SerializeField] private StatementAudioPlayer _statementAudioPlayer;

		private VisualElement _dialogFrame;
		private VisualElement _actorPortrait;
		private Label _statementText;
		private Label _actorName;
		private VisualElement _options;

		private void Awake()
		{
			var uiDocument = GetComponent<UIDocument>();
			
			_dialogFrame = uiDocument.rootVisualElement.Q<VisualElement>("DialogFrame");
			_actorPortrait = uiDocument.rootVisualElement.Q<VisualElement>("PortraitFrame");
			_statementText = uiDocument.rootVisualElement.Q<Label>("StatementText");
			_actorName = uiDocument.rootVisualElement.Q<Label>("ActorName");
			_options = uiDocument.rootVisualElement.Q<VisualElement>("Options");

			Hide();
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

		private void OnDialogueStarted(DialogueTree dlg) => Show();

		private void Hide() => _dialogFrame.AddToClassList("Hidden");

		private void Show() => _dialogFrame.RemoveFromClassList("Hidden");
		
		public void SelectOption(int value)
		{
			var dialogOptions = _options.Query<DialogOption>().ToList();
			if (value > dialogOptions.Count) return;
			dialogOptions[value - 1].SelectOption();
		}

		private void OnDialoguePaused(DialogueTree dlg) => Hide();

		private void OnDialogueFinished(DialogueTree dlg)
		{
			Hide();
			CleanupChoiceButton();
		}

		private void CleanupChoiceButton() => _options.Clear();

		private void OnSubtitlesRequest(SubtitlesRequestInfo info) => StartCoroutine(Internal_OnSubtitlesRequestInfo(info));

		private void SetActor(IDialogueActor actor, Mood mood)
		{
			var portrait = actor.PortraitByMood(mood);
			_actorPortrait.style.backgroundImage = portrait ? new StyleBackground(portrait) : new StyleBackground(StyleKeyword.Initial);
			_actorName.text = actor.Name;
		}

		private IEnumerator Internal_OnSubtitlesRequestInfo(SubtitlesRequestInfo info){

			var text = info.statement.Text;
			var audio = info.statement.Audio;
			var actor = info.actor;

			SetActor(actor, info.statement.Mood);

			_statementText.text = text;
			if (audio != null){
				yield return _statementAudioPlayer.Play(audio);
			}

			CleanupChoiceButton();
			CreateOption("(Continue)", ()=>
			{
				CleanupChoiceButton();
				info.Continue();
			});
		}

		private void OnMultipleChoiceRequest(MultipleChoiceRequestInfo info)
		{
			SetActor(info.actor, info.statement.Mood);
			_statementText.text = info.statement.Text;
			
			// TODO: Play audio
			
			CleanupChoiceButton();
			
			foreach (var (statement, value) in info.options)
			{
				CreateOption(statement.Text, () => Finalize(info, value));
			}
		}

		private void CreateOption(string text, Action onClick) => _options.Add(new DialogOption($"{_options.childCount + 1}.   {text}", onClick));

		private void Finalize(MultipleChoiceRequestInfo info, int index){
			_options.style.opacity = new StyleFloat(StyleKeyword.Initial);
			CleanupChoiceButton();
			info.SelectOption(index);
		}
	}
}
