using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.DialogueTrees;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DualityGame.Quests {
	public class DialogueUI : MonoBehaviour
	{
		//Options...
		[Header("Input Options")]
		[SerializeField] private bool _skipOnInput;
		[SerializeField] private bool _waitForInput;
		[SerializeField] private InputActionReference _inputAction;

		//Group...
		[Header("Subtitles")]
		[SerializeField] private DialogUITalk _dialogUITalk;
		[SerializeField] private RectTransform _subtitlesGroup;
		[SerializeField] private DialogAudioPlayer _audioPlayer;
		[SerializeField] private DialogUIName _actorName;
		[SerializeField] private DialogUIPortrait _actorPortrait;
		[SerializeField] private DialogUIWaitForIndicator _waitForIndicator;

		//Group...
		[Header("Multiple Choice")]
		[SerializeField] private RectTransform _optionsGroup;
		[SerializeField] private Button _optionButton;
		
		private Dictionary<Button, int> _cachedButtons;
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
			_dialogUITalk.FastForward();
			_audioPlayer.FastForward();
		}

		private void Start(){
			_subtitlesGroup.gameObject.SetActive(false);
			_optionsGroup.gameObject.SetActive(false);
			_optionButton.gameObject.SetActive(false);
			_waitForIndicator.Hide();
			_originalSubsPosition = _subtitlesGroup.transform.position;
		}

		private void OnDialogueStarted(DialogueTree dlg){
			_inputAction.action.Enable();
		}

		private void OnDialoguePaused(DialogueTree dlg){
			_subtitlesGroup.gameObject.SetActive(false);
			_optionsGroup.gameObject.SetActive(false);
		}

		private void OnDialogueFinished(DialogueTree dlg){
			_subtitlesGroup.gameObject.SetActive(false);
			_optionsGroup.gameObject.SetActive(false);
			if (_cachedButtons == null) return;
			foreach (var tempBtn in _cachedButtons.Keys){
				if (tempBtn != null){
					Destroy(tempBtn.gameObject);
				}
			}
			_cachedButtons = null;
			_inputAction.action.Disable();
		}

		private void OnSubtitlesRequest(SubtitlesRequestInfo info) => StartCoroutine(Internal_OnSubtitlesRequestInfo(info));

		private IEnumerator Internal_OnSubtitlesRequestInfo(SubtitlesRequestInfo info){

			var text = info.statement.text;
			var audio = info.statement.audio;
			var actor = info.actor;

			_subtitlesGroup.gameObject.SetActive(true);
			
			_actorName.SetActor(actor);
			_actorPortrait.SetActor(actor);
			_dialogUITalk.SetActor(actor);

			if (audio != null){
				_audioPlayer.SetActor(actor);
				_dialogUITalk.ShowText(text);
				yield return _audioPlayer.Play(audio);
			} else {
				yield return _dialogUITalk.TypeText(text);
			}

			if (_waitForInput)
			{
				_fastForward = false;
				_waitForIndicator.Show();
				while(!_fastForward) {
					yield return null;
				}
				_waitForIndicator.Hide();
			}

			yield return null;
			_subtitlesGroup.gameObject.SetActive(false);
			info.Continue();
		}

		private void OnMultipleChoiceRequest(MultipleChoiceRequestInfo info){

			_optionsGroup.gameObject.SetActive(true);
			var buttonHeight = _optionButton.GetComponent<RectTransform>().rect.height;
			_optionsGroup.sizeDelta = new Vector2(_optionsGroup.sizeDelta.x, (info.options.Values.Count * buttonHeight) + 20);

			_cachedButtons = new Dictionary<Button, int>();
			var i = 0;

			foreach (var (key, value) in info.options){
				var btn = (Button)Instantiate(_optionButton);
				btn.gameObject.SetActive(true);
				btn.transform.SetParent(_optionsGroup.transform, false);
				btn.transform.localPosition = (Vector2)_optionButton.transform.localPosition - new Vector2(0, buttonHeight * i);
				btn.GetComponentInChildren<TMP_Text>().text = key.text;
				_cachedButtons.Add(btn, value);
				btn.onClick.AddListener( ()=> { Finalize(info, _cachedButtons[btn]);	});
				i++;
			}

			if (info.showLastStatement){
				_subtitlesGroup.gameObject.SetActive(true);
				var newY = _optionsGroup.position.y + _optionsGroup.sizeDelta.y + 1;
				_subtitlesGroup.position = new Vector2(_subtitlesGroup.position.x, newY);
			}

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
				SetMassAlpha(_optionsGroup, Mathf.Lerp(1, 0, timer/info.availableTime));
				yield return null;
			}
			
			if (_isWaitingChoice){
				Finalize(info, info.options.Values.Last());
			}
		}

		private void Finalize(MultipleChoiceRequestInfo info, int index){
			_isWaitingChoice = false;
			SetMassAlpha(_optionsGroup, 1f);
			_optionsGroup.gameObject.SetActive(false);
			if (info.showLastStatement){
				_subtitlesGroup.gameObject.SetActive(false);
				_subtitlesGroup.transform.position = _originalSubsPosition;
			}
			foreach (var tempBtn in _cachedButtons.Keys){
				Destroy(tempBtn.gameObject);
			}
			info.SelectOption(index);
		}

		private void SetMassAlpha(RectTransform root, float alpha){
			foreach(var graphic in root.GetComponentsInChildren<CanvasRenderer>()){
				graphic.SetAlpha(alpha);
			}
		}
	}
}
