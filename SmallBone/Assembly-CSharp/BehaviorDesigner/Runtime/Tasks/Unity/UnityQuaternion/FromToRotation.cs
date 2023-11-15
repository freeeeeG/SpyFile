using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion
{
	// Token: 0x0200156A RID: 5482
	[TaskDescription("Stores a rotation which rotates from the first direction to the second.")]
	[TaskCategory("Unity/Quaternion")]
	public class FromToRotation : Action
	{
		// Token: 0x060069B6 RID: 27062 RVA: 0x001302A6 File Offset: 0x0012E4A6
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Quaternion.FromToRotation(this.fromDirection.Value, this.toDirection.Value);
			return TaskStatus.Success;
		}

		// Token: 0x060069B7 RID: 27063 RVA: 0x001302D0 File Offset: 0x0012E4D0
		public override void OnReset()
		{
			this.fromDirection = (this.toDirection = Vector3.zero);
			this.storeResult = Quaternion.identity;
		}

		// Token: 0x0400557B RID: 21883
		[Tooltip("The from rotation")]
		public SharedVector3 fromDirection;

		// Token: 0x0400557C RID: 21884
		[Tooltip("The to rotation")]
		public SharedVector3 toDirection;

		// Token: 0x0400557D RID: 21885
		[RequiredField]
		[Tooltip("The stored result")]
		public SharedQuaternion storeResult;
	}
}
