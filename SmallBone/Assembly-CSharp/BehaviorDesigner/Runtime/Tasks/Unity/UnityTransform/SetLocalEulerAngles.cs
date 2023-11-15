using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x02001518 RID: 5400
	[TaskCategory("Unity/Transform")]
	[TaskDescription("Sets the local euler angles of the Transform. Returns Success.")]
	public class SetLocalEulerAngles : Action
	{
		// Token: 0x06006899 RID: 26777 RVA: 0x0012E038 File Offset: 0x0012C238
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600689A RID: 26778 RVA: 0x0012E078 File Offset: 0x0012C278
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.targetTransform.localEulerAngles = this.localEulerAngles.Value;
			return TaskStatus.Success;
		}

		// Token: 0x0600689B RID: 26779 RVA: 0x0012E0AB File Offset: 0x0012C2AB
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.localEulerAngles = Vector3.zero;
		}

		// Token: 0x04005491 RID: 21649
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005492 RID: 21650
		[Tooltip("The local euler angles of the Transform")]
		public SharedVector3 localEulerAngles;

		// Token: 0x04005493 RID: 21651
		private Transform targetTransform;

		// Token: 0x04005494 RID: 21652
		private GameObject prevGameObject;
	}
}
