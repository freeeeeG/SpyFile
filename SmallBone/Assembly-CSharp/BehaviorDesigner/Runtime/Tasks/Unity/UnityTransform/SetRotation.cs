using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x0200151F RID: 5407
	[TaskCategory("Unity/Transform")]
	[TaskDescription("Sets the rotation of the Transform. Returns Success.")]
	public class SetRotation : Action
	{
		// Token: 0x060068B5 RID: 26805 RVA: 0x0012E40C File Offset: 0x0012C60C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x060068B6 RID: 26806 RVA: 0x0012E44C File Offset: 0x0012C64C
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.targetTransform.rotation = this.rotation.Value;
			return TaskStatus.Success;
		}

		// Token: 0x060068B7 RID: 26807 RVA: 0x0012E47F File Offset: 0x0012C67F
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.rotation = Quaternion.identity;
		}

		// Token: 0x040054AD RID: 21677
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040054AE RID: 21678
		[Tooltip("The rotation of the Transform")]
		public SharedQuaternion rotation;

		// Token: 0x040054AF RID: 21679
		private Transform targetTransform;

		// Token: 0x040054B0 RID: 21680
		private GameObject prevGameObject;
	}
}
