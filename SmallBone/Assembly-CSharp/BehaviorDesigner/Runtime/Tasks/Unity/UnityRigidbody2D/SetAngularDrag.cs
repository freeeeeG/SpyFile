using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x0200155B RID: 5467
	[TaskCategory("Unity/Rigidbody2D")]
	[TaskDescription("Sets the angular drag of the Rigidbody2D. Returns Success.")]
	public class SetAngularDrag : Action
	{
		// Token: 0x0600697E RID: 27006 RVA: 0x0012FB98 File Offset: 0x0012DD98
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600697F RID: 27007 RVA: 0x0012FBD8 File Offset: 0x0012DDD8
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			this.rigidbody2D.angularDrag = this.angularDrag.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006980 RID: 27008 RVA: 0x0012FC0B File Offset: 0x0012DE0B
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.angularDrag = 0f;
		}

		// Token: 0x04005547 RID: 21831
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005548 RID: 21832
		[Tooltip("The angular drag of the Rigidbody2D")]
		public SharedFloat angularDrag;

		// Token: 0x04005549 RID: 21833
		private Rigidbody2D rigidbody2D;

		// Token: 0x0400554A RID: 21834
		private GameObject prevGameObject;
	}
}
