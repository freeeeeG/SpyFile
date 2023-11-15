using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs
{
	// Token: 0x02001573 RID: 5491
	[TaskDescription("Stores the value with the specified key from the PlayerPrefs.")]
	[TaskCategory("Unity/PlayerPrefs")]
	public class GetFloat : Action
	{
		// Token: 0x060069D0 RID: 27088 RVA: 0x0013053C File Offset: 0x0012E73C
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = PlayerPrefs.GetFloat(this.key.Value, this.defaultValue.Value);
			return TaskStatus.Success;
		}

		// Token: 0x060069D1 RID: 27089 RVA: 0x00130565 File Offset: 0x0012E765
		public override void OnReset()
		{
			this.key = "";
			this.defaultValue = 0f;
			this.storeResult = 0f;
		}

		// Token: 0x04005591 RID: 21905
		[Tooltip("The key to store")]
		public SharedString key;

		// Token: 0x04005592 RID: 21906
		[Tooltip("The default value")]
		public SharedFloat defaultValue;

		// Token: 0x04005593 RID: 21907
		[RequiredField]
		[Tooltip("The value retrieved from the PlayerPrefs")]
		public SharedFloat storeResult;
	}
}
