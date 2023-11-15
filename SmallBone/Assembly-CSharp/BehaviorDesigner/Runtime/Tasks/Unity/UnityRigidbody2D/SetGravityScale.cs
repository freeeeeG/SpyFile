using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x0200155E RID: 5470
	[TaskCategory("Unity/Rigidbody2D")]
	[TaskDescription("Sets the gravity scale of the Rigidbody2D. Returns Success.")]
	public class SetGravityScale : Action
	{
		// Token: 0x0600698A RID: 27018 RVA: 0x0012FD3C File Offset: 0x0012DF3C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600698B RID: 27019 RVA: 0x0012FD7C File Offset: 0x0012DF7C
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			this.rigidbody2D.gravityScale = this.gravityScale.Value;
			return TaskStatus.Success;
		}

		// Token: 0x0600698C RID: 27020 RVA: 0x0012FDAF File Offset: 0x0012DFAF
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.gravityScale = 0f;
		}

		// Token: 0x04005553 RID: 21843
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005554 RID: 21844
		[Tooltip("The gravity scale of the Rigidbody2D")]
		public SharedFloat gravityScale;

		// Token: 0x04005555 RID: 21845
		private Rigidbody2D rigidbody2D;

		// Token: 0x04005556 RID: 21846
		private GameObject prevGameObject;
	}
}
