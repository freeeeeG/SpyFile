using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x02001559 RID: 5465
	[TaskDescription("Moves the Rigidbody2D to the specified position. Returns Success.")]
	[TaskCategory("Unity/Rigidbody2D")]
	public class MovePosition : Action
	{
		// Token: 0x06006976 RID: 26998 RVA: 0x0012FA80 File Offset: 0x0012DC80
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006977 RID: 26999 RVA: 0x0012FAC0 File Offset: 0x0012DCC0
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody is null");
				return TaskStatus.Failure;
			}
			this.rigidbody2D.MovePosition(this.position.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006978 RID: 27000 RVA: 0x0012FAF3 File Offset: 0x0012DCF3
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.position = Vector2.zero;
		}

		// Token: 0x0400553F RID: 21823
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005540 RID: 21824
		[Tooltip("The new position of the Rigidbody")]
		public SharedVector2 position;

		// Token: 0x04005541 RID: 21825
		private Rigidbody2D rigidbody2D;

		// Token: 0x04005542 RID: 21826
		private GameObject prevGameObject;
	}
}
