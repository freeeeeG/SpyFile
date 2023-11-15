using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLight
{
	// Token: 0x020015C0 RID: 5568
	[TaskCategory("Unity/Light")]
	[TaskDescription("Sets the intensity of the light.")]
	public class SetIntensity : Action
	{
		// Token: 0x06006ACE RID: 27342 RVA: 0x00132C10 File Offset: 0x00130E10
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.light = defaultGameObject.GetComponent<Light>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006ACF RID: 27343 RVA: 0x00132C50 File Offset: 0x00130E50
		public override TaskStatus OnUpdate()
		{
			if (this.light == null)
			{
				Debug.LogWarning("Light is null");
				return TaskStatus.Failure;
			}
			this.light.intensity = this.intensity.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006AD0 RID: 27344 RVA: 0x00132C83 File Offset: 0x00130E83
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.intensity = 0f;
		}

		// Token: 0x040056AA RID: 22186
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040056AB RID: 22187
		[Tooltip("The intensity to set")]
		public SharedFloat intensity;

		// Token: 0x040056AC RID: 22188
		private Light light;

		// Token: 0x040056AD RID: 22189
		private GameObject prevGameObject;
	}
}
