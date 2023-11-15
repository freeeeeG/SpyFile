using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x02001561 RID: 5473
	[TaskCategory("Unity/Rigidbody2D")]
	[TaskDescription("Sets the velocity of the Rigidbody2D. Returns Success.")]
	public class SetVelocity : Action
	{
		// Token: 0x06006996 RID: 27030 RVA: 0x0012FEDC File Offset: 0x0012E0DC
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006997 RID: 27031 RVA: 0x0012FF1C File Offset: 0x0012E11C
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			this.rigidbody2D.velocity = this.velocity.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006998 RID: 27032 RVA: 0x0012FF4F File Offset: 0x0012E14F
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.velocity = Vector2.zero;
		}

		// Token: 0x0400555F RID: 21855
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005560 RID: 21856
		[Tooltip("The velocity of the Rigidbody2D")]
		public SharedVector2 velocity;

		// Token: 0x04005561 RID: 21857
		private Rigidbody2D rigidbody2D;

		// Token: 0x04005562 RID: 21858
		private GameObject prevGameObject;
	}
}
