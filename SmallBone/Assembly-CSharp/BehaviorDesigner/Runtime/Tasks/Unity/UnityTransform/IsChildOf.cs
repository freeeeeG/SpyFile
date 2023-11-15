using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x02001512 RID: 5394
	[TaskCategory("Unity/Transform")]
	[TaskDescription("Returns Success if the transform is a child of the specified GameObject.")]
	public class IsChildOf : Conditional
	{
		// Token: 0x06006881 RID: 26753 RVA: 0x0012DC40 File Offset: 0x0012BE40
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006882 RID: 26754 RVA: 0x0012DC80 File Offset: 0x0012BE80
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			if (!this.targetTransform.IsChildOf(this.transformName.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006883 RID: 26755 RVA: 0x0012DCB7 File Offset: 0x0012BEB7
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.transformName = null;
		}

		// Token: 0x04005474 RID: 21620
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005475 RID: 21621
		[Tooltip("The interested transform")]
		public SharedTransform transformName;

		// Token: 0x04005476 RID: 21622
		private Transform targetTransform;

		// Token: 0x04005477 RID: 21623
		private GameObject prevGameObject;
	}
}
