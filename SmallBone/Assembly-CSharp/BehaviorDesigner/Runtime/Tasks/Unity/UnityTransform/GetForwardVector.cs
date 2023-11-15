using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x02001508 RID: 5384
	[TaskDescription("Stores the forward vector of the Transform. Returns Success.")]
	[TaskCategory("Unity/Transform")]
	public class GetForwardVector : Action
	{
		// Token: 0x06006859 RID: 26713 RVA: 0x0012D6C8 File Offset: 0x0012B8C8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600685A RID: 26714 RVA: 0x0012D708 File Offset: 0x0012B908
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.targetTransform.forward;
			return TaskStatus.Success;
		}

		// Token: 0x0600685B RID: 26715 RVA: 0x0012D73B File Offset: 0x0012B93B
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = Vector3.zero;
		}

		// Token: 0x0400544C RID: 21580
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400544D RID: 21581
		[RequiredField]
		[Tooltip("The position of the Transform")]
		public SharedVector3 storeValue;

		// Token: 0x0400544E RID: 21582
		private Transform targetTransform;

		// Token: 0x0400544F RID: 21583
		private GameObject prevGameObject;
	}
}
