using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x0200151C RID: 5404
	[TaskDescription("Sets the parent of the Transform. Returns Success.")]
	[TaskCategory("Unity/Transform")]
	public class SetParent : Action
	{
		// Token: 0x060068A9 RID: 26793 RVA: 0x0012E268 File Offset: 0x0012C468
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x060068AA RID: 26794 RVA: 0x0012E2A8 File Offset: 0x0012C4A8
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.targetTransform.parent = this.parent.Value;
			return TaskStatus.Success;
		}

		// Token: 0x060068AB RID: 26795 RVA: 0x0012E2DB File Offset: 0x0012C4DB
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.parent = null;
		}

		// Token: 0x040054A1 RID: 21665
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040054A2 RID: 21666
		[Tooltip("The parent of the Transform")]
		public SharedTransform parent;

		// Token: 0x040054A3 RID: 21667
		private Transform targetTransform;

		// Token: 0x040054A4 RID: 21668
		private GameObject prevGameObject;
	}
}
