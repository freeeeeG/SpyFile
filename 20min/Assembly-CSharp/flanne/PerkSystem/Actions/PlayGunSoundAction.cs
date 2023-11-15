using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001C8 RID: 456
	public class PlayGunSoundAction : Action
	{
		// Token: 0x06000A3A RID: 2618 RVA: 0x00027F6E File Offset: 0x0002616E
		public override void Init()
		{
			this.myGun = PlayerController.Instance.gun;
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x00027F80 File Offset: 0x00026180
		public override void Activate(GameObject target)
		{
			SoundEffectSO gunshotSFX = this.myGun.gunData.gunshotSFX;
			if (gunshotSFX == null)
			{
				return;
			}
			gunshotSFX.Play(null);
		}

		// Token: 0x04000734 RID: 1844
		[NonSerialized]
		private Gun myGun;
	}
}
