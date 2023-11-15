using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x0200150C RID: 5388
	[TaskCategory("Unity/Transform")]
	[TaskDescription("Stores the local scale of the Transform. Returns Success.")]
	public class GetLocalScale : Action
	{
		// Token: 0x06006869 RID: 26729 RVA: 0x0012D8F8 File Offset: 0x0012BAF8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600686A RID: 26730 RVA: 0x0012D938 File Offset: 0x0012BB38
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.targetTransform.localScale;
			return TaskStatus.Success;
		}

		// Token: 0x0600686B RID: 26731 RVA: 0x0012D96B File Offset: 0x0012BB6B
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = Vector3.zero;
		}

		// Token: 0x0400545C RID: 21596
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400545D RID: 21597
		[Tooltip("The local scale of the Transform")]
		[RequiredField]
		public SharedVector3 storeValue;

		// Token: 0x0400545E RID: 21598
		private Transform targetTransform;

		// Token: 0x0400545F RID: 21599
		private GameObject prevGameObject;
	}
}
