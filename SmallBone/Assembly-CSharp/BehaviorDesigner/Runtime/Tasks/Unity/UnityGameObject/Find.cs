using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
	// Token: 0x020015DC RID: 5596
	[TaskDescription("Finds a GameObject by name. Returns success if an object is found.")]
	[TaskCategory("Unity/GameObject")]
	public class Find : Action
	{
		// Token: 0x06006B29 RID: 27433 RVA: 0x00133453 File Offset: 0x00131653
		public override TaskStatus OnUpdate()
		{
			this.storeValue.Value = GameObject.Find(this.gameObjectName.Value);
			if (!(this.storeValue.Value != null))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006B2A RID: 27434 RVA: 0x00133486 File Offset: 0x00131686
		public override void OnReset()
		{
			this.gameObjectName = null;
			this.storeValue = null;
		}

		// Token: 0x040056E7 RID: 22247
		[Tooltip("The GameObject name to find")]
		public SharedString gameObjectName;

		// Token: 0x040056E8 RID: 22248
		[RequiredField]
		[Tooltip("The object found by name")]
		public SharedGameObject storeValue;
	}
}
