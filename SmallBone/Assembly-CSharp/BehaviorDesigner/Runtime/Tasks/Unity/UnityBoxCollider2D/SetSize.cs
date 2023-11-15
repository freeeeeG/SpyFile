using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityBoxCollider2D
{
	// Token: 0x020015F0 RID: 5616
	[TaskCategory("Unity/BoxCollider2D")]
	[TaskDescription("Sets the size of the BoxCollider2D. Returns Success.")]
	public class SetSize : Action
	{
		// Token: 0x06006B6C RID: 27500 RVA: 0x00133DE0 File Offset: 0x00131FE0
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.boxCollider2D = defaultGameObject.GetComponent<BoxCollider2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006B6D RID: 27501 RVA: 0x00133E20 File Offset: 0x00132020
		public override TaskStatus OnUpdate()
		{
			if (this.boxCollider2D == null)
			{
				Debug.LogWarning("BoxCollider2D is null");
				return TaskStatus.Failure;
			}
			this.boxCollider2D.size = this.size.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006B6E RID: 27502 RVA: 0x00133E53 File Offset: 0x00132053
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.size = Vector2.zero;
		}

		// Token: 0x04005725 RID: 22309
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005726 RID: 22310
		[Tooltip("The size of the BoxCollider2D")]
		public SharedVector2 size;

		// Token: 0x04005727 RID: 22311
		private BoxCollider2D boxCollider2D;

		// Token: 0x04005728 RID: 22312
		private GameObject prevGameObject;
	}
}
