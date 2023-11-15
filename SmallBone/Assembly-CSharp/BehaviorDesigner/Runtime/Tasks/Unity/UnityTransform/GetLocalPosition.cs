using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x0200150A RID: 5386
	[TaskDescription("Stores the local position of the Transform. Returns Success.")]
	[TaskCategory("Unity/Transform")]
	public class GetLocalPosition : Action
	{
		// Token: 0x06006861 RID: 26721 RVA: 0x0012D7E0 File Offset: 0x0012B9E0
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006862 RID: 26722 RVA: 0x0012D820 File Offset: 0x0012BA20
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.targetTransform.localPosition;
			return TaskStatus.Success;
		}

		// Token: 0x06006863 RID: 26723 RVA: 0x0012D853 File Offset: 0x0012BA53
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = Vector3.zero;
		}

		// Token: 0x04005454 RID: 21588
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005455 RID: 21589
		[Tooltip("The local position of the Transform")]
		[RequiredField]
		public SharedVector3 storeValue;

		// Token: 0x04005456 RID: 21590
		private Transform targetTransform;

		// Token: 0x04005457 RID: 21591
		private GameObject prevGameObject;
	}
}
