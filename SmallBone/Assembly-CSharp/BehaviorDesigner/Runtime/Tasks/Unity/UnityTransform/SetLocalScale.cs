using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x0200151B RID: 5403
	[TaskDescription("Sets the local scale of the Transform. Returns Success.")]
	[TaskCategory("Unity/Transform")]
	public class SetLocalScale : Action
	{
		// Token: 0x060068A5 RID: 26789 RVA: 0x0012E1DC File Offset: 0x0012C3DC
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x060068A6 RID: 26790 RVA: 0x0012E21C File Offset: 0x0012C41C
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.targetTransform.localScale = this.localScale.Value;
			return TaskStatus.Success;
		}

		// Token: 0x060068A7 RID: 26791 RVA: 0x0012E24F File Offset: 0x0012C44F
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.localScale = Vector3.zero;
		}

		// Token: 0x0400549D RID: 21661
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400549E RID: 21662
		[Tooltip("The local scale of the Transform")]
		public SharedVector3 localScale;

		// Token: 0x0400549F RID: 21663
		private Transform targetTransform;

		// Token: 0x040054A0 RID: 21664
		private GameObject prevGameObject;
	}
}
