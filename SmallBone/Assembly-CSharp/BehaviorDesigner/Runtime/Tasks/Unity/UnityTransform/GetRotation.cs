using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x02001510 RID: 5392
	[TaskCategory("Unity/Transform")]
	[TaskDescription("Stores the rotation of the Transform. Returns Success.")]
	public class GetRotation : Action
	{
		// Token: 0x06006879 RID: 26745 RVA: 0x0012DB28 File Offset: 0x0012BD28
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600687A RID: 26746 RVA: 0x0012DB68 File Offset: 0x0012BD68
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.targetTransform.rotation;
			return TaskStatus.Success;
		}

		// Token: 0x0600687B RID: 26747 RVA: 0x0012DB9B File Offset: 0x0012BD9B
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = Quaternion.identity;
		}

		// Token: 0x0400546C RID: 21612
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400546D RID: 21613
		[RequiredField]
		[Tooltip("The rotation of the Transform")]
		public SharedQuaternion storeValue;

		// Token: 0x0400546E RID: 21614
		private Transform targetTransform;

		// Token: 0x0400546F RID: 21615
		private GameObject prevGameObject;
	}
}
