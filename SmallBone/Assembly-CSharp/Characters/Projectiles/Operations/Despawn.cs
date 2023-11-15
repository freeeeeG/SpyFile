using System;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x02000777 RID: 1911
	public class Despawn : HitOperation
	{
		// Token: 0x06002765 RID: 10085 RVA: 0x0007636C File Offset: 0x0007456C
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit)
		{
			projectile.Despawn();
		}
	}
}
