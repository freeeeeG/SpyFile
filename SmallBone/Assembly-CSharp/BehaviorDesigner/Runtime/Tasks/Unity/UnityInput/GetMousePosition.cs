using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput
{
	// Token: 0x020015CF RID: 5583
	[TaskDescription("Stores the mouse position.")]
	[TaskCategory("Unity/Input")]
	public class GetMousePosition : Action
	{
		// Token: 0x06006B02 RID: 27394 RVA: 0x0013321E File Offset: 0x0013141E
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Input.mousePosition;
			return TaskStatus.Success;
		}

		// Token: 0x06006B03 RID: 27395 RVA: 0x00133231 File Offset: 0x00131431
		public override void OnReset()
		{
			this.storeResult = Vector3.zero;
		}

		// Token: 0x040056D7 RID: 22231
		[RequiredField]
		[Tooltip("The stored result")]
		public SharedVector3 storeResult;
	}
}
