using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
	// Token: 0x020015DB RID: 5595
	[TaskCategory("Unity/GameObject")]
	[TaskDescription("Destorys the specified GameObject immediately. Returns Success.")]
	public class DestroyImmediate : Action
	{
		// Token: 0x06006B26 RID: 27430 RVA: 0x00133431 File Offset: 0x00131631
		public override TaskStatus OnUpdate()
		{
			UnityEngine.Object.DestroyImmediate(base.GetDefaultGameObject(this.targetGameObject.Value));
			return TaskStatus.Success;
		}

		// Token: 0x06006B27 RID: 27431 RVA: 0x0013344A File Offset: 0x0013164A
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x040056E6 RID: 22246
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;
	}
}
