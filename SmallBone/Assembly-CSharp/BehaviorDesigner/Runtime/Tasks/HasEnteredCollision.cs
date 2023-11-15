using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014C0 RID: 5312
	[TaskCategory("Physics")]
	[TaskDescription("Returns success when a collision starts. This task will only receive the physics callback if it is being reevaluated (with a conditional abort or under a parallel task).")]
	public class HasEnteredCollision : Conditional
	{
		// Token: 0x0600674B RID: 26443 RVA: 0x0012B073 File Offset: 0x00129273
		public override TaskStatus OnUpdate()
		{
			if (!this.enteredCollision)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x0600674C RID: 26444 RVA: 0x0012B080 File Offset: 0x00129280
		public override void OnEnd()
		{
			this.enteredCollision = false;
		}

		// Token: 0x0600674D RID: 26445 RVA: 0x0012B08C File Offset: 0x0012928C
		public override void OnCollisionEnter2D(Collision2D collision)
		{
			if (string.IsNullOrEmpty(this.tag.Value) || collision.gameObject.CompareTag(this.tag.Value))
			{
				this.collidedGameObject.Value = collision.gameObject;
				this.enteredCollision = true;
			}
		}

		// Token: 0x0600674E RID: 26446 RVA: 0x0012B0DB File Offset: 0x001292DB
		public override void OnReset()
		{
			this.tag = "";
			this.collidedGameObject = null;
		}

		// Token: 0x04005362 RID: 21346
		[Tooltip("The tag of the GameObject to check for a collision against")]
		public SharedString tag = "";

		// Token: 0x04005363 RID: 21347
		[Tooltip("The object that started the collision")]
		public SharedGameObject collidedGameObject;

		// Token: 0x04005364 RID: 21348
		private bool enteredCollision;
	}
}
