using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x02001519 RID: 5401
	[TaskDescription("Sets the local position of the Transform. Returns Success.")]
	[TaskCategory("Unity/Transform")]
	public class SetLocalPosition : Action
	{
		// Token: 0x0600689D RID: 26781 RVA: 0x0012E0C4 File Offset: 0x0012C2C4
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600689E RID: 26782 RVA: 0x0012E104 File Offset: 0x0012C304
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.targetTransform.localPosition = this.localPosition.Value;
			return TaskStatus.Success;
		}

		// Token: 0x0600689F RID: 26783 RVA: 0x0012E137 File Offset: 0x0012C337
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.localPosition = Vector3.zero;
		}

		// Token: 0x04005495 RID: 21653
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005496 RID: 21654
		[Tooltip("The local position of the Transform")]
		public SharedVector3 localPosition;

		// Token: 0x04005497 RID: 21655
		private Transform targetTransform;

		// Token: 0x04005498 RID: 21656
		private GameObject prevGameObject;
	}
}
