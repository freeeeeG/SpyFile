using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput
{
	// Token: 0x020015D3 RID: 5587
	[TaskCategory("Unity/Input")]
	[TaskDescription("Returns success when the specified key is released.")]
	public class IsKeyUp : Conditional
	{
		// Token: 0x06006B0E RID: 27406 RVA: 0x001332B0 File Offset: 0x001314B0
		public override TaskStatus OnUpdate()
		{
			if (!Input.GetKeyUp(this.key))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006B0F RID: 27407 RVA: 0x001332C2 File Offset: 0x001314C2
		public override void OnReset()
		{
			this.key = KeyCode.None;
		}

		// Token: 0x040056DB RID: 22235
		[Tooltip("The key to test")]
		public KeyCode key;
	}
}
