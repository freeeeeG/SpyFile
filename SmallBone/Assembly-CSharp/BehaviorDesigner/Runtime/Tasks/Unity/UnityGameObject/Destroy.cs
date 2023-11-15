using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
	// Token: 0x020015DA RID: 5594
	[TaskCategory("Unity/GameObject")]
	[TaskDescription("Destorys the specified GameObject. Returns Success.")]
	public class Destroy : Action
	{
		// Token: 0x06006B23 RID: 27427 RVA: 0x001333DC File Offset: 0x001315DC
		public override TaskStatus OnUpdate()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (this.time == 0f)
			{
				UnityEngine.Object.Destroy(defaultGameObject);
			}
			else
			{
				UnityEngine.Object.Destroy(defaultGameObject, this.time);
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006B24 RID: 27428 RVA: 0x0013341D File Offset: 0x0013161D
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.time = 0f;
		}

		// Token: 0x040056E4 RID: 22244
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040056E5 RID: 22245
		[Tooltip("Time to destroy the GameObject in")]
		public float time;
	}
}
