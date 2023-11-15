using System;
using Level;
using UnityEngine;

namespace Characters.Projectiles.Operations.Customs
{
	// Token: 0x020007A3 RID: 1955
	public class DropCentauros : HitOperation
	{
		// Token: 0x060027F7 RID: 10231 RVA: 0x00078C88 File Offset: 0x00076E88
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit)
		{
			PoolObject poolObject = this._droppedCentauros.Spawn(raycastHit.point);
			if (raycastHit.normal.x >= 0f)
			{
				poolObject.transform.localScale = new Vector2(-1f, 1f);
				return;
			}
			poolObject.transform.localScale = new Vector2(1f, 1f);
		}

		// Token: 0x0400222A RID: 8746
		[SerializeField]
		private DroppedCentauros _droppedCentauros;
	}
}
