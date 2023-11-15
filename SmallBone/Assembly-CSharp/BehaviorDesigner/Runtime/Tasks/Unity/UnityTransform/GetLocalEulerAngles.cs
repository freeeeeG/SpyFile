using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x02001509 RID: 5385
	[TaskCategory("Unity/Transform")]
	[TaskDescription("Stores the local euler angles of the Transform. Returns Success.")]
	public class GetLocalEulerAngles : Action
	{
		// Token: 0x0600685D RID: 26717 RVA: 0x0012D754 File Offset: 0x0012B954
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600685E RID: 26718 RVA: 0x0012D794 File Offset: 0x0012B994
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.targetTransform.localEulerAngles;
			return TaskStatus.Success;
		}

		// Token: 0x0600685F RID: 26719 RVA: 0x0012D7C7 File Offset: 0x0012B9C7
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = Vector3.zero;
		}

		// Token: 0x04005450 RID: 21584
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005451 RID: 21585
		[Tooltip("The local euler angles of the Transform")]
		[RequiredField]
		public SharedVector3 storeValue;

		// Token: 0x04005452 RID: 21586
		private Transform targetTransform;

		// Token: 0x04005453 RID: 21587
		private GameObject prevGameObject;
	}
}
