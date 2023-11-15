using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs
{
	// Token: 0x02001577 RID: 5495
	[TaskDescription("Saves the PlayerPrefs.")]
	[TaskCategory("Unity/PlayerPrefs")]
	public class Save : Action
	{
		// Token: 0x060069DC RID: 27100 RVA: 0x0013066E File Offset: 0x0012E86E
		public override TaskStatus OnUpdate()
		{
			PlayerPrefs.Save();
			return TaskStatus.Success;
		}
	}
}
