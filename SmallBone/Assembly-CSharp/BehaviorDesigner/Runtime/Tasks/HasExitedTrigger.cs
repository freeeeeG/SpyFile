using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014C6 RID: 5318
	[TaskCategory("Physics")]
	[TaskDescription("Returns success when an object exits the trigger. This task will only receive the physics callback if it is being reevaluated (with a conditional abort or under a parallel task).")]
	public class HasExitedTrigger : Conditional
	{
		// Token: 0x06006769 RID: 26473 RVA: 0x0012B3F4 File Offset: 0x001295F4
		public override TaskStatus OnUpdate()
		{
			if (!this.exitedTrigger)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x0600676A RID: 26474 RVA: 0x0012B401 File Offset: 0x00129601
		public override void OnEnd()
		{
			this.exitedTrigger = false;
		}

		// Token: 0x0600676B RID: 26475 RVA: 0x0012B40C File Offset: 0x0012960C
		public override void OnTriggerExit2D(Collider2D other)
		{
			if (string.IsNullOrEmpty(this.tag.Value) || other.gameObject.CompareTag(this.tag.Value))
			{
				this.otherGameObject.Value = other.gameObject;
				this.exitedTrigger = true;
			}
		}

		// Token: 0x0600676C RID: 26476 RVA: 0x0012B45B File Offset: 0x0012965B
		public override void OnReset()
		{
			this.tag = "";
			this.otherGameObject = null;
		}

		// Token: 0x04005374 RID: 21364
		[Tooltip("The tag of the GameObject to check for a trigger against")]
		public SharedString tag = "";

		// Token: 0x04005375 RID: 21365
		[Tooltip("The object that exited the trigger")]
		public SharedGameObject otherGameObject;

		// Token: 0x04005376 RID: 21366
		private bool exitedTrigger;
	}
}
