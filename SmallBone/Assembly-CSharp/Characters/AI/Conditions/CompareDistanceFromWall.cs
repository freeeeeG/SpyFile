using System;
using UnityEngine;

namespace Characters.AI.Conditions
{
	// Token: 0x020011C5 RID: 4549
	public class CompareDistanceFromWall : Condition
	{
		// Token: 0x0600597D RID: 22909 RVA: 0x0010A538 File Offset: 0x00108738
		protected override bool Check(AIController controller)
		{
			Collider2D lastStandingCollider = controller.character.movement.controller.collisionState.lastStandingCollider;
			if (lastStandingCollider == null)
			{
				return false;
			}
			Bounds bounds = lastStandingCollider.bounds;
			float num = (controller.character.transform.position.x > bounds.center.x) ? Mathf.Abs(bounds.max.x - controller.character.transform.position.x) : Mathf.Abs(bounds.min.x - controller.character.transform.position.x);
			CompareDistanceFromWall.Comparer compare = this._compare;
			if (compare != CompareDistanceFromWall.Comparer.GreaterThan)
			{
				return compare == CompareDistanceFromWall.Comparer.LessThan && num <= this._distanceFromWall;
			}
			return num >= this._distanceFromWall;
		}

		// Token: 0x04004843 RID: 18499
		[SerializeField]
		private CompareDistanceFromWall.Comparer _compare;

		// Token: 0x04004844 RID: 18500
		[SerializeField]
		private float _distanceFromWall;

		// Token: 0x020011C6 RID: 4550
		private enum Comparer
		{
			// Token: 0x04004846 RID: 18502
			GreaterThan,
			// Token: 0x04004847 RID: 18503
			LessThan
		}
	}
}
