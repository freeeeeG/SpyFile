using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput
{
	// Token: 0x020015C9 RID: 5577
	[TaskDescription("Stores the acceleration value.")]
	[TaskCategory("Unity/Input")]
	public class GetAcceleration : Action
	{
		// Token: 0x06006AF0 RID: 27376 RVA: 0x00133065 File Offset: 0x00131265
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Input.acceleration;
			return TaskStatus.Success;
		}

		// Token: 0x06006AF1 RID: 27377 RVA: 0x00133078 File Offset: 0x00131278
		public override void OnReset()
		{
			this.storeResult = Vector3.zero;
		}

		// Token: 0x040056CA RID: 22218
		[RequiredField]
		[Tooltip("The stored result")]
		public SharedVector3 storeResult;
	}
}
