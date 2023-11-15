using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x02001547 RID: 5447
	[TaskCategory("Unity/SharedVariable")]
	[TaskDescription("Gets the Transform from the GameObject. Returns Success.")]
	public class SharedGameObjectToTransform : Action
	{
		// Token: 0x06006930 RID: 26928 RVA: 0x0012F156 File Offset: 0x0012D356
		public override TaskStatus OnUpdate()
		{
			if (this.sharedGameObject.Value == null)
			{
				return TaskStatus.Failure;
			}
			this.sharedTransform.Value = this.sharedGameObject.Value.GetComponent<Transform>();
			return TaskStatus.Success;
		}

		// Token: 0x06006931 RID: 26929 RVA: 0x0012F189 File Offset: 0x0012D389
		public override void OnReset()
		{
			this.sharedGameObject = null;
			this.sharedTransform = null;
		}

		// Token: 0x04005500 RID: 21760
		[Tooltip("The GameObject to get the Transform of")]
		public SharedGameObject sharedGameObject;

		// Token: 0x04005501 RID: 21761
		[RequiredField]
		[Tooltip("The Transform to set")]
		public SharedTransform sharedTransform;
	}
}
