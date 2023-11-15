using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x02001513 RID: 5395
	[TaskCategory("Unity/Transform")]
	[TaskDescription("Rotates the transform so the forward vector points at worldPosition. Returns Success.")]
	public class LookAt : Action
	{
		// Token: 0x06006885 RID: 26757 RVA: 0x0012DCC8 File Offset: 0x0012BEC8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006886 RID: 26758 RVA: 0x0012DD08 File Offset: 0x0012BF08
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			if (this.targetLookAt.Value != null)
			{
				this.targetTransform.LookAt(this.targetLookAt.Value.transform);
			}
			else
			{
				this.targetTransform.LookAt(this.worldPosition.Value, this.worldUp);
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006887 RID: 26759 RVA: 0x0012DD7C File Offset: 0x0012BF7C
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.targetLookAt = null;
			this.worldPosition = Vector3.up;
			this.worldUp = Vector3.up;
		}

		// Token: 0x04005478 RID: 21624
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005479 RID: 21625
		[Tooltip("The GameObject to look at. If null the world position will be used.")]
		public SharedGameObject targetLookAt;

		// Token: 0x0400547A RID: 21626
		[Tooltip("Point to look at")]
		public SharedVector3 worldPosition;

		// Token: 0x0400547B RID: 21627
		[Tooltip("Vector specifying the upward direction")]
		public Vector3 worldUp;

		// Token: 0x0400547C RID: 21628
		private Transform targetTransform;

		// Token: 0x0400547D RID: 21629
		private GameObject prevGameObject;
	}
}
