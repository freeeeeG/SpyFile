using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014C3 RID: 5315
	[TaskDescription("Returns success when an object enters the 2D trigger. This task will only receive the physics callback if it is being reevaluated (with a conditional abort or under a parallel task).")]
	[TaskCategory("Physics")]
	public class HasEnteredTrigger2D : Conditional
	{
		// Token: 0x0600675A RID: 26458 RVA: 0x0012B23C File Offset: 0x0012943C
		public override TaskStatus OnUpdate()
		{
			if (!this.enteredTrigger)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x0600675B RID: 26459 RVA: 0x0012B249 File Offset: 0x00129449
		public override void OnEnd()
		{
			this.enteredTrigger = false;
		}

		// Token: 0x0600675C RID: 26460 RVA: 0x0012B254 File Offset: 0x00129454
		public override void OnTriggerEnter2D(Collider2D other)
		{
			if (string.IsNullOrEmpty(this.tag.Value) || other.gameObject.CompareTag(this.tag.Value))
			{
				this.otherGameObject.Value = other.gameObject;
				this.enteredTrigger = true;
			}
		}

		// Token: 0x0600675D RID: 26461 RVA: 0x0012B2A3 File Offset: 0x001294A3
		public override void OnReset()
		{
			this.tag = "";
			this.otherGameObject = null;
		}

		// Token: 0x0400536B RID: 21355
		[Tooltip("The tag of the GameObject to check for a trigger against")]
		public SharedString tag = "";

		// Token: 0x0400536C RID: 21356
		[Tooltip("The object that entered the trigger")]
		public SharedGameObject otherGameObject;

		// Token: 0x0400536D RID: 21357
		private bool enteredTrigger;
	}
}
