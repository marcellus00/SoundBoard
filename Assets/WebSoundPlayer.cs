using System.Collections;
using UnityEngine;

namespace SoundBoard
{
	public class WebSoundPlayer : SoundPlayer
	{
		public override string Description
		{
			get { return "Play sound from URL: " + Path; }
		}

		protected override IEnumerator LoadCoroutine()
		{
			var www = new WWW(Path);

			while (www.progress < 1)
			{
				ReportProgress(www.progress);
				yield return null;
			}

			Clip = www.GetAudioClip(false);
			DispatchLoadingFinished();

			if (!string.IsNullOrEmpty(www.error))
				Debug.LogError("[DiskSoundPlayer] URL failed: " + Path);
		}
	}
}
