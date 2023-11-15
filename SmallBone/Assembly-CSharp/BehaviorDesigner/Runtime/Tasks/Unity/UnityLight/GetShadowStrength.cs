using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLight
{
	// Token: 0x020015BA RID: 5562
	[TaskCategory("Unity/Light")]
	[TaskDescription("Stores the color of the light.")]
	public class GetShadowStrength : Action
	{
		// Token: 0x06006AB6 RID: 27318 RVA: 0x001328D8 File Offset: 0x00130AD8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.light = defaultGameObject.GetComponent<Light>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006AB7 RID: 27319 RVA: 0x00132918 File Offset: 0x00130B18
		public override TaskStatus OnUpdate()
		{
			if (this.light == null)
			{
				Debug.LogWarning("Light is null");
				return TaskStatus.Failure;
			}
			this.storeValue = this.light.shadowStrength;
			return TaskStatus.Success;
		}

		// Token: 0x06006AB8 RID: 27320 RVA: 0x0013294B File Offset: 0x00130B4B
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 0f;
		}

		// Token: 0x04005692 RID: 22162
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005693 RID: 22163
		[Tooltip("The color to store")]
		[RequiredField]
		public SharedFloat storeValue;

		// Token: 0x04005694 RID: 22164
		private Light light;

		// Token: 0x04005695 RID: 22165
		private GameObject prevGameObject;
	}
}
