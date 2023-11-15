using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityCircleCollider2D
{
	// Token: 0x020015EE RID: 5614
	[TaskDescription("Sets the radius of the CircleCollider2D. Returns Success.")]
	[TaskCategory("Unity/CircleCollider2D")]
	public class SetRadius : Action
	{
		// Token: 0x06006B64 RID: 27492 RVA: 0x00133CC8 File Offset: 0x00131EC8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.circleCollider2D = defaultGameObject.GetComponent<CircleCollider2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006B65 RID: 27493 RVA: 0x00133D08 File Offset: 0x00131F08
		public override TaskStatus OnUpdate()
		{
			if (this.circleCollider2D == null)
			{
				Debug.LogWarning("CircleCollider2D is null");
				return TaskStatus.Failure;
			}
			this.circleCollider2D.radius = this.radius.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006B66 RID: 27494 RVA: 0x00133D3B File Offset: 0x00131F3B
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.radius = 0f;
		}

		// Token: 0x0400571D RID: 22301
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400571E RID: 22302
		[Tooltip("The radius of the CircleCollider2D")]
		public SharedFloat radius;

		// Token: 0x0400571F RID: 22303
		private CircleCollider2D circleCollider2D;

		// Token: 0x04005720 RID: 22304
		private GameObject prevGameObject;
	}
}
