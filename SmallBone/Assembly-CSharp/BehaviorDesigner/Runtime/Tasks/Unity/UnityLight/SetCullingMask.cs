using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLight
{
	// Token: 0x020015BF RID: 5567
	[TaskCategory("Unity/Light")]
	[TaskDescription("Sets the culling mask of the light.")]
	public class SetCullingMask : Action
	{
		// Token: 0x06006ACA RID: 27338 RVA: 0x00132B88 File Offset: 0x00130D88
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.light = defaultGameObject.GetComponent<Light>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006ACB RID: 27339 RVA: 0x00132BC8 File Offset: 0x00130DC8
		public override TaskStatus OnUpdate()
		{
			if (this.light == null)
			{
				Debug.LogWarning("Light is null");
				return TaskStatus.Failure;
			}
			this.light.cullingMask = this.cullingMask.value;
			return TaskStatus.Success;
		}

		// Token: 0x06006ACC RID: 27340 RVA: 0x00132BFB File Offset: 0x00130DFB
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.cullingMask = -1;
		}

		// Token: 0x040056A6 RID: 22182
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040056A7 RID: 22183
		[Tooltip("The culling mask to set")]
		public LayerMask cullingMask;

		// Token: 0x040056A8 RID: 22184
		private Light light;

		// Token: 0x040056A9 RID: 22185
		private GameObject prevGameObject;
	}
}
