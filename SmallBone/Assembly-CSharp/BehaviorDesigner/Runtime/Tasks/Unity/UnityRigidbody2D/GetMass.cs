using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x02001553 RID: 5459
	[TaskCategory("Unity/Rigidbody2D")]
	[TaskDescription("Stores the mass of the Rigidbody2D. Returns Success.")]
	public class GetMass : Action
	{
		// Token: 0x0600695E RID: 26974 RVA: 0x0012F760 File Offset: 0x0012D960
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600695F RID: 26975 RVA: 0x0012F7A0 File Offset: 0x0012D9A0
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.rigidbody2D.mass;
			return TaskStatus.Success;
		}

		// Token: 0x06006960 RID: 26976 RVA: 0x0012F7D3 File Offset: 0x0012D9D3
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 0f;
		}

		// Token: 0x04005529 RID: 21801
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400552A RID: 21802
		[Tooltip("The mass of the Rigidbody2D")]
		[RequiredField]
		public SharedFloat storeValue;

		// Token: 0x0400552B RID: 21803
		private Rigidbody2D rigidbody2D;

		// Token: 0x0400552C RID: 21804
		private GameObject prevGameObject;
	}
}
