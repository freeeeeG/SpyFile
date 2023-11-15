using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs
{
	// Token: 0x02001574 RID: 5492
	[TaskDescription("Stores the value with the specified key from the PlayerPrefs.")]
	[TaskCategory("Unity/PlayerPrefs")]
	public class GetInt : Action
	{
		// Token: 0x060069D3 RID: 27091 RVA: 0x00130597 File Offset: 0x0012E797
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = PlayerPrefs.GetInt(this.key.Value, this.defaultValue.Value);
			return TaskStatus.Success;
		}

		// Token: 0x060069D4 RID: 27092 RVA: 0x001305C0 File Offset: 0x0012E7C0
		public override void OnReset()
		{
			this.key = "";
			this.defaultValue = 0;
			this.storeResult = 0;
		}

		// Token: 0x04005594 RID: 21908
		[Tooltip("The key to store")]
		public SharedString key;

		// Token: 0x04005595 RID: 21909
		[Tooltip("The default value")]
		public SharedInt defaultValue;

		// Token: 0x04005596 RID: 21910
		[RequiredField]
		[Tooltip("The value retrieved from the PlayerPrefs")]
		public SharedInt storeResult;
	}
}
