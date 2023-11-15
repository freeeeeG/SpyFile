using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput
{
	// Token: 0x020015D2 RID: 5586
	[TaskDescription("Returns success when the specified key is pressed.")]
	[TaskCategory("Unity/Input")]
	public class IsKeyDown : Conditional
	{
		// Token: 0x06006B0B RID: 27403 RVA: 0x00133295 File Offset: 0x00131495
		public override TaskStatus OnUpdate()
		{
			if (!Input.GetKeyDown(this.key))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006B0C RID: 27404 RVA: 0x001332A7 File Offset: 0x001314A7
		public override void OnReset()
		{
			this.key = KeyCode.None;
		}

		// Token: 0x040056DA RID: 22234
		[Tooltip("The key to test")]
		public KeyCode key;
	}
}
