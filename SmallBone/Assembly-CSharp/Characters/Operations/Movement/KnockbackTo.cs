using System;
using UnityEngine;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E5B RID: 3675
	public class KnockbackTo : TargetedCharacterOperation
	{
		// Token: 0x060048F8 RID: 18680 RVA: 0x000D4CC4 File Offset: 0x000D2EC4
		public override void Run(Character owner, Character target)
		{
			if (target == null || !target.liveAndActive)
			{
				return;
			}
			Vector2 a;
			if (this._targetPlace != null)
			{
				a = MMMaths.RandomPointWithinBounds(this._targetPlace.bounds);
			}
			else
			{
				a = this._targetPoint.position;
			}
			Vector2 force = a - target.transform.position;
			target.movement.push.ApplyKnockback(owner, force, this._curve, this._ignoreOtherForce, this._expireOnGround);
		}

		// Token: 0x04003812 RID: 14354
		[Header("Destination")]
		[SerializeField]
		private Collider2D _targetPlace;

		// Token: 0x04003813 RID: 14355
		[SerializeField]
		private Transform _targetPoint;

		// Token: 0x04003814 RID: 14356
		[SerializeField]
		[Header("Force")]
		private Curve _curve;

		// Token: 0x04003815 RID: 14357
		[SerializeField]
		private bool _ignoreOtherForce = true;

		// Token: 0x04003816 RID: 14358
		[SerializeField]
		private bool _expireOnGround;
	}
}
