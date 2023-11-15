using System;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Actions.Constraints
{
	// Token: 0x02000978 RID: 2424
	public class EnemyWithinRangeConstraint : Constraint
	{
		// Token: 0x06003427 RID: 13351 RVA: 0x0009A6F8 File Offset: 0x000988F8
		static EnemyWithinRangeConstraint()
		{
			EnemyWithinRangeConstraint._enemyOverlapper.contactFilter.SetLayerMask(1024);
		}

		// Token: 0x06003428 RID: 13352 RVA: 0x0009A720 File Offset: 0x00098920
		public override bool Pass()
		{
			bool result;
			using (new UsingCollider(this._searchRange))
			{
				result = (EnemyWithinRangeConstraint._enemyOverlapper.OverlapCollider(this._searchRange).results.Count > 0);
			}
			return result;
		}

		// Token: 0x04002A36 RID: 10806
		private static readonly NonAllocOverlapper _enemyOverlapper = new NonAllocOverlapper(1);

		// Token: 0x04002A37 RID: 10807
		[SerializeField]
		private Collider2D _searchRange;
	}
}
