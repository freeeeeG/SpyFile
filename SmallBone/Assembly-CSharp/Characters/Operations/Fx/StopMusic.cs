using System;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Fx
{
	// Token: 0x02000F22 RID: 3874
	public sealed class StopMusic : CharacterOperation
	{
		// Token: 0x06004B90 RID: 19344 RVA: 0x000DE748 File Offset: 0x000DC948
		public override void Run(Character owner)
		{
			PersistentSingleton<SoundManager>.Instance.FadeOutBackgroundMusic(this._fadeOutTime);
		}

		// Token: 0x04003AD3 RID: 15059
		[SerializeField]
		private float _fadeOutTime = 1f;
	}
}
