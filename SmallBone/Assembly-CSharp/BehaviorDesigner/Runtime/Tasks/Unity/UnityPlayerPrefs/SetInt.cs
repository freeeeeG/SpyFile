using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs
{
	// Token: 0x02001579 RID: 5497
	[TaskDescription("Sets the value with the specified key from the PlayerPrefs.")]
	[TaskCategory("Unity/PlayerPrefs")]
	public class SetInt : Action
	{
		// Token: 0x060069E1 RID: 27105 RVA: 0x001306B6 File Offset: 0x0012E8B6
		public override TaskStatus OnUpdate()
		{
			PlayerPrefs.SetInt(this.key.Value, this.value.Value);
			return TaskStatus.Success;
		}

		// Token: 0x060069E2 RID: 27106 RVA: 0x001306D4 File Offset: 0x0012E8D4
		public override void OnReset()
		{
			this.key = "";
			this.value = 0;
		}

		// Token: 0x0400559D RID: 21917
		[Tooltip("The key to store")]
		public SharedString key;

		// Token: 0x0400559E RID: 21918
		[Tooltip("The value to set")]
		public SharedInt value;
	}
}
