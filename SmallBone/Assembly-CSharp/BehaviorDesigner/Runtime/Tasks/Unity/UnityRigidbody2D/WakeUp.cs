using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x02001563 RID: 5475
	[TaskDescription("Forces the Rigidbody2D to wake up. Returns Success.")]
	[TaskCategory("Unity/Rigidbody2D")]
	public class WakeUp : Conditional
	{
		// Token: 0x0600699E RID: 27038 RVA: 0x0012FFDC File Offset: 0x0012E1DC
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600699F RID: 27039 RVA: 0x0013001C File Offset: 0x0012E21C
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			this.rigidbody2D.WakeUp();
			return TaskStatus.Success;
		}

		// Token: 0x060069A0 RID: 27040 RVA: 0x00130044 File Offset: 0x0012E244
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x04005566 RID: 21862
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005567 RID: 21863
		private Rigidbody2D rigidbody2D;

		// Token: 0x04005568 RID: 21864
		private GameObject prevGameObject;
	}
}
