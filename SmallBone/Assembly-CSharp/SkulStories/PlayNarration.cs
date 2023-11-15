using System;
using System.Collections;
using FX;
using Singletons;
using UnityEngine;

namespace SkulStories
{
	// Token: 0x02000111 RID: 273
	public class PlayNarration : Sequence
	{
		// Token: 0x06000566 RID: 1382 RVA: 0x000107C1 File Offset: 0x0000E9C1
		public override IEnumerator CRun()
		{
			if (this._soundInfo.audioClip == null || !this._narration.sceneVisible)
			{
				yield break;
			}
			float length = this._soundInfo.length;
			if (this._soundInfo.length == 0f)
			{
				length = this._soundInfo.audioClip.length;
			}
			yield return this.CWaitForTime(0.5f);
			this.PlaySound();
			yield return this.CWaitForTime(length);
			yield break;
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x000107D0 File Offset: 0x0000E9D0
		private void PlaySound()
		{
			if (!this._narration.sceneVisible)
			{
				return;
			}
			PersistentSingleton<SoundManager>.Instance.PlayNarrationSound(this._soundInfo);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x000107F0 File Offset: 0x0000E9F0
		private IEnumerator CWaitForTime(float length)
		{
			float elapsed = 0f;
			while (length > elapsed)
			{
				elapsed += Chronometer.global.deltaTime;
				yield return null;
				if (this._narration.skipped || !this._narration.sceneVisible)
				{
					PersistentSingleton<SoundManager>.Instance.StopNarrationSound();
					base.StopAllCoroutines();
					break;
				}
			}
			yield break;
		}

		// Token: 0x0400041D RID: 1053
		[SerializeField]
		private SoundInfo _soundInfo;

		// Token: 0x0400041E RID: 1054
		private const float _delay = 0.5f;
	}
}
