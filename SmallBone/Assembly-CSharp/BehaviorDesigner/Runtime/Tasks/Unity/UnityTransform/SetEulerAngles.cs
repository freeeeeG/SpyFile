using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x02001516 RID: 5398
	[TaskDescription("Sets the euler angles of the Transform. Returns Success.")]
	[TaskCategory("Unity/Transform")]
	public class SetEulerAngles : Action
	{
		// Token: 0x06006891 RID: 26769 RVA: 0x0012DF20 File Offset: 0x0012C120
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006892 RID: 26770 RVA: 0x0012DF60 File Offset: 0x0012C160
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.targetTransform.eulerAngles = this.eulerAngles.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006893 RID: 26771 RVA: 0x0012DF93 File Offset: 0x0012C193
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.eulerAngles = Vector3.zero;
		}

		// Token: 0x04005489 RID: 21641
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400548A RID: 21642
		[Tooltip("The euler angles of the Transform")]
		public SharedVector3 eulerAngles;

		// Token: 0x0400548B RID: 21643
		private Transform targetTransform;

		// Token: 0x0400548C RID: 21644
		private GameObject prevGameObject;
	}
}
