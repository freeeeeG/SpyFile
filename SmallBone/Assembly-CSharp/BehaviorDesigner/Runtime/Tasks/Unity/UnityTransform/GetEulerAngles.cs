using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x02001507 RID: 5383
	[TaskDescription("Stores the euler angles of the Transform. Returns Success.")]
	[TaskCategory("Unity/Transform")]
	public class GetEulerAngles : Action
	{
		// Token: 0x06006855 RID: 26709 RVA: 0x0012D63C File Offset: 0x0012B83C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006856 RID: 26710 RVA: 0x0012D67C File Offset: 0x0012B87C
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.targetTransform.eulerAngles;
			return TaskStatus.Success;
		}

		// Token: 0x06006857 RID: 26711 RVA: 0x0012D6AF File Offset: 0x0012B8AF
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = Vector3.zero;
		}

		// Token: 0x04005448 RID: 21576
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005449 RID: 21577
		[RequiredField]
		[Tooltip("The euler angles of the Transform")]
		public SharedVector3 storeValue;

		// Token: 0x0400544A RID: 21578
		private Transform targetTransform;

		// Token: 0x0400544B RID: 21579
		private GameObject prevGameObject;
	}
}
