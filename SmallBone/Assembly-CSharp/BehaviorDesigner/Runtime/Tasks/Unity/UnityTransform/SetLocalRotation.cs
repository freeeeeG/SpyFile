using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x0200151A RID: 5402
	[TaskCategory("Unity/Transform")]
	[TaskDescription("Sets the local rotation of the Transform. Returns Success.")]
	public class SetLocalRotation : Action
	{
		// Token: 0x060068A1 RID: 26785 RVA: 0x0012E150 File Offset: 0x0012C350
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x060068A2 RID: 26786 RVA: 0x0012E190 File Offset: 0x0012C390
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.targetTransform.localRotation = this.localRotation.Value;
			return TaskStatus.Success;
		}

		// Token: 0x060068A3 RID: 26787 RVA: 0x0012E1C3 File Offset: 0x0012C3C3
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.localRotation = Quaternion.identity;
		}

		// Token: 0x04005499 RID: 21657
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400549A RID: 21658
		[Tooltip("The local rotation of the Transform")]
		public SharedQuaternion localRotation;

		// Token: 0x0400549B RID: 21659
		private Transform targetTransform;

		// Token: 0x0400549C RID: 21660
		private GameObject prevGameObject;
	}
}
