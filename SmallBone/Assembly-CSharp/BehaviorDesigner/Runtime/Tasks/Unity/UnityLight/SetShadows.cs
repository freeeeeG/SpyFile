using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLight
{
	// Token: 0x020015C4 RID: 5572
	[TaskCategory("Unity/Light")]
	[TaskDescription("Sets the shadow type of the light.")]
	public class SetShadows : Action
	{
		// Token: 0x06006ADE RID: 27358 RVA: 0x00132E40 File Offset: 0x00131040
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.light = defaultGameObject.GetComponent<Light>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006ADF RID: 27359 RVA: 0x00132E80 File Offset: 0x00131080
		public override TaskStatus OnUpdate()
		{
			if (this.light == null)
			{
				Debug.LogWarning("Light is null");
				return TaskStatus.Failure;
			}
			this.light.shadows = this.shadows;
			return TaskStatus.Success;
		}

		// Token: 0x06006AE0 RID: 27360 RVA: 0x00132EAE File Offset: 0x001310AE
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x040056BA RID: 22202
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040056BB RID: 22203
		[Tooltip("The shadow type to set")]
		public LightShadows shadows;

		// Token: 0x040056BC RID: 22204
		private Light light;

		// Token: 0x040056BD RID: 22205
		private GameObject prevGameObject;
	}
}
