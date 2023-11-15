using System;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Actions.Constraints
{
	// Token: 0x02000977 RID: 2423
	public class EnemyStatusWithinRangeConstraint : Constraint
	{
		// Token: 0x06003424 RID: 13348 RVA: 0x0009A603 File Offset: 0x00098803
		static EnemyStatusWithinRangeConstraint()
		{
			EnemyStatusWithinRangeConstraint._enemyOverlapper.contactFilter.SetLayerMask(1024);
		}

		// Token: 0x06003425 RID: 13349 RVA: 0x0009A62C File Offset: 0x0009882C
		public override bool Pass()
		{
			bool result;
			using (new UsingCollider(this._searchRange))
			{
				ReadonlyBoundedList<Collider2D> results = EnemyStatusWithinRangeConstraint._enemyOverlapper.OverlapCollider(this._searchRange).results;
				for (int i = 0; i < results.Count; i++)
				{
					Target component = results[i].GetComponent<Target>();
					if (!(component == null) && !(component.character == null) && !(component.character.status == null) && component.character.status.freezed && component.character.status.IsApplying(this._status))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x04002A33 RID: 10803
		private static readonly NonAllocOverlapper _enemyOverlapper = new NonAllocOverlapper(32);

		// Token: 0x04002A34 RID: 10804
		[SerializeField]
		private CharacterStatus.Kind _status;

		// Token: 0x04002A35 RID: 10805
		[SerializeField]
		private Collider2D _searchRange;
	}
}
