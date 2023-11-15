using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x0200154F RID: 5455
	[TaskDescription("Stores the angular velocity of the Rigidbody2D. Returns Success.")]
	[TaskCategory("Unity/Rigidbody2D")]
	public class GetAngularVelocity : Action
	{
		// Token: 0x0600694E RID: 26958 RVA: 0x0012F534 File Offset: 0x0012D734
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600694F RID: 26959 RVA: 0x0012F574 File Offset: 0x0012D774
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.rigidbody2D.angularVelocity;
			return TaskStatus.Success;
		}

		// Token: 0x06006950 RID: 26960 RVA: 0x0012F5A7 File Offset: 0x0012D7A7
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 0f;
		}

		// Token: 0x04005519 RID: 21785
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400551A RID: 21786
		[RequiredField]
		[Tooltip("The angular velocity of the Rigidbody2D")]
		public SharedFloat storeValue;

		// Token: 0x0400551B RID: 21787
		private Rigidbody2D rigidbody2D;

		// Token: 0x0400551C RID: 21788
		private GameObject prevGameObject;
	}
}
