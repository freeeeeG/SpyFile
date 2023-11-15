using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityBoxCollider2D
{
	// Token: 0x020015EF RID: 5615
	[TaskDescription("Stores the size of the BoxCollider2D. Returns Success.")]
	[TaskCategory("Unity/BoxCollider2D")]
	public class GetSize : Action
	{
		// Token: 0x06006B68 RID: 27496 RVA: 0x00133D54 File Offset: 0x00131F54
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.boxCollider2D = defaultGameObject.GetComponent<BoxCollider2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006B69 RID: 27497 RVA: 0x00133D94 File Offset: 0x00131F94
		public override TaskStatus OnUpdate()
		{
			if (this.boxCollider2D == null)
			{
				Debug.LogWarning("BoxCollider2D is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.boxCollider2D.size;
			return TaskStatus.Success;
		}

		// Token: 0x06006B6A RID: 27498 RVA: 0x00133DC7 File Offset: 0x00131FC7
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = Vector2.zero;
		}

		// Token: 0x04005721 RID: 22305
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005722 RID: 22306
		[RequiredField]
		[Tooltip("The size of the BoxCollider2D")]
		public SharedVector2 storeValue;

		// Token: 0x04005723 RID: 22307
		private BoxCollider2D boxCollider2D;

		// Token: 0x04005724 RID: 22308
		private GameObject prevGameObject;
	}
}
