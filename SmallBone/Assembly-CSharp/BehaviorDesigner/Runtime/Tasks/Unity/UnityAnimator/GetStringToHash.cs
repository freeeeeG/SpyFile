using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x02001621 RID: 5665
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Converts the state name to its corresponding hash code. Returns Success.")]
	public class GetStringToHash : Action
	{
		// Token: 0x06006C2D RID: 27693 RVA: 0x001358CC File Offset: 0x00133ACC
		public override TaskStatus OnUpdate()
		{
			this.storeValue.Value = Animator.StringToHash(this.stateName.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006C2E RID: 27694 RVA: 0x001358EA File Offset: 0x00133AEA
		public override void OnReset()
		{
			this.stateName = "";
			this.storeValue = 0;
		}

		// Token: 0x040057E6 RID: 22502
		[Tooltip("The name of the state to convert to a hash code")]
		public SharedString stateName;

		// Token: 0x040057E7 RID: 22503
		[Tooltip("The hash value")]
		[RequiredField]
		public SharedInt storeValue;
	}
}
