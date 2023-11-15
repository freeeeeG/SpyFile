using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001C9 RID: 457
	public class PlaySoundAction : Action
	{
		// Token: 0x06000A3D RID: 2621 RVA: 0x00027F9E File Offset: 0x0002619E
		public override void Activate(GameObject target)
		{
			this.soundFX.Play(null);
		}

		// Token: 0x04000735 RID: 1845
		[SerializeField]
		private SoundEffectSO soundFX;
	}
}
