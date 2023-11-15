using System;
using Characters.Operations;
using Hardmode;
using Singletons;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x02000784 RID: 1924
	public sealed class InHardmode : HitOperation
	{
		// Token: 0x06002787 RID: 10119 RVA: 0x00076946 File Offset: 0x00074B46
		public void Awake()
		{
			this._inHardmode.Initialize();
			this._inNormal.Initialize();
		}

		// Token: 0x06002788 RID: 10120 RVA: 0x0007695E File Offset: 0x00074B5E
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit)
		{
			if (Singleton<HardmodeManager>.Instance.hardmode)
			{
				this._inHardmode.Run(projectile.owner);
				return;
			}
			this._inNormal.Run(projectile.owner);
		}

		// Token: 0x040021A5 RID: 8613
		[SerializeField]
		[HitOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _inHardmode;

		// Token: 0x040021A6 RID: 8614
		[HitOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation.Subcomponents _inNormal;
	}
}
