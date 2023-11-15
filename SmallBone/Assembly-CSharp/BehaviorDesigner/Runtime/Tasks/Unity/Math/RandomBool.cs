using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
	// Token: 0x020015AF RID: 5551
	[TaskDescription("Sets a random bool value")]
	[TaskCategory("Unity/Math")]
	public class RandomBool : Action
	{
		// Token: 0x06006A91 RID: 27281 RVA: 0x00132435 File Offset: 0x00130635
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = (UnityEngine.Random.value < 0.5f);
			return TaskStatus.Success;
		}

		// Token: 0x0400566F RID: 22127
		[Tooltip("The variable to store the result")]
		public SharedBool storeResult;
	}
}
