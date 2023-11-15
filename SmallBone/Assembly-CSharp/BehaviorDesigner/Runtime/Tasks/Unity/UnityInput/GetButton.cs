using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput
{
	// Token: 0x020015CC RID: 5580
	[TaskCategory("Unity/Input")]
	[TaskDescription("Stores the state of the specified button.")]
	public class GetButton : Action
	{
		// Token: 0x06006AF9 RID: 27385 RVA: 0x0013317C File Offset: 0x0013137C
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Input.GetButton(this.buttonName.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006AFA RID: 27386 RVA: 0x0013319A File Offset: 0x0013139A
		public override void OnReset()
		{
			this.buttonName = "Fire1";
			this.storeResult = false;
		}

		// Token: 0x040056D1 RID: 22225
		[Tooltip("The name of the button")]
		public SharedString buttonName;

		// Token: 0x040056D2 RID: 22226
		[Tooltip("The stored result")]
		[RequiredField]
		public SharedBool storeResult;
	}
}
