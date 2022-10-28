using System.Collections;
using System.Linq;
using NodeCanvas.DialogueTrees;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace DualityGame.DialogSystem.UI {
	public class DialogUI : MonoBehaviour
	{
		//Options...
		[Header("Input Options")]
		[SerializeField] private bool _skipOnInput;
		[SerializeField] private bool _waitForInput;
		[SerializeField] private InputActionReference _inputAction;

		//Group...
		[Header("Statement")]
		[SerializeField] private StatementAudioPlayer _statementAudioPlayer;
		[SerializeField] private VisualTreeAsset _optionTemplate;

		
		private bool _isWaitingChoice;
		private bool _fastForward = false;

		private VisualElement _dialogFrame;
		private VisualElement _actorPortrait;
		private Label _statementText;
		private Label _actorName;
		private VisualElement _waitForInputIndicator;
		private VisualElement _multipleChoice;
		private VisualElement _options;

		private void Awake()
		{
			var uiDocument = GetComponent<UIDocument>();
			_dialogFrame = uiDocument.rootVisualElement.Q<VisualElement>("DialogFrame");
			_actorPortrait = uiDocument.rootVisualElement.Q<VisualElement>("PortraitFrame");
			_statementText = uiDocument.rootVisualElement.Q<Label>("StatementText");
			_actorName = uiDocument.rootVisualElement.Q<Label>("ActorName");
			_waitForInputIndicator = uiDocument.rootVisualElement.Q<VisualElement>("WaitForInput");
			_multipleChoice = uiDocument.rootVisualElement.Q<VisualElement>("MultipleChoice");
			_options = uiDocument.rootVisualElement.Q<VisualElement>("Options");
			
			_dialogFrame.AddToClassList("Hidden");
			_multipleChoice.AddToClassList("Hidden");
			_waitForInputIndicator.AddToClassList("Hidden");
		}

		private void OnEnable() {
			DialogueTree.OnDialogueStarted       += OnDialogueStarted;
			DialogueTree.OnDialoguePaused        += OnDialoguePaused;
			DialogueTree.OnDialogueFinished      += OnDialogueFinished;
			DialogueTree.OnSubtitlesRequest      += OnSubtitlesRequest;
			DialogueTree.OnMultipleChoiceRequest += OnMultipleChoiceRequest;
			_inputAction.action.performed += FastForward;
		}

		private void OnDisable() {
			DialogueTree.OnDialogueStarted       -= OnDialogueStarted;
			DialogueTree.OnDialoguePaused        -= OnDialoguePaused;
			DialogueTree.OnDialogueFinished      -= OnDialogueFinished;
			DialogueTree.OnSubtitlesRequest      -= OnSubtitlesRequest;
			DialogueTree.OnMultipleChoiceRequest -= OnMultipleChoiceRequest;
			_inputAction.action.performed -= FastForward;
		}

		private void FastForward(InputAction.CallbackContext ctx)
		{
			if (ctx.phase != InputActionPhase.Performed) return;
			_fastForward = true;
			if (!_skipOnInput) return;
			_statementAudioPlayer.FastForward();
		}

		private void OnDialogueStarted(DialogueTree dlg){
			_inputAction.action.Enable();
			_dialogFrame.RemoveFromClassList("Hidden");
		}

		private void OnDialoguePaused(DialogueTree dlg){
			_dialogFrame.AddToClassList("Hidden");
		}

		private void OnDialogueFinished(DialogueTree dlg){
			_dialogFrame.AddToClassList("Hidden");
			CleanupChoiceButton();
			_inputAction.action.Disable();
		}

		private void CleanupChoiceButton() => _options.Clear();

		private void OnSubtitlesRequest(SubtitlesRequestInfo info) => StartCoroutine(Internal_OnSubtitlesRequestInfo(info));

		private void SetActor(IDialogueActor actor)
		{
			_actorPortrait.style.backgroundImage = actor.portrait ? new StyleBackground(actor.portrait) : new StyleBackground(StyleKeyword.Initial);
			_actorName.text = actor.name;
		}

		private IEnumerator Internal_OnSubtitlesRequestInfo(SubtitlesRequestInfo info){

			var text = info.statement.text;
			var audio = info.statement.audio;
			var actor = info.actor;

			SetActor(actor);

			_statementText.text = text;
			if (audio != null){
				_statementAudioPlayer.SetActor(actor);
				yield return _statementAudioPlayer.Play(audio);
			}

			if (_waitForInput)
			{
				_fastForward = false;
				yield return new WaitForSeconds(0.5f);
				_waitForInputIndicator.RemoveFromClassList("Hidden");
				while(!_fastForward) {
					yield return null;
				}
				_waitForInputIndicator.AddToClassList("Hidden");
			}

			yield return null;
			
			info.Continue();
		}

		private void OnMultipleChoiceRequest(MultipleChoiceRequestInfo info)
		{
			CleanupChoiceButton();
			
			var idx = 1;
			foreach (var (statement, value) in info.options)
			{
				var option = _optionTemplate.Instantiate();
				var button = option.contentContainer.Q<Button>("Option");
				button.text = $"{idx}.   {statement.text}";
				button.RegisterCallback<ClickEvent>(_ => Finalize(info, value));
				_options.Add(option);
				idx++;
			}

			//_subtitlesGroup.gameObject.SetActive(info.showLastStatement);
			_multipleChoice.RemoveFromClassList("Hidden");

			if (info.availableTime > 0){
				StartCoroutine(CountDown(info));
			}
		}

		private IEnumerator CountDown(MultipleChoiceRequestInfo info){
			_isWaitingChoice = true;
			var timer = 0f;
			while (timer < info.availableTime){
				if (_isWaitingChoice == false){
					yield break;
				}
				timer += Time.deltaTime;
				_options.style.opacity = Mathf.Lerp(1, 0, timer/info.availableTime);
				yield return null;
			}
			
			if (_isWaitingChoice){
				Finalize(info, info.options.Values.Last());
			}
		}

		private void Finalize(MultipleChoiceRequestInfo info, int index){
			_multipleChoice.AddToClassList("Hidden");
			_isWaitingChoice = false;
			_options.style.opacity = new StyleFloat(StyleKeyword.Initial);
			CleanupChoiceButton();
			info.SelectOption(index);
		}
	}
}
