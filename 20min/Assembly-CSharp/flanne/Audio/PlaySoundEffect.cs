using System;
using UnityEngine;

namespace flanne.Audio
{
	// Token: 0x02000240 RID: 576
	public class PlaySoundEffect : MonoBehaviour
	{
		// Token: 0x06000C9F RID: 3231 RVA: 0x0002E129 File Offset: 0x0002C329
		public void Play()
		{
			SoundEffectSO soundEffectSO = this.soundFX;
			if (soundEffectSO == null)
			{
				return;
			}
			soundEffectSO.Play(null);
		}

		// Token: 0x040008D6 RID: 2262
		[SerializeField]
		private SoundEffectSO soundFX;
	}
}
