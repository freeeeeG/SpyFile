using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLight
{
	// Token: 0x020015BB RID: 5563
	[TaskCategory("Unity/Light")]
	[TaskDescription("Stores the spot angle of the light.")]
	public class GetSpotAngle : Action
	{
		// Token: 0x06006ABA RID: 27322 RVA: 0x00132964 File Offset: 0x00130B64
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.light = defaultGameObject.GetComponent<Light>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006ABB RID: 27323 RVA: 0x001329A4 File Offset: 0x00130BA4
		public override TaskStatus OnUpdate()
		{
			if (this.light == null)
			{
				Debug.LogWarning("Light is null");
				return TaskStatus.Failure;
			}
			this.storeValue = this.light.spotAngle;
			return TaskStatus.Success;
		}

		// Token: 0x06006ABC RID: 27324 RVA: 0x001329D7 File Offset: 0x00130BD7
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 0f;
		}

		// Token: 0x04005696 RID: 22166
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005697 RID: 22167
		[Tooltip("The spot angle to store")]
		[RequiredField]
		public SharedFloat storeValue;

		// Token: 0x04005698 RID: 22168
		private Light light;

		// Token: 0x04005699 RID: 22169
		private GameObject prevGameObject;
	}
}
