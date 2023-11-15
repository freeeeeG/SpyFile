using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityCircleCollider2D
{
	// Token: 0x020015EB RID: 5611
	[TaskDescription("Stores the offset of the CircleCollider2D. Returns Success.")]
	[TaskCategory("Unity/CircleCollider2D")]
	public class GetOffset : Action
	{
		// Token: 0x06006B58 RID: 27480 RVA: 0x00133B14 File Offset: 0x00131D14
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.circleCollider2D = defaultGameObject.GetComponent<CircleCollider2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006B59 RID: 27481 RVA: 0x00133B54 File Offset: 0x00131D54
		public override TaskStatus OnUpdate()
		{
			if (this.circleCollider2D == null)
			{
				Debug.LogWarning("CircleCollider2D is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.circleCollider2D.offset;
			return TaskStatus.Success;
		}

		// Token: 0x06006B5A RID: 27482 RVA: 0x00133B8C File Offset: 0x00131D8C
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = Vector3.zero;
		}

		// Token: 0x04005711 RID: 22289
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005712 RID: 22290
		[RequiredField]
		[Tooltip("The offset of the CircleCollider2D")]
		public SharedVector3 storeValue;

		// Token: 0x04005713 RID: 22291
		private CircleCollider2D circleCollider2D;

		// Token: 0x04005714 RID: 22292
		private GameObject prevGameObject;
	}
}
