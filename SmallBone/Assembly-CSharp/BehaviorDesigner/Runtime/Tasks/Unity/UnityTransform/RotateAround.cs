using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x02001515 RID: 5397
	[TaskDescription("Applies a rotation. Returns Success.")]
	[TaskCategory("Unity/Transform")]
	public class RotateAround : Action
	{
		// Token: 0x0600688D RID: 26765 RVA: 0x0012DE50 File Offset: 0x0012C050
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600688E RID: 26766 RVA: 0x0012DE90 File Offset: 0x0012C090
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.targetTransform.RotateAround(this.point.Value, this.axis.Value, this.angle.Value);
			return TaskStatus.Success;
		}

		// Token: 0x0600688F RID: 26767 RVA: 0x0012DEE4 File Offset: 0x0012C0E4
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.point = Vector3.zero;
			this.axis = Vector3.zero;
			this.angle = 0f;
		}

		// Token: 0x04005483 RID: 21635
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005484 RID: 21636
		[Tooltip("Point to rotate around")]
		public SharedVector3 point;

		// Token: 0x04005485 RID: 21637
		[Tooltip("Axis to rotate around")]
		public SharedVector3 axis;

		// Token: 0x04005486 RID: 21638
		[Tooltip("Amount to rotate")]
		public SharedFloat angle;

		// Token: 0x04005487 RID: 21639
		private Transform targetTransform;

		// Token: 0x04005488 RID: 21640
		private GameObject prevGameObject;
	}
}
