using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityCircleCollider2D
{
	// Token: 0x020015EC RID: 5612
	[TaskDescription("Stores the radius of the CircleCollider2D. Returns Success.")]
	[TaskCategory("Unity/CircleCollider2D")]
	public class GetRadius : Action
	{
		// Token: 0x06006B5C RID: 27484 RVA: 0x00133BA8 File Offset: 0x00131DA8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.circleCollider2D = defaultGameObject.GetComponent<CircleCollider2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006B5D RID: 27485 RVA: 0x00133BE8 File Offset: 0x00131DE8
		public override TaskStatus OnUpdate()
		{
			if (this.circleCollider2D == null)
			{
				Debug.LogWarning("CircleCollider2D is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.circleCollider2D.radius;
			return TaskStatus.Success;
		}

		// Token: 0x06006B5E RID: 27486 RVA: 0x00133C1B File Offset: 0x00131E1B
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 0f;
		}

		// Token: 0x04005715 RID: 22293
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005716 RID: 22294
		[Tooltip("The radius of the CircleCollider2D")]
		[RequiredField]
		public SharedFloat storeValue;

		// Token: 0x04005717 RID: 22295
		private CircleCollider2D circleCollider2D;

		// Token: 0x04005718 RID: 22296
		private GameObject prevGameObject;
	}
}
