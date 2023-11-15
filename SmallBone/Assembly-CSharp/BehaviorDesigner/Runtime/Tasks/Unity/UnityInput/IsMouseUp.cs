using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput
{
	// Token: 0x020015D5 RID: 5589
	[TaskCategory("Unity/Input")]
	[TaskDescription("Returns success when the specified mouse button is pressed.")]
	public class IsMouseUp : Conditional
	{
		// Token: 0x06006B14 RID: 27412 RVA: 0x001332F0 File Offset: 0x001314F0
		public override TaskStatus OnUpdate()
		{
			if (!Input.GetMouseButtonUp(this.buttonIndex.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006B15 RID: 27413 RVA: 0x00133307 File Offset: 0x00131507
		public override void OnReset()
		{
			this.buttonIndex = 0;
		}

		// Token: 0x040056DD RID: 22237
		[Tooltip("The button index")]
		public SharedInt buttonIndex;
	}
}
