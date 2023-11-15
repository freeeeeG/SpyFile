using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x02001505 RID: 5381
	[TaskDescription("Stores the transform child at the specified index. Returns Success.")]
	[TaskCategory("Unity/Transform")]
	public class GetChild : Action
	{
		// Token: 0x0600684D RID: 26701 RVA: 0x0012D518 File Offset: 0x0012B718
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600684E RID: 26702 RVA: 0x0012D558 File Offset: 0x0012B758
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.targetTransform.GetChild(this.index.Value);
			return TaskStatus.Success;
		}

		// Token: 0x0600684F RID: 26703 RVA: 0x0012D596 File Offset: 0x0012B796
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.index = 0;
			this.storeValue = null;
		}

		// Token: 0x0400543F RID: 21567
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005440 RID: 21568
		[Tooltip("The index of the child")]
		public SharedInt index;

		// Token: 0x04005441 RID: 21569
		[RequiredField]
		[Tooltip("The child of the Transform")]
		public SharedTransform storeValue;

		// Token: 0x04005442 RID: 21570
		private Transform targetTransform;

		// Token: 0x04005443 RID: 21571
		private GameObject prevGameObject;
	}
}
