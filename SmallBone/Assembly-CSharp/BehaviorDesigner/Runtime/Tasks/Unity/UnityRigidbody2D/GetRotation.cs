using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x02001555 RID: 5461
	[TaskCategory("Unity/Rigidbody2D")]
	[TaskDescription("Stores the rotation of the Rigidbody2D. Returns Success.")]
	public class GetRotation : Action
	{
		// Token: 0x06006966 RID: 26982 RVA: 0x0012F878 File Offset: 0x0012DA78
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006967 RID: 26983 RVA: 0x0012F8B8 File Offset: 0x0012DAB8
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.rigidbody2D.rotation;
			return TaskStatus.Success;
		}

		// Token: 0x06006968 RID: 26984 RVA: 0x0012F8EB File Offset: 0x0012DAEB
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 0f;
		}

		// Token: 0x04005531 RID: 21809
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005532 RID: 21810
		[Tooltip("The rotation of the Rigidbody2D")]
		[RequiredField]
		public SharedFloat storeValue;

		// Token: 0x04005533 RID: 21811
		private Rigidbody2D rigidbody2D;

		// Token: 0x04005534 RID: 21812
		private GameObject prevGameObject;
	}
}
