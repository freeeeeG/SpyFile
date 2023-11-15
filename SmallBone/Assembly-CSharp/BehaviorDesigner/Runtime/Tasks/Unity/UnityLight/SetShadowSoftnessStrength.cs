using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLight
{
	// Token: 0x020015C3 RID: 5571
	[TaskDescription("Sets the shadow strength of the light.")]
	[TaskCategory("Unity/Light")]
	public class SetShadowSoftnessStrength : Action
	{
		// Token: 0x06006ADA RID: 27354 RVA: 0x00132DB4 File Offset: 0x00130FB4
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.light = defaultGameObject.GetComponent<Light>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006ADB RID: 27355 RVA: 0x00132DF4 File Offset: 0x00130FF4
		public override TaskStatus OnUpdate()
		{
			if (this.light == null)
			{
				Debug.LogWarning("Light is null");
				return TaskStatus.Failure;
			}
			this.light.shadowStrength = this.shadowStrength.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006ADC RID: 27356 RVA: 0x00132E27 File Offset: 0x00131027
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.shadowStrength = 0f;
		}

		// Token: 0x040056B6 RID: 22198
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040056B7 RID: 22199
		[Tooltip("The shadow strength to set")]
		public SharedFloat shadowStrength;

		// Token: 0x040056B8 RID: 22200
		private Light light;

		// Token: 0x040056B9 RID: 22201
		private GameObject prevGameObject;
	}
}
