using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x0200154D RID: 5453
	[TaskDescription("Applies a torque to the Rigidbody2D. Returns Success.")]
	[TaskCategory("Unity/Rigidbody2D")]
	public class AddTorque : Action
	{
		// Token: 0x06006946 RID: 26950 RVA: 0x0012F41C File Offset: 0x0012D61C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006947 RID: 26951 RVA: 0x0012F45C File Offset: 0x0012D65C
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			this.rigidbody2D.AddTorque(this.torque.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006948 RID: 26952 RVA: 0x0012F48F File Offset: 0x0012D68F
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.torque = 0f;
		}

		// Token: 0x04005511 RID: 21777
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005512 RID: 21778
		[Tooltip("The amount of torque to apply")]
		public SharedFloat torque;

		// Token: 0x04005513 RID: 21779
		private Rigidbody2D rigidbody2D;

		// Token: 0x04005514 RID: 21780
		private GameObject prevGameObject;
	}
}
