using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x0200154B RID: 5451
	[TaskDescription("Applies a force to the Rigidbody2D. Returns Success.")]
	[TaskCategory("Unity/Rigidbody2D")]
	public class AddForce : Action
	{
		// Token: 0x0600693E RID: 26942 RVA: 0x0012F2E8 File Offset: 0x0012D4E8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600693F RID: 26943 RVA: 0x0012F328 File Offset: 0x0012D528
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			this.rigidbody2D.AddForce(this.force.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006940 RID: 26944 RVA: 0x0012F35B File Offset: 0x0012D55B
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.force = Vector2.zero;
		}

		// Token: 0x04005508 RID: 21768
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005509 RID: 21769
		[Tooltip("The amount of force to apply")]
		public SharedVector2 force;

		// Token: 0x0400550A RID: 21770
		private Rigidbody2D rigidbody2D;

		// Token: 0x0400550B RID: 21771
		private GameObject prevGameObject;
	}
}
