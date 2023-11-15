using System;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Gear.Quintessences.Constraints
{
	// Token: 0x020008F9 RID: 2297
	public sealed class EnemyWithinRangeConstraint : Constraint
	{
		// Token: 0x06003101 RID: 12545 RVA: 0x00092824 File Offset: 0x00090A24
		static EnemyWithinRangeConstraint()
		{
			EnemyWithinRangeConstraint._enemyOverlapper.contactFilter.SetLayerMask(1024);
		}

		// Token: 0x06003102 RID: 12546 RVA: 0x0009284C File Offset: 0x00090A4C
		public override bool Pass()
		{
			bool result;
			using (new UsingCollider(this._searchRange))
			{
				result = (EnemyWithinRangeConstraint._enemyOverlapper.OverlapCollider(this._searchRange).results.Count > 0);
			}
			return result;
		}

		// Token: 0x04002855 RID: 10325
		private static readonly NonAllocOverlapper _enemyOverlapper = new NonAllocOverlapper(1);

		// Token: 0x04002856 RID: 10326
		[SerializeField]
		private Collider2D _searchRange;
	}
}
