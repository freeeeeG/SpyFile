using System;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x02000793 RID: 1939
	public class ScreenFlash : Operation
	{
		// Token: 0x060027B3 RID: 10163 RVA: 0x000773DE File Offset: 0x000755DE
		public override void Run(IProjectile projectile)
		{
			Singleton<ScreenFlashSpawner>.Instance.Spawn(this._info);
		}

		// Token: 0x040021C9 RID: 8649
		[SerializeField]
		private ScreenFlash.Info _info;
	}
}
