using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
	// Token: 0x020014F5 RID: 5365
	[TaskCategory("Unity/Vector2")]
	[TaskDescription("Stores the right vector value.")]
	public class GetRightVector : Action
	{
		// Token: 0x0600681E RID: 26654 RVA: 0x0012CE1A File Offset: 0x0012B01A
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector2.right;
			return TaskStatus.Success;
		}

		// Token: 0x0600681F RID: 26655 RVA: 0x0012CE2D File Offset: 0x0012B02D
		public override void OnReset()
		{
			this.storeResult = Vector2.zero;
		}

		// Token: 0x0400540D RID: 21517
		[RequiredField]
		[Tooltip("The stored result")]
		public SharedVector2 storeResult;
	}
}
