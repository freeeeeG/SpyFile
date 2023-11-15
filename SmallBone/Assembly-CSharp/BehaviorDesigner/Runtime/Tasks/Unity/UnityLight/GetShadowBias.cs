using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLight
{
	// Token: 0x020015B9 RID: 5561
	[TaskCategory("Unity/Light")]
	[TaskDescription("Stores the shadow bias of the light.")]
	public class GetShadowBias : Action
	{
		// Token: 0x06006AB2 RID: 27314 RVA: 0x0013284C File Offset: 0x00130A4C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.light = defaultGameObject.GetComponent<Light>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006AB3 RID: 27315 RVA: 0x0013288C File Offset: 0x00130A8C
		public override TaskStatus OnUpdate()
		{
			if (this.light == null)
			{
				Debug.LogWarning("Light is null");
				return TaskStatus.Failure;
			}
			this.storeValue = this.light.shadowBias;
			return TaskStatus.Success;
		}

		// Token: 0x06006AB4 RID: 27316 RVA: 0x001328BF File Offset: 0x00130ABF
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 0f;
		}

		// Token: 0x0400568E RID: 22158
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400568F RID: 22159
		[Tooltip("The shadow bias to store")]
		[RequiredField]
		public SharedFloat storeValue;

		// Token: 0x04005690 RID: 22160
		private Light light;

		// Token: 0x04005691 RID: 22161
		private GameObject prevGameObject;
	}
}
