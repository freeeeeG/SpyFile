using System;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Fx
{
	// Token: 0x02000F19 RID: 3865
	public class SetInternalMusicVolume : Operation
	{
		// Token: 0x06004B6B RID: 19307 RVA: 0x000DDFB7 File Offset: 0x000DC1B7
		public override void Run()
		{
			if (this._easeTime > 0f)
			{
				PersistentSingleton<SoundManager>.Instance.SetInternalMusicVolume(this._volume, this._easeTime, this._easeCurve);
				return;
			}
			PersistentSingleton<SoundManager>.Instance.SetInternalMusicVolume(this._volume);
		}

		// Token: 0x06004B6C RID: 19308 RVA: 0x000DDFF3 File Offset: 0x000DC1F3
		public override void Stop()
		{
			PersistentSingleton<SoundManager>.Instance.ResetInternalMusicVolume();
		}

		// Token: 0x04003AA3 RID: 15011
		[SerializeField]
		private float _volume;

		// Token: 0x04003AA4 RID: 15012
		[SerializeField]
		private float _easeTime;

		// Token: 0x04003AA5 RID: 15013
		[SerializeField]
		private AnimationCurve _easeCurve;
	}
}
