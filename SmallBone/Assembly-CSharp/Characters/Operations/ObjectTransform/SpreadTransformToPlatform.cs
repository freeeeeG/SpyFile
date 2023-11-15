using System;
using UnityEngine;

namespace Characters.Operations.ObjectTransform
{
	// Token: 0x02000FC2 RID: 4034
	public sealed class SpreadTransformToPlatform : CharacterOperation
	{
		// Token: 0x06004E21 RID: 20001 RVA: 0x000E9C40 File Offset: 0x000E7E40
		public override void Run(Character owner)
		{
			Collider2D lastStandingCollider;
			if (this._lastStandingCollider)
			{
				lastStandingCollider = owner.movement.controller.collisionState.lastStandingCollider;
			}
			else
			{
				owner.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, this._belowRayDistance);
			}
			if (lastStandingCollider == null)
			{
				return;
			}
			float x = lastStandingCollider.bounds.size.x;
			this._target.transform.position = new Vector2(lastStandingCollider.bounds.center.x, lastStandingCollider.bounds.max.y);
			this._target.transform.localScale = new Vector2(x, this._target.transform.localScale.y);
		}

		// Token: 0x04003E27 RID: 15911
		[Information("부모를 Map으로", InformationAttribute.InformationType.Warning, true)]
		[SerializeField]
		private Transform _target;

		// Token: 0x04003E28 RID: 15912
		[SerializeField]
		private float _belowRayDistance = 100f;

		// Token: 0x04003E29 RID: 15913
		[SerializeField]
		private bool _lastStandingCollider;
	}
}
