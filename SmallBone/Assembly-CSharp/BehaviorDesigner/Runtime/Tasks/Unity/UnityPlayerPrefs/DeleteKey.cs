using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs
{
	// Token: 0x02001572 RID: 5490
	[TaskDescription("Deletes the specified key from the PlayerPrefs.")]
	[TaskCategory("Unity/PlayerPrefs")]
	public class DeleteKey : Action
	{
		// Token: 0x060069CD RID: 27085 RVA: 0x00130517 File Offset: 0x0012E717
		public override TaskStatus OnUpdate()
		{
			PlayerPrefs.DeleteKey(this.key.Value);
			return TaskStatus.Success;
		}

		// Token: 0x060069CE RID: 27086 RVA: 0x0013052A File Offset: 0x0012E72A
		public override void OnReset()
		{
			this.key = "";
		}

		// Token: 0x04005590 RID: 21904
		[Tooltip("The key to delete")]
		public SharedString key;
	}
}
