using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x02001560 RID: 5472
	[TaskDescription("Sets the mass of the Rigidbody2D. Returns Success.")]
	[TaskCategory("Unity/Rigidbody2D")]
	public class SetMass : Action
	{
		// Token: 0x06006992 RID: 27026 RVA: 0x0012FE50 File Offset: 0x0012E050
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006993 RID: 27027 RVA: 0x0012FE90 File Offset: 0x0012E090
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			this.rigidbody2D.mass = this.mass.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006994 RID: 27028 RVA: 0x0012FEC3 File Offset: 0x0012E0C3
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.mass = 0f;
		}

		// Token: 0x0400555B RID: 21851
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400555C RID: 21852
		[Tooltip("The mass of the Rigidbody2D")]
		public SharedFloat mass;

		// Token: 0x0400555D RID: 21853
		private Rigidbody2D rigidbody2D;

		// Token: 0x0400555E RID: 21854
		private GameObject prevGameObject;
	}
}
