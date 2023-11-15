using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput
{
	// Token: 0x020015D4 RID: 5588
	[TaskDescription("Returns success when the specified mouse button is pressed.")]
	[TaskCategory("Unity/Input")]
	public class IsMouseDown : Conditional
	{
		// Token: 0x06006B11 RID: 27409 RVA: 0x001332CB File Offset: 0x001314CB
		public override TaskStatus OnUpdate()
		{
			if (!Input.GetMouseButtonDown(this.buttonIndex.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006B12 RID: 27410 RVA: 0x001332E2 File Offset: 0x001314E2
		public override void OnReset()
		{
			this.buttonIndex = 0;
		}

		// Token: 0x040056DC RID: 22236
		[Tooltip("The button index")]
		public SharedInt buttonIndex;
	}
}
