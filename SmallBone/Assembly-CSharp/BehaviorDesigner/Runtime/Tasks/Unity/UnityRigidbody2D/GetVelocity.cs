using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x02001556 RID: 5462
	[TaskDescription("Stores the velocity of the Rigidbody2D. Returns Success.")]
	[TaskCategory("Unity/Rigidbody2D")]
	public class GetVelocity : Action
	{
		// Token: 0x0600696A RID: 26986 RVA: 0x0012F904 File Offset: 0x0012DB04
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600696B RID: 26987 RVA: 0x0012F944 File Offset: 0x0012DB44
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.rigidbody2D.velocity;
			return TaskStatus.Success;
		}

		// Token: 0x0600696C RID: 26988 RVA: 0x0012F977 File Offset: 0x0012DB77
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = Vector2.zero;
		}

		// Token: 0x04005535 RID: 21813
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005536 RID: 21814
		[Tooltip("The velocity of the Rigidbody2D")]
		[RequiredField]
		public SharedVector2 storeValue;

		// Token: 0x04005537 RID: 21815
		private Rigidbody2D rigidbody2D;

		// Token: 0x04005538 RID: 21816
		private GameObject prevGameObject;
	}
}
