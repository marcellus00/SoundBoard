using System.Collections;
using UnityEngine;

namespace SoundBoard
{
	public  class ResourceSoundPlayer : SoundPlayer
	{
		public override string Description
		{
			get { return "Play sound from resource: " + Path; }
		}

		protected override IEnumerator LoadCoroutine()
		{
			var request = Resources.LoadAsync<AudioClip>(Path);

			while (!request.isDone)
			{
				ReportProgress(request.progress);
				yield return null;
			}

			Clip = (AudioClip)request.asset;
			DispatchLoadingFinished();
			
			if (Clip == null)
				Debug.LogError("[DiskSoundPlayer] Cannot find recource: " + Path);
		}
	}
}
