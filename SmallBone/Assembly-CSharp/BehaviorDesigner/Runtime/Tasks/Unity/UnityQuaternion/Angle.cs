using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion
{
	// Token: 0x02001566 RID: 5478
	[TaskDescription("Stores the angle in degrees between two rotations.")]
	[TaskCategory("Unity/Quaternion")]
	public class Angle : Action
	{
		// Token: 0x060069AA RID: 27050 RVA: 0x0013014B File Offset: 0x0012E34B
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Quaternion.Angle(this.firstRotation.Value, this.secondRotation.Value);
			return TaskStatus.Success;
		}

		// Token: 0x060069AB RID: 27051 RVA: 0x00130174 File Offset: 0x0012E374
		public override void OnReset()
		{
			this.firstRotation = (this.secondRotation = Quaternion.identity);
			this.storeResult = 0f;
		}

		// Token: 0x04005570 RID: 21872
		[Tooltip("The first rotation")]
		public SharedQuaternion firstRotation;

		// Token: 0x04005571 RID: 21873
		[Tooltip("The second rotation")]
		public SharedQuaternion secondRotation;

		// Token: 0x04005572 RID: 21874
		[Tooltip("The stored result")]
		[RequiredField]
		public SharedFloat storeResult;
	}
}
