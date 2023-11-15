using System;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x02000776 RID: 1910
	public class ClearBounceHistory : Operation
	{
		// Token: 0x06002763 RID: 10083 RVA: 0x0007635E File Offset: 0x0007455E
		public override void Run(IProjectile projectile)
		{
			this._bounce.lastCollision = null;
		}

		// Token: 0x0400217C RID: 8572
		[SerializeField]
		private Bounce _bounce;
	}
}
