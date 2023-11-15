using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
	// Token: 0x020015D8 RID: 5592
	[TaskCategory("Unity/GameObject")]
	[TaskDescription("Returns Success if the layermasks match, otherwise Failure.")]
	public class CompareLayerMask : Conditional
	{
		// Token: 0x06006B1D RID: 27421 RVA: 0x00133361 File Offset: 0x00131561
		public override TaskStatus OnUpdate()
		{
			if ((1 << base.GetDefaultGameObject(this.targetGameObject.Value).layer & this.layermask.value) == 0)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006B1E RID: 27422 RVA: 0x0013338F File Offset: 0x0013158F
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x040056E0 RID: 22240
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040056E1 RID: 22241
		[Tooltip("The layermask to compare against")]
		public LayerMask layermask;
	}
}
