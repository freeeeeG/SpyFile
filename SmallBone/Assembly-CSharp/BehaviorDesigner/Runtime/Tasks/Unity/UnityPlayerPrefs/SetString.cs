using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs
{
	// Token: 0x0200157A RID: 5498
	[TaskDescription("Sets the value with the specified key from the PlayerPrefs.")]
	[TaskCategory("Unity/PlayerPrefs")]
	public class SetString : Action
	{
		// Token: 0x060069E4 RID: 27108 RVA: 0x001306F2 File Offset: 0x0012E8F2
		public override TaskStatus OnUpdate()
		{
			PlayerPrefs.SetString(this.key.Value, this.value.Value);
			return TaskStatus.Success;
		}

		// Token: 0x060069E5 RID: 27109 RVA: 0x00130710 File Offset: 0x0012E910
		public override void OnReset()
		{
			this.key = "";
			this.value = "";
		}

		// Token: 0x0400559F RID: 21919
		[Tooltip("The key to store")]
		public SharedString key;

		// Token: 0x040055A0 RID: 21920
		[Tooltip("The value to set")]
		public SharedString value;
	}
}
