using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x0200155F RID: 5471
	[TaskCategory("Unity/Rigidbody2D")]
	[TaskDescription("Sets the is kinematic value of the Rigidbody2D. Returns Success.")]
	public class SetIsKinematic : Action
	{
		// Token: 0x0600698E RID: 27022 RVA: 0x0012FDC8 File Offset: 0x0012DFC8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600698F RID: 27023 RVA: 0x0012FE08 File Offset: 0x0012E008
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			this.rigidbody2D.isKinematic = this.isKinematic.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006990 RID: 27024 RVA: 0x0012FE3B File Offset: 0x0012E03B
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.isKinematic = false;
		}

		// Token: 0x04005557 RID: 21847
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005558 RID: 21848
		[Tooltip("The is kinematic value of the Rigidbody2D")]
		public SharedBool isKinematic;

		// Token: 0x04005559 RID: 21849
		private Rigidbody2D rigidbody2D;

		// Token: 0x0400555A RID: 21850
		private GameObject prevGameObject;
	}
}
