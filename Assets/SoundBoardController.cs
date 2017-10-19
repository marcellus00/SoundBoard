using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoundBoard
{
	[RequireComponent(typeof(AudioSource))]
	public class SoundBoardController : MonoBehaviour
	{
		[SerializeField] protected ProgressBar _progressBar;
		[SerializeField] private Button _historyButton;

		private Button[] _buttons;
		private SoundPlayer _lastPlayer;

		private AudioSource _source;
		private readonly Queue<SoundPlayer> _queue = new Queue<SoundPlayer>();

		private bool _loopMode;

		public void Play(SoundPlayer soundPlayer)
		{
			if (_lastPlayer != null)
			{
				_lastPlayer.PlayFinished -= OnPlayFinished;
				_lastPlayer.LoadingFinished -= OnLoadingFinished;
				_lastPlayer.LoadingProgress -= OnProgress;

				
			}

			if (_lastPlayer != soundPlayer)
				Enqueue(soundPlayer);

			_lastPlayer = soundPlayer;
			ToogleButtons(false);

			soundPlayer.LoadingProgress += OnProgress;
			soundPlayer.LoadingFinished += OnLoadingFinished;
			soundPlayer.Load();
		}

		public void PlayHistory()
		{
			if (_queue.Count <= 0) return;
			ToogleButtons(_loopMode);
			if (_loopMode)
			{
				_loopMode = false;
				_source.Stop();
			}
			else
			{
				_loopMode = true;
				Play(_queue.Dequeue());
			}
		}

		private void ToogleButtons(bool on)
		{
			foreach (var button in _buttons)
				button.interactable = on;

			_historyButton.interactable = _loopMode || on;
		}

		private void OnProgress(float progress)
		{
			_progressBar.SetProgress(progress);
		}

		private void OnLoadingFinished(SoundPlayer soundPlayer)
		{
			soundPlayer.LoadingFinished -= OnLoadingFinished;
			_progressBar.SetProgress(1);

			if(!_loopMode)
				ToogleButtons(true);

			soundPlayer.PlayFinished += OnPlayFinished;
			soundPlayer.Play(_source);
		}

		private void OnPlayFinished(SoundPlayer soundPlayer)
		{
			soundPlayer.LoadingProgress -= OnProgress;
			soundPlayer.PlayFinished -= OnPlayFinished;
			if (_loopMode)
				Play(_queue.Dequeue());
		}

		private void Awake()
		{
			_source = GetComponent<AudioSource>();
			_buttons = FindObjectsOfType<Button>();
		}

		private void Enqueue(SoundPlayer player)
		{
			_queue.Enqueue(player);
			while (_queue.Count > 3)
				_queue.Dequeue();
		}
	}
}
