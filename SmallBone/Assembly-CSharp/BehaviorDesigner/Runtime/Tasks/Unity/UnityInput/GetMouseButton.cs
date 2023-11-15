using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput
{
	// Token: 0x020015CE RID: 5582
	[TaskCategory("Unity/Input")]
	[TaskDescription("Stores the state of the specified mouse button.")]
	public class GetMouseButton : Action
	{
		// Token: 0x06006AFF RID: 27391 RVA: 0x001331E6 File Offset: 0x001313E6
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Input.GetMouseButton(this.buttonIndex.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006B00 RID: 27392 RVA: 0x00133204 File Offset: 0x00131404
		public override void OnReset()
		{
			this.buttonIndex = 0;
			this.storeResult = false;
		}

		// Token: 0x040056D5 RID: 22229
		[Tooltip("The index of the button")]
		public SharedInt buttonIndex;

		// Token: 0x040056D6 RID: 22230
		[RequiredField]
		[Tooltip("The stored result")]
		public SharedBool storeResult;
	}
}
