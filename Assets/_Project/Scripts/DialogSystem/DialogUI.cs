using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DualityGame.DialogSystem.UI;
using NodeCanvas.DialogueTrees;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DualityGame.DialogSystem {
	public class DialogUI : MonoBehaviour
	{
		[SerializeField] private UnityEvent<IDialogueActor> _onStatementActorChange;
		[SerializeField] private UnityEvent<IDialogueActor> _onMultipleChoiceActorChange;

		//Options...
		[Header("Input Options")]
		[SerializeField] private bool _skipOnInput;
		[SerializeField] private bool _waitForInput;
		[SerializeField] private InputActionReference _inputAction;

		[Header("Dialog frame")]
		[SerializeField] private GameObject _dialogFrame;
		
		//Group...
		[Header("Statement")]
		[SerializeField] private GameObject _subtitlesGroup;
		[SerializeField] private StatementText _statementText;
		[SerializeField] private StatementAudioPlayer _statementAudioPlayer;
		[SerializeField] private WaitForInputIndicator _waitForInputIndicator;

		//Group...
		[Header("Multiple Choice")]
		[SerializeField] private GameObject _multipleChoicesFrame;
		[SerializeField] private CanvasGroup _optionsGroup;
		[SerializeField] private DialogChoice _optionButtonPrefab;
		
		private readonly List<DialogChoice> _optionButtons = new();
		private Vector2 _originalSubsPosition;
		private bool _isWaitingChoice;
		private bool _fastForward = false;
		
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
			_statementText.FastForward();
			_statementAudioPlayer.FastForward();
		}

		private void Start(){
			_dialogFrame.SetActive(false);
			_multipleChoicesFrame.SetActive(false);
			_optionButtonPrefab.gameObject.SetActive(false);
			_waitForInputIndicator.Hide();
		}

		private void OnDialogueStarted(DialogueTree dlg){
			_inputAction.action.Enable();
		}

		private void OnDialoguePaused(DialogueTree dlg){
			_dialogFrame.SetActive(false);
		}

		private void OnDialogueFinished(DialogueTree dlg){
			_dialogFrame.SetActive(false);
			CleanupChoiceButton();
			_inputAction.action.Disable();
		}

		private void CleanupChoiceButton()
		{
			foreach (var tempBtn in _optionButtons)
			{
				if (tempBtn == null) continue;
				Destroy(tempBtn.gameObject);
			}
			_optionButtons.Clear();
		}

		private void OnSubtitlesRequest(SubtitlesRequestInfo info) => StartCoroutine(Internal_OnSubtitlesRequestInfo(info));

		private IEnumerator Internal_OnSubtitlesRequestInfo(SubtitlesRequestInfo info){

			var text = info.statement.text;
			var audio = info.statement.audio;
			var actor = info.actor;

			_dialogFrame.SetActive(true);
			_onStatementActorChange.Invoke(actor);

			if (audio != null){
				_statementAudioPlayer.SetActor(actor);
				_statementText.ShowText(text);
				yield return _statementAudioPlayer.Play(audio);
			} else {
				yield return _statementText.TypeText(text);
			}

			if (_waitForInput)
			{
				_fastForward = false;
				_waitForInputIndicator.Show();
				while(!_fastForward) {
					yield return null;
				}
				_waitForInputIndicator.Hide();
			}

			yield return null;
			_dialogFrame.SetActive(false);
			info.Continue();
		}

		private void OnMultipleChoiceRequest(MultipleChoiceRequestInfo info) {
			_optionsGroup.gameObject.SetActive(true);
			_onMultipleChoiceActorChange.Invoke(info.actor);

			var idx = 1;
			foreach (var (statement, value) in info.options){
				var btn = Instantiate(_optionButtonPrefab, _optionsGroup.transform);
				btn.gameObject.SetActive(true);
				btn.Initialize(info.actor, $"{idx}.   {statement.text}", ()=> Finalize(info, value));
				_optionButtons.Add(btn);
				idx++;
			}

			_optionButtons[0].Select();

			_subtitlesGroup.gameObject.SetActive(info.showLastStatement);		
			_multipleChoicesFrame.SetActive(true);
			_dialogFrame.SetActive(true);

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
				_optionsGroup.alpha = Mathf.Lerp(1, 0, timer/info.availableTime);
				yield return null;
			}
			
			if (_isWaitingChoice){
				Finalize(info, info.options.Values.Last());
			}
		}

		private void Finalize(MultipleChoiceRequestInfo info, int index){
			_dialogFrame.SetActive(false);
			_subtitlesGroup.SetActive(true);
			_multipleChoicesFrame.SetActive(false);
			_isWaitingChoice = false;
			_optionsGroup.alpha = 1f;
			CleanupChoiceButton();
			info.SelectOption(index);
		}
	}
}
