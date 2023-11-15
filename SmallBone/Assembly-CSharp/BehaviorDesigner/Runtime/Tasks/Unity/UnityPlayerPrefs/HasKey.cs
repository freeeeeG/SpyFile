using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs
{
	// Token: 0x02001576 RID: 5494
	[TaskDescription("Retruns success if the specified key exists.")]
	[TaskCategory("Unity/PlayerPrefs")]
	public class HasKey : Conditional
	{
		// Token: 0x060069D9 RID: 27097 RVA: 0x00130645 File Offset: 0x0012E845
		public override TaskStatus OnUpdate()
		{
			if (!PlayerPrefs.HasKey(this.key.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x060069DA RID: 27098 RVA: 0x0013065C File Offset: 0x0012E85C
		public override void OnReset()
		{
			this.key = "";
		}

		// Token: 0x0400559A RID: 21914
		[Tooltip("The key to check")]
		public SharedString key;
	}
}
