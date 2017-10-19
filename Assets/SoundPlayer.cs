using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SoundBoard
{
	public abstract class SoundPlayer : MonoBehaviour
	{
		public AudioClip Clip { get; protected set; }

		public string Path;
		public event Action<SoundPlayer> LoadingFinished;
		public event Action<float> LoadingProgress;
		public event Action<SoundPlayer> PlayFinished;

		public abstract string Description { get; }

		public void Load()
		{
			StartCoroutine(LoadCoroutine());
		}

		public void Play(AudioSource audioSource)
		{
			if (Clip == null)
			{
				Debug.LogError("[SoundPlayer] No sound to play.");
				DispatchPlayingFinished();
				return;
			}
			audioSource.clip = Clip;
			audioSource.Play();
			StartCoroutine(PlayCountdown(audioSource));
		}

		private IEnumerator PlayCountdown(AudioSource audioSource)
		{
			var endTime = Time.time + audioSource.clip.length;
			while (Time.time < endTime)
				yield return null;
			DispatchPlayingFinished();
		}

		protected virtual IEnumerator LoadCoroutine()
		{
			DispatchLoadingFinished();
			yield break;
		}

		protected void DispatchLoadingFinished()
		{
			if (LoadingFinished != null) LoadingFinished(this);
		}

		protected void DispatchPlayingFinished()
		{
			if (PlayFinished != null) PlayFinished(this);
		}

		protected void ReportProgress(float progress)
		{
			if (LoadingProgress != null) LoadingProgress(progress);
		}

		private void Awake()
		{
			var text = GetComponentInChildren<Text>();
			if (text != null) text.text = Description;
		}
	}
}