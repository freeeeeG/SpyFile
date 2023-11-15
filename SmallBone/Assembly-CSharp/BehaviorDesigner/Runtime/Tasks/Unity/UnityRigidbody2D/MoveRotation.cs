using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x0200155A RID: 5466
	[TaskCategory("Unity/Rigidbody2D")]
	[TaskDescription("Rotates the Rigidbody2D to the specified rotation. Returns Success.")]
	public class MoveRotation : Action
	{
		// Token: 0x0600697A RID: 27002 RVA: 0x0012FB0C File Offset: 0x0012DD0C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600697B RID: 27003 RVA: 0x0012FB4C File Offset: 0x0012DD4C
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody is null");
				return TaskStatus.Failure;
			}
			this.rigidbody2D.MoveRotation(this.rotation.Value);
			return TaskStatus.Success;
		}

		// Token: 0x0600697C RID: 27004 RVA: 0x0012FB7F File Offset: 0x0012DD7F
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.rotation = 0f;
		}

		// Token: 0x04005543 RID: 21827
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005544 RID: 21828
		[Tooltip("The new rotation of the Rigidbody")]
		public SharedFloat rotation;

		// Token: 0x04005545 RID: 21829
		private Rigidbody2D rigidbody2D;

		// Token: 0x04005546 RID: 21830
		private GameObject prevGameObject;
	}
}
