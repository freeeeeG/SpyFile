using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x0200150B RID: 5387
	[TaskCategory("Unity/Transform")]
	[TaskDescription("Stores the local rotation of the Transform. Returns Success.")]
	public class GetLocalRotation : Action
	{
		// Token: 0x06006865 RID: 26725 RVA: 0x0012D86C File Offset: 0x0012BA6C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006866 RID: 26726 RVA: 0x0012D8AC File Offset: 0x0012BAAC
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.targetTransform.localRotation;
			return TaskStatus.Success;
		}

		// Token: 0x06006867 RID: 26727 RVA: 0x0012D8DF File Offset: 0x0012BADF
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = Quaternion.identity;
		}

		// Token: 0x04005458 RID: 21592
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005459 RID: 21593
		[Tooltip("The local rotation of the Transform")]
		[RequiredField]
		public SharedQuaternion storeValue;

		// Token: 0x0400545A RID: 21594
		private Transform targetTransform;

		// Token: 0x0400545B RID: 21595
		private GameObject prevGameObject;
	}
}
