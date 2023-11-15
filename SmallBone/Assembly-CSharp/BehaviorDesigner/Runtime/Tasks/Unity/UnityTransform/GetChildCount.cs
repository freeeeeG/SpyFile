using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x02001506 RID: 5382
	[TaskDescription("Stores the number of children a Transform has. Returns Success.")]
	[TaskCategory("Unity/Transform")]
	public class GetChildCount : Action
	{
		// Token: 0x06006851 RID: 26705 RVA: 0x0012D5B4 File Offset: 0x0012B7B4
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006852 RID: 26706 RVA: 0x0012D5F4 File Offset: 0x0012B7F4
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.targetTransform.childCount;
			return TaskStatus.Success;
		}

		// Token: 0x06006853 RID: 26707 RVA: 0x0012D627 File Offset: 0x0012B827
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 0;
		}

		// Token: 0x04005444 RID: 21572
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005445 RID: 21573
		[Tooltip("The number of children")]
		[RequiredField]
		public SharedInt storeValue;

		// Token: 0x04005446 RID: 21574
		private Transform targetTransform;

		// Token: 0x04005447 RID: 21575
		private GameObject prevGameObject;
	}
}
