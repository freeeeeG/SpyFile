using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014C5 RID: 5317
	[TaskDescription("Returns success when a 2D collision ends. This task will only receive the physics callback if it is being reevaluated (with a conditional abort or under a parallel task).")]
	[TaskCategory("Physics")]
	public class HasExitedCollision2D : Conditional
	{
		// Token: 0x06006764 RID: 26468 RVA: 0x0012B35C File Offset: 0x0012955C
		public override TaskStatus OnUpdate()
		{
			if (!this.exitedCollision)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006765 RID: 26469 RVA: 0x0012B369 File Offset: 0x00129569
		public override void OnEnd()
		{
			this.exitedCollision = false;
		}

		// Token: 0x06006766 RID: 26470 RVA: 0x0012B374 File Offset: 0x00129574
		public override void OnCollisionExit2D(Collision2D collision)
		{
			if (string.IsNullOrEmpty(this.tag.Value) || collision.gameObject.CompareTag(this.tag.Value))
			{
				this.collidedGameObject.Value = collision.gameObject;
				this.exitedCollision = true;
			}
		}

		// Token: 0x06006767 RID: 26471 RVA: 0x0012B3C3 File Offset: 0x001295C3
		public override void OnReset()
		{
			this.tag = "";
			this.collidedGameObject = null;
		}

		// Token: 0x04005371 RID: 21361
		[Tooltip("The tag of the GameObject to check for a collision against")]
		public SharedString tag = "";

		// Token: 0x04005372 RID: 21362
		[Tooltip("The object that exited the collision")]
		public SharedGameObject collidedGameObject;

		// Token: 0x04005373 RID: 21363
		private bool exitedCollision;
	}
}
