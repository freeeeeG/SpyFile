using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x0200151E RID: 5406
	[TaskDescription("Sets the right vector of the Transform. Returns Success.")]
	[TaskCategory("Unity/Transform")]
	public class SetRightVector : Action
	{
		// Token: 0x060068B1 RID: 26801 RVA: 0x0012E380 File Offset: 0x0012C580
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x060068B2 RID: 26802 RVA: 0x0012E3C0 File Offset: 0x0012C5C0
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.targetTransform.right = this.position.Value;
			return TaskStatus.Success;
		}

		// Token: 0x060068B3 RID: 26803 RVA: 0x0012E3F3 File Offset: 0x0012C5F3
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.position = Vector3.zero;
		}

		// Token: 0x040054A9 RID: 21673
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040054AA RID: 21674
		[Tooltip("The position of the Transform")]
		public SharedVector3 position;

		// Token: 0x040054AB RID: 21675
		private Transform targetTransform;

		// Token: 0x040054AC RID: 21676
		private GameObject prevGameObject;
	}
}
