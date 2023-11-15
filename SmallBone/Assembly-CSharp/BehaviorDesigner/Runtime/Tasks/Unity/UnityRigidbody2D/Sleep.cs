using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x02001562 RID: 5474
	[TaskCategory("Unity/Rigidbody2D")]
	[TaskDescription("Forces the Rigidbody2D to sleep at least one frame. Returns Success.")]
	public class Sleep : Conditional
	{
		// Token: 0x0600699A RID: 27034 RVA: 0x0012FF68 File Offset: 0x0012E168
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600699B RID: 27035 RVA: 0x0012FFA8 File Offset: 0x0012E1A8
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			this.rigidbody2D.Sleep();
			return TaskStatus.Success;
		}

		// Token: 0x0600699C RID: 27036 RVA: 0x0012FFD0 File Offset: 0x0012E1D0
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x04005563 RID: 21859
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005564 RID: 21860
		private Rigidbody2D rigidbody2D;

		// Token: 0x04005565 RID: 21861
		private GameObject prevGameObject;
	}
}
