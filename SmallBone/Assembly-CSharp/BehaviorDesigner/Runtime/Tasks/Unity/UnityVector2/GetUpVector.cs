using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
	// Token: 0x020014F7 RID: 5367
	[TaskCategory("Unity/Vector2")]
	[TaskDescription("Stores the up vector value.")]
	public class GetUpVector : Action
	{
		// Token: 0x06006824 RID: 26660 RVA: 0x0012CE8E File Offset: 0x0012B08E
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector2.up;
			return TaskStatus.Success;
		}

		// Token: 0x06006825 RID: 26661 RVA: 0x0012CEA1 File Offset: 0x0012B0A1
		public override void OnReset()
		{
			this.storeResult = Vector2.zero;
		}

		// Token: 0x04005410 RID: 21520
		[RequiredField]
		[Tooltip("The stored result")]
		public SharedVector2 storeResult;
	}
}
