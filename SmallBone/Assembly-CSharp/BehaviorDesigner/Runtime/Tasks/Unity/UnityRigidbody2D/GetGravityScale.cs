using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x02001551 RID: 5457
	[TaskDescription("Stores the gravity scale of the Rigidbody2D. Returns Success.")]
	[TaskCategory("Unity/Rigidbody2D")]
	public class GetGravityScale : Action
	{
		// Token: 0x06006956 RID: 26966 RVA: 0x0012F64C File Offset: 0x0012D84C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006957 RID: 26967 RVA: 0x0012F68C File Offset: 0x0012D88C
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.rigidbody2D.gravityScale;
			return TaskStatus.Success;
		}

		// Token: 0x06006958 RID: 26968 RVA: 0x0012F6BF File Offset: 0x0012D8BF
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 0f;
		}

		// Token: 0x04005521 RID: 21793
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005522 RID: 21794
		[Tooltip("The gravity scale of the Rigidbody2D")]
		[RequiredField]
		public SharedFloat storeValue;

		// Token: 0x04005523 RID: 21795
		private Rigidbody2D rigidbody2D;

		// Token: 0x04005524 RID: 21796
		private GameObject prevGameObject;
	}
}
