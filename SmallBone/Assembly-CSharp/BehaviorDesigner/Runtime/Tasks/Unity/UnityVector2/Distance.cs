using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
	// Token: 0x020014F2 RID: 5362
	[TaskCategory("Unity/Vector2")]
	[TaskDescription("Returns the distance between two Vector2s.")]
	public class Distance : Action
	{
		// Token: 0x06006815 RID: 26645 RVA: 0x0012CCD0 File Offset: 0x0012AED0
		public override TaskStatus OnUpdate()
		{
			if (!this._compareOnlyX)
			{
				this.storeResult.Value = Vector2.Distance(this.firstVector2.Value, this.secondVector2.Value);
			}
			else
			{
				this.storeResult.Value = Mathf.Abs(this.firstVector2.Value.x - this.secondVector2.Value.x);
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006816 RID: 26646 RVA: 0x0012CD3F File Offset: 0x0012AF3F
		public override void OnReset()
		{
			this.firstVector2 = Vector2.zero;
			this.secondVector2 = Vector2.zero;
			this.storeResult = 0f;
		}

		// Token: 0x04005404 RID: 21508
		[Tooltip("The first Vector2")]
		public SharedVector2 firstVector2;

		// Token: 0x04005405 RID: 21509
		[Tooltip("The second Vector2")]
		public SharedVector2 secondVector2;

		// Token: 0x04005406 RID: 21510
		[RequiredField]
		[Tooltip("The distance")]
		public SharedFloat storeResult;

		// Token: 0x04005407 RID: 21511
		[SerializeField]
		private bool _compareOnlyX;
	}
}
