using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x02001520 RID: 5408
	[TaskDescription("Sets the up vector of the Transform. Returns Success.")]
	[TaskCategory("Unity/Transform")]
	public class SetUpVector : Action
	{
		// Token: 0x060068B9 RID: 26809 RVA: 0x0012E498 File Offset: 0x0012C698
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x060068BA RID: 26810 RVA: 0x0012E4D8 File Offset: 0x0012C6D8
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.targetTransform.up = this.position.Value;
			return TaskStatus.Success;
		}

		// Token: 0x060068BB RID: 26811 RVA: 0x0012E50B File Offset: 0x0012C70B
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.position = Vector3.zero;
		}

		// Token: 0x040054B1 RID: 21681
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040054B2 RID: 21682
		[Tooltip("The position of the Transform")]
		public SharedVector3 position;

		// Token: 0x040054B3 RID: 21683
		private Transform targetTransform;

		// Token: 0x040054B4 RID: 21684
		private GameObject prevGameObject;
	}
}
