using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014C2 RID: 5314
	[TaskDescription("Returns success when an object enters the trigger. This task will only receive the physics callback if it is being reevaluated (with a conditional abort or under a parallel task).")]
	[TaskCategory("Physics")]
	public class HasEnteredTrigger : Conditional
	{
		// Token: 0x06006755 RID: 26453 RVA: 0x0012B1A4 File Offset: 0x001293A4
		public override TaskStatus OnUpdate()
		{
			if (!this.enteredTrigger)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006756 RID: 26454 RVA: 0x0012B1B1 File Offset: 0x001293B1
		public override void OnEnd()
		{
			this.enteredTrigger = false;
		}

		// Token: 0x06006757 RID: 26455 RVA: 0x0012B1BC File Offset: 0x001293BC
		public override void OnTriggerEnter2D(Collider2D other)
		{
			if (string.IsNullOrEmpty(this.tag.Value) || other.gameObject.CompareTag(this.tag.Value))
			{
				this.otherGameObject.Value = other.gameObject;
				this.enteredTrigger = true;
			}
		}

		// Token: 0x06006758 RID: 26456 RVA: 0x0012B20B File Offset: 0x0012940B
		public override void OnReset()
		{
			this.tag = "";
			this.otherGameObject = null;
		}

		// Token: 0x04005368 RID: 21352
		[Tooltip("The tag of the GameObject to check for a trigger against")]
		public SharedString tag = "";

		// Token: 0x04005369 RID: 21353
		[Tooltip("The object that entered the trigger")]
		public SharedGameObject otherGameObject;

		// Token: 0x0400536A RID: 21354
		private bool enteredTrigger;
	}
}
