using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLight
{
	// Token: 0x020015B6 RID: 5558
	[TaskDescription("Stores the light's cookie size.")]
	[TaskCategory("Unity/Light")]
	public class GetCookieSize : Action
	{
		// Token: 0x06006AA6 RID: 27302 RVA: 0x001326A8 File Offset: 0x001308A8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.light = defaultGameObject.GetComponent<Light>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006AA7 RID: 27303 RVA: 0x001326E8 File Offset: 0x001308E8
		public override TaskStatus OnUpdate()
		{
			if (this.light == null)
			{
				Debug.LogWarning("Light is null");
				return TaskStatus.Failure;
			}
			this.storeValue = this.light.cookieSize;
			return TaskStatus.Success;
		}

		// Token: 0x06006AA8 RID: 27304 RVA: 0x0013271B File Offset: 0x0013091B
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 0f;
		}

		// Token: 0x04005682 RID: 22146
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005683 RID: 22147
		[Tooltip("The size to store")]
		[RequiredField]
		public SharedFloat storeValue;

		// Token: 0x04005684 RID: 22148
		private Light light;

		// Token: 0x04005685 RID: 22149
		private GameObject prevGameObject;
	}
}
