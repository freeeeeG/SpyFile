using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x0200154C RID: 5452
	[TaskDescription("Applies a force at the specified position to the Rigidbody2D. Returns Success.")]
	[TaskCategory("Unity/Rigidbody2D")]
	public class AddForceAtPosition : Action
	{
		// Token: 0x06006942 RID: 26946 RVA: 0x0012F374 File Offset: 0x0012D574
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006943 RID: 26947 RVA: 0x0012F3B4 File Offset: 0x0012D5B4
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			this.rigidbody2D.AddForceAtPosition(this.force.Value, this.position.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006944 RID: 26948 RVA: 0x0012F3F2 File Offset: 0x0012D5F2
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.force = Vector2.zero;
			this.position = Vector2.zero;
		}

		// Token: 0x0400550C RID: 21772
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400550D RID: 21773
		[Tooltip("The amount of force to apply")]
		public SharedVector2 force;

		// Token: 0x0400550E RID: 21774
		[Tooltip("The position of the force")]
		public SharedVector2 position;

		// Token: 0x0400550F RID: 21775
		private Rigidbody2D rigidbody2D;

		// Token: 0x04005510 RID: 21776
		private GameObject prevGameObject;
	}
}
