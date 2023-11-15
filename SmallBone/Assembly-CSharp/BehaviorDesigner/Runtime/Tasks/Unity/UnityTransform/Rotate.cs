using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x02001514 RID: 5396
	[TaskCategory("Unity/Transform")]
	[TaskDescription("Applies a rotation. Returns Success.")]
	public class Rotate : Action
	{
		// Token: 0x06006889 RID: 26761 RVA: 0x0012DDA8 File Offset: 0x0012BFA8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600688A RID: 26762 RVA: 0x0012DDE8 File Offset: 0x0012BFE8
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.targetTransform.Rotate(this.eulerAngles.Value, this.relativeTo);
			return TaskStatus.Success;
		}

		// Token: 0x0600688B RID: 26763 RVA: 0x0012DE21 File Offset: 0x0012C021
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.eulerAngles = Vector3.zero;
			this.relativeTo = Space.Self;
		}

		// Token: 0x0400547E RID: 21630
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400547F RID: 21631
		[Tooltip("Amount to rotate")]
		public SharedVector3 eulerAngles;

		// Token: 0x04005480 RID: 21632
		[Tooltip("Specifies which axis the rotation is relative to")]
		public Space relativeTo = Space.Self;

		// Token: 0x04005481 RID: 21633
		private Transform targetTransform;

		// Token: 0x04005482 RID: 21634
		private GameObject prevGameObject;
	}
}
