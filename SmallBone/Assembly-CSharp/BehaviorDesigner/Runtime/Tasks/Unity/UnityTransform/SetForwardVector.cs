using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x02001517 RID: 5399
	[TaskCategory("Unity/Transform")]
	[TaskDescription("Sets the forward vector of the Transform. Returns Success.")]
	public class SetForwardVector : Action
	{
		// Token: 0x06006895 RID: 26773 RVA: 0x0012DFAC File Offset: 0x0012C1AC
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006896 RID: 26774 RVA: 0x0012DFEC File Offset: 0x0012C1EC
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.targetTransform.forward = this.position.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006897 RID: 26775 RVA: 0x0012E01F File Offset: 0x0012C21F
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.position = Vector3.zero;
		}

		// Token: 0x0400548D RID: 21645
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400548E RID: 21646
		[Tooltip("The position of the Transform")]
		public SharedVector3 position;

		// Token: 0x0400548F RID: 21647
		private Transform targetTransform;

		// Token: 0x04005490 RID: 21648
		private GameObject prevGameObject;
	}
}
