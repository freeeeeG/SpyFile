using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x02001550 RID: 5456
	[TaskDescription("Stores the drag of the Rigidbody2D. Returns Success.")]
	[TaskCategory("Unity/Rigidbody2D")]
	public class GetDrag : Action
	{
		// Token: 0x06006952 RID: 26962 RVA: 0x0012F5C0 File Offset: 0x0012D7C0
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006953 RID: 26963 RVA: 0x0012F600 File Offset: 0x0012D800
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.rigidbody2D.drag;
			return TaskStatus.Success;
		}

		// Token: 0x06006954 RID: 26964 RVA: 0x0012F633 File Offset: 0x0012D833
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 0f;
		}

		// Token: 0x0400551D RID: 21789
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400551E RID: 21790
		[Tooltip("The drag of the Rigidbody2D")]
		[RequiredField]
		public SharedFloat storeValue;

		// Token: 0x0400551F RID: 21791
		private Rigidbody2D rigidbody2D;

		// Token: 0x04005520 RID: 21792
		private GameObject prevGameObject;
	}
}
