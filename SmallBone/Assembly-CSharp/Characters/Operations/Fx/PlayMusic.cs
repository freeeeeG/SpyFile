using System;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Fx
{
	// Token: 0x02000F15 RID: 3861
	public sealed class PlayMusic : CharacterOperation
	{
		// Token: 0x06004B61 RID: 19297 RVA: 0x000DDE3C File Offset: 0x000DC03C
		public override void Run(Character owner)
		{
			PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(this._audioClipInfo);
		}

		// Token: 0x04003A9A RID: 15002
		[SerializeField]
		private MusicInfo _audioClipInfo;
	}
}
