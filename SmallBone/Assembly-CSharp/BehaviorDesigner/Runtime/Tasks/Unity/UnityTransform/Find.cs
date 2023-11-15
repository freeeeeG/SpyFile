using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x02001502 RID: 5378
	[TaskCategory("Unity/Transform")]
	[TaskDescription("Finds a transform by name. Returns success if an object is found.")]
	public class Find : Action
	{
		// Token: 0x06006842 RID: 26690 RVA: 0x0012D2CC File Offset: 0x0012B4CC
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006843 RID: 26691 RVA: 0x0012D30C File Offset: 0x0012B50C
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.targetTransform.Find(this.transformName.Value);
			if (!(this.storeValue.Value != null))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006844 RID: 26692 RVA: 0x0012D36A File Offset: 0x0012B56A
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.transformName = null;
			this.storeValue = null;
		}

		// Token: 0x04005430 RID: 21552
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005431 RID: 21553
		[Tooltip("The transform name to find")]
		public SharedString transformName;

		// Token: 0x04005432 RID: 21554
		[RequiredField]
		[Tooltip("The object found by name")]
		public SharedTransform storeValue;

		// Token: 0x04005433 RID: 21555
		private Transform targetTransform;

		// Token: 0x04005434 RID: 21556
		private GameObject prevGameObject;
	}
}
