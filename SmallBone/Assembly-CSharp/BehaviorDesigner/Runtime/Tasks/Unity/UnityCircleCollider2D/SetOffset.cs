using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityCircleCollider2D
{
	// Token: 0x020015ED RID: 5613
	[TaskDescription("Sets the offset of the CircleCollider2D. Returns Success.")]
	[TaskCategory("Unity/CircleCollider2D")]
	public class SetOffset : Action
	{
		// Token: 0x06006B60 RID: 27488 RVA: 0x00133C34 File Offset: 0x00131E34
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.circleCollider2D = defaultGameObject.GetComponent<CircleCollider2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006B61 RID: 27489 RVA: 0x00133C74 File Offset: 0x00131E74
		public override TaskStatus OnUpdate()
		{
			if (this.circleCollider2D == null)
			{
				Debug.LogWarning("CircleCollider2D is null");
				return TaskStatus.Failure;
			}
			this.circleCollider2D.offset = this.offset.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006B62 RID: 27490 RVA: 0x00133CAC File Offset: 0x00131EAC
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.offset = Vector3.zero;
		}

		// Token: 0x04005719 RID: 22297
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400571A RID: 22298
		[Tooltip("The offset of the CircleCollider2D")]
		public SharedVector3 offset;

		// Token: 0x0400571B RID: 22299
		private CircleCollider2D circleCollider2D;

		// Token: 0x0400571C RID: 22300
		private GameObject prevGameObject;
	}
}
