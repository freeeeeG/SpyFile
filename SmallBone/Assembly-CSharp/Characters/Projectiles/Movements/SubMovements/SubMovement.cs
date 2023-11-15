using System;
using UnityEngine;

namespace Characters.Projectiles.Movements.SubMovements
{
	// Token: 0x020007DF RID: 2015
	public abstract class SubMovement : MonoBehaviour
	{
		// Token: 0x060028BB RID: 10427
		public abstract void Move(IProjectile projectile);
	}
}
