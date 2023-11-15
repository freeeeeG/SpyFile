using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x02001511 RID: 5393
	[TaskDescription("Stores the up vector of the Transform. Returns Success.")]
	[TaskCategory("Unity/Transform")]
	public class GetUpVector : Action
	{
		// Token: 0x0600687D RID: 26749 RVA: 0x0012DBB4 File Offset: 0x0012BDB4
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x0600687E RID: 26750 RVA: 0x0012DBF4 File Offset: 0x0012BDF4
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.targetTransform.up;
			return TaskStatus.Success;
		}

		// Token: 0x0600687F RID: 26751 RVA: 0x0012DC27 File Offset: 0x0012BE27
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = Vector3.zero;
		}

		// Token: 0x04005470 RID: 21616
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005471 RID: 21617
		[Tooltip("The position of the Transform")]
		[RequiredField]
		public SharedVector3 storeValue;

		// Token: 0x04005472 RID: 21618
		private Transform targetTransform;

		// Token: 0x04005473 RID: 21619
		private GameObject prevGameObject;
	}
}
