using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x0200150F RID: 5391
	[TaskDescription("Stores the right vector of the Transform. Returns Success.")]
	[TaskCategory("Unity/Transform")]
	public class GetRightVector : Action
	{
		// Token: 0x06006875 RID: 26741 RVA: 0x0012DA9C File Offset: 0x0012BC9C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006876 RID: 26742 RVA: 0x0012DADC File Offset: 0x0012BCDC
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.targetTransform.right;
			return TaskStatus.Success;
		}

		// Token: 0x06006877 RID: 26743 RVA: 0x0012DB0F File Offset: 0x0012BD0F
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = Vector3.zero;
		}

		// Token: 0x04005468 RID: 21608
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005469 RID: 21609
		[RequiredField]
		[Tooltip("The position of the Transform")]
		public SharedVector3 storeValue;

		// Token: 0x0400546A RID: 21610
		private Transform targetTransform;

		// Token: 0x0400546B RID: 21611
		private GameObject prevGameObject;
	}
}
