using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLight
{
	// Token: 0x020015B7 RID: 5559
	[TaskDescription("Stores the intensity of the light.")]
	[TaskCategory("Unity/Light")]
	public class GetIntensity : Action
	{
		// Token: 0x06006AAA RID: 27306 RVA: 0x00132734 File Offset: 0x00130934
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.light = defaultGameObject.GetComponent<Light>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006AAB RID: 27307 RVA: 0x00132774 File Offset: 0x00130974
		public override TaskStatus OnUpdate()
		{
			if (this.light == null)
			{
				Debug.LogWarning("Light is null");
				return TaskStatus.Failure;
			}
			this.storeValue = this.light.intensity;
			return TaskStatus.Success;
		}

		// Token: 0x06006AAC RID: 27308 RVA: 0x001327A7 File Offset: 0x001309A7
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 0f;
		}

		// Token: 0x04005686 RID: 22150
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005687 RID: 22151
		[RequiredField]
		[Tooltip("The intensity to store")]
		public SharedFloat storeValue;

		// Token: 0x04005688 RID: 22152
		private Light light;

		// Token: 0x04005689 RID: 22153
		private GameObject prevGameObject;
	}
}
