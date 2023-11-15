using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput
{
	// Token: 0x020015CD RID: 5581
	[TaskCategory("Unity/Input")]
	[TaskDescription("Stores the pressed state of the specified key.")]
	public class GetKey : Action
	{
		// Token: 0x06006AFC RID: 27388 RVA: 0x001331B8 File Offset: 0x001313B8
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Input.GetKey(this.key);
			return TaskStatus.Success;
		}

		// Token: 0x06006AFD RID: 27389 RVA: 0x001331D1 File Offset: 0x001313D1
		public override void OnReset()
		{
			this.key = KeyCode.None;
			this.storeResult = false;
		}

		// Token: 0x040056D3 RID: 22227
		[Tooltip("The key to test.")]
		public KeyCode key;

		// Token: 0x040056D4 RID: 22228
		[Tooltip("The stored result")]
		[RequiredField]
		public SharedBool storeResult;
	}
}
