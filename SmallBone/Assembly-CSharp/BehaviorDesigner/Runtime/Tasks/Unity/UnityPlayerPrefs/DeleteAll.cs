using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs
{
	// Token: 0x02001571 RID: 5489
	[TaskDescription("Deletes all entries from the PlayerPrefs.")]
	[TaskCategory("Unity/PlayerPrefs")]
	public class DeleteAll : Action
	{
		// Token: 0x060069CB RID: 27083 RVA: 0x0013050F File Offset: 0x0012E70F
		public override TaskStatus OnUpdate()
		{
			PlayerPrefs.DeleteAll();
			return TaskStatus.Success;
		}
	}
}
