using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Febucci.UI;
using NodeCanvas.DialogueTrees;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DualityGame.Quests {

	public class DialogueUI : MonoBehaviour {

		[System.Serializable]
		public class SubtitleDelays
		{
			public float finalDelay     = 1.2f;
		}

		//Options...
		[Header("Input Options")]
		[SerializeField] public bool _skipOnInput;
		[SerializeField] public bool _waitForInput;
		[SerializeField] public InputActionReference _inputAction;

		//Group...
		[Header("Subtitles")]
		[SerializeField] public RectTransform _subtitlesGroup;
		[SerializeField] public TextAnimatorPlayer _actorSpeech;
		[SerializeField] public TMP_Text _actorName;
		[SerializeField] public Image _actorPortrait;
		[SerializeField] public RectTransform _waitInputIndicator;
		[SerializeField] public SubtitleDelays _subtitleDelays = new SubtitleDelays();
		[SerializeField] public List<AudioClip> _typingSounds;

		//Group...
		[Header("Multiple Choice")]
		[SerializeField] public RectTransform _optionsGroup;
		[SerializeField] public Button _optionButton;
		
		private Dictionary<Button, int> _cachedButtons;
		private Vector2 _originalSubsPosition;
		private bool _isWaitingChoice;

		private AudioSource _localSource;
		private bool _skip = false;
		private AudioSource localSource => _localSource != null ? _localSource : _localSource = gameObject.AddComponent<AudioSource>();

		private void OnEnable() {
			DialogueTree.OnDialogueStarted       += OnDialogueStarted;
			DialogueTree.OnDialoguePaused        += OnDialoguePaused;
			DialogueTree.OnDialogueFinished      += OnDialogueFinished;
			DialogueTree.OnSubtitlesRequest      += OnSubtitlesRequest;
			DialogueTree.OnMultipleChoiceRequest += OnMultipleChoiceRequest;
			_actorSpeech.onCharacterVisible.AddListener(PlayTypeSound);
			_inputAction.action.performed += Skip;
		}

		private void OnDisable() {
			DialogueTree.OnDialogueStarted       -= OnDialogueStarted;
			DialogueTree.OnDialoguePaused        -= OnDialoguePaused;
			DialogueTree.OnDialogueFinished      -= OnDialogueFinished;
			DialogueTree.OnSubtitlesRequest      -= OnSubtitlesRequest;
			DialogueTree.OnMultipleChoiceRequest -= OnMultipleChoiceRequest;
			_actorSpeech.onCharacterVisible.RemoveListener(PlayTypeSound);
			_inputAction.action.performed -= Skip;
		}

		private void Skip(InputAction.CallbackContext ctx) {
			if (ctx.phase == InputActionPhase.Performed) _skip = true;
		}

		private void Start(){
			_subtitlesGroup.gameObject.SetActive(false);
			_optionsGroup.gameObject.SetActive(false);
			_optionButton.gameObject.SetActive(false);
			_waitInputIndicator.gameObject.SetActive(false);
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
			
			_actorName.text = actor.name;
			_actorSpeech.textAnimator.tmproText.color = actor.dialogueColor;
			
			_actorPortrait.gameObject.SetActive( actor.portraitSprite != null );
			_actorPortrait.sprite = actor.portraitSprite;

			if (audio != null){
				var actorSource = actor.transform != null? actor.transform.GetComponent<AudioSource>() : null;
				var playSource = actorSource != null? actorSource : localSource;
				playSource.clip = audio;
				playSource.Play();
				_actorSpeech.useTypeWriter = false;
				_actorSpeech.ShowText(text);
				var timer = 0f;
				_skip = false;
				while (timer < audio.length)
				{
					if (_skipOnInput && _skip){
						playSource.Stop();
						break;
					}
					timer += Time.deltaTime;
					yield return null;
				}
			} else {
				var dialogShown = false;
				void OnTextShowed() => dialogShown = true;
				_actorSpeech.useTypeWriter = true;
				_actorSpeech.onTextShowed.AddListener(OnTextShowed);
				_actorSpeech.ShowText(text);

				_skip = false;
				while (!dialogShown) {
					if (_skipOnInput && _skip) {
						_actorSpeech.SkipTypewriter();
					}
					yield return null;
				}
				_actorSpeech.onTextShowed.RemoveListener(OnTextShowed);

				if (!_waitForInput) yield return new WaitForSeconds(_subtitleDelays.finalDelay);
			}

			if (_waitForInput)
			{
				_skip = false;
				_waitInputIndicator.gameObject.SetActive(true);
				while(!_skip) {
					yield return null;
				}
				_waitInputIndicator.gameObject.SetActive(false);
			}

			yield return null;
			_subtitlesGroup.gameObject.SetActive(false);
			info.Continue();
		}

		private void PlayTypeSound(char arg0)
		{
			if (_typingSounds.Count <= 0) return;
			var sound = _typingSounds[ Random.Range(0, _typingSounds.Count) ];
			if (sound != null){
				localSource.PlayOneShot(sound, Random.Range(0.6f, 1f));
			}
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

		IEnumerator CountDown(MultipleChoiceRequestInfo info){
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

		void Finalize(MultipleChoiceRequestInfo info, int index){
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

		void SetMassAlpha(RectTransform root, float alpha){
			foreach(var graphic in root.GetComponentsInChildren<CanvasRenderer>()){
				graphic.SetAlpha(alpha);
			}
		}
	}
}
