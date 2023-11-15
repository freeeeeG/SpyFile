using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x0200150D RID: 5389
	[TaskDescription("Stores the parent of the Transform. Returns Success.")]
	[TaskCategory("Unity/Transform")]
	public class GetParent : Action
	{
		// Token: 0x0600686D RID: 26733 RVA: 0x0012D984 File Offset: 0x0012BB84
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600686E RID: 26734 RVA: 0x0012D9C4 File Offset: 0x0012BBC4
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.targetTransform.parent;
			return TaskStatus.Success;
		}

		// Token: 0x0600686F RID: 26735 RVA: 0x0012D9F7 File Offset: 0x0012BBF7
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = null;
		}

		// Token: 0x04005460 RID: 21600
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005461 RID: 21601
		[Tooltip("The parent of the Transform")]
		[RequiredField]
		public SharedTransform storeValue;

		// Token: 0x04005462 RID: 21602
		private Transform targetTransform;

		// Token: 0x04005463 RID: 21603
		private GameObject prevGameObject;
	}
}
