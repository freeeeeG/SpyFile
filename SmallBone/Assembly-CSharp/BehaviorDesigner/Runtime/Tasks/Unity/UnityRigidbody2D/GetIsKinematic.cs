using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x02001552 RID: 5458
	[TaskDescription("Stores the is kinematic value of the Rigidbody2D. Returns Success.")]
	[TaskCategory("Unity/Rigidbody2D")]
	public class GetIsKinematic : Action
	{
		// Token: 0x0600695A RID: 26970 RVA: 0x0012F6D8 File Offset: 0x0012D8D8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600695B RID: 26971 RVA: 0x0012F718 File Offset: 0x0012D918
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.rigidbody2D.isKinematic;
			return TaskStatus.Success;
		}

		// Token: 0x0600695C RID: 26972 RVA: 0x0012F74B File Offset: 0x0012D94B
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = false;
		}

		// Token: 0x04005525 RID: 21797
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005526 RID: 21798
		[RequiredField]
		[Tooltip("The is kinematic value of the Rigidbody2D")]
		public SharedBool storeValue;

		// Token: 0x04005527 RID: 21799
		private Rigidbody2D rigidbody2D;

		// Token: 0x04005528 RID: 21800
		private GameObject prevGameObject;
	}
}
