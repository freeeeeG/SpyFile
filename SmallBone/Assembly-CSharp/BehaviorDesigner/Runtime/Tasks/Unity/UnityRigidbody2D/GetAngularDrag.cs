using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D
{
	// Token: 0x0200154E RID: 5454
	[TaskDescription("Stores the angular drag of the Rigidbody2D. Returns Success.")]
	[TaskCategory("Unity/Rigidbody2D")]
	public class GetAngularDrag : Action
	{
		// Token: 0x0600694A RID: 26954 RVA: 0x0012F4A8 File Offset: 0x0012D6A8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600694B RID: 26955 RVA: 0x0012F4E8 File Offset: 0x0012D6E8
		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.rigidbody2D.angularDrag;
			return TaskStatus.Success;
		}

		// Token: 0x0600694C RID: 26956 RVA: 0x0012F51B File Offset: 0x0012D71B
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 0f;
		}

		// Token: 0x04005515 RID: 21781
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005516 RID: 21782
		[Tooltip("The angular drag of the Rigidbody2D")]
		[RequiredField]
		public SharedFloat storeValue;

		// Token: 0x04005517 RID: 21783
		private Rigidbody2D rigidbody2D;

		// Token: 0x04005518 RID: 21784
		private GameObject prevGameObject;
	}
}
