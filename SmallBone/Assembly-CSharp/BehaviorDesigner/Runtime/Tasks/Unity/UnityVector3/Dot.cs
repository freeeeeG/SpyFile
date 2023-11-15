using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
	// Token: 0x020014E0 RID: 5344
	[TaskCategory("Unity/Vector3")]
	[TaskDescription("Stores the dot product of two Vector3 values.")]
	public class Dot : Action
	{
		// Token: 0x060067E2 RID: 26594 RVA: 0x0012C5F7 File Offset: 0x0012A7F7
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector3.Dot(this.leftHandSide.Value, this.rightHandSide.Value);
			return TaskStatus.Success;
		}

		// Token: 0x060067E3 RID: 26595 RVA: 0x0012C620 File Offset: 0x0012A820
		public override void OnReset()
		{
			this.leftHandSide = Vector3.zero;
			this.rightHandSide = Vector3.zero;
			this.storeResult = 0f;
		}

		// Token: 0x040053D1 RID: 21457
		[Tooltip("The left hand side of the dot product")]
		public SharedVector3 leftHandSide;

		// Token: 0x040053D2 RID: 21458
		[Tooltip("The right hand side of the dot product")]
		public SharedVector3 rightHandSide;

		// Token: 0x040053D3 RID: 21459
		[RequiredField]
		[Tooltip("The dot product result")]
		public SharedFloat storeResult;
	}
}
