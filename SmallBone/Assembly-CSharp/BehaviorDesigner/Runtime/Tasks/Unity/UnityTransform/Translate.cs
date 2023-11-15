using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x02001521 RID: 5409
	[TaskDescription("Moves the transform in the direction and distance of translation. Returns Success.")]
	[TaskCategory("Unity/Transform")]
	public class Translate : Action
	{
		// Token: 0x060068BD RID: 26813 RVA: 0x0012E524 File Offset: 0x0012C724
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x060068BE RID: 26814 RVA: 0x0012E564 File Offset: 0x0012C764
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.targetTransform.Translate(this.translation.Value, this.relativeTo);
			return TaskStatus.Success;
		}

		// Token: 0x060068BF RID: 26815 RVA: 0x0012E59D File Offset: 0x0012C79D
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.translation = Vector3.zero;
			this.relativeTo = Space.Self;
		}

		// Token: 0x040054B5 RID: 21685
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040054B6 RID: 21686
		[Tooltip("Move direction and distance")]
		public SharedVector3 translation;

		// Token: 0x040054B7 RID: 21687
		[Tooltip("Specifies which axis the rotation is relative to")]
		public Space relativeTo = Space.Self;

		// Token: 0x040054B8 RID: 21688
		private Transform targetTransform;

		// Token: 0x040054B9 RID: 21689
		private GameObject prevGameObject;
	}
}
