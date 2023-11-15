using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLight
{
	// Token: 0x020015C5 RID: 5573
	[TaskCategory("Unity/Light")]
	[TaskDescription("Sets the spot angle of the light.")]
	public class SetSpotAngle : Action
	{
		// Token: 0x06006AE2 RID: 27362 RVA: 0x00132EB8 File Offset: 0x001310B8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.light = defaultGameObject.GetComponent<Light>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006AE3 RID: 27363 RVA: 0x00132EF8 File Offset: 0x001310F8
		public override TaskStatus OnUpdate()
		{
			if (this.light == null)
			{
				Debug.LogWarning("Light is null");
				return TaskStatus.Failure;
			}
			this.light.spotAngle = this.spotAngle.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006AE4 RID: 27364 RVA: 0x00132F2B File Offset: 0x0013112B
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.spotAngle = 0f;
		}

		// Token: 0x040056BE RID: 22206
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040056BF RID: 22207
		[Tooltip("The spot angle to set")]
		public SharedFloat spotAngle;

		// Token: 0x040056C0 RID: 22208
		private Light light;

		// Token: 0x040056C1 RID: 22209
		private GameObject prevGameObject;
	}
}
