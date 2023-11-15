using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014C7 RID: 5319
	[TaskCategory("Physics")]
	[TaskDescription("Returns success when an object exits the 2D trigger. This task will only receive the physics callback if it is being reevaluated (with a conditional abort or under a parallel task).")]
	public class HasExitedTrigger2D : Conditional
	{
		// Token: 0x0600676E RID: 26478 RVA: 0x0012B48C File Offset: 0x0012968C
		public override TaskStatus OnUpdate()
		{
			if (!this.exitedTrigger)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x0600676F RID: 26479 RVA: 0x0012B499 File Offset: 0x00129699
		public override void OnEnd()
		{
			this.exitedTrigger = false;
		}

		// Token: 0x06006770 RID: 26480 RVA: 0x0012B4A4 File Offset: 0x001296A4
		public override void OnTriggerExit2D(Collider2D other)
		{
			if (string.IsNullOrEmpty(this.tag.Value) || other.gameObject.CompareTag(this.tag.Value))
			{
				this.otherGameObject.Value = other.gameObject;
				this.exitedTrigger = true;
			}
		}

		// Token: 0x06006771 RID: 26481 RVA: 0x0012B4F3 File Offset: 0x001296F3
		public override void OnReset()
		{
			this.tag = "";
			this.otherGameObject = null;
		}

		// Token: 0x04005377 RID: 21367
		[Tooltip("The tag of the GameObject to check for a trigger against")]
		public SharedString tag = "";

		// Token: 0x04005378 RID: 21368
		[Tooltip("The object that exited the trigger")]
		public SharedGameObject otherGameObject;

		// Token: 0x04005379 RID: 21369
		private bool exitedTrigger;
	}
}
