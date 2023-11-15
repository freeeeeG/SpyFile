using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014C4 RID: 5316
	[TaskCategory("Physics")]
	[TaskDescription("Returns success when a collision ends. This task will only receive the physics callback if it is being reevaluated (with a conditional abort or under a parallel task).")]
	public class HasExitedCollision : Conditional
	{
		// Token: 0x0600675F RID: 26463 RVA: 0x0012B2D4 File Offset: 0x001294D4
		public override TaskStatus OnUpdate()
		{
			if (!this.exitedCollision)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006760 RID: 26464 RVA: 0x0012B2E1 File Offset: 0x001294E1
		public override void OnEnd()
		{
			this.exitedCollision = false;
		}

		// Token: 0x06006761 RID: 26465 RVA: 0x0012B2EC File Offset: 0x001294EC
		public override void OnCollisionExit2D(Collision2D collision)
		{
			if (string.IsNullOrEmpty(this.tag.Value) || collision.gameObject.CompareTag(this.tag.Value))
			{
				this.collidedGameObject.Value = collision.gameObject;
				this.exitedCollision = true;
			}
		}

		// Token: 0x06006762 RID: 26466 RVA: 0x0012B33B File Offset: 0x0012953B
		public override void OnReset()
		{
			this.collidedGameObject = null;
		}

		// Token: 0x0400536E RID: 21358
		[Tooltip("The tag of the GameObject to check for a collision against")]
		public SharedString tag = "";

		// Token: 0x0400536F RID: 21359
		[Tooltip("The object that exited the collision")]
		public SharedGameObject collidedGameObject;

		// Token: 0x04005370 RID: 21360
		private bool exitedCollision;
	}
}
