using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014A9 RID: 5289
	[TaskDescription("벽과 캐릭터의 거리 비교")]
	public sealed class CompareFromWall : Conditional
	{
		// Token: 0x06006716 RID: 26390 RVA: 0x0012A404 File Offset: 0x00128604
		public override TaskStatus OnUpdate()
		{
			Character value = this._character.Value;
			Collider2D lastStandingCollider = value.movement.controller.collisionState.lastStandingCollider;
			if (lastStandingCollider == null)
			{
				return TaskStatus.Failure;
			}
			Bounds bounds = lastStandingCollider.bounds;
			float num = (value.transform.position.x > bounds.center.x) ? Mathf.Abs(bounds.max.x - value.transform.position.x) : Mathf.Abs(bounds.min.x - value.transform.position.x);
			float value2 = this._distance.Value;
			CompareFromWall.Comparer comparer = this._comparer;
			if (comparer != CompareFromWall.Comparer.GreaterThan)
			{
				if (comparer != CompareFromWall.Comparer.LessThan)
				{
					return TaskStatus.Failure;
				}
				if (num <= value2)
				{
					return TaskStatus.Success;
				}
				return TaskStatus.Failure;
			}
			else
			{
				if (num >= value2)
				{
					return TaskStatus.Success;
				}
				return TaskStatus.Failure;
			}
		}

		// Token: 0x0400530A RID: 21258
		[SerializeField]
		private SharedCharacter _character;

		// Token: 0x0400530B RID: 21259
		[SerializeField]
		private SharedFloat _distance;

		// Token: 0x0400530C RID: 21260
		[SerializeField]
		private CompareFromWall.Comparer _comparer;

		// Token: 0x020014AA RID: 5290
		private enum Comparer
		{
			// Token: 0x0400530E RID: 21262
			GreaterThan,
			// Token: 0x0400530F RID: 21263
			LessThan
		}
	}
}
