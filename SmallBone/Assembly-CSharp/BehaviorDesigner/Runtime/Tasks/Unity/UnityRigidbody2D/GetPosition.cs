using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x02001554 RID: 5460
	[TaskCategory("Unity/Rigidbody2D")]
	[TaskDescription("Stores the position of the Rigidbody2D. Returns Success.")]
	public class GetPosition : Action
	{
		// Token: 0x06006962 RID: 26978 RVA: 0x0012F7EC File Offset: 0x0012D9EC
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006963 RID: 26979 RVA: 0x0012F82C File Offset: 0x0012DA2C
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.rigidbody2D.position;
			return TaskStatus.Success;
		}

		// Token: 0x06006964 RID: 26980 RVA: 0x0012F85F File Offset: 0x0012DA5F
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = Vector2.zero;
		}

		// Token: 0x0400552D RID: 21805
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400552E RID: 21806
		[Tooltip("The velocity of the Rigidbody2D")]
		[RequiredField]
		public SharedVector2 storeValue;

		// Token: 0x0400552F RID: 21807
		private Rigidbody2D rigidbody2D;

		// Token: 0x04005530 RID: 21808
		private GameObject prevGameObject;
	}
}
