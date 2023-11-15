using System;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x02000791 RID: 1937
	public sealed class PlaySound : Operation
	{
		// Token: 0x060027AF RID: 10159 RVA: 0x00077387 File Offset: 0x00075587
		public override void Run(IProjectile projectile)
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._soundInfo, (this._position == null) ? base.transform.position : this._position.position);
		}

		// Token: 0x040021C6 RID: 8646
		[SerializeField]
		private SoundInfo _soundInfo;

		// Token: 0x040021C7 RID: 8647
		[SerializeField]
		private Transform _position;
	}
}
