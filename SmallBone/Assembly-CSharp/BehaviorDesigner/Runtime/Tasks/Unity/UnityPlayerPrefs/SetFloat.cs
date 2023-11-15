using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs
{
	// Token: 0x02001578 RID: 5496
	[TaskCategory("Unity/PlayerPrefs")]
	[TaskDescription("Sets the value with the specified key from the PlayerPrefs.")]
	public class SetFloat : Action
	{
		// Token: 0x060069DE RID: 27102 RVA: 0x00130676 File Offset: 0x0012E876
		public override TaskStatus OnUpdate()
		{
			PlayerPrefs.SetFloat(this.key.Value, this.value.Value);
			return TaskStatus.Success;
		}

		// Token: 0x060069DF RID: 27103 RVA: 0x00130694 File Offset: 0x0012E894
		public override void OnReset()
		{
			this.key = "";
			this.value = 0f;
		}

		// Token: 0x0400559B RID: 21915
		[Tooltip("The key to store")]
		public SharedString key;

		// Token: 0x0400559C RID: 21916
		[Tooltip("The value to set")]
		public SharedFloat value;
	}
}
