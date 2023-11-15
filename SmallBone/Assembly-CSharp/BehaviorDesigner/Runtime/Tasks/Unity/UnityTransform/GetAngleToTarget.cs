using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x02001503 RID: 5379
	[TaskDescription("Gets the Angle between a GameObject's forward direction and a target. Returns Success.")]
	[TaskCategory("Unity/Transform")]
	public class GetAngleToTarget : Action
	{
		// Token: 0x06006846 RID: 26694 RVA: 0x0012D384 File Offset: 0x0012B584
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006847 RID: 26695 RVA: 0x0012D3C4 File Offset: 0x0012B5C4
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			Vector3 a;
			if (this.targetObject.Value != null)
			{
				a = this.targetObject.Value.transform.InverseTransformPoint(this.targetPosition.Value);
			}
			else
			{
				a = this.targetPosition.Value;
			}
			if (this.ignoreHeight.Value)
			{
				a.y = this.targetTransform.position.y;
			}
			Vector3 from = a - this.targetTransform.position;
			this.storeValue.Value = Vector3.Angle(from, this.targetTransform.forward);
			return TaskStatus.Success;
		}

		// Token: 0x06006848 RID: 26696 RVA: 0x0012D480 File Offset: 0x0012B680
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.targetObject = null;
			this.targetPosition = Vector3.zero;
			this.ignoreHeight = true;
			this.storeValue = 0f;
		}

		// Token: 0x04005435 RID: 21557
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005436 RID: 21558
		[Tooltip("The target object to measure the angle to. If null the targetPosition will be used.")]
		public SharedGameObject targetObject;

		// Token: 0x04005437 RID: 21559
		[Tooltip("The world position to measure an angle to. If the targetObject is also not null, this value is used as an offset from that object's position.")]
		public SharedVector3 targetPosition;

		// Token: 0x04005438 RID: 21560
		[Tooltip("Ignore height differences when calculating the angle?")]
		public SharedBool ignoreHeight = true;

		// Token: 0x04005439 RID: 21561
		[RequiredField]
		[Tooltip("The angle to the target")]
		public SharedFloat storeValue;

		// Token: 0x0400543A RID: 21562
		private Transform targetTransform;

		// Token: 0x0400543B RID: 21563
		private GameObject prevGameObject;
	}
}
