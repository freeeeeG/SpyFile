using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLight
{
	// Token: 0x020015BE RID: 5566
	[TaskCategory("Unity/Light")]
	[TaskDescription("Sets the light's cookie size.")]
	public class SetCookieSize : Action
	{
		// Token: 0x06006AC6 RID: 27334 RVA: 0x00132AFC File Offset: 0x00130CFC
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.light = defaultGameObject.GetComponent<Light>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006AC7 RID: 27335 RVA: 0x00132B3C File Offset: 0x00130D3C
		public override TaskStatus OnUpdate()
		{
			if (this.light == null)
			{
				Debug.LogWarning("Light is null");
				return TaskStatus.Failure;
			}
			this.light.cookieSize = this.cookieSize.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006AC8 RID: 27336 RVA: 0x00132B6F File Offset: 0x00130D6F
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.cookieSize = 0f;
		}

		// Token: 0x040056A2 RID: 22178
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040056A3 RID: 22179
		[Tooltip("The size to set")]
		public SharedFloat cookieSize;

		// Token: 0x040056A4 RID: 22180
		private Light light;

		// Token: 0x040056A5 RID: 22181
		private GameObject prevGameObject;
	}
}
