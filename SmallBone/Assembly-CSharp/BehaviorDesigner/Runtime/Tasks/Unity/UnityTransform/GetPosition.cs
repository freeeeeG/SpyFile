using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x0200150E RID: 5390
	[TaskDescription("Stores the position of the Transform. Returns Success.")]
	[TaskCategory("Unity/Transform")]
	public class GetPosition : Action
	{
		// Token: 0x06006871 RID: 26737 RVA: 0x0012DA08 File Offset: 0x0012BC08
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006872 RID: 26738 RVA: 0x0012DA48 File Offset: 0x0012BC48
		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.targetTransform.position;
			return TaskStatus.Success;
		}

		// Token: 0x06006873 RID: 26739 RVA: 0x0012DA80 File Offset: 0x0012BC80
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = Vector2.zero;
		}

		// Token: 0x04005464 RID: 21604
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005465 RID: 21605
		[Tooltip("Can the target GameObject be empty?")]
		[RequiredField]
		public SharedVector2 storeValue;

		// Token: 0x04005466 RID: 21606
		private Transform targetTransform;

		// Token: 0x04005467 RID: 21607
		private GameObject prevGameObject;
	}
}
