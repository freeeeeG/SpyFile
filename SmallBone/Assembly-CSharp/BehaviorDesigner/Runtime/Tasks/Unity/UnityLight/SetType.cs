using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLight
{
	// Token: 0x020015C6 RID: 5574
	[TaskCategory("Unity/Light")]
	[TaskDescription("Sets the type of the light.")]
	public class SetType : Action
	{
		// Token: 0x06006AE6 RID: 27366 RVA: 0x00132F44 File Offset: 0x00131144
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.light = defaultGameObject.GetComponent<Light>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006AE7 RID: 27367 RVA: 0x00132F84 File Offset: 0x00131184
		public override TaskStatus OnUpdate()
		{
			if (this.light == null)
			{
				Debug.LogWarning("Light is null");
				return TaskStatus.Failure;
			}
			this.light.type = this.type;
			return TaskStatus.Success;
		}

		// Token: 0x06006AE8 RID: 27368 RVA: 0x00132FB2 File Offset: 0x001311B2
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x040056C2 RID: 22210
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040056C3 RID: 22211
		[Tooltip("The type to set")]
		public LightType type;

		// Token: 0x040056C4 RID: 22212
		private Light light;

		// Token: 0x040056C5 RID: 22213
		private GameObject prevGameObject;
	}
}
