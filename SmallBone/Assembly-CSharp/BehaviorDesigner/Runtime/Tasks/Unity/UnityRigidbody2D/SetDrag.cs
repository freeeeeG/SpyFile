using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x0200155D RID: 5469
	[TaskDescription("Sets the drag of the Rigidbody2D. Returns Success.")]
	[TaskCategory("Unity/Rigidbody2D")]
	public class SetDrag : Action
	{
		// Token: 0x06006986 RID: 27014 RVA: 0x0012FCB0 File Offset: 0x0012DEB0
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006987 RID: 27015 RVA: 0x0012FCF0 File Offset: 0x0012DEF0
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			this.rigidbody2D.drag = this.drag.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006988 RID: 27016 RVA: 0x0012FD23 File Offset: 0x0012DF23
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.drag = 0f;
		}

		// Token: 0x0400554F RID: 21839
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005550 RID: 21840
		[Tooltip("The drag of the Rigidbody2D")]
		public SharedFloat drag;

		// Token: 0x04005551 RID: 21841
		private Rigidbody2D rigidbody2D;

		// Token: 0x04005552 RID: 21842
		private GameObject prevGameObject;
	}
}
