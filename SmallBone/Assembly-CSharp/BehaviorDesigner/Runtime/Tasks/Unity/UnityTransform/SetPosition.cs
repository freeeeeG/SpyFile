using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x0200151D RID: 5405
	[TaskDescription("Sets the position of the Transform. Returns Success.")]
	[TaskCategory("Unity/Transform")]
	public class SetPosition : Action
	{
		// Token: 0x060068AD RID: 26797 RVA: 0x0012E2EC File Offset: 0x0012C4EC
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x060068AE RID: 26798 RVA: 0x0012E32C File Offset: 0x0012C52C
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.targetTransform.position = this.position.Value;
			return TaskStatus.Success;
		}

		// Token: 0x060068AF RID: 26799 RVA: 0x0012E364 File Offset: 0x0012C564
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.position = Vector2.zero;
		}

		// Token: 0x040054A5 RID: 21669
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040054A6 RID: 21670
		[Tooltip("The position of the Transform")]
		public SharedVector2 position;

		// Token: 0x040054A7 RID: 21671
		private Transform targetTransform;

		// Token: 0x040054A8 RID: 21672
		private GameObject prevGameObject;
	}
}
