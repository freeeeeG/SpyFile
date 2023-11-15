using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x02001558 RID: 5464
	[TaskCategory("Unity/Rigidbody2D")]
	[TaskDescription("Returns Success if the Rigidbody2D is sleeping, otherwise Failure.")]
	public class IsSleeping : Conditional
	{
		// Token: 0x06006972 RID: 26994 RVA: 0x0012FA08 File Offset: 0x0012DC08
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006973 RID: 26995 RVA: 0x0012FA48 File Offset: 0x0012DC48
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			if (!this.rigidbody2D.IsSleeping())
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006974 RID: 26996 RVA: 0x0012FA74 File Offset: 0x0012DC74
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x0400553C RID: 21820
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400553D RID: 21821
		private Rigidbody2D rigidbody2D;

		// Token: 0x0400553E RID: 21822
		private GameObject prevGameObject;
	}
}
