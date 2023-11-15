using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs
{
	// Token: 0x02001575 RID: 5493
	[TaskDescription("Stores the value with the specified key from the PlayerPrefs.")]
	[TaskCategory("Unity/PlayerPrefs")]
	public class GetString : Action
	{
		// Token: 0x060069D6 RID: 27094 RVA: 0x001305EA File Offset: 0x0012E7EA
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = PlayerPrefs.GetString(this.key.Value, this.defaultValue.Value);
			return TaskStatus.Success;
		}

		// Token: 0x060069D7 RID: 27095 RVA: 0x00130613 File Offset: 0x0012E813
		public override void OnReset()
		{
			this.key = "";
			this.defaultValue = "";
			this.storeResult = "";
		}

		// Token: 0x04005597 RID: 21911
		[Tooltip("The key to store")]
		public SharedString key;

		// Token: 0x04005598 RID: 21912
		[Tooltip("The default value")]
		public SharedString defaultValue;

		// Token: 0x04005599 RID: 21913
		[RequiredField]
		[Tooltip("The value retrieved from the PlayerPrefs")]
		public SharedString storeResult;
	}
}
