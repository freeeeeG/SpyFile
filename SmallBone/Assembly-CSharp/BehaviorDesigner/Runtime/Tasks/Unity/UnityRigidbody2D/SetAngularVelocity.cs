using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x0200155C RID: 5468
	[TaskCategory("Unity/Rigidbody2D")]
	[TaskDescription("Sets the angular velocity of the Rigidbody2D. Returns Success.")]
	public class SetAngularVelocity : Action
	{
		// Token: 0x06006982 RID: 27010 RVA: 0x0012FC24 File Offset: 0x0012DE24
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006983 RID: 27011 RVA: 0x0012FC64 File Offset: 0x0012DE64
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			this.rigidbody2D.angularVelocity = this.angularVelocity.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006984 RID: 27012 RVA: 0x0012FC97 File Offset: 0x0012DE97
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.angularVelocity = 0f;
		}

		// Token: 0x0400554B RID: 21835
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400554C RID: 21836
		[Tooltip("The angular velocity of the Rigidbody2D")]
		public SharedFloat angularVelocity;

		// Token: 0x0400554D RID: 21837
		private Rigidbody2D rigidbody2D;

		// Token: 0x0400554E RID: 21838
		private GameObject prevGameObject;
	}
}
