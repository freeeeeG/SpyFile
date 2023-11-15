using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLight
{
	// Token: 0x020015C2 RID: 5570
	[TaskCategory("Unity/Light")]
	[TaskDescription("Sets the shadow bias of the light.")]
	public class SetShadowBias : Action
	{
		// Token: 0x06006AD6 RID: 27350 RVA: 0x00132D28 File Offset: 0x00130F28
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.light = defaultGameObject.GetComponent<Light>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006AD7 RID: 27351 RVA: 0x00132D68 File Offset: 0x00130F68
		public override TaskStatus OnUpdate()
		{
			if (this.light == null)
			{
				Debug.LogWarning("Light is null");
				return TaskStatus.Failure;
			}
			this.light.shadowBias = this.shadowBias.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006AD8 RID: 27352 RVA: 0x00132D9B File Offset: 0x00130F9B
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.shadowBias = 0f;
		}

		// Token: 0x040056B2 RID: 22194
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040056B3 RID: 22195
		[Tooltip("The shadow bias to set")]
		public SharedFloat shadowBias;

		// Token: 0x040056B4 RID: 22196
		private Light light;

		// Token: 0x040056B5 RID: 22197
		private GameObject prevGameObject;
	}
}
