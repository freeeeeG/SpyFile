using System;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x0200076A RID: 1898
	public class ActivateObject : HitOperation
	{
		// Token: 0x06002747 RID: 10055 RVA: 0x00075E34 File Offset: 0x00074034
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit)
		{
			Vector2 point = raycastHit.point;
			UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate<GameObject>(this._target, point, Quaternion.identity), 10f);
		}

		// Token: 0x04002168 RID: 8552
		[SerializeField]
		private GameObject _target;
	}
}
