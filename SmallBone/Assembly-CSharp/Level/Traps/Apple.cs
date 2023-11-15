using System;
using Characters;
using Characters.Projectiles;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x0200064D RID: 1613
	public class Apple : DestructibleObject
	{
		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x0600206D RID: 8301 RVA: 0x000622B7 File Offset: 0x000604B7
		public override Collider2D collider
		{
			get
			{
				return this._collider;
			}
		}

		// Token: 0x0600206E RID: 8302 RVA: 0x000622C0 File Offset: 0x000604C0
		public override void Hit(Character from, ref Damage damage, Vector2 force)
		{
			if (damage.amount == 0.0)
			{
				return;
			}
			float direction = Mathf.Atan2(force.y, force.x) * 57.29578f + UnityEngine.Random.Range(-10f, 10f);
			float num = Mathf.Clamp(force.magnitude, 2f, 6f);
			this._projectile.reusable.Spawn(base.transform.position, true).GetComponent<Projectile>().Fire(from, num, direction, false, false, num, null, 0f);
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04001B7E RID: 7038
		[SerializeField]
		private Projectile _projectile;

		// Token: 0x04001B7F RID: 7039
		[SerializeField]
		private Collider2D _collider;
	}
}
