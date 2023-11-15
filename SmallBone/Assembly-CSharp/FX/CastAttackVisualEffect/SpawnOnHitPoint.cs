using System;
using Characters;
using UnityEngine;

namespace FX.CastAttackVisualEffect
{
	// Token: 0x0200028D RID: 653
	public class SpawnOnHitPoint : CastAttackVisualEffect
	{
		// Token: 0x06000CB5 RID: 3253 RVA: 0x0002968A File Offset: 0x0002788A
		private void Awake()
		{
			if (this._critical.effect == null)
			{
				this._critical = this._normal;
			}
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x000296AB File Offset: 0x000278AB
		public override void Spawn(Vector3 position)
		{
			this._normal.Spawn(position, 0f, 1f);
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x000296C4 File Offset: 0x000278C4
		public override void Spawn(Character owner, Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit)
		{
			this._normal.Spawn(raycastHit.point, owner, 0f, 1f).transform.localScale = ((owner.lookingDirection == Character.LookingDirection.Right) ? Vector3.one : new Vector3(-1f, 1f, 1f));
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x00029720 File Offset: 0x00027920
		public override void Spawn(Character owner, Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Damage damage, ITarget target)
		{
			(damage.critical ? this._critical : this._normal).Spawn(raycastHit.point, owner, 0f, 1f);
		}

		// Token: 0x04000AE5 RID: 2789
		[SerializeField]
		private EffectInfo _normal;

		// Token: 0x04000AE6 RID: 2790
		[SerializeField]
		private EffectInfo _critical;
	}
}
