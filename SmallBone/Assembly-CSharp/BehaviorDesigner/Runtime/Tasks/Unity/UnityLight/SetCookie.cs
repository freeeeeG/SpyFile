using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLight
{
	// Token: 0x020015BD RID: 5565
	[TaskCategory("Unity/Light")]
	[TaskDescription("Sets the cookie of the light.")]
	public class SetCookie : Action
	{
		// Token: 0x06006AC2 RID: 27330 RVA: 0x00132A7C File Offset: 0x00130C7C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.light = defaultGameObject.GetComponent<Light>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006AC3 RID: 27331 RVA: 0x00132ABC File Offset: 0x00130CBC
		public override TaskStatus OnUpdate()
		{
			if (this.light == null)
			{
				Debug.LogWarning("Light is null");
				return TaskStatus.Failure;
			}
			this.light.cookie = this.cookie;
			return TaskStatus.Success;
		}

		// Token: 0x06006AC4 RID: 27332 RVA: 0x00132AEA File Offset: 0x00130CEA
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.cookie = null;
		}

		// Token: 0x0400569E RID: 22174
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400569F RID: 22175
		[Tooltip("The cookie to set")]
		public Texture2D cookie;

		// Token: 0x040056A0 RID: 22176
		private Light light;

		// Token: 0x040056A1 RID: 22177
		private GameObject prevGameObject;
	}
}
