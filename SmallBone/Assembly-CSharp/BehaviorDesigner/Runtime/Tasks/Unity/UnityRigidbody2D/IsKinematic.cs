using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x02001557 RID: 5463
	[TaskCategory("Unity/Rigidbody2D")]
	[TaskDescription("Returns Success if the Rigidbody2D is kinematic, otherwise Failure.")]
	public class IsKinematic : Conditional
	{
		// Token: 0x0600696E RID: 26990 RVA: 0x0012F990 File Offset: 0x0012DB90
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600696F RID: 26991 RVA: 0x0012F9D0 File Offset: 0x0012DBD0
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			if (!this.rigidbody2D.isKinematic)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006970 RID: 26992 RVA: 0x0012F9FC File Offset: 0x0012DBFC
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x04005539 RID: 21817
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400553A RID: 21818
		private Rigidbody2D rigidbody2D;

		// Token: 0x0400553B RID: 21819
		private GameObject prevGameObject;
	}
}
