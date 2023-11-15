using System;
using Characters;
using Characters.Movements;
using UnityEngine;

namespace FX.SmashAttackVisualEffect
{
	// Token: 0x0200027A RID: 634
	public class SpawnOnHitPoint : SmashAttackVisualEffect
	{
		// Token: 0x06000C5A RID: 3162 RVA: 0x00021E46 File Offset: 0x00020046
		private void Awake()
		{
			if (this._critical.effect == null)
			{
				this._critical = this._normal;
			}
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x00021E68 File Offset: 0x00020068
		public override void Spawn(Character owner, Push push, RaycastHit2D raycastHit, Movement.CollisionDirection direction, Damage damage, ITarget target)
		{
			Vector3 zero = Vector3.zero;
			Vector3 min = owner.collider.bounds.min;
			Vector3 max = owner.collider.bounds.max;
			switch (direction)
			{
			case Movement.CollisionDirection.Above:
				zero.x = UnityEngine.Random.Range(min.x, max.x);
				zero.y = max.y;
				break;
			case Movement.CollisionDirection.Below:
				zero.x = UnityEngine.Random.Range(min.x, max.x);
				zero.y = min.y;
				break;
			case Movement.CollisionDirection.Left:
				zero.x = min.x;
				zero.y = UnityEngine.Random.Range(min.y, max.y);
				break;
			case Movement.CollisionDirection.Right:
				zero.x = max.x;
				zero.y = UnityEngine.Random.Range(min.y, max.y);
				break;
			}
			EffectInfo effectInfo = damage.critical ? this._critical : this._normal;
			float extraAngle = this._referSmashDirection ? (Mathf.Atan2(push.direction.y, push.direction.x) * 57.29578f) : 0f;
			Vector3 scale = (owner.lookingDirection == Character.LookingDirection.Right) ? Vector3.one : new Vector3(-1f, 1f, 1f);
			effectInfo.Spawn(zero, extraAngle, 1f).transform.localScale.Scale(scale);
		}

		// Token: 0x04000A61 RID: 2657
		[SerializeField]
		private bool _referSmashDirection;

		// Token: 0x04000A62 RID: 2658
		[SerializeField]
		private EffectInfo _normal;

		// Token: 0x04000A63 RID: 2659
		[SerializeField]
		private EffectInfo _critical;
	}
}
