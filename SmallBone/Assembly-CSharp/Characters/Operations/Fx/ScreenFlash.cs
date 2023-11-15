using System;
using FX;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Fx
{
	// Token: 0x02000F18 RID: 3864
	public class ScreenFlash : CharacterOperation
	{
		// Token: 0x06004B68 RID: 19304 RVA: 0x000DDF7C File Offset: 0x000DC17C
		public override void Run(Character owner)
		{
			this._instance = Singleton<ScreenFlashSpawner>.Instance.Spawn(this._info);
		}

		// Token: 0x06004B69 RID: 19305 RVA: 0x000DDF94 File Offset: 0x000DC194
		public override void Stop()
		{
			if (Service.quitting)
			{
				return;
			}
			if (this._instance != null)
			{
				this._instance.FadeOut();
			}
		}

		// Token: 0x04003AA1 RID: 15009
		[SerializeField]
		private ScreenFlash.Info _info;

		// Token: 0x04003AA2 RID: 15010
		private ScreenFlash _instance;
	}
}
