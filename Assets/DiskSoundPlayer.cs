using System.Collections;
using UnityEngine;

namespace SoundBoard
{
	public class DiskSoundPlayer : SoundPlayer
	{
		public override string Description
		{
			get { return "Play sound from disk path: " + Path; }
		}
		
		protected override IEnumerator LoadCoroutine()
		{
			if (!System.IO.File.Exists(Path))
			{
				Debug.LogError("[DiskSoundPlayer] File doesn't exist: " + Path);
				DispatchLoadingFinished();
				yield break;
			}

			var www = new WWW("file://" + Path);

			while (www.progress < 1)
			{
				ReportProgress(www.progress);
				yield return null;
			}

			Clip = www.GetAudioClip(false);
		}
	}
}
