using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014C1 RID: 5313
	[TaskDescription("Returns success when a 2D collision starts. This task will only receive the physics callback if it is being reevaluated (with a conditional abort or under a parallel task).")]
	[TaskCategory("Physics")]
	public class HasEnteredCollision2D : Conditional
	{
		// Token: 0x06006750 RID: 26448 RVA: 0x0012B10C File Offset: 0x0012930C
		public override TaskStatus OnUpdate()
		{
			if (!this.enteredCollision)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006751 RID: 26449 RVA: 0x0012B119 File Offset: 0x00129319
		public override void OnEnd()
		{
			this.enteredCollision = false;
		}

		// Token: 0x06006752 RID: 26450 RVA: 0x0012B124 File Offset: 0x00129324
		public override void OnCollisionEnter2D(Collision2D collision)
		{
			if (string.IsNullOrEmpty(this.tag.Value) || collision.gameObject.CompareTag(this.tag.Value))
			{
				this.collidedGameObject.Value = collision.gameObject;
				this.enteredCollision = true;
			}
		}

		// Token: 0x06006753 RID: 26451 RVA: 0x0012B173 File Offset: 0x00129373
		public override void OnReset()
		{
			this.tag = "";
			this.collidedGameObject = null;
		}

		// Token: 0x04005365 RID: 21349
		[Tooltip("The tag of the GameObject to check for a collision against")]
		public SharedString tag = "";

		// Token: 0x04005366 RID: 21350
		[Tooltip("The object that started the collision")]
		public SharedGameObject collidedGameObject;

		// Token: 0x04005367 RID: 21351
		private bool enteredCollision;
	}
}
